using Assignment_Webshop.Models;
using Microsoft.EntityFrameworkCore;

namespace Assignment_Webshop;

internal class Admin
{
    private static List<string> _list = new List<string>
    {
        "'H': Home",
        "'C': Manage categories.",
        "'P': Manage products.",
        "'U': Manage users.",
        "'S': Manage suppliers.",
        "'O': Manage orders."
    };
    private static Window _menu = new Window("Admin menu", 25, 0, _list);
    private static Window _adminList { get; set; } = new Window(2, 8);
    private static string _headerDefault = "- 'N'ew - 'E'dit - 'D'elete";
    private static readonly Dictionary<SubPage, string> _subPageHeaders = new Dictionary<SubPage, string>
    {
        { SubPage.Categories, $"Categories {_headerDefault}" },
        { SubPage.Products, $"Products {_headerDefault}" },
        { SubPage.Users, $"Users {_headerDefault}" },
        { SubPage.Suppliers, $"Suppliers {_headerDefault}" },
        { SubPage.Default, $"Welcome to the Admin Panel!" }
    };

    internal static void Page()
    {
        Banner();
        UpdateHeader();
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
            case SubPage.Checkout:
                AdminOrders();
                break;
        }
    }

    private static void AdminOrders()
    {
        List<string> orderList = new List<string>();

        using (var db = new AdvNookContext())
        {
            var orders = from o in db.Orders
                         join u in db.Users on o.UserId equals u.Id
                         join s in db.Shippings on o.ShippingId equals s.Id
                         select new
                         {
                             OrderId = o.Id,
                             OrderDate = o.OrderDate,
                             Username = u.Username,
                             OrderStatus = o.Status,
                             ShippingType = s.Type,
                         };

            // Om du vill konvertera resultatet till en lista av strängar för att visa i UI
            orderList = orders.Select(o =>
                $"{o.OrderId.ToString().PadRight(10)}" +
                $"{o.OrderDate.ToString("yyyy-MM-dd").PadRight(15)}" +
                $"{o.Username.PadRight(20)}" +
                $"{o.OrderStatus.ToString().PadRight(15)}" +
                $"{o.ShippingType.PadRight(15)}"
            ).ToList();
        }


        List<string> tableHeader = new List<string>
    {
        $"{"Order ID".PadRight(10)}{"Order Date".PadRight(15)}{"User".PadRight(20)}{"Status".PadRight(15)}{"Shipping".PadRight(15)}"
    };

        orderList.Insert(0, tableHeader[0]);

        _adminList.TextRows = orderList.Count == 0 ? new List<string> { "No orders found." } : orderList;

        _adminList.Navigate();
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
                else if (key == ConsoleKey.E)
                {
                    Category.EditCategory(_adminList, (int)_adminList.SelectedIndex);
                }
                else if (key == ConsoleKey.D)
                {
                    Category.RemoveCategory(_adminList, (int)_adminList.SelectedIndex);
                }
                break;

            case SubPage.Products:
                if (key == ConsoleKey.N)
                {
                    Product.AddNewProduct();
                }
                else if (key == ConsoleKey.E)
                {
                    Product.EditProduct(_adminList, (int)_adminList.SelectedIndex);
                }
                else if (key == ConsoleKey.D)
                {
                    Product.RemoveProduct(_adminList, (int)_adminList.SelectedIndex);
                }
                break;

            case SubPage.Users:
                if (key == ConsoleKey.N)
                {
                    User.AddUser();
                }
                else if (key == ConsoleKey.E)
                {
                    User.EditUser(_adminList, (int)_adminList.SelectedIndex);
                }
                else if (key == ConsoleKey.D)
                {
                    User.RemoveUser(_adminList, (int)_adminList.SelectedIndex);
                }
                break;

            case SubPage.Suppliers:
                if (key == ConsoleKey.N)
                {
                    Supplier.AddNewSupplier();
                }
                else if (key == ConsoleKey.E)
                {
                    Supplier.EditSupplier(_adminList, (int)_adminList.SelectedIndex);
                }
                else if (key == ConsoleKey.D)
                {
                    Supplier.RemoveSupplier(_adminList, (int)_adminList.SelectedIndex);
                }
                break;
        }
    }
    private static void UpdateHeader()
    {
        if (_subPageHeaders.TryGetValue(Program.ActiveSubPage, out string header))
        {
            _adminList.Header = header;
        }
        else
        {
            _adminList.Header = "Unknown Section"; // Fallback om en ny subpage saknas i ordboken
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
                    $"{p.Name.PadRight(35)}" +
                    $"{p.Price.ToString("C").PadRight(15)}" +
                    $"{p.Amount.ToString().PadRight(10)}" +
                    $"{(p.Featured ? "Yes" : "No").PadRight(20)}" +
                    $"{string.Join(", ", p.Categories.Select(c => c.Name))}"
                ).ToList();
        }

        List<string> tableHeader = new List<string>
        {
            $"{"Name".PadRight(35)}{"Price".PadRight(15)}{"Amount".PadRight(10)}{"Featured".PadRight(20)}{"Categories"}"
        };

        products.Insert(0, tableHeader[0]);

        _adminList.TextRows = products.Count == 0 ? new List<string> { "No products available." } : products;

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

        _adminList.Navigate();
    }

    private static void AdminUsers()
    {
        List<string> users = new List<string>();
        using (var db = new AdvNookContext())
        {
            users = db.Users
                .Select(u =>
                    $"{u.FirstName} {u.LastName}".PadRight(30) +
                    $"{u.Username.PadRight(20)}" +
                    $"{u.Email.PadRight(30)}" +
                    $"{(u.Admin ? "Yes" : "No")}".PadRight(20)
                ).ToList();
        }

        List<string> tableHeader = new List<string>
        {
            $"{"Name".PadRight(30)}{"Username".PadRight(20)}{"Email".PadRight(30)}{"Admin".PadRight(15)}"
        };

        users.Insert(0, tableHeader[0]);

        _adminList.TextRows = users.Count == 0 ? new List<string> { "No users found." } : users;

        _adminList.Navigate();
    }


    private static void AdminSupplier()
    {
        List<string> suppliers = new List<string>();

        using (var db = new AdvNookContext())
        {
            suppliers = db.Suppliers
                .Select(s => 
                    $"{s.Name.PadRight(20)}" +
                    $"{s.Country}"
            ).ToList();
        }

        _adminList.TextRows = suppliers.Count == 0 ? new List<string> { "No suppliers found." } : suppliers;

        _adminList.Navigate();
    }


    private static void Banner()
    {
        //List<string> bannerAlt = new List<string>{
        //    " █████╗ ██████╗ ███╗   ███╗██╗███╗   ██╗",
        //    "██╔══██╗██╔══██╗████╗ ████║██║████╗  ██║",
        //    "███████║██║  ██║██╔████╔██║██║██╔██╗ ██║",
        //    "██╔══██║██║  ██║██║╚██╔╝██║██║██║╚██╗██║",
        //    "██║  ██║██████╔╝██║ ╚═╝ ██║██║██║ ╚████║",
        //    "╚═╝  ╚═╝╚═════╝ ╚═╝     ╚═╝╚═╝╚═╝  ╚═══╝"
        //};
        List<string> banner = new List<string>
        {
            "╔═╗╔╦╗╔╦╗╦╔╗╔",
            "╠═╣ ║║║║║║║║║",
            "╩ ╩═╩╝╩ ╩╩╝╚╝"
        };
        int bannerLength = banner[0].Length;
        int leftPos = (Console.WindowWidth - bannerLength) / 2;
        Window title = new Window("", leftPos, 0, banner);
        title.Draw();
    }
}
