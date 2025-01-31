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

    internal static void EditUser(Window _adminList, int index)
    {
        string item = _adminList.TextRows[index].Split(' ', StringSplitOptions.RemoveEmptyEntries)[2];
        using (var db = new AdvNookContext())
        {
            var user = db.Users.FirstOrDefault(u => u.Username == item);
            if (user != null)
            {
                Window editUserWindow = new Window("Edit User", 125, 0, new List<string>
            {
                "First Name:".PadRight(20) + user.FirstName,
                "Last Name:".PadRight(20) + user.LastName,
                "Age:".PadRight(20) + user.Age,
                "Email:".PadRight(20) + user.Email,
                "Address:".PadRight(20) + user.Adress,
                "City:".PadRight(20) + user.City,
                "Admin (Y/N):".PadRight(20) + (user.Admin ? "Y" : "N")
            });
                editUserWindow.Draw();

                foreach (var row in editUserWindow.TextRows)
                {
                    int editIndex = editUserWindow.TextRows.IndexOf(row);
                    Console.SetCursorPosition(127 + row.Length, editIndex + 1);

                    switch (editIndex)
                    {
                        case 0: // First Name
                            string firstName = Console.ReadLine();
                            if (!string.IsNullOrEmpty(firstName)) user.FirstName = firstName;
                            break;

                        case 1: // Last Name
                            string lastName = Console.ReadLine();
                            if (!string.IsNullOrEmpty(lastName)) user.LastName = lastName;
                            break;

                        case 2: // Age
                            int newAge = Methods.Checkint();
                            if (newAge > 0) user.Age = newAge;
                            break;

                        case 3: // Email
                            string email = Console.ReadLine();
                            if (!string.IsNullOrEmpty(email)) user.Email = email;
                            break;

                        case 4: // Address
                            string address = Console.ReadLine();
                            if (!string.IsNullOrEmpty(address)) user.Adress = address;
                            break;

                        case 5: // City
                            string city = Console.ReadLine();
                            if (!string.IsNullOrEmpty(city)) user.City = city;
                            break;

                        case 6: // Admin
                            ConsoleKey adminKey = Console.ReadKey().Key;
                            user.Admin = adminKey == ConsoleKey.Y;
                            break;
                    }
                }

                db.Users.Update(user);
                db.SaveChanges();

                Window successWindow = new Window("User Updated", new List<string> {
                $"The user {user.Username} has been updated.",
                "Press any key to continue..."
            });
                successWindow.Draw();
                while (Console.KeyAvailable == false) { }
            }
        }
    }

    internal static void RemoveUser(Window _adminList, int index)
    {
        string item = _adminList.TextRows[index];
        using (var db = new AdvNookContext())
        {
            var user = db.Users.FirstOrDefault(u => u.Username == item);
            if (user != null)
            {
                db.Users.Remove(user);
                db.SaveChanges();

                Window successWindow = new Window("User Deleted", new List<string> {
                $"The user {user.Username} has been deleted.",
                "Press any key to continue..."
            });
                successWindow.Draw();
                while (Console.KeyAvailable == false) { }
            }
        }
    }
}
