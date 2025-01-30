using Assignment_Webshop.Models;
using Inlämningsuppgift_Webshop;
using System.Text;

namespace Assignment_Webshop;

internal class Program
{
    static List<ConsoleKey> MainPageKeys { get; } = new List<ConsoleKey>
    {
        ConsoleKey.H,
        ConsoleKey.A
    };
    static List<ConsoleKey> SubPageKeys { get; } = new List<ConsoleKey>
    {
        ConsoleKey.P,
        ConsoleKey.C,
        ConsoleKey.U,
        ConsoleKey.B,
        ConsoleKey.S,
        ConsoleKey.O
    };
    static List<ConsoleKey> AdminKeysList { get; } = new List<ConsoleKey>
    {
        ConsoleKey.N,
        ConsoleKey.E,
        ConsoleKey.D
    };

    public static MainPage ActiveMainPage = MainPage.Start;
    public static SubPage ActiveSubPage = SubPage.Default;
    public static ConsoleKeyInfo KeyInfo { get; set; }

    static async Task Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;
        Task checkLogin = Methods.GetLoggedInUserAsync();

        while (true)
        {
            Console.Clear();
            switch (ActiveMainPage)
            {
                case MainPage.Start:
                    Start.Page(checkLogin);
                    break;
                case MainPage.Admin:
                    if (Login.ActiveUser is not null && Login.ActiveUser.Admin)
                    {
                        Admin.Page();
                    }
                    else
                    {
                        ActiveMainPage = MainPage.Start;
                    }
                    break;
            }
            if (!checkLogin.IsCompleted)
            {
                await checkLogin;
            }

            Login.DrawLogin();

            KeyInfo = Console.ReadKey(true);
            if (MainPageKeys.Contains(KeyInfo.Key))
            {
                ActiveMainPage = SelectMainPage(KeyInfo.Key);
            }
            if (SubPageKeys.Contains(KeyInfo.Key))
            {
                ActiveSubPage = SelectSubPage(KeyInfo.Key);
            }
            SelectItem(KeyInfo.Key);
        }
    }

    private static SubPage SelectSubPage(ConsoleKey key)
    {
        switch (key)
        {
            case ConsoleKey.P:
                return SubPage.Products;

            case ConsoleKey.C:
                return SubPage.Categories;

            case ConsoleKey.U:
                return SubPage.Users;

            case ConsoleKey.S:
                if (ActiveMainPage == MainPage.Admin)
                { return SubPage.Suppliers; }
                else { return SubPage.Search; }

            case ConsoleKey.B:
                return SubPage.Basket;

            case ConsoleKey.O:
                return SubPage.Checkout;

            default:
                return SubPage.Default;
        }
    }

    private static MainPage SelectMainPage(ConsoleKey key)
    {
        switch (key)
        {
            case ConsoleKey.H:
                if (ActiveSubPage != SubPage.Default)
                    ActiveSubPage = SubPage.Default;
                return MainPage.Start;

            case ConsoleKey.A:
                if (Login.ActiveUser is not null && Login.ActiveUser.Admin)
                {
                    return MainPage.Admin;
                }
                else
                {
                    return MainPage.Start;
                }

            default:
                return MainPage.Start;
        }
    }

    public static void SelectItem(ConsoleKey key)
    {
        switch (key)
        {
            case ConsoleKey.A:
                if (Login.ActiveUser is null)
                {
                    User.AddUser();
                }
                break;

            default: break;
        }

        if (LoginKeys(key) || AdminKeys(key) || StartKeys(key)) return;
    }

    private static bool StartKeys(ConsoleKey key)
    {
        if (ActiveMainPage == MainPage.Start)
        {
            Start.SelectItem(key);
            return true;
        }
        return false;
    }

    private static bool AdminKeys(ConsoleKey key)
    {
        if (ActiveMainPage == MainPage.Admin && AdminKeysList.Contains(key))
        {
            Admin.SelectAdminItem(key);
            return true;
        }
        return false;
    }

    private static bool LoginKeys(ConsoleKey key)
    {
        if (key == ConsoleKey.L)
        {
            if (Login.ActiveUser is null)
            {
                Login.Prompt();
                return true;
            }
            if (Login.ActiveUser != null)
            {
                using (var db = new AdvNookContext())
                {
                    var activeUser = db.Users.FirstOrDefault(u => u.LoggedIn == true);
                    activeUser.LoggedIn = false;
                    db.SaveChanges();
                }
                Login.ActiveUser = null;
            }
            Login.DrawLogin();
            return true;
        }
        return false;
    }

    internal static void CloseAllDbConnections()
    {
        var db = new AdvNookContext();
        db.Dispose();
    }
}
