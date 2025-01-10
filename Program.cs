
namespace Inlämningsuppgift_Webshop;

internal class Program
{
    static void Main(string[] args)
    {
        Banner();
        for (int i = 0; i < 3; i++)
        {
            Featured(i);
        }
    }

    private static void Featured(int i)
    {
        List<string> features = new List<string>
        {
            "a      ",
            "b      ",
            "c      "
        };
        Window featured = new Window($"Feature {i + 1}", 5 + (20 * i), 8, features);
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
        Window title = new Window("Welcome to", 5, 0, advNook);
        title.Draw();
    }
}