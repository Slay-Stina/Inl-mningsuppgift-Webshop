using Inlämningsuppgift_Webshop;
using Microsoft.EntityFrameworkCore;

namespace Assignment_Webshop.Models;

internal class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public virtual ICollection<Category> Categories { get; set; }
    public int Amount { get; set; }
    public virtual Supplier Supplier { get; set; }
    public bool Featured { get; set; }
    public virtual ICollection<BasketProduct> BasketProducts { get; set; } = new List<BasketProduct>();

    internal static void AddNewProduct()
    {
        using (var db = new AdvNookContext())
        {
            //hämtar Leverantörer------------------------------
            List<Supplier> suppliers = db.Suppliers.ToList();

            Window suppliersWindow = new Window("Enter Supplier ID", 20, 30, suppliers.Select(s => $"{s.Id}".PadRight(3) + $" {s.Name}").ToList());

            List<string> supHeader = new List<string>
            {
                "",
                "ID".PadRight(3) + " Name"
            };
            suppliersWindow.TextRows.InsertRange(0, supHeader);

            //Hämtar kategorier ---------------------------------------

            List<Category> categories = db.Categories.ToList();

            Window categoryWindow = new Window("", 20, 30, categories.Select(c => $"{c.Id}".PadRight(3) + $" {c.Name}").ToList());

            List<string> catHeader = new List<string>
            {
                "ID".PadRight(3) + " Name"
            };
            categoryWindow.TextRows.InsertRange(0, catHeader);

            Window productForm = new Window("New Product", 20, 20, new List<string> {
                "Name:",
                "Description:",
                "Category/ies (separate with ','):",
                "Supplier:",
                "Price:",
                "Stock Quantity:",
                "Featured ('Y'es/'N'o):",
                "".PadRight(100)
            });
            productForm.Draw();

            Product newProduct = new Product();

            foreach (string row in productForm.TextRows)
            {
                int index = productForm.TextRows.IndexOf(row);
                Console.SetCursorPosition(23 + row.Length, index + 21);
                switch (index)
                {
                    case 0:
                        newProduct.Name = Console.ReadLine();
                        break;
                    case 1:
                        newProduct.Description = Console.ReadLine();
                        break;
                    case 2:
                        categoryWindow.Draw();
                        Console.SetCursorPosition(23 + row.Length, index + 21);
                        string categoryInput = Console.ReadLine();
                        List<int> selectedCategoryIds = categoryInput.Split(',')
                            .Where(id => int.TryParse(id.Trim(), out _))
                            .Select(id => int.Parse(id.Trim()))
                            .ToList();
                        List<Category> selectedCategories = db.Categories.Where(c => selectedCategoryIds.Contains(c.Id)).ToList();
                        newProduct.Categories = selectedCategories;
                        break;
                    case 3:
                        suppliersWindow.Draw();
                        Console.SetCursorPosition(23 + row.Length, index + 21);
                        int selectedSupplierId;
                        if (int.TryParse(Console.ReadLine(), out selectedSupplierId))
                        {
                            newProduct.Supplier = suppliers.FirstOrDefault(s => s.Id == selectedSupplierId);
                        }
                        break;
                    case 4:
                        newProduct.Price = Methods.Checkint();
                        break;
                    case 5:
                        newProduct.Amount = Methods.Checkint();
                        break;
                    case 6:
                        newProduct.Featured = Console.ReadKey().Key == ConsoleKey.Y ? true : false;
                        break;
                }
            }

            db.Products.Add(newProduct);
            db.SaveChanges();

            Window successWindow = new Window("Product Added", new List<string> {
                $"The product {newProduct.Name} has been added.",
                "Press any key to continue..."
            });
            successWindow.Draw();
            while (Console.KeyAvailable == false) { }
        }
    }

    internal static void EditProduct(Window list, int index)
    {
        string item = list.TextRows[index];
        string itemName = item.Split(' ')[0];
        using (var db = new AdvNookContext())
        {
            var product = db.Products.Include(p => p.Categories).FirstOrDefault(p => p.Name == itemName);
            if (product != null)
            {
                Window editProductWindow = new Window($"Edit {product.Name}", 10, 20, new List<string>
            {
                "Name:".PadRight(20) + product.Name,
                "Description:".PadRight(20) + product.Description,
                "Price:".PadRight(20) + product.Price,
                "Stock Quantity:".PadRight(20) + product.Amount,
                "Featured (Y/N):".PadRight(20) + (product.Featured ? "Y" : "N"),
                "Categories (comma-separated IDs):".PadRight(20),
            });
                editProductWindow.Draw();

                foreach (var row in editProductWindow.TextRows)
                {
                    int editIndex = editProductWindow.TextRows.IndexOf(row);
                    Console.SetCursorPosition(30 + row.Length, editProductWindow.Top + 1);

                    switch (editIndex)
                    {
                        case 0:
                            string newName = Console.ReadLine();
                            if (!string.IsNullOrEmpty(newName)) product.Name = newName;
                            break;

                        case 1:
                            string newDescription = Console.ReadLine();
                            if (!string.IsNullOrEmpty(newDescription)) product.Description = newDescription;
                            break;

                        case 2:
                            if (decimal.TryParse(Console.ReadLine(), out decimal newPrice)) product.Price = newPrice;
                            break;

                        case 3:
                            product.Amount = Methods.Checkint();
                            break;

                        case 4:
                            product.Featured = Console.ReadKey().Key == ConsoleKey.Y ? true : false;
                            break;

                        case 5:
                            string categoryInput = Console.ReadLine();
                            if (!string.IsNullOrEmpty(categoryInput))
                            {
                                List<int> categoryIds = categoryInput.Split(',')
                                    .Where(id => int.TryParse(id.Trim(), out _))
                                    .Select(id => int.Parse(id.Trim()))
                                    .ToList();
                                product.Categories = db.Categories.Where(c => categoryIds.Contains(c.Id)).ToList();
                            }
                            break;
                    }
                }

                db.Products.Update(product);
                db.SaveChanges();

                Window successWindow = new Window("Product Updated", new List<string>
                { $"The product {product.Name} has been updated.", "Press any key to continue..." });
                successWindow.Draw();
                while (Console.KeyAvailable == false) { }
            }
        }
    }

    internal static void RemoveProduct(Window list, int index)
    {
        string item = list.TextRows[index];
        using (var db = new AdvNookContext())
        {
            var product = db.Products.FirstOrDefault(p => p.Name == item);
            if (product != null)
            {
                db.Products.Remove(product);
                db.SaveChanges();

                Window successWindow = new Window("Product Deleted", new List<string>
                { $"The product {product.Name} has been deleted.", "Press any key to continue..." });
                successWindow.Draw();
                while (Console.KeyAvailable == false) { }
            }
        }
    }
}