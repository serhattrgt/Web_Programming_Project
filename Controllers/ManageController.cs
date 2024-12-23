// Import necessary namespaces for MVC, Identity, and Entity Framework functionality
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Web_Programming_Proje.Data;
using Web_Programming_Proje.ViewModels;

namespace Web_Programming_Proje.Controllers;

// ManageController handles user management operations like viewing, deleting users
public class ManageController : Controller
{
    // Declare necessary services for authentication, user management, and database context
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly StoreDbContext _context;

    // Constructor to inject dependencies for SignInManager, UserManager, and DbContext
    public ManageController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager, StoreDbContext storeDbContext)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _context = storeDbContext;
    }

    // Action to display a paginated list of all users (only accessible by Admin role)
    [Authorize(Roles = "Admin")]
    [HttpGet]
    public IActionResult AllUsers(int page = 1)
    {
        const int PageSize = 10;  // Define the number of users per page
        int totalUsers = _context.Users.Count();  // Get the total number of users in the system
        int totalPages = (int)Math.Ceiling(totalUsers / (double)PageSize);  // Calculate total pages based on total users

        // Fetch a paginated list of users with related data (Address, Roles, Orders)
        var userList = _context.Users
            .Include(u => u.Address)  // Include related address data
            .Include(u => u.Roles)  // Include related roles data
            .Include(u => u.Orders)  // Include related orders data
            .ThenInclude(o => o.OrderProducts)  // Include order products for each order
            .Skip((page - 1) * PageSize)  // Skip the records based on the current page
            .Take(PageSize)  // Take only the number of records specified by PageSize
            .ToList();

        // Map the fetched user data to a view model for display
        var manageUserView = userList.Select(user => new ManageUserViewModel
        {
            UserID = user.UserID,
            Name = user.Name,
            Surname = user.Surname,
            UserName = user.UserName,
            // Display address or a default message if no address is found
            OpenAdress = user.Address!.OpenAddress == null ? "Kullaniciya Ait Kayitli Adres Bulunamadi" : user.Address.OpenAddress,
            Roles = user.Roles.ToList(),  // List the roles of the user
            OrderCount = user.Orders.Count()  // Count the number of orders associated with the user
        }).ToList();

        // Pass the current page and total pages to the view for pagination
        ViewBag.CurrentPage = page;
        ViewBag.TotalPages = totalPages;

        // Return the list of users to the view
        return View(manageUserView);
    }

    // Action to delete a user (only accessible by Admin role)
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(long userID)
    {
        // Find the user to be deleted from the database
        var willRemoveUser = await _context.Users.FirstOrDefaultAsync(u => u.UserID == userID);

        // If the user is not found, throw an exception
        if (willRemoveUser == null)
        {
            throw new Exception("There is no user in the system with id :" + userID);
        }

        // Find the corresponding IdentityUser object for the user
        var identityUser = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == willRemoveUser.UserName);

        // Remove the user from the Users table in the database
        _context.Users.Remove(willRemoveUser);
        await _context.SaveChangesAsync();  // Save changes to the database

        // If the IdentityUser exists, delete it from the Identity system
        if (identityUser != null)
        {
            await _userManager.DeleteAsync(identityUser);
        }

        // Redirect to the AllUsers action to refresh the user list
        return RedirectToAction("AllUsers", "Manage");
    }
}
