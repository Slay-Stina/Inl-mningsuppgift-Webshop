using Inlämningsuppgift_Webshop.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inlämningsuppgift_Webshop;

internal class Methods
{
    public static int SelectedIndex = 0;
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
            { Login.ActiveUser = await db.Users.Where(u => u.LoggedIn == true).FirstOrDefaultAsync(); }
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
    public static void MiniMenu()
    {
        List<string> list = new List<string>
        {
            "Startsida",
            "Produkter",
            "Kategorier"
        };
        if (Login.ActiveUser is not null && Login.ActiveUser.Admin)
        {
            list.Add("Admin");
        }
        int index = 1;
        Console.SetCursorPosition(2, 8);
        foreach (string s in list)
        {
            Console.Write(index + ". " + s.PadRight(8) + "\t");
            index++;
        }
    }

    internal static void SelectList(List<string> list, int left, int top)
    {
        SelectedIndex = 0;
        for (int i = 0; i < list.Count; i++)
        {
            if (i == SelectedIndex)
            {
                Console.BackgroundColor = ConsoleColor.White;
                Console.ForegroundColor = ConsoleColor.Black;
            }
            else
            {
                Console.ResetColor();
            }
            Console.SetCursorPosition(left, top + i + 1);
            Console.WriteLine(list[i]);
        }
        ConsoleKeyInfo keyInfo = Console.ReadKey(true);
        if (Console.KeyAvailable)
        {
            if(keyInfo.Key == ConsoleKey.DownArrow && SelectedIndex < list.Count)
            {
                SelectedIndex++;
            }
            if (keyInfo.Key == ConsoleKey.UpArrow && SelectedIndex > 0)
            {
                SelectedIndex--;
            }
        }
    }
}
