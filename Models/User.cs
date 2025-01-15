using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inlämningsuppgift_Webshop.Models;

public class User
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public int Age { get; set; }
    public string Adress { get; set; }
    public string City { get; set; }
    public string Email { get; set; }
    public bool Admin { get; set; }
    public bool LoggedIn { get; set; } = false;

    public static void AddUser()
    {
        using (var db = new AdvNookContext())
        {
            User newUser = new User();
            Console.Write("Förnamn: ");
            newUser.FirstName = Console.ReadLine();
            Console.Write("Efternamn: ");
            newUser.LastName = Console.ReadLine();
            Console.Write("Ålder: ");
            newUser.Age = Methods.Checkint();
            Console.Write("E-post: ");
            newUser.Email = Console.ReadLine();
            Console.Write("Adress: ");
            newUser.Adress = Console.ReadLine();
            Console.Write("Stad: ");
            newUser.City = Console.ReadLine();
            newUser.Admin = newUser.FirstName == "August" && newUser.LastName == "Högbom" ? true : false;

            db.Users.Add(newUser);
            db.SaveChanges();
        }
    }
}
