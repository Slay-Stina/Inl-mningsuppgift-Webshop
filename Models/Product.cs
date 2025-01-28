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
                // Error message for invalid selection
                Window errorWindow = new Window("ERROR", "Invalid supplier number!");
                errorWindow.Draw();
                return;
            }

            Supplier selectedSupplier = suppliers.Where(s => s.Id == selectedSupplierId).FirstOrDefault();

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

            // Add the product to the database
            db.Products.Add(newProduct);
            db.SaveChanges();

            // Success window
            Window successWindow = new Window("Product Added", new List<string> {
            $"The product {newProduct.Name} has been added.",
            "Press any key to continue..."
        });
            successWindow.Draw();
            while (Console.KeyAvailable == false)
            { }
        }
    }

    internal static void EditProduct()
    {
        throw new NotImplementedException();
    }

    internal static void RemoveProduct()
    {
        throw new NotImplementedException();
    }
}
