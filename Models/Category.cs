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
    public virtual ICollection<Product> Products { get; set; }

    internal static void AddNewCategory()
    {
        Window newCat = new Window("Ny Kategori", new List<string> { "Namn:".PadRight(20) });
        newCat.Draw();
        Console.SetCursorPosition(51, 21);
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
                Window error = new Window("ERROR","Det du angett har blivit fel.");
                error.Draw();
            }
        }
    }

    internal static void EditCategory(Window list, int index)
    {
        string item = list.TextRows[index];
        using (var db = new AdvNookContext())
        {
            var category = db.Categories.Where(c => c.Name == item).FirstOrDefault();
            if (category != null)
            {
                Window editCat = new Window("Redigera Kategori", new List<string> { "Nytt Namn:".PadRight(20) });
                editCat.Draw();
                Console.SetCursorPosition(51, 21);
                string catName = Console.ReadLine();
                if (catName is not "" && catName is not null)
                {
                    try
                    { 
                        category.Name = catName; 
                        db.Categories.Update(category); 
                    }
                    catch (Exception e) 
                    { Console.WriteLine(e.InnerException); }
                    finally
                    { db.SaveChanges(); }
                }
                else
                {
                    Window error = new Window("ERROR", "Det du angett har blivit fel.");
                    error.Draw();
                }
            }
        }
    }

    internal static void RemoveCategory(Window list, int index)
    {
        string item = list.TextRows[index];
        using (var db = new AdvNookContext())
        {
            var category = db.Categories.Where(c => c.Name == item).FirstOrDefault();
            try
            { db.Categories.Remove(category);
            }
            finally
            { db.SaveChanges(); }
        }
    }
}
