using FlipazonPortal.Models;

namespace FlipazonPortal.Views
{
    public class PageSharedStorage
    {
        public bool IsAuthenticated { get; set; }
        public User User { get; set; }
        public int ConsoleWidth { get; }

        public PageSharedStorage()
        {
            #if DEBUG
                IsAuthenticated = true;
                User = new User() { UserId = 20, Email = "test@test.com"};
            #else
                IsAuthenticated = false;
                User = new User();
            #endif
            ConsoleWidth = Console.WindowWidth;
        }
    }

}
