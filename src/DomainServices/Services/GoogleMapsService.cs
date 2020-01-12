using Festispec.DomainServices.Interfaces;
using Festispec.Models;
using Festispec.Models.EntityMapping;
using Festispec.Models.Exception;
using Festispec.Models.Google;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Festispec.DomainServices.Services
{
    public class GoogleMapsService : IGoogleMapsService
    {
        private readonly string _apiKey;
        private readonly HttpClient _client;
        private readonly FestispecContext _db;
        private readonly string _sessionToken;

        private readonly ISyncService<DistanceResult> _syncService;

        public GoogleMapsService(FestispecContext db, ISyncService<DistanceResult> syncService, IConfiguration config)
        {
            _client = new HttpClient
            {
                BaseAddress = new Uri("https://maps.googleapis.com/maps/api/")
            };

            _sessionToken = new string(Enumerable.Repeat("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789", 10)
                .Select(s => s[new Random().Next(s.Length)]).ToArray());
            _db = db;
            _syncService = syncService;
            _apiKey = config["ApiKeys:Google"];
        }

        public async Task<List<Prediction>> GetSuggestions(string input)
        {
            HttpResponseMessage request = await _client.GetAsync(
                $"place/autocomplete/json?input={Uri.EscapeDataString(input)}&components=country:nl|country:be|country:de&sessiontoken={_sessionToken}&language=nl&key={_apiKey}");
            var result = JsonConvert.DeserializeObject<AutocompleteResponse>(await request.Content.ReadAsStringAsync());

            if (result.Status.Equals(GoogleStatusCodes.ZeroResults))
                throw new GoogleZeroResultsException();

            if (!result.Status.Equals(GoogleStatusCodes.Ok))
                throw new GoogleMapsApiException();

            return result.Predictions;
        }

        public async Task<Address> GetAddress(string placeId)
        {
            var request = await _client.GetAsync(
                $"place/details/json?place_id={placeId}&fields=address_component,formatted_address,geometry&sessiontoken={_sessionToken}&language=nl&key={_apiKey}");
            var result = JsonConvert.DeserializeObject<PlaceDetailResponse>(await request.Content.ReadAsStringAsync());

            if (!result.Status.Equals(GoogleStatusCodes.Ok))
                throw new GoogleMapsApiException();

            int.TryParse(
                Regex.Replace(GetComponent(result.Place, "street_number")?.LongName ?? string.Empty, "[^.0-9]", ""),
                out var houseNumber);

            return new Address
            {
                City = GetComponent(result.Place, "locality")?.LongName,
                ZipCode = GetComponent(result.Place, "postal_code")?.LongName,
                HouseNumber = houseNumber,
                Country = GetComponent(result.Place, "country")?.LongName,
                StreetName = GetComponent(result.Place, "route")?.LongName,
                Suffix = Regex.Replace(GetComponent(result.Place, "street_number")?.LongName ?? string.Empty, @"[\d-]",
                    string.Empty),
                Latitude = result.Place.Geometry.Location.Latitude,
                Longitude = result.Place.Geometry.Location.Longitude
            };
        }

        public async Task<double> CalculateDistance(Address origin, Address destination)
        {
            DistanceResult existing = await _db.DistanceResults
                .FirstOrDefaultAsync(x => x.Origin.Id == origin.Id && x.Destination.Id == destination.Id);

            if (existing != null)
                return existing.Distance;

            var request = await _client.GetAsync(
                "distancematrix/json?units=metric" +
                $"&origins={origin.Latitude.ToString(CultureInfo.InvariantCulture).Replace(",", ".")},{origin.Longitude.ToString(CultureInfo.InvariantCulture).Replace(",", ".")}" +
                $"&destinations={destination.Latitude.ToString(CultureInfo.InvariantCulture).Replace(",", ".")},{destination.Longitude.ToString(CultureInfo.InvariantCulture).Replace(",", ".")}" +
                $"&language=nl&key={_apiKey}");
            var result =
                JsonConvert.DeserializeObject<DistanceMatrixResponse>(await request.Content.ReadAsStringAsync());

            if (!result.Status.Equals(GoogleStatusCodes.Ok))
                throw new GoogleMapsApiException();

            var distanceResult = new DistanceResult
            {
                Origin = await _db.Addresses.FirstOrDefaultAsync(a => a.Id == origin.Id),
                Destination = await _db.Addresses.FirstOrDefaultAsync(a => a.Id == destination.Id),
                Distance = Math.Round((double) result.Rows[0].Elements[0].Distance.DistanceValue / 1000, 2)
            };

            _db.DistanceResults.Add(distanceResult);
            await _db.SaveChangesAsync();

            return distanceResult.Distance;
        }

        private AddressComponent GetComponent(Place place, string name)
        {
            return place.AddressComponents.FirstOrDefault(x => x.Types.Contains(name));
        }

        public void Sync()
        {
            FestispecContext db = _syncService.GetSyncContext();
            
            List<DistanceResult> distanceResults = db.DistanceResults
                .Include(i => i.Destination)
                .Include(i => i.Origin)
                .ToList();

            _syncService.Flush();
            _syncService.AddEntities(distanceResults);
            _syncService.SaveChanges();
        }
    }
}