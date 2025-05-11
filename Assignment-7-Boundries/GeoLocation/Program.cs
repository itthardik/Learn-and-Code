namespace GeoLocation
{
    internal class Program
    {
        static async Task Main()
        {
            try
            {
                await GeoLocationApp.GetUserResponse();
            }
            catch (GeoLocationException ex)
            {
                Console.WriteLine($"GeoLocation Error: {ex.Message}");
            }

            Console.ReadKey();
        }
    }
}
