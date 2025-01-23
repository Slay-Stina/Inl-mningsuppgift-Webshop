using Inlämningsuppgift_Webshop.Models;
using System.Collections.Generic;
using System.Diagnostics;

namespace Inlämningsuppgift_Webshop;

internal class Program
{
    static List<ConsoleKey> MainPageKeys { get; } = new List<ConsoleKey>
    {
        ConsoleKey.NumPad1,
        ConsoleKey.NumPad4,
        ConsoleKey.D1,
        ConsoleKey.D4
    };

    static List<ConsoleKey> AdminKeysList { get; } = new List<ConsoleKey>
    {
        ConsoleKey.K,
        ConsoleKey.P,
        ConsoleKey.U,
        ConsoleKey.N,
        ConsoleKey.M
    };

    public static MainPage ActiveMainPage = MainPage.Startpage;
    public static SubPage ActiveSubPage = SubPage.Default;

    static async Task Main(string[] args)
    {
        Task checkLogin = Methods.GetLoggedInUserAsync();

        while (true)
        {
            Console.Clear();
            switch (ActiveMainPage)
            {
                case MainPage.Startpage:
                    Start.Page();
                    break;
                case MainPage.Admin:
                    Admin.Page();
                    break;
            }
            if (!checkLogin.IsCompleted)
            {
                await checkLogin;
            }
            
            Methods.MiniMenu();
            Login.DrawLogin();

            ConsoleKeyInfo keyInfo = Console.ReadKey(true);
            if (MainPageKeys.Contains(keyInfo.Key))
            {
                ActiveMainPage = SelectMainPage(keyInfo.Key);
            }

            ActiveSubPage = SelectSubPage(keyInfo.Key);

            SelectItem(keyInfo.Key);
        }
    }

    private static SubPage SelectSubPage(ConsoleKey key)
    {
        switch (key)
        {
            case ConsoleKey.NumPad2:
            case ConsoleKey.D2:
                return SubPage.Products;

            case ConsoleKey.NumPad3:
            case ConsoleKey.D3:
                return SubPage.Categories;

            case ConsoleKey.U:
                return SubPage.Users;

            default:
                return SubPage.Default;
        }
    }

    private static MainPage SelectMainPage(ConsoleKey key)
    {
        switch (key)
        {
            case ConsoleKey.NumPad1:
            case ConsoleKey.D1:
                return MainPage.Startpage;

            case ConsoleKey.NumPad4:
            case ConsoleKey.D4:
                if (Login.ActiveUser is not null && Login.ActiveUser.Admin)
                {
                    return MainPage.Admin;
                }
                else
                {
                    return MainPage.Startpage;
                }

            default:
                return MainPage.Startpage;
        }
    }

    public static void SelectItem(ConsoleKey key)
    {
        if (LoginKeys(key) || AdminKeys(key)) return;

        switch (key)
        {
            case ConsoleKey.A:
                User.AddUser();
                break;

            //case ConsoleKey.UpArrow:
            //    if(ActiveSubPage is not SubPage.Default)
            //    {
                    
            //    }
            //    break;
            //case ConsoleKey.DownArrow:
            //    if(ActiveSubPage is not SubPage.Default)
            //    {
                    
            //    }
            //    break;
            default: break;
        }
    }

    private static bool AdminKeys(ConsoleKey key)
    {
        if (Login.ActiveUser is not null && Login.ActiveUser.Admin && AdminKeysList.Contains(key))
        {
            Admin.SelectAdminItem(key);
            return true;
        }
        return false;
    }

    private static bool LoginKeys(ConsoleKey key)
    {
        if(key == ConsoleKey.L)
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
}