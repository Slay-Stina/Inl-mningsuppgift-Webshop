using Azure;
using Inlämningsuppgift_Webshop.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inlämningsuppgift_Webshop;

internal class Start
{
    private static List<Product> _productpage = new List<Product>();
    private static Window _pageWindow = new Window(35, 10);
    public static void Page()
    {
        Banner();
        Methods.MiniMenu();

        switch (Program.ActiveSubPage)
        {
            case SubPage.Default:
                _pageWindow.Header = "";
                for (int i = 0; i < 3; i++)
                {
                    Featured(i);
                }
                break;
            case SubPage.Categories:
                ListCategories();
                break;
            case SubPage.Products:
                ListProducts();
                break;
            case SubPage.Users:
                //Users();
                break;
        }
    }

    private static void ListProducts()
    {
        _pageWindow.Header = "Produkter";

        List<string> productList = new List<string>();
        using (var db = new AdvNookContext())
        {
            _productpage = db.Products.ToList();
            productList = _productpage.Select(p => $"{p.Name} - {p.Price}").ToList();
        }
        _pageWindow.TextRows = productList;
        _pageWindow.Draw();
    }

    private static void ListCategories()
    {
        _pageWindow.Header = "Kategorier";

        throw new NotImplementedException();
    }

    private static void Featured(int i)
    {
        char[] xyz = ['X', 'Y', 'Z'];
        List<string> features = new List<string>
        {
            "a      ",
            "b      ",
            "c      "
        };
        Window featured = new Window($"{xyz[i]}", 2 + (20 * i), 10, features);
        featured.Draw();
    }

    private static void Banner()
    {
        List<string> advNook = new List<string>{
            " █████╗ ██████╗ ██╗   ██╗███████╗███╗   ██╗████████╗██╗   ██╗██████╗ ███████╗    ███╗   ██╗ ██████╗  ██████╗ ██╗  ██╗",
            "██╔══██╗██╔══██╗██║   ██║██╔════╝████╗  ██║╚══██╔══╝██║   ██║██╔══██╗██╔════╝    ████╗  ██║██╔═══██╗██╔═══██╗██║ ██╔╝",
            "███████║██║  ██║██║   ██║█████╗  ██╔██╗ ██║   ██║   ██║   ██║██████╔╝█████╗      ██╔██╗ ██║██║   ██║██║   ██║█████╔╝ ",
            "██╔══██║██║  ██║╚██╗ ██╔╝██╔══╝  ██║╚██╗██║   ██║   ██║   ██║██╔══██╗██╔══╝      ██║╚██╗██║██║   ██║██║   ██║██╔═██╗ ",
            "██║  ██║██████╔╝ ╚████╔╝ ███████╗██║ ╚████║   ██║   ╚██████╔╝██║  ██║███████╗    ██║ ╚████║╚██████╔╝╚██████╔╝██║  ██╗",
            "╚═╝  ╚═╝╚═════╝   ╚═══╝  ╚══════╝╚═╝  ╚═══╝   ╚═╝    ╚═════╝ ╚═╝  ╚═╝╚══════╝    ╚═╝  ╚═══╝ ╚═════╝  ╚═════╝ ╚═╝  ╚═╝"
        };
        Window title = new Window("Welcome to", 2, 0, advNook);
        title.Draw();
    }
}
