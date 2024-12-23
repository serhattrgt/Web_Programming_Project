// Import necessary namespaces for MVC, Entity Framework, and data models
using System.Runtime.Intrinsics.Arm;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web_Programming_Proje.Data;
using Web_Programming_Proje.Models;
using Web_Programming_Proje.ViewModels;

namespace Web_Programming_Proje.Controllers;

// OrderController handles the operations related to orders, such as buying products, viewing order details, and creating orders
public class OrderController : Controller
{
    private readonly string CartSessionKey = "Cart";  // Key for storing the cart in session
    private readonly StoreDbContext _context;  // Database context for interacting with the database

    // Constructor to inject the database context
    public OrderController(StoreDbContext context)
    {
        _context = context;
    }

    // Action to add a product to the cart or increase its quantity if already in the cart
    public IActionResult Buy(long productID)
    {
        // Retrieve the current cart from session
        List<MyCartItem> cart = GetCart();
        var cartItem = cart.FirstOrDefault(p => p.ProductID == productID);  // Check if the product is already in the cart
        var dbProduct = _context.Products.FirstOrDefault(p => p.ProductID == productID);  // Fetch product details from the database

        if (cartItem == null)
        {
            // If the product is not in the cart, add it
            var productToAddCart = new MyCartItem
            {
                ProductID = dbProduct!.ProductID,
                ProductName = dbProduct.ProductName!,
                UnitPrice = dbProduct.Price,
                Quantity = 1,
                TotalPrice = dbProduct.Price
            };
            cart.Add(productToAddCart);  // Add the product to the cart
            SaveCart(cart);  // Save the updated cart to session
        }
        else
        {
            // If the product is already in the cart, increase the quantity
            if (cartItem!.Quantity >= 1)
            {
                cartItem.Quantity++;
            }
            cartItem.TotalPrice = cartItem.UnitPrice * cartItem.Quantity;  // Update the total price based on the new quantity
            SaveCart(cart);  // Save the updated cart to session
        }

        // Redirect to the MyCart action to display the cart
        return RedirectToAction("MyCart", "Cart");
    }

    // Action to view the details of an order
    [HttpGet]
    public IActionResult OrderDetails(long orderID)
    {
        // Fetch the order details including the products associated with the order
        var orders = _context.Orders
            .Include(order => order.OrderProducts)  // Include the related order products
            .ThenInclude(orderProduct => orderProduct.Product)  // Include the product details for each order product
            .FirstOrDefault(order => order.OrderID == orderID);

        // If no order is found, throw an exception
        if (orders == null)
        {
            throw new Exception("There is no order with the id " + orderID);
        }

        // Fetch the payment details for the order
        var payment = _context.Payments.FirstOrDefault(p => p.PaymentID == orders.PaymentID);

        // Create the OrderDetailsViewModel to pass to the view
        var orderDetails = new OrderDetailsViewModel
        {
            OrderID = orderID,
            PaymentType = payment!.PaymentType,
            OrderDate = orders.OrderDate,
            IsDelivered = orders.IsDelivered,
            Quantity = orders.OrderProducts.Sum(op => op.Quantity),  // Sum the quantities of all products in the order
            TotalPriceOfOrder = orders.OrderProducts.Sum(op => op.TotalPrice),  // Sum the total prices of all products in the order
            ProductDetails = orders.OrderProducts.Select(op => new ProductDetailViewModel
            {
                ProductName = op.ProductName,
                ProductPrice = op.UnitPrice,
                ProductQuantity = op.Quantity
            }).ToList()  // Create a list of product details for each product in the order
        };

        // Return the order details view with the view model
        return View(orderDetails);
    }

    // Action to create a new order from the cart
    [HttpGet]
    public IActionResult CreateOrder()
    {
        // Retrieve the current user and their address from the database
        var user = _context.Users
            .Include(u => u.Orders)
            .Include(u => u.Address)
            .FirstOrDefault(u => u.UserName == User.Identity!.Name);

        // Retrieve the payment method for "Credit Card"
        var paymentStyle = _context.Payments.FirstOrDefault(p => p.PaymentType == "Credit Card");

        // Get the current cart items
        List<MyCartItem> cartItems = GetCart();

        // Get the list of product IDs from the cart items
        var productIDs = cartItems.Select(cartItem => cartItem.ProductID).ToList();

        // Fetch the products that are in the cart
        var productsToBuy = _context.Products
            .Where(p => productIDs.Contains(p.ProductID))
            .ToList();

        // Create a new order object
        var newOrder = new Order
        {
            OrderDate = DateTime.Now,
            DeliveryDate = DateTime.Now.AddDays(2),
            ShippingAddress = user.Address.OpenAddress,
            IsDelivered = false,
            Payment = paymentStyle,
            User = user
        };

        // Loop through the products and add them to the order
        foreach (var product in productsToBuy)
        {
            var cartItem = cartItems.First(c => c.ProductID == product.ProductID);  // Get the cart item for the product

            // Create a new OrderProduct object for each product in the order
            var orderProduct = new OrderProduct
            {
                ProductName = product.ProductName!,
                Quantity = cartItem.Quantity,
                UnitPrice = product.Price,
                TotalPrice = cartItem.Quantity * product.Price,
                Order = newOrder,
                Product = product
            };

            newOrder.OrderProducts.Add(orderProduct);  // Add the product to the order

            product.StockAmount -= cartItem.Quantity;  // Decrease the stock of the product based on the quantity ordered

            // If there is not enough stock, return a BadRequest
            if (product.StockAmount < 0)
            {
                return BadRequest($"Not enough stock for product: {product.ProductName}");
            }
        }

        // Add the new order to the database and save changes
        _context.Orders.Add(newOrder);
        _context.SaveChanges();

        // Clear the cart after the order is created
        ClearCart();

        // Redirect to the UserDetails action to view the user's profile
        return RedirectToAction("UserDetails", "Profile");
    }

    // Action to display the payment page
    [HttpGet]
    public IActionResult Payment()
    {
        // Retrieve the current user's address from the database
        var user = _context.Users
            .Include(u => u.Address)
            .FirstOrDefault(u => u.UserName == User.Identity.Name);

        // If the user does not have an address, prompt them to add one
        if (user.Address.OpenAddress == null || user.Address.OpenAddress.Length == 0)
        {
            TempData["Error"] = "Önce adresini yaz yeğenim, geldin profiline.";
            return RedirectToAction("UpdateUser", "Profile", new { userID = user.UserID });
        }

        // Return the payment view
        return View();
    }

    // Helper method to retrieve the cart from session
    private List<MyCartItem> GetCart()
    {
        var cart = HttpContext.Session.Get<List<MyCartItem>>(CartSessionKey);
        return cart ?? new List<MyCartItem>();  // Return an empty cart if none exists
    }

    // Helper method to save the cart to session
    private void SaveCart(List<MyCartItem> cart)
    {
        HttpContext.Session.Set(CartSessionKey, cart);
    }

    // Helper method to clear the cart from session
    private void ClearCart()
    {
        SaveCart(new List<MyCartItem>());
    }
}
