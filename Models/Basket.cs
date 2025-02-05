using Inlämningsuppgift_Webshop;

namespace Assignment_Webshop.Models;

internal class Basket
{
    public int Id { get; set; }
    public virtual ICollection<BasketProduct> BasketProducts { get; set; } = new List<BasketProduct>();
    private static string _header => "'B'asket - Check'O'ut" + (Program.ActiveSubPage == SubPage.Basket ? " - ↑↓ Navigate - ←→ Change quantity" : "");
    private static Window _window = new Window(_header, 95, 0);
    private static List<string> _emptyBasket = new List<string> { "The basket is empty" };
    public static Basket GuestBasket { get; set; } = new Basket();

    private static Basket _currentBasket = Login.ActiveUser != null ? Login.ActiveUser.Basket : GuestBasket;

    public static void DrawBasket()
    {
        _currentBasket = Login.ActiveUser != null ? Login.ActiveUser.Basket : GuestBasket;
        UpdateWindow();

        if (Program.ActiveSubPage == SubPage.Basket)
        {
            ChangeQuantity();
            _window.Navigate();
        }
        else
        {
            _window.SelectedIndex = null;
            _window.Draw();
        }
    }
    public static void AddProductToBasket(int id)
    {
        _currentBasket = Login.ActiveUser != null ? Login.ActiveUser.Basket : GuestBasket;

        // Lägg till produkten i den aktuella varukorgen
        var basketProduct = _currentBasket.BasketProducts
            .FirstOrDefault(bp => bp.ProductId == id);

        if (basketProduct != null)
        {
            basketProduct.Quantity++;
        }
        else
        {
            _currentBasket.BasketProducts.Add(new BasketProduct
            {
                Product = Start.ProductList.FirstOrDefault(p => p.Id == id),
                ProductId = id,
                Quantity = 1
            });
        }

        // Uppdatera varukorgen i databasen
        using (var db = new AdvNookContext())
        {
            // Om användaren är inloggad, spara varukorgen
            if (Login.ActiveUser != null)
            {
                db.Update(_currentBasket);
                db.SaveChanges();
            }
        }
        DrawBasket();
    }

    private static void UpdateWindow()
    {
        _window.Header = _header;

        if (_currentBasket.BasketProducts != null && _currentBasket.BasketProducts.Count > 0)
        {
            var basketProducts = _currentBasket.BasketProducts
                .GroupBy(bp => bp.Product.Name)
                .Select(g =>
                {
                    var quantityAndName = $"{g.Sum(bp => bp.Quantity)}st {g.Key.PadRight(30)}";
                    var price = $"{g.Sum(bp => bp.Quantity * bp.Product.Price):C}".PadLeft(15);
                    return $"{quantityAndName}{price}";
                })
                .ToList();

            decimal totalPrice = _currentBasket.BasketProducts
                .Sum(bp => bp.Quantity * bp.Product.Price);

            basketProducts.Add(new string('-', 50));
            basketProducts.Add($"Total: {totalPrice:C}".PadLeft(50));

            _window.TextRows = basketProducts;
        }
        else
        {
            _window.TextRows = _emptyBasket;
        }
    }
    public static void ChangeQuantity()
    {
        if (_currentBasket.BasketProducts is null || !_currentBasket.BasketProducts.Any() || _window.SelectedIndex is null)
            return;

        var selectedProduct = _currentBasket.BasketProducts.ElementAtOrDefault(_window.SelectedIndex.Value);

        if (Program.KeyInfo.Key == ConsoleKey.RightArrow)
        {
            selectedProduct.Quantity++;
        }
        else if (Program.KeyInfo.Key == ConsoleKey.LeftArrow)
        {
            selectedProduct.Quantity--;

            if (selectedProduct.Quantity <= 0)
            {
                _currentBasket.BasketProducts.Remove(selectedProduct);
            }
        }

        using (var db = new AdvNookContext())
        {
            var productInDb = db.BasketProduct.FirstOrDefault(bp => bp.Id == selectedProduct.Id);

            if (productInDb != null)
            {
                if (selectedProduct.Quantity > 0)
                {
                    productInDb.Quantity = selectedProduct.Quantity;
                }
                else
                {
                    db.BasketProduct.Remove(productInDb);
                }
                db.SaveChanges();
            }
        }
        UpdateWindow();
    }
}
