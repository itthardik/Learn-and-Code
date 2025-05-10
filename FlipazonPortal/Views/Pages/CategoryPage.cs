using FlipazonPortal.Helper;
using FlipazonPortal.Models;
using FlipazonPortal.Services;
using System.Net;

namespace FlipazonPortal.Views.Pages
{
    public class CategoryPage(ProductPage productPage) : IPage
    {
        public async Task Render()
        {
            PageHelper.DisplayHeader();
            ResponseMessage response = await ProductService.GetAllProducts();

            PageHelper.CenterText("Below is the list of all categories:\n");
            Console.WriteLine();

            int backKey = DisplayCategories(response);

            int choice = PromptCategorySelection();
            await ProcessCategorySelection(choice, backKey);
        }

        private static int DisplayCategories(ResponseMessage response)
        {
            var categories = response.Data!.AsArray();
            foreach (var product in categories)
            {
                DisplayCategoryLine(product![0]!.ToString(), product[1]!.ToString());
            }

            int backKey = categories.Count + 1;
            DisplayCategoryLine(backKey.ToString(), "Back");

            Console.WriteLine("\n");
            return backKey;
        }
        private static void DisplayCategoryLine(string key, string name, int spacing1 = 23, int spacing2 = 19)
        {
            PageHelper.CenterText(PageHelper.JoinWithSpacing([key + ".", PageHelper.JoinWithSpacing([name, " "], spacing2)], spacing1));
            PageHelper.CenterText("\n");
        }
        
        private static int PromptCategorySelection()
        {
            PageHelper.CenterText("Which category of products do you want to see? : ");
            return int.TryParse(Console.ReadLine(), out int choice) ? choice : -1;
        }

        private async Task ProcessCategorySelection(int choice, int backKey)
        {
            if (choice == backKey)
            {
                await PageHelper.ShowInfoToast("Going back to the previous menu...");
                return;
            }

            ResponseMessage response = await ProductService.GetProductsByCategoryId(choice);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                productPage.LoadStates(response.Data);
                await productPage.Render();
                return;
            }

            await PageHelper.ShowErrorToast("Invalid choice. Please try again.");
        }
    }
}
