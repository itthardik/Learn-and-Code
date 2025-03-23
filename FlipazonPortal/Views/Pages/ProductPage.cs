using FlipazonPortal.Helper;
using FlipazonPortal.Models;
using FlipazonPortal.Services;
using System.Net;
using System.Text.Json.Nodes;

namespace FlipazonPortal.Views.Pages
{
    public class ProductPage(PageSharedStorage pageManager) : IPage
    {
        private JsonNode? products;

        public void LoadStates(JsonNode? products) => this.products = products;

        public async Task Render()
        {
            PageHelper.DisplayHeader();
            PageHelper.CenterText($"Below is the list of all products from {products![1]}:\n");
            Console.WriteLine();

            int backKey = DisplayProducts();
            Console.WriteLine();
            PageHelper.CenterText("Choose product Key to add product to cart: ");

            await HandleProductSelection(backKey);
        }

        private int DisplayProducts()
        {
            var productList = products![2]!.AsArray();
            foreach (var product in productList)
            {
                PageHelper.CenterText(PageHelper.JoinWithSpacing(
                    [$"{product![0]}.", PageHelper.JoinWithSpacing([product![1]!.ToString(), $"${product![2]}"], 50)], 60));
                Console.WriteLine();
            }

            int backKey = productList.Count + 1;
            Console.WriteLine();
            PageHelper.CenterText($"{backKey}. Back to Home Page\n");
            return backKey;
        }

        private async Task HandleProductSelection(int backKey)
        {
            string choiceString = Console.ReadLine() ?? "";

            if (int.TryParse(choiceString, out int choice) && products![2]!.AsArray().Any(p => p![0]!.ToString() == choiceString))
            {
                var cartItem = new CartItemRequest { ProductId = choice, Quantity = 1, UserId = pageManager.User.UserId };
                var res = await CartService.AddOrUpdateCartItem(cartItem);

                if (res.StatusCode == HttpStatusCode.OK)
                {
                    Console.WriteLine();
                    await PageHelper.ShowSuccessToast("Item Added to Cart. Proceeding to Home Page...");
                    return;
                }
            }
            Console.WriteLine();
            await (choice == backKey
                ? PageHelper.ShowInfoToast("Proceeding to Home Page...")
                : PageHelper.ShowErrorToast("Invalid choice. Proceeding to Home Page..."));

        }
    }
}
