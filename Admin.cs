using Inlämningsuppgift_Webshop.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inlämningsuppgift_Webshop;

internal class Admin
{
    private static List<string> _list = new List<string>
    {
        "'K': Hantera kategorier.",
        "'P': Hantera produkter.",
        "'U': Hantera användare."
    };
    private static Window menu = new Window("Admin meny", 2, 10, _list);
    //private static SubPage _adminPage { get; set; } = SubPage.Default;
    internal static void Page()
    {
        Banner();
        menu.Draw();
        switch (Program.ActiveSubPage)
        {
            case SubPage.Default:
                break;
            case SubPage.Categories:
                AdminCategories();
                break;
        }
    }

    public static void SelectAdminItem(ConsoleKey choice)
    {
        switch (choice)
        {
            case ConsoleKey.K:
                Program.ActiveSubPage = SubPage.Categories;
                break;
            case ConsoleKey.P:
                break;
            case ConsoleKey.U:
                break;
            case ConsoleKey.N:
                Category.AddNew();
                Console.Clear();
                AdminCategories();
                break;
            case ConsoleKey.M:
                Product.AddNew();
                Console.Clear();
                break;
        }
    }

    private static void AdminCategories()
    {
        List<string> categories = new List<string>();
        using (var db = new AdvNookContext())
        {
            categories = db.Categories.Select(k => k.Name).ToList();
        }
        Window categoryList = new Window("'N'y - 'R'edigera", 35, 10, categories.Count == 0 ? new List<string> { "" } : categories);
        //categoryList.Navigate(keyInfo.Key);
        categoryList.Draw();
    }

    private static void Banner()
    {
        List<string> admin = new List<string>{
            " █████╗ ██████╗ ███╗   ███╗██╗███╗   ██╗",
            "██╔══██╗██╔══██╗████╗ ████║██║████╗  ██║",
            "███████║██║  ██║██╔████╔██║██║██╔██╗ ██║",
            "██╔══██║██║  ██║██║╚██╔╝██║██║██║╚██╗██║",
            "██║  ██║██████╔╝██║ ╚═╝ ██║██║██║ ╚████║",
            "╚═╝  ╚═╝╚═════╝ ╚═╝     ╚═╝╚═╝╚═╝  ╚═══╝"
        };
        Window title = new Window("", Console.WindowWidth / 4, 0, admin);
        title.Draw();
    }
}
