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
    public string Username { get; set; }
    public int Age { get; set; }
    public string Adress { get; set; }
    public string City { get; set; }
    public string Email { get; set; }
    public bool Admin { get; set; }
    public bool LoggedIn { get; set; } = false;
    public static List<string> NewUserForm = new List<string>
    {
        "Förnamn:", "", "",
        "Efternamn:".PadRight(27), "", "",
        "Ålder:", "", "",
        "E-post:", "", "",
        "Adress:", "", "",
        "Stad:", "", "",
        "", ""
    };

    public static void AddUser()
    {
        using (var db = new AdvNookContext())
        {
            Window userForm = new Window("Ny Användare", 125,0,NewUserForm);
            userForm.Draw();

            User newUser = new User();

            Console.SetCursorPosition(127,2);
            newUser.FirstName = Console.ReadLine();
            Console.SetCursorPosition(127, 5);
            newUser.LastName = Console.ReadLine();
            Console.SetCursorPosition(127, 8);
            newUser.Age = Methods.Checkint();
            Console.SetCursorPosition(127, 11);
            newUser.Email = Console.ReadLine();
            Console.SetCursorPosition(127, 14);
            newUser.Adress = Console.ReadLine();
            Console.SetCursorPosition(127, 17);
            newUser.City = Console.ReadLine();
            newUser.Admin = newUser.FirstName == "August" && newUser.LastName == "Högbom" ? true : false;
            newUser.Username = newUser.FirstName.ToLower().Substring(0, 3) + newUser.LastName.ToLower().Substring(0, 3);
            Console.SetCursorPosition(127, 19);
            Console.Write($"Ditt användarnamn är {newUser.Username}");

            db.Users.Add(newUser);
            db.SaveChanges();
            Methods.KeyPress();
        }
    }
}
