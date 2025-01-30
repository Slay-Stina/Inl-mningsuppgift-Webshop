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
    public virtual ICollection<Basket>? Baskets { get; set; }
    public virtual Supplier Supplier { get; set; }
    public bool Featured { get; set; }

    internal static void AddNewProduct()
    {
        using (var db = new AdvNookContext())
        {
            List<Supplier> suppliers = db.Suppliers.ToList();

            Window suppliersWindow = new Window("Select Supplier", suppliers.Select(s => $"{s.Id} {s.Name}").ToList());
            suppliersWindow.Draw();

            Console.SetCursorPosition(48, 18);
            Console.WriteLine("Enter the supplier's number:");
            int selectedSupplierId;
            Console.SetCursorPosition(48, 19);

            if (!int.TryParse(Console.ReadLine(), out selectedSupplierId) || !suppliers.Select(s => s.Id).Contains(selectedSupplierId))
            {
                Window errorWindow = new Window("ERROR", "Invalid supplier number!");
                errorWindow.Draw();
                return;
            }

            Supplier selectedSupplier = suppliers.FirstOrDefault(s => s.Id == selectedSupplierId);

            Window productForm = new Window("New Product", 125, 0, new List<string> {
                "Name:",
                "Description:",
                "Category/ies (separate with ','):",
                "Price:",
                "Stock Quantity:",
                "Featured ('Y'es/'N'o):"
            });
            productForm.Draw();

            Product newProduct = new Product
            {
                Supplier = selectedSupplier
            };

            foreach (string row in productForm.TextRows)
            {
                int index = productForm.TextRows.IndexOf(row);
                Console.SetCursorPosition(127 + row.Length, index + 1);
                switch (index)
                {
                    case 0:
                        newProduct.Name = Console.ReadLine();
                        break;
                    case 1:
                        newProduct.Description = Console.ReadLine();
                        break;
                    case 2:
                        string categoryInput = Console.ReadLine();
                        List<int> selectedCategoryIds = categoryInput.Split(',')
                            .Where(id => int.TryParse(id.Trim(), out _))
                            .Select(id => int.Parse(id.Trim()))
                            .ToList();
                        List<Category> selectedCategories = db.Categories.Where(c => selectedCategoryIds.Contains(c.Id)).ToList();
                        newProduct.Categories = selectedCategories;
                        break;
                    case 3:
                        newProduct.Price = Methods.Checkint();
                        break;
                    case 4:
                        newProduct.Amount = Methods.Checkint();
                        break;
                    case 5:
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
                Window editProductWindow = new Window($"Edit {product.Name}", new List<string>
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