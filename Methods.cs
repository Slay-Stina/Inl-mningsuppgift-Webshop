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
                Console.Clear();
                Console.WriteLine("TESTTESTETSTETETTETSTTETETTSTETTSTETSTTSTETETSTETSTETSTETS");
                break;
            case 'c':
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

    internal static async Task GetLoggedInUserAsync(AdvNookContext db)
    {
        try
        {
            Program.LoggedInUser = await db.Users.FirstOrDefaultAsync(u => u.LoggedIn == true);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching logged-in user: {ex.Message}");
        }
    }
}
