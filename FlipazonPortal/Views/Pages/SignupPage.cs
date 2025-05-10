using FlipazonPortal.Helper;
using FlipazonPortal.Models;
using FlipazonPortal.Services;
using System.Net;

namespace FlipazonPortal.Views.Pages
{
    public class SignupPage : IPage
    {
        public async Task Render()
        {
            PageHelper.DisplayHeader();
            PageHelper.CenterText("Create Flipazon Account\n");
            Console.WriteLine();

            string email = PromptEmail();
            string password = await PromptPasswordWithConfirmation();

            ResponseMessage response = await AuthService.SignUp(email, password);
            await ProcessSignupResponse(response);
        }

        private static string PromptEmail()
        {
            PageHelper.CenterText("Enter Email:\n");
            PageHelper.CenterText("", -10);
            return Console.ReadLine() ?? "";
        }

        private static async Task<string> PromptPasswordWithConfirmation()
        {
            while (true)
            {
                PageHelper.CenterText("Enter Password:\n");
                PageHelper.CenterText("", -10);
                string password = PasswordHelper.ReadPassword();

                PageHelper.CenterText("Confirm Password:\n");
                PageHelper.CenterText("", -10);
                string confirmPassword = PasswordHelper.ReadPassword();

                if (password == confirmPassword)
                    return password;

                await PageHelper.ShowErrorToast("Passwords do not match. Please try again.\n");
            }
        }

        private static async Task ProcessSignupResponse(ResponseMessage response)
        {
            if (response.StatusCode == HttpStatusCode.OK)
            {
                await PageHelper.ShowSuccessToast("Registration successful! You can now log in.", 2000);
            }
            else
            {
                await PageHelper.ShowErrorToast($"Registration failed: {response.Message}", 3000);
            }
        }

        
    }
}
