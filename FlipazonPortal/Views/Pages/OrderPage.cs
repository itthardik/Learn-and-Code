using FlipazonPortal.Helper;
using FlipazonPortal.Models;
using FlipazonPortal.Services;
using System.Net;
using System.Text.Json.Nodes;

namespace FlipazonPortal.Views.Pages
{
    public class OrderPage(PageSharedStorage pageManager) : IPage
    {
        public async Task Render()
        {
            PageHelper.DisplayHeader();

            var orders = await IsValidOrderResponse(await OrderService.GetOrdersByUserId(pageManager.User.UserId));
            if (orders == null) return;

            PageHelper.CenterText("Your Orders:\n");
            Console.WriteLine();

            foreach (var order in orders)
            {
                DisplayOrderDetails(order!);
                Console.WriteLine();
                PageHelper.DrawLine(60);
                Console.WriteLine("\n");
            }

            PageHelper.CenterText("Press any key to return to the main menu...");
            Console.ReadKey();
        }

        private static async Task<JsonArray?> IsValidOrderResponse(ResponseMessage response)
        {
            if (response.StatusCode != HttpStatusCode.OK)
            {
                await PageHelper.ShowErrorToast("Failed to fetch orders or no orders found.", 1000);
                return null;
            }

            var orders = response.Data?[1]?.AsArray();
            if (orders == null || orders.Count == 0)
            {
                await PageHelper.ShowInfoToast("You have not placed any orders yet.", 1000);
                return null;
            }

            return orders;
        }

        private static void DisplayOrderDetails(JsonNode order)
        {
            var (orderId, totalAmount, orderDate) = ExtractOrderDetails(order);

            PageHelper.CenterText(PageHelper.JoinWithSpacing(["Order ID", "Total Amount", "Order Date"], 60),0,ConsoleColor.Blue);
            Console.WriteLine("");
            PageHelper.CenterText(PageHelper.JoinWithSpacing([orderId, totalAmount, orderDate], 60));
            Console.WriteLine("\n");
            PageHelper.CenterText(PageHelper.CenterAlignTwoTexts("Product Name", PageHelper.JoinWithSpacing(["Quantity", "Price"], 20), 21, 45), 0, ConsoleColor.Blue);
            Console.WriteLine("");

            foreach (var item in order![4]!.AsArray())
            {
                var (productName, quantity, price) = ExtractOrderItemDetails(item!);
                PageHelper.CenterText(PageHelper.CenterAlignTwoTexts(productName, PageHelper.JoinWithSpacing([quantity.ToString(), price], 20), 21, 45));
                Console.WriteLine();
            }
        }

        private static (string orderId, string totalAmount, string orderDate) ExtractOrderDetails(JsonNode order) =>
        (
            order![0]!.ToString(),
            $"${order[3]}",
            DateTime.Parse(order[2]!.ToString()).ToShortDateString()
        );

        private static (string productName, int quantity, string price) ExtractOrderItemDetails(JsonNode item) =>
        (
            item![6]![1]!.ToString(),
            int.Parse(item![3]!.ToString()),
            item![4]!.ToString()
        );
    }
}