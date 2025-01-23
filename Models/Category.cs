using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inlämningsuppgift_Webshop.Models;

internal class Category
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<Product> Products { get; set; }

    internal static void AddNew()
    {
        Window newCat = new Window("Ny Kategori", 70, 10, new List<string> { "Namn:".PadRight(20) });
        newCat.Draw();
        Console.SetCursorPosition(78, 11);
        using (var db = new AdvNookContext())
        {
            string catName = Console.ReadLine();
            if (catName is not "" && catName is not null)
            {
                db.Categories.Add(new Category { Name = catName });
                db.SaveChanges();
            }
            else
            {
                Window.Error.Draw();
            }
        }
    }
}
