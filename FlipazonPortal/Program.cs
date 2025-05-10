using FlipazonPortal.Views;
using FlipazonPortal.Views.Pages;
using Microsoft.Extensions.DependencyInjection;

namespace FlipazonPortal
{
    class Program
    {
        static async Task Main()
        {
            try
            {
                var services = new ServiceCollection();

                services.AddScoped<PageSharedStorage>();

                services.AddTransient<CartPage>();
                services.AddTransient<CategoryPage>();
                services.AddTransient<HomePage>();
                services.AddTransient<LoginPage>();
                services.AddTransient<OrderPage>();
                services.AddTransient<ProductPage>();
                services.AddTransient<SignupPage>();

                var serviceProvider = services.BuildServiceProvider();

                var homePage = serviceProvider.GetRequiredService<HomePage>();
                await homePage.Render();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            }
        }
    }
}

