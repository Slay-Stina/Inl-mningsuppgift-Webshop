using Assignment_Webshop.Models;
using Assignment_Webshop;
using Microsoft.EntityFrameworkCore;

namespace Assignment_Webshop;

internal class Login
{
    public static User? ActiveUser { get; set; }
    public static string DefaultHeader = "'L'ogin";
    public static string ActiveHeader = "'L'ogout";
    private static Window _window { get; set; } = new Window(2, 0);
    public static List<string> DefaultText = new List<string>
    {
        "'A'dd User".PadRight(19)
    };
    public static void Prompt()
    {
        List<string> loginText = new List<string>
        {
        "Username:".PadRight(19)
        };
        _window.TextRows = loginText;
        _window.Draw();
        Console.SetCursorPosition(loginText[0].Length - 6, 1);
        string username = Console.ReadLine();
        using (var db = new AdvNookContext())
        {
            try
            {
                ActiveUser = db.Users.Include(u => u.Basket.Products).FirstOrDefault(u => u.Username == username);
                if (ActiveUser != null)
                    ActiveUser.LoggedIn = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("User not found, try again or add a new user.");
                Console.WriteLine(ex.Message);
            }
            db.SaveChanges();
        }
    }

    internal static void DrawLogin()
    {
        _window.Header = ActiveUser is null ? DefaultHeader : ActiveHeader;
        _window.TextRows = ActiveUser is null ? DefaultText : new List<string>
        {$"{ActiveUser.FirstName} {ActiveUser.LastName}".PadRight(19),
        ActiveUser is not null ? "'U'ser" : "",
        ActiveUser is not null && ActiveUser.Admin ? "'A'dminpage" : ""};
        _window.Draw();
    }
}