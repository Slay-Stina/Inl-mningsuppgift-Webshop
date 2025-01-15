using Inlämningsuppgift_Webshop.Models;
using System.Diagnostics;

namespace Inlämningsuppgift_Webshop;

internal class Program
{
    public static User? LoggedInUser { get; set; }
    static async Task Main(string[] args)
    {
        var db = new AdvNookContext();
        Task checkLogin = Methods.GetLoggedInUserAsync(db);

        while (true)
        {
            MiniMenu();
            Banner();
            for (int i = 0; i < 3; i++)
            {
                Featured(i);
            }
            await checkLogin;
            LogIn();

            Methods.SelectItem();
        }
    }

    private static void MiniMenu()
    {
        string buffer = new string('/', 20);
        List<string> list = new List<string>
        {
            "Hem",
            "Nyheter",
            "Outlet",
            "Kategorier",
        };
        int index = 1;
        Console.SetCursorPosition(2, 0);
        Console.Write(buffer + "\t");
        foreach (string s in list)
        {
            Console.Write(index + ". " + s.PadRight(8) + "\t");
            index++;
        }
        Console.Write(buffer);
    }

    private static void LogIn()
    {
        List<string> userName = new List<string>();
        userName.Add(LoggedInUser == null ? "Press 'L' to log in." : LoggedInUser.FirstName + " " + LoggedInUser.LastName[0]);
        Window logIn = new Window($"Log in", 125 , 0, userName);
        logIn.Draw();
    }

    private static void Featured(int i)
    {
        List<string> features = new List<string>
        {
            "a      ",
            "b      ",
            "c      "
        };
        Window featured = new Window($"Feature {i + 1}", 2 + (20 * i), 10, features);
        featured.Draw();
    }

    public static void Banner()
    {
        List<string> advNook = new List<string>{
            " █████╗ ██████╗ ██╗   ██╗███████╗███╗   ██╗████████╗██╗   ██╗██████╗ ███████╗    ███╗   ██╗ ██████╗  ██████╗ ██╗  ██╗",
            "██╔══██╗██╔══██╗██║   ██║██╔════╝████╗  ██║╚══██╔══╝██║   ██║██╔══██╗██╔════╝    ████╗  ██║██╔═══██╗██╔═══██╗██║ ██╔╝",
            "███████║██║  ██║██║   ██║█████╗  ██╔██╗ ██║   ██║   ██║   ██║██████╔╝█████╗      ██╔██╗ ██║██║   ██║██║   ██║█████╔╝ ",
            "██╔══██║██║  ██║╚██╗ ██╔╝██╔══╝  ██║╚██╗██║   ██║   ██║   ██║██╔══██╗██╔══╝      ██║╚██╗██║██║   ██║██║   ██║██╔═██╗ ",
            "██║  ██║██████╔╝ ╚████╔╝ ███████╗██║ ╚████║   ██║   ╚██████╔╝██║  ██║███████╗    ██║ ╚████║╚██████╔╝╚██████╔╝██║  ██╗",
            "╚═╝  ╚═╝╚═════╝   ╚═══╝  ╚══════╝╚═╝  ╚═══╝   ╚═╝    ╚═════╝ ╚═╝  ╚═╝╚══════╝    ╚═╝  ╚═══╝ ╚═════╝  ╚═════╝ ╚═╝  ╚═╝"
        };
        Window title = new Window("Welcome to", 2, 2, advNook);
        title.Draw();
    }
}