using FlipazonPortal.Helper;
using FlipazonPortal.Models;
using FlipazonPortal.Services;
using System.Net;

namespace FlipazonPortal.Views.Pages
{
    public class LoginPage(PageSharedStorage pageManager) : IPage
    {
        public async Task Render()
        {
            PageHelper.DisplayHeader();
            PageHelper.CenterText("Login to your Flipazon Account\n");
            Console.WriteLine();

            var (email, password) = PromptUserCredentials();

            Console.WriteLine();
            ResponseMessage response = await AuthService.Login(email, password);

            await ProcessLoginResponse(response);
        }

        private static (string Email, string Password) PromptUserCredentials()
        {
            PageHelper.CenterText("Enter Email: ");
            string email = Console.ReadLine() ?? "";

            PageHelper.CenterText("Enter Password: ");
            string password = PasswordHelper.ReadPassword();

            return (email, password);
        }
        private async Task ProcessLoginResponse(ResponseMessage response)
        {
            if (response.StatusCode == HttpStatusCode.OK)
            {
                pageManager.User.UserId = int.TryParse(response!.Data![1]![0]!.ToString(), out int userID) ? userID : 0;
                pageManager.User.Email = response!.Data![1]![1]!.ToString();
                pageManager.IsAuthenticated = true;
                await PageHelper.ShowSuccessToast("Login successful! Proceeding to Home Page...", 2000);
            }
            else
            {
                pageManager.IsAuthenticated = false;
                await PageHelper.ShowErrorToast($"Login failed: {response.Message}", 3000);
            }
        }
    }
}
