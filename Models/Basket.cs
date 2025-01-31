using Inlämningsuppgift_Webshop;
using Microsoft.EntityFrameworkCore;

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
    public static void AddProductToBasket(Product product)
    {
        // Lägg till produkten i den aktuella varukorgen
        var basketProduct = _currentBasket.BasketProducts
            .FirstOrDefault(bp => bp.ProductId == product.Id);

        if (basketProduct != null)
        {
            basketProduct.Quantity++;
        }
        else
        {
            _currentBasket.BasketProducts.Add(new BasketProduct { Product = product, Quantity = 1 });
        }

        // Uppdatera varukorgen i databasen
        using (var db = new AdvNookContext())
        {
            // Om användaren är inloggad, spara varukorgen
            if (Login.ActiveUser != null)
            {
                db.Entry(_currentBasket).State = EntityState.Modified;
            }
            else
            {
                // Om vi har en gästvarukorg, skapa en ny eller uppdatera befintlig
                db.Entry(_currentBasket).State = EntityState.Added;
            }

            db.SaveChanges();
        }

        _currentBasket = Login.ActiveUser != null ? Login.ActiveUser.Basket : GuestBasket;
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
        if (_currentBasket.BasketProducts != null && _currentBasket.BasketProducts.Count > 0 && _window.SelectedIndex is not null)
        {
            var selectedProduct = _currentBasket.BasketProducts.ElementAtOrDefault(_window.SelectedIndex.Value);

            if (Program.KeyInfo.Key == ConsoleKey.RightArrow && selectedProduct != null)
            {
                // Öka kvantiteten och uppdatera databasen
                selectedProduct.Quantity++;
                UpdateProductInDatabase(selectedProduct);  // Uppdaterar kvantiteten i databasen
            }
            else if (Program.KeyInfo.Key == ConsoleKey.LeftArrow && selectedProduct != null)
            {
                if (selectedProduct.Quantity > 1)
                {
                    // Minska kvantiteten och uppdatera databasen
                    selectedProduct.Quantity--;
                    UpdateProductInDatabase(selectedProduct);  // Uppdaterar kvantiteten i databasen
                }
                else
                {
                    // Ta bort produkten från varukorgen om kvantiteten är 1
                    _currentBasket.BasketProducts.Remove(selectedProduct);
                    RemoveProductFromDatabase(selectedProduct);  // Ta bort produkten från databasen
                }
            }

            UpdateWindow();  // Uppdatera fönstret efter att ha ändrat kvantiteten
        }
    }

    private static void UpdateProductInDatabase(BasketProduct basketProduct)
    {
        using (var db = new AdvNookContext())
        {
            var product = db.BasketProduct.FirstOrDefault(bp => bp.Id == basketProduct.Id); // Hitta rätt BasketProduct
            if (product != null)
            {
                product.Quantity = basketProduct.Quantity;  // Uppdatera kvantiteten
                db.SaveChanges();  // Spara ändringarna i databasen
            }
        }
    }

    private static void RemoveProductFromDatabase(BasketProduct basketProduct)
    {
        using (var db = new AdvNookContext())
        {
            var product = db.BasketProduct.FirstOrDefault(bp => bp.Id == basketProduct.Id); // Hitta rätt BasketProduct
            if (product != null)
            {
                db.BasketProduct.Remove(product);  // Ta bort produkten från databasen
                db.SaveChanges();  // Spara ändringarna i databasen
            }
        }
    }
}
