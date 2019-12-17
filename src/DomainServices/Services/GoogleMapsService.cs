using Festispec.Models;
using Festispec.Models.Google;
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
            int.TryParse(GetComponent(place, "street_number")?.LongName, out int houseNumber);
            return new Address
            {
                City = GetComponent(place, "locality")?.LongName,
                ZipCode = GetComponent(place, "postal_code")?.LongName,
                HouseNumber = houseNumber,
                Country = GetComponent(place, "country")?.LongName,
                StreetName = GetComponent(place, "route")?.LongName,
                Suffix = "",
                Latitude = place.Geometry.Location.Latitude,
                Longitude = place.Geometry.Location.Longitude
            };
        }


        private AddressComponent GetComponent(Place place, string name)
        {
            return place.AddressComponents.FirstOrDefault(x => x.Types.Contains(name));
        }








    }
}
