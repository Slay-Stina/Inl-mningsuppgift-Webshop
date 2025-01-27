using Inlämningsuppgift_Webshop.Models;
using Microsoft.EntityFrameworkCore;

namespace Inlämningsuppgift_Webshop;

internal class Admin
{
    private static List<string> _list = new List<string>
    {
        "'H': Hem",
        "'K': Hantera kategorier.",
        "'P': Hantera produkter.",
        "'U': Hantera användare.",
        "'S': Hantera leverantörer."
    };
    private static Window _menu = new Window("Admin meny", 2, 0, _list);
    private static Window _adminList { get; set; } = new Window( 2, 10);

    internal static void Page()
    {
        Banner();
        _menu.Draw();
        switch (Program.ActiveSubPage)
        {
            case SubPage.Default:
                break;
            case SubPage.Categories:
                AdminCategories();
                break;
            case SubPage.Products:
                AdminProducts();
                break;
            case SubPage.Users:
                AdminUsers();
                break;
            case SubPage.Suppliers:
                AdminSupplier();
                break;
        }
    }

    public static void SelectAdminItem(ConsoleKey key)
    {
        switch (Program.ActiveSubPage)
        {
            case SubPage.Categories:
                if (key == ConsoleKey.N)
                {
                    Category.AddNewCategory();
                }
                else if (key == ConsoleKey.R)
                {
                    Category.EditCategory();
                }
                else
                {
                    Console.WriteLine("Ogiltigt val. Tryck 'N' för att lägga till eller 'R' för att redigera.");
                }
                break;

            case SubPage.Products:
                if (key == ConsoleKey.N)
                {
                    Product.AddNewProduct();
                }
                else if (key == ConsoleKey.R)
                {
                    Product.EditProduct();
                }
                else
                {
                    Console.WriteLine("Ogiltigt val. Tryck 'N' för att lägga till eller 'R' för att redigera.");
                }
                break;

            case SubPage.Users:
                if (key == ConsoleKey.N)
                {
                    User.AddUser();
                }
                else if (key == ConsoleKey.R)
                {
                    User.EditUser();
                }
                else
                {
                    Console.WriteLine("Ogiltigt val. Tryck 'N' för att lägga till eller 'R' för att redigera.");
                }
                break;

            case SubPage.Suppliers:
                if (key == ConsoleKey.N)
                {
                    Supplier.AddNewSupplier();
                }
                else if (key == ConsoleKey.R)
                {
                    Supplier.EditSupplier();
                }
                else
                {
                    Console.WriteLine("Ogiltigt val. Tryck 'N' för att lägga till eller 'R' för att redigera.");
                }
                break;

            default:
                Console.WriteLine("Ingen giltig sida aktiv.");
                break;
        }
    }

    private static void AdminProducts()
    {
        List<string> products = new List<string>();

        using (var db = new AdvNookContext())
        {
            products = db.Products
                .Include(p => p.Categories)
                .Select(p =>
                    $"{p.Name.PadRight(20)}" +
                    $"{p.Price.ToString("C").PadRight(20)}" +
                    $"{p.Amount}".PadRight(20) +
                    $"Featured: {(p.Featured ? "Ja" : "Nej")}".PadRight(20) +
                    $"{string.Join(", ", p.Categories.Select(c => c.Name))}"
                ).ToList();
        }
        _adminList.TextRows = products.Count == 0 ? new List<string> { "" } : products;
        _adminList.Header = $"Produkter - 'N'y - 'R'edigera";

        _adminList.Navigate();
    }

    private static void AdminCategories()
    {
        List<string> categories = new List<string>();
        using (var db = new AdvNookContext())
        {
            categories = db.Categories.Select(k => k.Name).ToList();
        }
        _adminList.TextRows = categories.Count == 0 ? new List<string> { "" } : categories;
        _adminList.Header = $"Kategorier - 'N'y - 'R'edigera";

        _adminList.Navigate();
    }

    private static void AdminUsers()
    {
        throw new NotImplementedException();
    }

    private static void AdminSupplier()
    {
        List<string> suppliers = new List<string>();
        using (var db = new AdvNookContext())
        {
            suppliers = db.Suppliers.Select(s => s.Name).ToList();
        }
        Window supplierList = new Window("Leverantörer - 'N'y - 'R'edigera", 35, 10, suppliers.Count == 0 ? new List<string> { "" } : suppliers);
        //categoryList.Navigate(keyInfo.Key);
        supplierList.Draw();
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
