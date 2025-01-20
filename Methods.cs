using Inlämningsuppgift_Webshop.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inlämningsuppgift_Webshop;

internal class Methods
{
    public static void SelectItem()
    {
        ConsoleKeyInfo keyInfo = Console.ReadKey();
        char choise = keyInfo.KeyChar;
        switch(choise)
        {
            case 'l':
                if(Program.LoggedInUser is null)
                {
                    Login.Prompt();
                    return;
                }
                if(Program.LoggedInUser != null)
                {
                    using(var db = new AdvNookContext())
                    { 
                        var activeUser = db.Users.FirstOrDefault(u => u.LoggedIn == true);
                        activeUser.LoggedIn = false;
                        db.SaveChanges();
                    }
                    Program.LoggedInUser = null;
                }
                Login.DrawLogin();
                break;
            case 'a':
                User.AddUser();
                break;
            case '5':
                if(Program.LoggedInUser.Admin)
                Admin();
                break;
        }
    }
    public static int Checkint()
    {
        while (true)
        {
            if (int.TryParse(Console.ReadLine(), out int number))
            {
                return number;
            }
            else
            {
                Console.WriteLine("Du måste ange ett tal.");
            }
        }
    }

    internal static async Task GetLoggedInUserAsync()
    {
        try
        {
            using(var db = new AdvNookContext())
            { Program.LoggedInUser = await db.Users.Where(u => u.LoggedIn == true).FirstOrDefaultAsync(); }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching logged-in user: {ex.Message}");
        }
    }
    public static void KeyPress()
    {
        List<string> keys = new List<string> { "Tryck på valfri knapp för att fortsätta..." };
        Window pressKey = new Window("", 50, 20, keys);
        pressKey.Draw();
        while (Console.KeyAvailable == false)
        { }
            return;
    }
}
