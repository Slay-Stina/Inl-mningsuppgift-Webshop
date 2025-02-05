using Assignment_Webshop.Models;
using Dapper;
using Inlämningsuppgift_Webshop;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Linq;

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
        "'O': Manage orders.",
        "'Q': Queries"
    };
    private static Window _menu = new Window("Admin menu", 25, 0, _list);
    private static Window _adminList { get; set; } = new Window(2, 9);
    private static Window _query { get; set; } = new Window(50, 9);
    private static string _headerDefault = "- 'N'ew - 'E'dit - 'D'elete";
    private static readonly Dictionary<SubPage, string> _subPageHeaders = new Dictionary<SubPage, string>
    {
        { SubPage.Categories, $"Categories {_headerDefault}" },
        { SubPage.Products, $"Products {_headerDefault}" },
        { SubPage.Users, $"Users {_headerDefault}" },
        { SubPage.Suppliers, $"Suppliers {_headerDefault}" },
        { SubPage.Checkout, "Orders - 'E'dit shipping status - 'V'iew order details" },
        {SubPage.Queries, "Useful queries - ↑↓ Navigate - → Select" },
        { SubPage.Default, "Welcome to the Admin Panel!" }
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
            case SubPage.Queries:
                QueryList();
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
                else if (key == ConsoleKey.E)
                {
                    Category.EditCategory(_adminList, _adminList.SelectedIndex.Value);
                }
                else if (key == ConsoleKey.D)
                {
                    Category.RemoveCategory(_adminList, _adminList.SelectedIndex.Value);
                }
                break;

            case SubPage.Products:
                if (key == ConsoleKey.N)
                {
                    Product.AddNewProduct();
                }
                else if (key == ConsoleKey.E)
                {
                    Product.EditProduct(_adminList, _adminList.SelectedIndex.Value);
                }
                else if (key == ConsoleKey.D)
                {
                    Product.RemoveProduct(_adminList, _adminList.SelectedIndex.Value);
                }
                break;

            case SubPage.Users:
                if (key == ConsoleKey.N)
                {
                    User.AddUser();
                }
                else if (key == ConsoleKey.E)
                {
                    User.EditUser(_adminList, _adminList.SelectedIndex.Value);
                }
                else if (key == ConsoleKey.D)
                {
                    User.RemoveUser(_adminList, _adminList.SelectedIndex.Value);
                }
                break;

            case SubPage.Suppliers:
                if (key == ConsoleKey.N)
                {
                    Supplier.AddNewSupplier();
                }
                else if (key == ConsoleKey.E)
                {
                    Supplier.EditSupplier(_adminList, _adminList.SelectedIndex.Value);
                }
                else if (key == ConsoleKey.D)
                {
                    Supplier.RemoveSupplier(_adminList, _adminList.SelectedIndex.Value);
                }
                break;
            case SubPage.Checkout:
                if ( key == ConsoleKey.V)
                {
                    ViewOrder(_adminList.SelectedIndex.Value);
                }
                if (key == ConsoleKey.E)
                {
                    EditOrder(_adminList.SelectedIndex.Value);
                }
                break;
            case SubPage.Queries:
                if (key == ConsoleKey.RightArrow)
                {
                    ShowQuery(_adminList.SelectedIndex.Value);
                }
                if (key != ConsoleKey.LeftArrow)
                {
                    Program.ActiveSubPage = SubPage.Queries;
                }
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
                             o.OrderDate,
                             u.Username,
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

    internal static void EditOrder(int selectedIndex)
    {
        int orderId = int.Parse(_adminList.TextRows[selectedIndex].Split(' ')[0]);

        using (var db = new AdvNookContext())
        {
            var order = db.Orders.FirstOrDefault(o => o.Id == orderId);
            if (order == null)
            {
                Console.WriteLine("Order not found.");
                return;
            }

            // Skapa lista över orderstatusar
            List<string> statusList = Enum.GetNames(typeof(OrderStatus))
                                          .Select((name, index) => $"{index + 1}. {name}")
                                          .ToList();

            // Rita fönstret med nuvarande status
            Window orderStatus = new Window($"Current Status: {order.Status}", statusList);
            orderStatus.Draw();

            // Låt användaren välja ny status
            int userChoice = 0;
            while (true)
            {
                Console.SetCursorPosition(50, 27);
                Console.Write("Enter your choice (1 - {0}): ", statusList.Count);
                if (int.TryParse(Console.ReadLine(), out userChoice) &&
                    userChoice > 0 &&
                    userChoice <= statusList.Count)
                {
                    break;
                }
                Console.WriteLine("Invalid input. Please try again.");
            }

            // Uppdatera statusen
            OrderStatus newStatus = (OrderStatus)(userChoice - 1);
            order.Status = newStatus;

            // Spara ändringar i databasen
            db.Orders.Update(order);
            db.SaveChanges();

            Window successWindow = new Window("SUCCESS", new List<string>
            {
                $"Order #{order.Id} status updated to {order.Status}.",
                "Press any key to return..."
            });
            successWindow.Draw();
            while (!Console.KeyAvailable) { }
        }
    }


    internal static void ViewOrder(int selectedIndex)
    {
        int orderId = int.Parse(_adminList.TextRows[selectedIndex].Split(' ')[0]);

        using (var db = new AdvNookContext())
        {
            var order = db.Orders.FirstOrDefault(o => o.Id == orderId);
            var user = db.Users.FirstOrDefault(u => u.Id == order.UserId);
            var shipping = db.Shippings.FirstOrDefault(s => s.Id == order.ShippingId);
            var orderDetails = db.OrderDetails
                .Where(od => od.OrderId == order.Id)
                .ToList();

            if (order == null || user == null || shipping == null || orderDetails.Count == 0)
            {
                Console.WriteLine("Order not found or incomplete data.");
                return;
            }

            // Bygg en lista med orderdetaljer
            List<string> orderInfo = new List<string>
        {
            $"Order ID: {order.Id}",
            $"Order Date: {order.OrderDate:yyyy-MM-dd}",
            $"Status: {order.Status}",
            $"Customer: {user.FirstName} {user.LastName}",
            $"Email: {user.Email}",
            $"Address: {user.Adress}, {user.City}",
            $"Shipping Type: {shipping.Type}",
            $"Shipping Price: {shipping.Price:C}",
            "",
            "Products:"
        };

            decimal totalPrice = shipping.Price;

            foreach (var detail in orderDetails)
            {
                var product = db.Products.FirstOrDefault(p => p.Id == detail.ProductId);
                if (product != null)
                {
                    string productInfo = $"{detail.Quantity}x {product.Name} @ {detail.UnitPrice:C} each";
                    orderInfo.Add(productInfo);
                    totalPrice += detail.Quantity * detail.UnitPrice;
                }
            }

            orderInfo.Add(new string('-', 50));
            orderInfo.Add($"Total Price: {totalPrice:C}");

            // Visa detaljerna i ett fönster
            Window orderDetailsWindow = new Window($"Order Details - Order #{order.Id}",50,15, orderInfo);
            orderDetailsWindow.Draw();

            Console.WriteLine("\nPress any key to return...");
            while (!Console.KeyAvailable) { }
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
                    $"{u.Username.PadRight(20)}" +
                    $"{u.FirstName} {u.LastName}".PadRight(30) +
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
                    $"{s.Name.PadRight(30)}" +
                    $"{s.Country.PadLeft(20)}"
            ).ToList();
        }

        List<string> tableHeader = new List<string>
        {
            $"{"Name".PadRight(30)}{"Country".PadLeft(20)}"
        };

        suppliers.Insert(0, tableHeader[0]);

        _adminList.TextRows = suppliers.Count == 0 ? new List<string> { "No suppliers found." } : suppliers;

        _adminList.Navigate();
    }

    private static void QueryList()
    {
        _adminList.TextRows = new List<string>
        {
            "Orders per user",
            "Revenue per day",
            "Most sold products",
            "Customers Favorite Products",
            "Number of orders by order status"
        };

        _adminList.Navigate();
    }
    private static void ShowQuery(int selectedIndex)
    {
        using (var db = new AdvNookContext())
        {
            switch (selectedIndex)
            {
                case 0:
                    OrderPerUser(db);
                    break;
                case 1:
                    TotalRevenue();
                    break;
                case 2:
                    TopProducts(db);
                    break;
                case 3:
                    CustomerFavoriteProducts();
                    break;
                case 4:
                    OrderByStatus(db);
                    break;
            }
        }
    }


    private static void OrderPerUser(AdvNookContext db)
    {
        _query.Header = "Orders Per User";

        _query.TextRows = db.Users
            .Select(user => new
            {
                UserId = user.Id,
                Name = user.FirstName + " " + user.LastName,
                TotalValue = db.Orders
                    .Where(o => o.UserId == user.Id)
                    .SelectMany(o => o.OrderDetails)
                    .Sum(od => od.UnitPrice * od.Quantity),
                OrderCount = db.Orders
                    .Where(o => o.UserId == user.Id).Count()
            }).OrderBy(x => x.Name)
            .Select(result =>
                $"{result.Name}".PadRight(20) +
                $"{result.TotalValue:C}".PadLeft(20) +
                $"{result.OrderCount}".PadLeft(20)
            )
            .ToList();

        string header = "User Name".PadRight(20) + "Total Value".PadLeft(20) + "Total # Orders".PadLeft(20);
        _query.TextRows.Insert(0, header);
        _query.Draw();
        while (Console.KeyAvailable == false) { }


    }
    private static void TopProducts(AdvNookContext db)
    {
        _query.Header = "Most Sold Products";

        _query.TextRows = db.OrderDetails
                        .GroupBy(od => od.Product.Name)
                        .Select(group => new
                        {
                            ProductName = group.Key,
                            TotalQuantity = group.Sum(od => od.Quantity)
                        })
                        .OrderByDescending(group => group.TotalQuantity)
                        .Select(group =>
                            $"{group.ProductName}".PadRight(35) +
                            $"{group.TotalQuantity}".PadLeft(10)
                        )
                        .ToList();

        string header = "Product Name".PadRight(35) + "Quantity Sold".PadLeft(10);
        _query.TextRows.Insert(0, header);
        _query.Draw();
        while (Console.KeyAvailable == false) { }

    }

    private static void OrderByStatus(AdvNookContext db)
    {
        _query.Header = "Orders by Status";

        var ordersByStatus = db.Orders
                            .GroupBy(o => o.Status)
                            .Select(group =>
                                $"{group.Key}".PadRight(20) +
                                $"{group.Count()}".PadLeft(20)
                            )
                            .ToList();

        string header = "Orderstatus".PadRight(20) + "Amount".PadLeft(20);
        _query.TextRows.Insert(0, header);
        _query.TextRows = ordersByStatus;
        _query.Draw();
        while (Console.KeyAvailable == false) { }
    }
    private static void CustomerFavoriteProducts()
    {
        _query.Header = "Customers' Favorite Products";

        using (var connection = new SqlConnection(Connection.StringAzure))
        {
            connection.Open();

            string sql = @"
            SELECT 
                CONCAT(u.FirstName, ' ', u.LastName) AS FullName,
                p.Name AS ProductName,
                SUM(od.Quantity) AS TotalQuantity
            FROM Users u
            INNER JOIN Orders o ON u.Id = o.UserId
            INNER JOIN OrderDetails od ON o.Id = od.OrderId
            INNER JOIN Products p ON od.ProductId = p.Id
            GROUP BY u.FirstName, u.LastName, p.Name, u.Id
            HAVING SUM(od.Quantity) = (
                SELECT TOP 1 SUM(od2.Quantity)
                FROM OrderDetails od2
                INNER JOIN Orders o2 ON od2.OrderId = o2.Id
                WHERE o2.UserId = u.Id
                GROUP BY od2.ProductId
                ORDER BY SUM(od2.Quantity) DESC
            )
            ORDER BY FullName";

            var favoriteProducts = connection.Query<(string FullName, string ProductName, int TotalQuantity)>(sql)
                .Select(entry =>
                    $"{entry.FullName}".PadRight(30) +
                    $"{entry.ProductName}".PadRight(30) +
                    $"{entry.TotalQuantity}".PadLeft(10)
                )
                .ToList();

            string header = "Customer Name".PadRight(30) + "Favorite Product".PadRight(30) + "Quantity".PadLeft(10);
            favoriteProducts.Insert(0, header);

            _query.TextRows = favoriteProducts;
            _query.Draw();
            while (Console.KeyAvailable == false) { }
        }
    }

    private static void TotalRevenue()
    {
        _query.Header = "Revenue per day";

        using (var connection = new SqlConnection(Connection.StringAzure))
        {
            connection.Open();

            string sql = @"
            SELECT 
                CAST(OrderDate as date) as OrderDate,
                SUM(od.Quantity * od.UnitPrice) as TotalRevenue
            FROM OrderDetails od
            INNER JOIN Orders o ON o.Id = od.OrderId
            group by CAST(OrderDate as date)
            ORDER BY OrderDate DESC";

            _query.TextRows = connection.Query<(DateTime SalesDate, decimal Revenue)>(sql)
                .Select(entry =>
                $"{entry.SalesDate.ToShortDateString()}".PadRight(20) +
                $"{entry.Revenue:C}".PadLeft(10)
                ).ToList();

            string header = "Order Date".PadRight(20) + "Revenue".PadLeft(10);
            _query.TextRows.Insert(0, header);
            _query.Draw();
            while (Console.KeyAvailable == false) { }
        }
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
