using FlipazonPortal.Helper;
using FlipazonPortal.Models;
using FlipazonPortal.Services;
using System.Net;
using System.Text.Json.Nodes;

namespace FlipazonPortal.Views.Pages
{
    public class CartPage(PageSharedStorage pageManager) : IPage
    {
        public async Task Render()
        {
            PageHelper.DisplayHeader();

            var cartItems = await GetValidCartItems();
            if (cartItems == null) return;

            DisplayCartItems(cartItems, out decimal totalAmount, out var orderItems);
            PageHelper.CenterText($"Total Amount: ${totalAmount}\n");

            if (!ConfirmOrder())
            {
                await PageHelper.ShowInfoToast("Returning to the main menu...", 1000);
                return;
            }

            await PlaceOrder(orderItems);
        }

        private async Task<JsonArray?> GetValidCartItems()
        {
            var response = await CartService.GetCartItemsByUserId(pageManager.User.UserId);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                await PageHelper.ShowErrorToast("Error retrieving cart items. Please try again later.");
                return null;
            }

            var cartItems = response.Data?.AsArray();
            if (cartItems == null || cartItems.Count == 0)
            {
                await PageHelper.ShowInfoToast("Your cart is empty. Add some products to proceed!");
                return null;
            }

            return cartItems;
        }

        private static void DisplayCartItems(JsonArray cartItems, out decimal totalAmount, out List<OrderItemDto> orderItems)
        {
            PageHelper.CenterText("Your Cart Items:\n");
            Console.WriteLine();
            PageHelper.CenterText(PageHelper.JoinWithSpacing([PageHelper.JoinWithSpacing(["Product", "Quantity"], 50), "Price"], 60));
            Console.WriteLine("\n");

            totalAmount = 0;
            orderItems = [];
            
            foreach (var item in cartItems)
            {
                var (productId, productName, quantity, price, totalPrice) = ExtractCartItemDetails(item!);
                totalAmount += totalPrice;

                orderItems.Add(new OrderItemDto { ProductId = productId, Quantity = quantity, Price = price });
                PageHelper.CenterText(PageHelper.JoinWithSpacing([PageHelper.JoinWithSpacing([productName, quantity.ToString()], 50), $"${totalPrice}"], 60));
                Console.WriteLine("\n");
            }
        }

        private static (int productId, string productName, int quantity, decimal price, decimal totalPrice) ExtractCartItemDetails(JsonNode item)
        {
            int productId = int.Parse(item![5]![0]!.ToString());
            string productName = item![5]![1]!.ToString();
            int quantity = int.Parse(item[3]!.ToString());
            decimal price = decimal.Parse(item![5]![2]!.ToString());
            decimal totalPrice = price * quantity;

            return (productId, productName, quantity, price, totalPrice);
        }

        private static bool ConfirmOrder()
        {
            Console.WriteLine();
            PageHelper.CenterText("Do you want to place an order? (Y/N): ");
            Console.WriteLine();
            return char.ToUpper(Console.ReadKey().KeyChar) == 'Y';
        }

        private async Task PlaceOrder(List<OrderItemDto> orderItems)
        {
            Console.WriteLine("\n");
            PageHelper.CenterText("Placing your order...");
            var response = await OrderService.PlaceOrder(pageManager.User.UserId, orderItems);

            if (response.StatusCode == HttpStatusCode.OK)
                await PageHelper.ShowSuccessToast("Order placed successfully!");
            else
                await PageHelper.ShowErrorToast("Failed to place the order. Try again.");
        }
    }
}
