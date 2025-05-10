using FlipazonPortal.Helper;
using System.Linq;

namespace FlipazonPortal.Views.Pages
{
    public class HomePage(PageSharedStorage pageManager, LoginPage loginPage, SignupPage signupPage, CategoryPage categoryPage, CartPage cartPage, OrderPage orderPage) : IPage
    {
        private record MenuOption(string Key, string Title, Func<Task> Action);

        public async Task Render()
        {
            bool exit = false;

            while (!exit)
            {
                PageHelper.DisplayHeader();
                PageHelper.CenterText("Welcome to Flipazon\n");
                Console.WriteLine();

                var menuOptions = pageManager.IsAuthenticated ? GetAuthenticatedMenu() : GetGuestMenu();
                DisplayMenu(menuOptions);

                Console.WriteLine();
                PageHelper.CenterText("Choose an option: ");
                string choice = Console.ReadLine() ?? "";

                exit = await ProcessSelection(choice, menuOptions);
            }
        }

        private List<MenuOption> GetAuthenticatedMenu() =>
        [
            new("1", "Browse Products", () => categoryPage.Render()),
            new("2", "View Cart", () => cartPage.Render()),
            new("3", "View Orders", () => orderPage.Render()),
            new("4", "Logout", () => Logout()),
            new("5", "Exit", () => Exit())
        ];

        private List<MenuOption> GetGuestMenu() =>
        [
            new("1", "Login", () => loginPage.Render()),
            new("2", "Sign Up", () => signupPage.Render()),
            new("3", "Exit", () => Exit())
        ];

        private static void DisplayMenu(List<MenuOption> menuOptions)
        {
            PageHelper.CenterLines([.. menuOptions.Select(option => PageHelper.CenterAlignTwoTexts($"{option.Key}.", option.Title, 16, 20))]);
        }

        private static async Task<bool> ProcessSelection(string choice, List<MenuOption> menuOptions)
        {
            var selectedOption = menuOptions.FirstOrDefault(option => option.Key == choice);

            if (selectedOption != null)
            {
                await selectedOption.Action();
                return selectedOption.Title.Equals("exit", StringComparison.CurrentCultureIgnoreCase);
            }

            await ShowInvalidChoiceMessage();
            return false;
        }

        private async Task Logout()
        {
            Console.WriteLine();
            await PageHelper.ShowInfoToast("Logging out...");
            pageManager.IsAuthenticated = false;
        }

        private static async Task Exit()
        {
            Console.WriteLine();
            await PageHelper.ShowInfoToast("Thank you for using Flipazon");
        }

        private static async Task ShowInvalidChoiceMessage()
        {
            Console.WriteLine();
            await PageHelper.ShowErrorToast("Invalid choice. Please try again.");
        }
    }
}
