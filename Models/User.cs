using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inlämningsuppgift_Webshop.Models;

internal class User
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
    public Basket Basket { get; set; } = new Basket();

    public static void AddUser()
    {
        using (var db = new AdvNookContext())
        {
            Window userForm = new Window("Ny Användare", 125,0, new List<string> {
            "Förnamn:",
            "Efternamn:",
            "Ålder:",
            "E-Post:",
            "Adress:",
            "Stad:",
            "".PadRight(20),
            });
            userForm.Draw();

            User newUser = new User();

            foreach (string row in userForm.TextRows)
            {
                int index = userForm.TextRows.IndexOf(row);
                Console.SetCursorPosition(127 + row.Length, index + 1);
                switch (index)
                {
                    case 0:
                        newUser.FirstName = Console.ReadLine();
                        break;
                    case 1:
                        newUser.LastName = Console.ReadLine();
                        break;
                    case 2:
                        newUser.Age = Methods.Checkint();
                        break;
                    case 3:
                        newUser.Email = Console.ReadLine();
                        break;
                    case 4:
                        newUser.Adress = Console.ReadLine();
                        break;
                    case 5:
                        newUser.City = Console.ReadLine();
                        break;
                }
            }
            newUser.Admin = newUser.FirstName == "August" && newUser.LastName == "Högbom" ? true : false;
            newUser.Username = newUser.FirstName.ToLower().Substring(0, 3) + newUser.LastName.ToLower().Substring(0, 3);

            db.Users.Add(newUser);
            db.SaveChanges();

            Window successWindow = new Window("Användare tillagd", new List<string> {
            $"Ditt användarnamn är {newUser.Username}",
            "Tryck på valfri knapp för att fortsätta...",
            });
            successWindow.Draw();
            while (Console.KeyAvailable == false)
            { }
            return;
        }
    }

    internal static void EditUser()
    {
        throw new NotImplementedException();
    }
}
