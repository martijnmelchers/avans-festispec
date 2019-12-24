using Festispec.Models;
using Festispec.Models.EntityMapping;
using Festispec.Models.Google;
using Microsoft.AspNetCore.Http.Extensions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
namespace Festispec.DomainServices.Services
{
    public class GoogleMapsService
    {
        private readonly HttpClient _client;
        private readonly HttpClient _clientStatic;
        private const string API_KEY = "AIzaSyDqy_DxcI0571BKIoakNuOj-eWQ6S_B3NM";
        private readonly string _sessionToken;


        private readonly FestispecContext _db;

        public GoogleMapsService(FestispecContext db)
        {

            _db = db;

            _client = new HttpClient
            {
                BaseAddress = new Uri("https://maps.googleapis.com/maps/api/place/")
            };

                _sessionToken =  new string(Enumerable.Repeat("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789", 10).Select(s => s[new Random().Next(s.Length)]).ToArray());


            _clientStatic = new HttpClient
            {
                BaseAddress = new Uri("https://maps.googleapis.com/maps/api/staticmap")
            };

        }

        public async Task<List<Prediction>> GetSuggestions(string input)
        {
            var request = await _client.GetAsync($"autocomplete/json?input={Uri.EscapeDataString(input)}&components=country:nl|country:be|country:de&sessiontoken={_sessionToken}&language=nl&key={API_KEY}");
            var s = await request.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<AutocompleteResponse>(await request.Content.ReadAsStringAsync());

            if (!result.Status.Equals(GoogleStatusCodes.Ok))
                throw new Exception();

            return result.Predictions;
        }

        public async Task<Place> GetPlace(string placeId)
        {
            var request = await _client.GetAsync($"details/json?place_id={placeId}&fields=address_component,formatted_address,geometry&sessiontoken={_sessionToken}&language=nl&key={API_KEY}");
            var result = JsonConvert.DeserializeObject<PlaceDetailResponse>(await request.Content.ReadAsStringAsync());

            if (!result.Status.Equals(GoogleStatusCodes.Ok))
                throw new Exception();

            return result.Place;
        }

        public Address PlaceToAddress(Place place)
        {
            var zipCode = place.AddressComponents.FirstOrDefault(x => x.Types.Contains("postal_code")).LongName;
            var houseNumber = int.Parse(place.AddressComponents.FirstOrDefault(x => x.Types.Contains("street_number")).LongName);
            var city = place.AddressComponents.FirstOrDefault(x => x.Types.Contains("locality")).LongName;
            var country = place.AddressComponents.FirstOrDefault(x => x.Types.Contains("country")).LongName;
            var streetName = place.AddressComponents.FirstOrDefault(x => x.Types.Contains("route")).LongName;

            return new Address
            {
                City = city,
                ZipCode = zipCode,
                HouseNumber = houseNumber,
                Country = country,
                StreetName = streetName,
                Suffix = "",
                Latitude = place.Geometry.Location.Latitude,
                Longitude = place.Geometry.Location.Longitude
            };
        }

        public async Task<string> GenerateStaticMap()
        {
            var queryBuilder = new QueryBuilder();

            queryBuilder.Add("key", API_KEY);
            queryBuilder.Add("center", "Netherlands");
            queryBuilder.Add("size", "1920x1080");

            foreach (Festival festival in _db.Festivals.ToList()) {
                var latitude = festival.Address.Latitude.ToString();
                var longitude = festival.Address.Longitude.ToString();
                var label = festival.FestivalName;
                var color = "blue";
                queryBuilder.Add("markers", CreateMarker(latitude, longitude, label, color));
            }

            return String.Format("{0}{1}","https://maps.googleapis.com/maps/api/staticmap", queryBuilder.ToString());
        }


        private string CreateMarker(string latitude, string longitude, string label, string color)
        {
            return String.Format("color:{0}|label:{1}|{2},{3}",color, label, latitude, longitude);
        }
    }
}