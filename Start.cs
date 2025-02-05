using Assignment_Webshop.Models;
using Inlämningsuppgift_Webshop;
using Inlämningsuppgift_Webshop.Models;
using Microsoft.EntityFrameworkCore;

namespace Assignment_Webshop;

internal class Start
{
    private static List<string> _list = new List<string>
    {
        "'H'ome",
        "'C'ategories.",
        "'P'roducts.",
        "'S'earch product"
    };
    private static Window _menu = new Window("Main Menu", 25, 0, _list);
    public static List<Product> ProductList = new List<Product>();
    public static Window PageWindow = new Window(2, 10);

    public static async void Page(Task checkLogin)
    {
        Banner();
        _menu.Draw();

        if (!checkLogin.IsCompleted)
        {
            await checkLogin;
        }

        Basket.DrawBasket();

        switch (Program.ActiveSubPage)
        {
            case SubPage.Default:
                PageWindow.Header = "";
                FeaturedProducts();
                break;
            case SubPage.Categories:
                ListCategories();
                break;
            case SubPage.CategoryProducts:
                ListCategoryProducts();
                break;
            case SubPage.Search:
                SearchProducts();
                break;
            case SubPage.Products:
                ListProducts();
                break;
            case SubPage.ProductDetails:
                ProductDetails();
                break;
            case SubPage.Checkout:
                Checkout();
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
                    Program.ActiveSubPage = SubPage.CategoryProducts;
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
                case SubPage.CategoryProducts:
                    Program.ActiveSubPage = SubPage.Categories;
                    break;
                case SubPage.Products:
                    ProductList.Clear();
                    break;
            }
        }
    }

    private static void ListProducts()
    {
        PageWindow.Header = "Products - ↑↓ Navigate - → Select";

        if (!ProductList.Any())
        {
            using (var db = new AdvNookContext())
            {
                ProductList = db.Products.ToList();
            }
        }

        PageWindow.TextRows = ProductList.Select(p => $"{p.Name} - {p.Price.ToString("C")}").ToList();
        PageWindow.Navigate();
    }


    internal static void ProductDetails()
    {
        Product product;
        string productName = PageWindow.TextRows[(int)PageWindow.SelectedIndex].Split(' ')[0];
        using (var db = new AdvNookContext())
        {
            product = db.Products
                                .Include(p => p.Categories)
                                .FirstOrDefault(p => p.Name.Contains(productName));
        }
        List<string> details = new List<string>
        {
            $"Price: {product.Price:c}",
            $"Categories: {string.Join(", ", product.Categories.Select(c => c.Name))}",
            ""
        };

        details.AddRange(Methods.WrapText(product.Description, 40));

        details.AddRange("", "'Enter' to add to cart");

        Window productDetailWindow = new Window($"{product.Name}", 50, 10, details);
        productDetailWindow.Draw();
        PageWindow.Draw();

        if (Program.KeyInfo.Key == ConsoleKey.Enter)
        {
            Basket.AddProductToBasket(product.Id);
        }
    }

    private static void ListCategories()
    {
        PageWindow.Header = "Categories - ↑↓ Navigate - → Select";

        using (var db = new AdvNookContext())
        {
            var categories = db.Categories.ToList();
            PageWindow.TextRows = categories.Select(c => c.Name).ToList();
        }

        PageWindow.Navigate();
    }

    internal static void ListCategoryProducts()
    {
        using (var db = new AdvNookContext())
        {
            int listIndex = (int)PageWindow.SelectedIndex;
            string categoryName = PageWindow.TextRows[listIndex];
            var category = db.Categories.Include(c => c.Products).FirstOrDefault(c => c.Name == categoryName);

            if (category != null)
            {
                ProductList = category.Products.ToList();
                PageWindow.TextRows = ProductList.Select(p => $"{p.Name} - {p.Price.ToString("C")}").ToList();

                Program.ActiveSubPage = SubPage.Products;
                PageWindow.SelectedIndex = 0;
            }
            else
            {
                Window errorWindow = new Window("ERROR", "Category is empty.");
                errorWindow.Draw();
            }
        }
    }

    internal static void SearchProducts()
    {
        Window searchWindow = new Window("Search Products", "");
        searchWindow.Draw();

        Console.SetCursorPosition(51, 21);
        string searchTerm = Console.ReadLine();

        using (var db = new AdvNookContext())
        {
            var searchResults = db.Products
                                  .Where(p => EF.Functions.Like(p.Name, $"%{searchTerm}%") ||
                                              EF.Functions.Like(p.Description, $"%{searchTerm}%"))
                                  .ToList();

            ProductList.Clear();
            if (searchResults.Any())
            {
                ProductList.AddRange(searchResults);
            }
            else
            {
                searchWindow.TextRows = new List<string> { "No results found." };
                searchWindow.Draw();
            }
        }

        Program.ActiveSubPage = SubPage.Products;
    }

    private static void FeaturedProducts()
    {
        using (var db = new AdvNookContext())
        {
            ProductList = db.Products
                                  .Where(p => p.Featured)
                                  .Take(3)
                                  .ToList();
        }

        char[] xyz = { 'X', 'Y', 'Z' };
        for (int i = 0; i < 3; i++)
        {
            var product = ProductList[i];
            List<string> featureDetails = new List<string>
        {
            product.Name.PadRight(30),
            product.Price.ToString("C"),
        };
            featureDetails.AddRange(Methods.WrapText(product.Description, 30));

            Window featuredWindow = new Window($"'{xyz[i]}' - Add to basket", 2 + (35 * i), 10, featureDetails);
            featuredWindow.Draw();
        }

        switch (Program.KeyInfo.Key)
        {
            case ConsoleKey.X:
            case ConsoleKey.Y:
            case ConsoleKey.Z:
                int productIndex = Array.IndexOf(xyz, Char.ToUpper(Program.KeyInfo.KeyChar));
                if (productIndex >= 0 && productIndex < ProductList.Count)
                {
                    Basket.AddProductToBasket(ProductList[productIndex].Id);
                }
                break;
        }
    }
    public static async void Checkout()
    {
        Program.CloseAllDbConnections();
        OrderLogger orderLogger = new OrderLogger();
        orderLogger.Name = Login.ActiveUser != null ? $"{Login.ActiveUser.FirstName} {Login.ActiveUser.LastName}" : "Guest";

        using (var db = new AdvNookContext())
        {
            // Hämta aktuell varukorg
            Basket basket = Login.ActiveUser != null ? Login.ActiveUser.Basket : Basket.GuestBasket;

            if (basket.BasketProducts == null || basket.BasketProducts.Count == 0)
            {
                Window errorWindow = new Window("ERROR", "Your basket is empty!");
                errorWindow.Draw();
                return;
            }

            // Steg 1: Visa fraktalternativ och låt användaren välja
            List<Shipping> shippings = db.Shippings.ToList();
            Window shippingWindow = new Window("Enter shipping ID:", 25, 10, shippings
                .Select(s =>
                $"{s.Id}".PadRight(10) +
                $"{s.Type}".PadRight(20) +
                $"{s.Price:C}".PadRight(20)
                ).ToList());

            List<string> shipHeader = new List<string>
            {
                "",
                "ID".PadRight(10) + "Type".PadRight(20) + "Price".PadRight(20)
            };
            shippingWindow.TextRows.InsertRange(0, shipHeader);
            shippingWindow.Draw();

            Console.SetCursorPosition(26, 11);

            int selectedShippingId;
            if (!int.TryParse(Console.ReadLine(), out selectedShippingId) || !shippings.Any(s => s.Id == selectedShippingId))
            {
                Console.Clear();
                Window errorWindow = new Window("ERROR", "Invalid shipping ID!");
                errorWindow.Draw();
                return;
            }

            Shipping selectedShipping = shippings.First(s => s.Id == selectedShippingId);

            // Steg 2: Skapa order
            Order newOrder = new Order
            {
                Status = OrderStatus.Pending,
                UserId = Login.ActiveUser != null ? Login.ActiveUser.Id : 0, // 0 för gäster
                ShippingId = selectedShipping.Id,
                OrderDate = DateTime.Now
            };
            orderLogger.Order = newOrder;
            db.Orders.Add(newOrder);
            db.SaveChanges();

            // Steg 3: Lägg till orderdetaljer
            foreach (var basketProduct in basket.BasketProducts)
            {
                var productToUpdate = db.Products.FirstOrDefault(p => p.Id == basketProduct.ProductId);
                if (productToUpdate != null)
                {
                    OrderDetail orderDetail = new OrderDetail
                    {
                        OrderId = newOrder.Id,
                        ProductId = productToUpdate.Id,
                        Quantity = basketProduct.Quantity,
                        UnitPrice = productToUpdate.Price
                    };

                    db.OrderDetails.Add(orderDetail);

                    productToUpdate.Amount -= basketProduct.Quantity;
                    db.Update(productToUpdate);
                }
            }

            decimal totalCost = selectedShipping.Price +
                                basket.BasketProducts.Sum(bp => bp.Quantity * bp.Product.Price);
            orderLogger.TotalPrice = totalCost;

            var basketProducts = db.BasketProduct.Where(bp => bp.BasketId == basket.Id);
            db.RemoveRange(basketProducts);
            basket.BasketProducts.Clear(); //Rensa varukorgen

            db.SaveChanges();

            // Steg 4: Visa orderbekräftelse
            List<string> confirmationText = new List<string>
            {
                "Order Confirmation:",
                $"Order ID: {newOrder.Id}",
                $"Shipping: {selectedShipping.Type} ({selectedShipping.Price:C})",
                $"Total Cost: {totalCost:C}",
                "",
                "Your order has been placed successfully!",
                "Press any key to return to the main menu..."
            };

            Task.Run(() => { Connection.OrderCollection().InsertOne(orderLogger); }); //Skicka till MongoDB

            Window confirmationWindow = new Window("Order Confirmation", confirmationText);
            confirmationWindow.Draw();

            while (Console.KeyAvailable == false) { }
            Program.ActiveSubPage = SubPage.Default;
        }
    }

    private static void Banner()
    {
        List<string> banner = new List<string>
        {
            "╔═╗╔╦╗╦  ╦╔═╗╔╗╔╔╦╗╦ ╦╦═╗╔═╗  ╔╗╔╔═╗╔═╗╦╔═",
            "╠═╣ ║║╚╗╔╝║╣ ║║║ ║ ║ ║╠╦╝║╣   ║║║║ ║║ ║╠╩╗",
            "╩ ╩═╩╝ ╚╝ ╚═╝╝╚╝ ╩ ╚═╝╩╚═╚═╝  ╝╚╝╚═╝╚═╝╩ ╩"
        };
        int bannerLength = banner[0].Length;
        Window title = new Window("Welcome to", 45, 0, banner);
        title.Draw();
    }
}
