// Import necessary namespaces for MVC functionality and application data models
using Microsoft.AspNetCore.Mvc;
using Web_Programming_Proje.Data;
using Web_Programming_Proje.Models;
using Web_Programming_Proje.ViewModels;

namespace Web_Programming_Proje.Controllers;

public class CartController : Controller
{
    private readonly StoreDbContext _context;  // Database context for accessing product data
    private readonly string CartSessionKey = "Cart";  // Key for storing cart in session

    // Constructor to inject StoreDbContext dependency
    public CartController(StoreDbContext storeDbContext)
    {
        _context = storeDbContext;
    }

    // Add a product to the cart
    [HttpGet]
    public IActionResult AddToCart(long productID)
    {
        // Retrieve the current cart from session
        List<MyCartItem> cart = GetCart();
        
        // Check if the product already exists in the cart
        var cartItem = cart.FirstOrDefault(c => c.ProductID == productID);

        if (cartItem != null)
        {
            // If the product is already in the cart, increase the quantity and update the total price
            cartItem.Quantity++;
            cartItem.TotalPrice = cartItem.Quantity * cartItem.UnitPrice;
        }
        else
        {
            // If the product is not in the cart, retrieve it from the database and create a new cart item
            var dbProduct = _context.Products.FirstOrDefault(p => p.ProductID == productID);
            var productToAddCart = new MyCartItem
            {
                ProductID = dbProduct!.ProductID,
                ProductName = dbProduct.ProductName!,
                UnitPrice = dbProduct.Price,
                Quantity = 1,
                TotalPrice = dbProduct.Price
            };
            cart.Add(productToAddCart);  // Add the new product to the cart
            SaveCart(cart);  // Save the updated cart to the session
        }

        // Redirect to the "AllVehicles" action of the "Vehicle" controller
        return RedirectToAction("AllVehicles", "Vehicle");
    }

    // Display the cart items
    [HttpGet]
    public IActionResult MyCart()
    {
        List<MyCartItem> cart = GetCart();  // Get the current cart from session
        return View(cart);  // Return the cart to the view
    }

    // Remove a product from the cart
    [HttpGet]
    public IActionResult RemoveFromCart(long productId)
    {
        var cart = GetCart();  // Retrieve the current cart from session
        MyCartItem? productToRemove = cart.FirstOrDefault(p => p.ProductID == productId);
        
        if (productToRemove != null)
        {
            // If the product is found in the cart, remove it
            cart.Remove(productToRemove);
            SaveCart(cart);  // Save the updated cart to the session
        }

        // Redirect to the "MyCart" action to display the updated cart
        return RedirectToAction("MyCart", "Cart");
    }

    // Decrease the quantity of a product in the cart
    [HttpGet]
    public IActionResult DecreaseQuantity(long productID)
    {
        List<MyCartItem> cart = GetCart();  // Retrieve the current cart from session
        var cartItem = cart.FirstOrDefault(p => p.ProductID == productID);

        if (cartItem!.Quantity == 1)
        {
            // If the quantity is 1, remove the product from the cart
            RemoveFromCart(productID);
        }
        else
        {
            // Otherwise, decrease the quantity and update the total price
            cartItem!.Quantity--;
            cartItem.TotalPrice = cartItem.UnitPrice * cartItem.Quantity;
            SaveCart(cart);  // Save the updated cart to the session
        }

        // Redirect to the "MyCart" action to display the updated cart
        return RedirectToAction("MyCart", "Cart");
    }

    // Increase the quantity of a product in the cart
    [HttpGet]
    public IActionResult IncreaseQuantity(long productID)
    {
        List<MyCartItem> cart = GetCart();  // Retrieve the current cart from session
        var cartItem = cart.FirstOrDefault(p => p.ProductID == productID);

        if (cartItem != null)
        {
            // If the product is found, increase the quantity and update the total price
            cartItem.Quantity++;
            cartItem.TotalPrice = cartItem.Quantity * cartItem.UnitPrice;
            SaveCart(cart);  // Save the updated cart to the session
        }

        // Redirect to the "MyCart" action to display the updated cart
        return RedirectToAction("MyCart", "Cart");
    }

    // Clear all items from the cart
    [HttpGet]
    public IActionResult ClearCart()
    {
        SaveCart(new List<MyCartItem>());  // Save an empty cart to the session
        return RedirectToAction("MyCart", "Cart");  // Redirect to the "MyCart" action to show the empty cart
    }

    // Helper method to get the cart from session
    private List<MyCartItem> GetCart()
    {
        var cart = HttpContext.Session.Get<List<MyCartItem>>(CartSessionKey);
        return cart ?? new List<MyCartItem>();  // Return an empty cart if no cart exists in the session
    }

    // Helper method to save the cart to session
    private void SaveCart(List<MyCartItem> cart)
    {
        HttpContext.Session.Set(CartSessionKey, cart);  // Save the cart to the session
    }
}
