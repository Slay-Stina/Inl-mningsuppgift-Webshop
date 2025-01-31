namespace Assignment_Webshop.Models;

internal class Supplier
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Country { get; set; }

    internal static void AddNewSupplier()
    {
        using (var db = new AdvNookContext())
        {
            Window newSupplierForm = new Window("New Supplier", 75, 10, new List<string> {
            "Name:",
            "Country:"
        });
            newSupplierForm.Draw();

            Supplier newSupplier = new Supplier();

            foreach (string row in newSupplierForm.TextRows)
            {
                int index = newSupplierForm.TextRows.IndexOf(row);
                Console.SetCursorPosition(83, index + 11);
                switch (index)
                {
                    case 0:
                        newSupplier.Name = Console.ReadLine();
                        break;
                    case 1:
                        newSupplier.Country = Console.ReadLine();
                        break;
                }
            }

            db.Suppliers.Add(newSupplier);
            db.SaveChanges();

            Window successWindow = new Window("Supplier Added", new List<string> {
            $"The supplier {newSupplier.Name} has been added.",
            "Press any key to continue..."
        });
            successWindow.Draw();
            while (Console.KeyAvailable == false)
            { }
        }
    }

    internal static void EditSupplier(Window _adminList, int index)
    {
        string item = _adminList.TextRows[index].Split(' ')[0];
        using (var db = new AdvNookContext())
        {
            var supplier = db.Suppliers.FirstOrDefault(s => s.Name.Contains(item));
            if (supplier != null)
            {
                Window editSupplierForm = new Window("Edit Supplier", new List<string> {
                "Name:".PadRight(20) + supplier.Name,
                "Country:".PadRight(20) + supplier.Country
            });
                editSupplierForm.Draw();

                foreach (string row in editSupplierForm.TextRows)
                {
                    int editIndex = editSupplierForm.TextRows.IndexOf(row);
                    Console.SetCursorPosition(row.Length + 20, editIndex + 21);

                    switch (editIndex)
                    {
                        case 0:
                            string name = Console.ReadLine();
                            if (!string.IsNullOrEmpty(name))
                                supplier.Name = name;
                            break;

                        case 1:
                            string country = Console.ReadLine();
                            if (!string.IsNullOrEmpty(country))
                                supplier.Country = country;
                            break;
                    }
                }

                db.Suppliers.Update(supplier);
                db.SaveChanges();

                Window successWindow = new Window("Supplier Updated", new List<string> {
                $"The supplier {supplier.Name} has been updated.",
                "Press any key to continue..."
            });
                successWindow.Draw();
                while (Console.KeyAvailable == false)
                { }
            }
        }
    }


    internal static void RemoveSupplier(Window _adminList, int index)
    {
        string item = _adminList.TextRows[index];
        using (var db = new AdvNookContext())
        {
            var supplier = db.Suppliers.FirstOrDefault(s => s.Name == item);
            if (supplier != null)
            {
                db.Suppliers.Remove(supplier);
                db.SaveChanges();

                Window successWindow = new Window("Supplier Removed", new List<string> {
                $"The supplier {supplier.Name} has been removed.",
                "Press any key to continue..."
            });
                successWindow.Draw();
                while (Console.KeyAvailable == false)
                { }
            }
        }
    }

}
