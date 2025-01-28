using Inlämningsuppgift_Webshop.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Diagnostics;

namespace Inlämningsuppgift_Webshop;

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
        ConsoleKey.K,
        ConsoleKey.U,
        ConsoleKey.S
    };
    static List<ConsoleKey> AdminKeysList { get; } = new List<ConsoleKey>
    {
        ConsoleKey.N,
        ConsoleKey.R,
        ConsoleKey.T
    };

    public static MainPage ActiveMainPage = MainPage.Startpage;
    public static SubPage ActiveSubPage = SubPage.Default;
    public static ConsoleKeyInfo KeyInfo {get; set;}

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
                    if (Login.ActiveUser is not null && Login.ActiveUser.Admin)
                    {
                        Admin.Page();
                    }
                    else
                    {
                        ActiveMainPage = MainPage.Startpage;
                    }
                    break;
            }
            if (!checkLogin.IsCompleted)
            {
                await checkLogin;
            }
            
            Login.DrawLogin();
            //DeleteBasket();
            Basket.DrawBasket();

            KeyInfo = Console.ReadKey(true);
            //ConsoleKeyInfo keyInfo = Console.ReadKey(true);
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

    private static void DeleteBasket() // Ifall man behöver cleara varukorgar
    {
        using (var db = new AdvNookContext())
        {
            var b = db.Baskets.ToList();
            foreach (var item in b)
            {
                db.Baskets.Remove(item);
            }
            db.SaveChanges();
        }
    }

    private static SubPage SelectSubPage(ConsoleKey key)
    {
        switch (key)
        {
            case ConsoleKey.P:
                return SubPage.Products;

            case ConsoleKey.K:
                return SubPage.Categories;

            case ConsoleKey.U:
                return SubPage.Users;

            case ConsoleKey.S:
                if(ActiveMainPage == MainPage.Admin)
                    { return SubPage.Suppliers; }
                else { return SubPage.Default; }
            default:
                return SubPage.Default;
        }
    }

    private static MainPage SelectMainPage(ConsoleKey key)
    {
        switch (key)
        {
            case ConsoleKey.H:
                if(ActiveSubPage != SubPage.Default)
                    ActiveSubPage = SubPage.Default;
                return MainPage.Startpage;

            case ConsoleKey.A:
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

        if (LoginKeys(key) || AdminKeys(key) || StartKeys(key) ) return;
    }

    private static bool StartKeys(ConsoleKey key)
    {
        if(ActiveMainPage == MainPage.Startpage)
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