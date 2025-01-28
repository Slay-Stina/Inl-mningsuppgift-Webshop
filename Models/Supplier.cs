namespace Assignment_Webshop.Models;

internal class Supplier
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Country { get; set; }

    internal static void AddNewSupplier()
    {
        Window newSup = new Window("Ny leverantör", 75, 10, new List<string> {
            "Namn:".PadRight(20),
            "Land:".PadRight(20)
        });
        newSup.Draw();
        Console.SetCursorPosition(83, 11);
        string name = Console.ReadLine();

        Console.SetCursorPosition(83, 12);
        string country = Console.ReadLine();

        using (var db = new AdvNookContext())
        {
            db.Suppliers.Add(new Supplier { Name = name, Country = country });
            db.SaveChanges();
        }
    }

    internal static void EditSupplier()
    {
        throw new NotImplementedException();
    }

    internal static void RemoveSupplier()
    {
        throw new NotImplementedException();
    }
}
