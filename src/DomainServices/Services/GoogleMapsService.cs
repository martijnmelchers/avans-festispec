using Festispec.DomainServices.Interfaces;
using Festispec.Models;
using Festispec.Models.Exception;
using Festispec.Models.Google;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Festispec.DomainServices.Services
{
    public class GoogleMapsService : IGoogleMapsService
    {
        private readonly HttpClient _client;
        private const string API_KEY = "AIzaSyB75U9ewy-e0nrRb4WKXXTTdalclxoipTs";
        private readonly string _sessionToken;
        public GoogleMapsService()
        {
            _client = new HttpClient
            {
                BaseAddress = new Uri("https://maps.googleapis.com/maps/api/place/")
            };

            _sessionToken =  new string(Enumerable.Repeat("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789", 10).Select(s => s[new Random().Next(s.Length)]).ToArray());
        }

        public async Task<List<Prediction>> GetSuggestions(string input)
        {
            var request = await _client.GetAsync($"autocomplete/json?input={Uri.EscapeDataString(input)}&components=country:nl|country:be|country:de&sessiontoken={_sessionToken}&language=nl&key={API_KEY}");
            var result = JsonConvert.DeserializeObject<AutocompleteResponse>(await request.Content.ReadAsStringAsync());

            if (!result.Status.Equals(GoogleStatusCodes.Ok))
                throw new GoogleMapsApiException();

            return result.Predictions;
        }

        public async Task<Address> GetAddress(string placeId)
        {
            var request = await _client.GetAsync($"details/json?place_id={placeId}&fields=address_component,formatted_address,geometry&sessiontoken={_sessionToken}&language=nl&key={API_KEY}");
            var result = JsonConvert.DeserializeObject<PlaceDetailResponse>(await request.Content.ReadAsStringAsync());

            if (!result.Status.Equals(GoogleStatusCodes.Ok))
                throw new GoogleMapsApiException();

            int.TryParse(Regex.Replace(GetComponent(result.Place, "street_number")?.LongName ?? string.Empty, "[^.0-9]", ""), out int houseNumber);

            return new Address
            {
                City = GetComponent(result.Place, "locality")?.LongName,
                ZipCode = GetComponent(result.Place, "postal_code")?.LongName,
                HouseNumber = houseNumber,
                Country = GetComponent(result.Place, "country")?.LongName,
                StreetName = GetComponent(result.Place, "route")?.LongName,
                Suffix = Regex.Replace(GetComponent(result.Place, "street_number")?.LongName ?? string.Empty, @"[\d-]", string.Empty),
                Latitude = result.Place.Geometry.Location.Latitude,
                Longitude = result.Place.Geometry.Location.Longitude
            };
        }

        private AddressComponent GetComponent(Place place, string name)
        {
            return place.AddressComponents.FirstOrDefault(x => x.Types.Contains(name));
        }








    }
}
