using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment_Webshop.Models;

internal class Category
{
    public int Id { get; set; }
    public string Name { get; set; }
    public virtual ICollection<Product> Products { get; set; }

    internal static void AddNewCategory()
    {
        Window newCategoryWindow = new Window("New Category", new List<string> { "Name:".PadRight(20) });
        newCategoryWindow.Draw();
        Console.SetCursorPosition(51, 21);

        using (var db = new AdvNookContext())
        {
            string categoryName = Console.ReadLine();
            if (!string.IsNullOrEmpty(categoryName))
            {
                db.Categories.Add(new Category { Name = categoryName });
                db.SaveChanges();
            }
        }
    }

    internal static void EditCategory(Window adminList, int index)
    {
        string item = adminList.TextRows[index];
        using (var db = new AdvNookContext())
        {
            var category = db.Categories.FirstOrDefault(c => c.Name == item);
            if (category != null)
            {
                Window editCatForm = new Window("Edit Category", new List<string> {
                "New Name:"
            });
                editCatForm.Draw();

                Console.SetCursorPosition(51, 21);
                string newName = Console.ReadLine();

                if (!string.IsNullOrEmpty(newName))
                {
                    category.Name = newName;
                    db.Categories.Update(category);
                    db.SaveChanges();

                    Window successWindow = new Window("Category Edited", new List<string> {
                    $"The category name has been updated to {category.Name}.",
                    "Press any key to continue..."
                });
                    successWindow.Draw();
                    while (Console.KeyAvailable == false)
                    { }
                }
            }
        }
    }

    internal static void RemoveCategory(Window _adminList, int index)
    {
        string item = _adminList.TextRows[index];
        using (var db = new AdvNookContext())
        {
            var category = db.Categories.FirstOrDefault(c => c.Name == item);
            if (category != null)
            {
                db.Categories.Remove(category);
                db.SaveChanges();

                Window successWindow = new Window("Category Removed", new List<string> {
                $"The category {category.Name} has been removed.",
                "Press any key to continue..."
            });
                successWindow.Draw();
                while (Console.KeyAvailable == false)
                { }
            }
        }
    }
}
