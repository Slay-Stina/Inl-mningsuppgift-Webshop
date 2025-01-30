
namespace Assignment_Webshop.Models;

internal class Basket
{
    public int Id { get; set; }
    public virtual ICollection<Product> Products { get; set; } = new List<Product>();

    private static Window _window = new Window("Basket", 110, 0);
    private static List<string> _emptyBasket = new List<string> { "The basket is empty" };
    public static Basket GuestBasket { get; set; } = new Basket();

    internal static void Page()
    {
        Banner();
    }
    public static void DrawBasket()
    {
        Basket basket;

        if (Login.ActiveUser != null)
        {
            basket = Login.ActiveUser.Basket;
        }
        else
        {
            basket = GuestBasket;
        }

        if (basket.Products != null && basket.Products.Count > 0)
        {
            var basketProducts = basket.Products.GroupBy(g => g.Name);
            var productList = basketProducts.Select(g => $"{g.Key.PadRight(10)} {g.Count()}st" /*- {g.Select(s => s.Price).Sum()}Kr*/).ToList();
            _window.TextRows = productList;
        }
        else
        {
            _window.TextRows = _emptyBasket;
        }
        _window.Draw();
    }
    private static void Banner()
    {
        List<string> bannerAlt = new List<string>
        {
            "██╗   ██╗ █████╗ ██████╗ ██╗   ██╗██╗  ██╗ ██████╗ ██████╗  ██████╗ ",
            "██║   ██║██╔══██╗██╔══██╗██║   ██║██║ ██╔╝██╔═══██╗██╔══██╗██╔════╝ ",
            "██║   ██║███████║██████╔╝██║   ██║█████╔╝ ██║   ██║██████╔╝██║  ███╗",
            "╚██╗ ██╔╝██╔══██║██╔══██╗██║   ██║██╔═██╗ ██║   ██║██╔══██╗██║   ██║",
            " ╚████╔╝ ██║  ██║██║  ██║╚██████╔╝██║  ██╗╚██████╔╝██║  ██║╚██████╔╝",
            "  ╚═══╝  ╚═╝  ╚═╝╚═╝  ╚═╝ ╚═════╝ ╚═╝  ╚═╝ ╚═════╝ ╚═╝  ╚═╝ ╚═════╝ ",
        };
        List<string> banner = new List<string>
        {
            "╔╗ ╔═╗╔═╗╦╔═╔═╗╔╦╗",
            "╠╩╗╠═╣╚═╗╠╩╗║╣  ║ ",
            "╚═╝╩ ╩╚═╝╩ ╩╚═╝ ╩ "
        };
        int bannerLength = banner[0].Length;
        int leftPos = (Console.WindowWidth - bannerLength) / 2;
        Window title = new Window("", leftPos, 0, banner);
        title.Draw();
    }
}
