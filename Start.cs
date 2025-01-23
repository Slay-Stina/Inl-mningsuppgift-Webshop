using Azure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inlämningsuppgift_Webshop;

internal class Start
{
    public static void Page()
    {
        Banner();
        for (int i = 0; i < 3; i++)
        {
            Featured(i);
        }
    }
    private static void Featured(int i)
    {
        char[] xyz = ['X', 'Y', 'Z'];
        List<string> features = new List<string>
        {
            "a      ",
            "b      ",
            "c      "
        };
        Window featured = new Window($"{xyz[i]}", 2 + (20 * i), 10, features);
        featured.Draw();
    }

    private static void Banner()
    {
        List<string> advNook = new List<string>{
            " █████╗ ██████╗ ██╗   ██╗███████╗███╗   ██╗████████╗██╗   ██╗██████╗ ███████╗    ███╗   ██╗ ██████╗  ██████╗ ██╗  ██╗",
            "██╔══██╗██╔══██╗██║   ██║██╔════╝████╗  ██║╚══██╔══╝██║   ██║██╔══██╗██╔════╝    ████╗  ██║██╔═══██╗██╔═══██╗██║ ██╔╝",
            "███████║██║  ██║██║   ██║█████╗  ██╔██╗ ██║   ██║   ██║   ██║██████╔╝█████╗      ██╔██╗ ██║██║   ██║██║   ██║█████╔╝ ",
            "██╔══██║██║  ██║╚██╗ ██╔╝██╔══╝  ██║╚██╗██║   ██║   ██║   ██║██╔══██╗██╔══╝      ██║╚██╗██║██║   ██║██║   ██║██╔═██╗ ",
            "██║  ██║██████╔╝ ╚████╔╝ ███████╗██║ ╚████║   ██║   ╚██████╔╝██║  ██║███████╗    ██║ ╚████║╚██████╔╝╚██████╔╝██║  ██╗",
            "╚═╝  ╚═╝╚═════╝   ╚═══╝  ╚══════╝╚═╝  ╚═══╝   ╚═╝    ╚═════╝ ╚═╝  ╚═╝╚══════╝    ╚═╝  ╚═══╝ ╚═════╝  ╚═════╝ ╚═╝  ╚═╝"
        };
        Window title = new Window("Welcome to", 2, 0, advNook);
        title.Draw();
    }
}
