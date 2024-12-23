// Import necessary namespaces for MVC, Identity, Entity Framework, and data models
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web_Programming_Proje.Data;
using Web_Programming_Proje.Models;
using Web_Programming_Proje.ViewModels;

namespace Web_Programming_Proje.Controllers;

public class ProfileController : Controller
{
    private readonly SignInManager<IdentityUser> _signInManager;  // For handling user sign-in
    private readonly UserManager<IdentityUser> _userManager;  // For managing user identities
    private readonly StoreDbContext _context;  // Database context for interacting with the application's data

    // Constructor to inject the dependencies
    public ProfileController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager, StoreDbContext storeDbContext)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _context = storeDbContext;
    }

    // Action to display user details and their orders
    [HttpGet]
    public IActionResult UserDetails()
    {
        // If the user is not logged in, redirect to the login page
        if (User.Identity?.Name == null)
        {
            return RedirectToAction("Login", "Auth");
        }

        // Retrieve the logged-in user's details and their orders, including the products in each order
        var loginUser = _context.Users
            .Include(user => user.Orders)
            .ThenInclude(order => order.OrderProducts)
            .ThenInclude(orderProduct => orderProduct.Product)
            .FirstOrDefault(user => user.UserName == User.Identity.Name);

        // If the user is not found, redirect to the login page
        if (loginUser == null)
        {
            return RedirectToAction("Login", "Auth");
        }

        List<OrderViewModel> ordersInfos;

        // If the user has orders, create a list of order view models
        if (loginUser?.Orders?.Any() == true)
        {
            ordersInfos = loginUser.Orders
                .Select(order => new OrderViewModel
                {
                    OrderID = order.OrderID,
                    OrderDate = order.OrderDate,
                    IsDelivered = order.OrderDate.AddDays(2) <= DateTime.Now,  // Check if the order is delivered
                    Quantity = order.OrderProducts.Sum(q => q.Quantity),  // Sum the quantities of products in the order
                    TotalPrice = order.OrderProducts.Sum(p => p.TotalPrice),  // Sum the total price of the order
                    ProductNames = order.OrderProducts.Select(op => op.Product.ProductName).ToList()  // Get product names in the order
                })
                .ToList();
        }
        else
        {
            ordersInfos = new List<OrderViewModel>();  // If no orders, return an empty list
        }

        // Create a view model to pass the user details and their orders to the view
        UserProfileViewModel userProfileView = new UserProfileViewModel
        {
            UserID = loginUser!.UserID,
            Name = loginUser!.Name,
            Surname = loginUser!.Surname,
            UserName = loginUser.UserName,
            Orders = ordersInfos
        };

        // Return the view with the user profile view model
        return View(userProfileView);
    }

    // Action to delete a user
    [HttpDelete("/Profile/Delete/{userId}")]
    public async Task<ActionResult> Delete(long userId)
    {
        // Find the user to be deleted from the database
        var willRemoveUser = await _context.Users.FirstOrDefaultAsync(u => u.UserID == userId);

        if (willRemoveUser == null)
        {
            return NotFound();  // Return NotFound if the user does not exist
        }

        // Remove the user from the database
        _context.Users.Remove(willRemoveUser);
        await _context.SaveChangesAsync();

        // Find the identity user associated with the user in the database
        var identityUser = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == willRemoveUser.UserName);
        if (identityUser != null)
        {
            // Delete the identity user
            var deleteResult = await _userManager.DeleteAsync(identityUser);
            if (!deleteResult.Succeeded)
            {
                return BadRequest("Kullanıcı Identity'den silinemedi");  // Return BadRequest if the identity deletion fails
            }

            // Sign out the user after deletion
            await _signInManager.SignOutAsync();
        }

        // Return Ok if the user was successfully deleted
        return Ok();
    }

    // Action to display the user update form
    [HttpGet]
    public ActionResult UpdateUser(long userID)
    {
        // If there is an error in TempData, add it to the model state
        if (TempData["Error"] != null)
        {
            ModelState.AddModelError("", TempData["Error"].ToString());
        }

        // Find the user to update by their ID
        var user = _context.Users.Include(u => u.Address)
            .FirstOrDefault(u => u.UserID == userID);
        if (user == null)
        {
            throw new Exception("There is no user in the system with id " + userID);  // Throw an exception if the user is not found
        }

        // Create a view model for the update form
        var updateForm = new UpdateUserViewModel
        {
            UserID = userID,
            Name = user.Name,
            Surname = user.Surname,
            UserName = user.UserName,
            Password = string.Empty,  // Password is initially empty for the user to input
            ConfirmedPassword = string.Empty,  // ConfirmedPassword is initially empty
            Address = user.Address!.OpenAddress
        };

        // Return the update form view with the view model
        return View(updateForm);
    }

    // Action to handle the user update form submission
    [HttpPost]
    public async Task<ActionResult> UpdateUser(UpdateUserViewModel updateRequest)
    {
        if (ModelState.IsValid)
        {
            // Retrieve the user to be updated from the database
            var userFromContext = await _context.Users
                .Include(u => u.Address)
                .FirstOrDefaultAsync(u => u.UserID == updateRequest.UserID);
            var userFromIdentity = await _userManager.FindByNameAsync(userFromContext!.UserName);

            var passwordEncoder = new PasswordHasher<object>();

            if (userFromContext == null)
            {
                throw new Exception("There is no user with id :" + updateRequest.UserID);
            }
            if (userFromIdentity == null)
            {
                throw new Exception("There is no user with username :" + userFromContext.UserName);
            }

            // Check if the username is already taken
            if (updateRequest.UserName != userFromContext.UserName)
            {
                if (await _context.Users.AnyAsync(u => u.UserName == updateRequest.UserName))
                {
                    ModelState.AddModelError(string.Empty, "The Username :" + updateRequest.UserName + " is already taken");
                    return View(updateRequest);
                }
            }

            // Check if the passwords match
            if (updateRequest.Password != updateRequest.ConfirmedPassword)
            {
                ModelState.AddModelError(string.Empty, "The entered passwords do not match");
                return View(updateRequest);
            }

            // Verify the user's current password
            var result = passwordEncoder.VerifyHashedPassword(userFromContext, userFromContext.Password, updateRequest.Password);

            // If the password is correct, update the user's details
            if (result == PasswordVerificationResult.Success)
            {
                userFromContext.Address.OpenAddress = updateRequest.Address;
                userFromContext.Name = updateRequest.Name;
                userFromContext.Surname = updateRequest.Surname;
                userFromContext.UserName = updateRequest.UserName;
                await _context.SaveChangesAsync();
            }
            else
            {
                ModelState.AddModelError("", "Kullanıcı şifresi yanlış girildi.");  // Add an error if the password is incorrect
            }

            // Update the identity user details
            userFromIdentity!.UserName = updateRequest.UserName;
            userFromIdentity.PasswordHash = _userManager.PasswordHasher.HashPassword(userFromIdentity, updateRequest.Password);
            await _userManager.UpdateAsync(userFromIdentity);

            // Sign out the user and sign them in again with the updated information
            await _signInManager.SignOutAsync();
            await _signInManager.SignInAsync(userFromIdentity, true);

            // Redirect to the user details page after successful update
            return RedirectToAction("UserDetails", "Profile");
        }
        else
        {
            // If the model state is invalid, return the view with the current data
            return View(updateRequest);
        }
    }
}
