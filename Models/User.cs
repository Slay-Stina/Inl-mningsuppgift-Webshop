namespace Assignment_Webshop.Models;

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
            Window userForm = new Window("New User", 125, 0, new List<string> {
            "First Name:",
            "Last Name:",
            "Age:",
            "Email:",
            "Address:",
            "City:",
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

            Window successWindow = new Window("User Added", new List<string> {
            $"Your username is {newUser.Username}",
            "Press any key to continue...",
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

    internal static void RemoveUser()
    {
        throw new NotImplementedException();
    }
}
