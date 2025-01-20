using Inlämningsuppgift_Webshop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace Inlämningsuppgift_Webshop;

internal class Login
{
    public static string DefaultHeader = "Logga in";
    public static string ActiveHeader = "Inloggad";
    public static int left = 125;
    public static int top = 0;
    public static List<string> DefaultText = new List<string> 
    {
        "Tryck 'L' för att logga in.",
        "Tryck 'A' för ny användare"
    };
    public static void Prompt()
    {
        List<string> loginText = new List<string>
        {
        "Användarnamn:".PadRight(27),
        ""
        };
        Program.LogInWindow.TextRows = loginText;
        Program.LogInWindow.Draw();
        Console.SetCursorPosition(127, 2);
        string username = Console.ReadLine();
        using (var db = new AdvNookContext())
        {
            try
            {
                Program.LoggedInUser = db.Users.Where(u => u.Username == username).FirstOrDefault();
                if(Program.LoggedInUser != null)
                Program.LoggedInUser.LoggedIn = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Användaren hittades inte, försök igen eller lägg till ny användare.");
                Console.WriteLine(ex.Message);
            }
            db.SaveChanges();
        }
    }

    internal static void DrawLogin()
    {
        Program.LogInWindow.Left = left; 
        Program.LogInWindow.Top = top;
        Program.LogInWindow.Header = Program.LoggedInUser is null ? DefaultHeader : ActiveHeader;
        Program.LogInWindow.TextRows = Program.LoggedInUser is null ? DefaultText : new List<string>
        {Program.LoggedInUser.FirstName + " " + Program.LoggedInUser.LastName,
        "",
        "Tryck 'L' för att logga ut."};
        Program.LogInWindow.Draw();
    }
}
