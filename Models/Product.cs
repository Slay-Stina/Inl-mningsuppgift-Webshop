using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inlämningsuppgift_Webshop.Models;

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
        // Hämta alla leverantörer från databasen
        using (var db = new AdvNookContext())
        {
        List<Supplier> suppliers;
        suppliers = db.Suppliers.ToList();
        
        // Visa leverantörerna så användaren kan välja
        Window suppliersWindow = new Window("Välj leverantör", suppliers.Select(s => $"{s.Id} {s.Name}").ToList());
        suppliersWindow.Draw();

        // Låt användaren välja en leverantör
        Console.SetCursorPosition(48, 18);
        Console.WriteLine("Ange leverantörens nummer:");
        int selectedSupplierId;
        Console.SetCursorPosition(48, 19);

        // Läs in användarens val
        if (!int.TryParse(Console.ReadLine(), out selectedSupplierId) || !suppliers.Select(s => s.Id).Contains(selectedSupplierId))
        {
            // Felmeddelande om ogiltigt val
            Window errorWindow = new Window("ERROR", "Ogiltigt leverantörsnummer!");
            errorWindow.Draw();
            return;
        }

        Supplier selectedSupplier = suppliers.Where(s => s.Id == selectedSupplierId).FirstOrDefault();

        // Skapa ett formulär för att lägga till produkten
        Window newProductWindow = new Window("Ny Produkt",  new List<string> {
        "Namn:",
        "Beskrivning:",
        "Kategori/er:",
        "Pris:",
        "Lagerantal:",
        "Featured ('J'a/'N'ej):".PadRight(50)
    });

        newProductWindow.Draw();

        // Läs in produktinformation
        Console.SetCursorPosition(57, 21);
        string name = Console.ReadLine();

        Console.SetCursorPosition(64, 22);
        string description = Console.ReadLine();

        // Hämta alla kategorier från databasen
        List<Category> categories;

        categories = db.Categories.ToList();

        // Visa kategorierna så användaren kan välja
        Window categoriesWindow = new Window("Välj kategorier (separera med ',')",50,29, categories.Select(c => $"{c.Id} {c.Name}").ToList());
        categoriesWindow.Draw();

        // Låt användaren välja kategorier
        Console.SetCursorPosition(64, 23);
        string categoryInput = Console.ReadLine();

        // Tolka användarens val
        List<int> selectedCategoryIds = categoryInput.Split(',')
            .Where(id => int.TryParse(id.Trim(), out _))
            .Select(id => int.Parse(id.Trim()))
            .ToList();

        List<Category> selectedCategories = categories.Where(c => selectedCategoryIds.Contains(c.Id)).ToList();

        Console.SetCursorPosition(57, 24);
        decimal price = Methods.Checkint();

        Console.SetCursorPosition(63, 25);
        int amount = Methods.Checkint();

        Console.SetCursorPosition(74, 26);
        bool featured = Console.ReadKey().Key == ConsoleKey.J ? true : false;

        // Skapa den nya produkten

            Product newProduct = new Product
            {
                Name = name,
                Description = description,
                Price = price,
                Amount = amount,
                Featured = featured,
                Supplier = selectedSupplier,
                Categories = selectedCategories
            };

            // Lägg till produkten i databasen
            db.Products.Add(newProduct);
            db.SaveChanges();

            // Framgångsmeddelande
            Window successWindow = new Window("SUCCESS", "Produkten har lagts till!");
            successWindow.Draw();
        }
    }

    internal static void EditProduct()
    {
        throw new NotImplementedException();
    }
}
