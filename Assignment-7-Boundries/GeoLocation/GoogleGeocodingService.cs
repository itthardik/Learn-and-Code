using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace GeoLocation
{
    public static class GoogleGeocodingService
    {
        private const string API_KEY = "1165d3768be1c9807038b3acc7fbfee8";

        private static string GetUrl(string place)
        {
            return $"https://maps.googleapis.com/maps/api/geocode/json?address={Uri.EscapeDataString(place)}&key={API_KEY}";
        }

        public static async Task<Coordinates> GetCoordinates(string place)
        {
            using HttpClient client = new HttpClient();
            string url = GetUrl(place);
            string response = await client.GetStringAsync(url);

            using JsonDocument json = JsonDocument.Parse(response);
            string status = json.RootElement.GetProperty("status").ToString();

            if (status != "OK")
            {
                json.RootElement.TryGetProperty("error_message", out var err);
                string errorMessage = err.GetString() ?? "Unknown error occurred.";
                throw new GeoLocationException(errorMessage);
            }

            var location = json.RootElement
                               .GetProperty("results")[0]
                               .GetProperty("geometry")
                               .GetProperty("location");

            return new Coordinates
            {
                Latitude = location.GetProperty("lat").GetDouble(),
                Longitude = location.GetProperty("lng").GetDouble()
            };
        }
    }
}
