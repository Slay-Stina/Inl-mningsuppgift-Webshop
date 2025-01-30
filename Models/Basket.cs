
using Inlämningsuppgift_Webshop;

namespace Assignment_Webshop.Models;

internal class Basket
{
    public int Id { get; set; }
    public virtual ICollection<BasketProduct> BasketProducts { get; set; } = new List<BasketProduct>();

    private static Window _window = new Window("'B'asket", 110, 0);
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

        if (basket.BasketProducts != null && basket.BasketProducts.Count > 0)
        {
            var basketProducts = basket.BasketProducts
                                        .GroupBy(bp => bp.Product.Name)
                                        .Select(g =>
                                            $"{g.Key} {g.Sum(bp => bp.Quantity)}st - {g.Sum(bp => bp.Quantity * bp.Product.Price):C}"
                                        )
                                        .ToList();

            _window.TextRows = basketProducts;
        }
        else
        {
            _window.TextRows = _emptyBasket;
        }

        _window.Draw();
    }

    private static void Banner()
    {
        //List<string> bannerAlt = new List<string>
        //{
        //    "██╗   ██╗ █████╗ ██████╗ ██╗   ██╗██╗  ██╗ ██████╗ ██████╗  ██████╗ ",
        //    "██║   ██║██╔══██╗██╔══██╗██║   ██║██║ ██╔╝██╔═══██╗██╔══██╗██╔════╝ ",
        //    "██║   ██║███████║██████╔╝██║   ██║█████╔╝ ██║   ██║██████╔╝██║  ███╗",
        //    "╚██╗ ██╔╝██╔══██║██╔══██╗██║   ██║██╔═██╗ ██║   ██║██╔══██╗██║   ██║",
        //    " ╚████╔╝ ██║  ██║██║  ██║╚██████╔╝██║  ██╗╚██████╔╝██║  ██║╚██████╔╝",
        //    "  ╚═══╝  ╚═╝  ╚═╝╚═╝  ╚═╝ ╚═════╝ ╚═╝  ╚═╝ ╚═════╝ ╚═╝  ╚═╝ ╚═════╝ ",
        //};
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
