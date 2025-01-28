using Assignment_Webshop.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Assignment_Webshop;

internal class Start
{
    private static List<string> _list = new List<string>
    {
        "'H'ome",
        "'C'ategories.",
        "'P'roducts."
    };
    private static Window _menu = new Window("Main Menu", 25, 0, _list);
    public static List<Product> ProductList = new List<Product>();
    public static Window PageWindow = new Window(2, 10);
    public static void Page()
    {
        Banner();
        _menu.Draw();

        switch (Program.ActiveSubPage)
        {
            case SubPage.Default:
                PageWindow.Header = "";
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
            case SubPage.ProductDetails:
                ProductDetails();
                break;
            case SubPage.Users:
                //Users();
                break;
        }
    }
    public static void SelectItem(ConsoleKey key)
    {
        if (key == ConsoleKey.RightArrow)
        {
            switch (Program.ActiveSubPage)
            {
                case SubPage.Categories:
                    //ListCatProd();
                    break;
                case SubPage.Products:
                    Program.ActiveSubPage = SubPage.ProductDetails;
                    break;
            }
        }
        if (key == ConsoleKey.LeftArrow)
        {
            switch (Program.ActiveSubPage)
            {
                case SubPage.ProductDetails:
                    Program.ActiveSubPage = SubPage.Products;
                    break;
            }
        }
    }

    private static void ListProducts()
    {
        PageWindow.Header = "Products - ↑↓ Navigate - → Select";

        List<string> productList = new List<string>();
        using (var db = new AdvNookContext())
        {
            ProductList = db.Products.ToList();
            productList = ProductList.Select(p => $"{p.Name} - {p.Price}").ToList();
        }
        PageWindow.TextRows = productList;
        PageWindow.Navigate();
    }
    internal static void ProductDetails()
    {
        using (var db = new AdvNookContext())
        {
            string selectedRow = PageWindow.TextRows[(int)PageWindow.SelectedIndex];
            Product product = db.Products.Include(p => p.Categories).FirstOrDefault(p => selectedRow.Contains(p.Name));

            List<string> details = new List<string>
        {
            product.Description,
            product.Price.ToString("C"),
            $"{string.Join(", ", product.Categories.Select(c => c.Name))}",
            "",
            "'Enter' to add to cart"
        };
            Window productDetailWindow = new Window($"{product.Name}", 50, 10, details);
            productDetailWindow.Draw();
            Start.PageWindow.Draw();

            if (Program.KeyInfo.Key == ConsoleKey.Enter)
            {
                AddToBasket(product, db);
            }
        }
    }
    internal static void AddToBasket(Product? product, AdvNookContext db)
    {
        if (product.Amount <= 0)
        {
            Window noStockWindow = new Window("Stock Error", $"{product.Name} is out of stock and could not be added.");
            noStockWindow.Draw();
            return;
        }

        var dbProduct = db.Products.FirstOrDefault(p => p.Id == product.Id);

        if (Login.ActiveUser != null)
        {
            var userBasket = db.Baskets.Include(b => b.Products).FirstOrDefault(b => b.Id == Login.ActiveUser.Basket.Id);
            if (userBasket != null)
            {
                userBasket.Products.Add(dbProduct);
            }
        }
        else
        {
            Basket.GuestBasket.Products.Add(dbProduct);
        }

        dbProduct.Amount--;
        db.SaveChanges();
    }

    private static void ListCategories()
    {
        PageWindow.Header = "Categories";

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
        List<string> bannerAlt = new List<string>{
            " █████╗ ██████╗ ██╗   ██╗███████╗███╗   ██╗████████╗██╗   ██╗██████╗ ███████╗    ███╗   ██╗ ██████╗  ██████╗ ██╗  ██╗",
            "██╔══██╗██╔══██╗██║   ██║██╔════╝████╗  ██║╚══██╔══╝██║   ██║██╔══██╗██╔════╝    ████╗  ██║██╔═══██╗██╔═══██╗██║ ██╔╝",
            "███████║██║  ██║██║   ██║█████╗  ██╔██╗ ██║   ██║   ██║   ██║██████╔╝█████╗      ██╔██╗ ██║██║   ██║██║   ██║█████╔╝ ",
            "██╔══██║██║  ██║╚██╗ ██╔╝██╔══╝  ██║╚██╗██║   ██║   ██║   ██║██╔══██╗██╔══╝      ██║╚██╗██║██║   ██║██║   ██║██╔═██╗ ",
            "██║  ██║██████╔╝ ╚████╔╝ ███████╗██║ ╚████║   ██║   ╚██████╔╝██║  ██║███████╗    ██║ ╚████║╚██████╔╝╚██████╔╝██║  ██╗",
            "╚═╝  ╚═╝╚═════╝   ╚═══╝  ╚══════╝╚═╝  ╚═══╝   ╚═╝    ╚═════╝ ╚═╝  ╚═╝╚══════╝    ╚═╝  ╚═══╝ ╚═════╝  ╚═════╝ ╚═╝  ╚═╝"
        };
        List<string> banner = new List<string>
        {
            "╔═╗╔╦╗╦  ╦╔═╗╔╗╔╔╦╗╦ ╦╦═╗╔═╗  ╔╗╔╔═╗╔═╗╦╔═",
            "╠═╣ ║║╚╗╔╝║╣ ║║║ ║ ║ ║╠╦╝║╣   ║║║║ ║║ ║╠╩╗",
            "╩ ╩═╩╝ ╚╝ ╚═╝╝╚╝ ╩ ╚═╝╩╚═╚═╝  ╝╚╝╚═╝╚═╝╩ ╩"
        };
        int bannerLength = banner[0].Length;
        int leftPos = (Console.WindowWidth - bannerLength) / 2;
        Window title = new Window("", leftPos, 0, banner);
        title.Draw();
    }
}