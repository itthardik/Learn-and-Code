namespace GeoLocation
{
    public static class GeoLocationApp
    {
        public static async Task GetUserResponse()
        {
            Console.WriteLine("Enter a place to get its coordinates:");
            string place = Console.ReadLine() ?? string.Empty;

            Coordinates coordinates = await GoogleGeocodingService.GetCoordinates(place);

            if (coordinates.Latitude.HasValue && coordinates.Longitude.HasValue)
            {
                Console.WriteLine($"Latitude: {coordinates.Latitude}");
                Console.WriteLine($"Longitude: {coordinates.Longitude}");
            }
            else
            {
                Console.WriteLine("Location not found.");
            }
        }
    }
}
