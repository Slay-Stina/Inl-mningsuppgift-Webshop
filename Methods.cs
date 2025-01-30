using Assignment_Webshop.Models;
using Microsoft.EntityFrameworkCore;

namespace Assignment_Webshop;

internal class Methods
{
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
                Console.WriteLine("You must enter a number.");
            }
        }
    }

    internal static async Task GetLoggedInUserAsync()
    {
        try
        {
            using (var db = new AdvNookContext())
            {
                Login.ActiveUser = await db.Users
                                           .Include(u => u.Basket)
                                           .ThenInclude(b => b.BasketProducts)
                                           .ThenInclude(bp => bp.Product)
                                           .FirstOrDefaultAsync(u => u.LoggedIn == true);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching logged-in user: {ex.Message}");
        }
    }

    public static List<string> WrapText(string text, int maxWidth)
    {
        List<string> wrappedText = new List<string>();
        while (text.Length > maxWidth)
        {
            int lastSpace = text.LastIndexOf(' ', maxWidth);
            if (lastSpace == -1) lastSpace = maxWidth;
            wrappedText.Add(text.Substring(0, lastSpace));
            text = text.Substring(lastSpace).TrimStart();
        }
        wrappedText.Add(text);
        return wrappedText;
    }
}
