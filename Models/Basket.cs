using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inlämningsuppgift_Webshop.Models;

internal class Basket
{
    public int Id { get; set; }
    public virtual ICollection<Product> Products { get; set; } = new List<Product>();

    private static Window _window = new Window("Varukorg", 125, 7);
    private static List<string> _emptyBasket = new List<string> { "Varukorgen är tom" };
    public static Basket GuestBasket {  get; set; } = new Basket();

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
            var productList = basketProducts.Select(g => $"{g.Key.PadRight(10)} {g.Count()}st - {g.Select(s => s.Price).Sum()}Kr").ToList();
            _window.TextRows = productList;
        }
        else
        {
            _window.TextRows = _emptyBasket;
        }
        _window.Draw();
    }

}
