// Import necessary namespaces for Identity and MVC functionality
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Web_Programming_Proje.Data;
using Web_Programming_Proje.Models;
using Web_Programming_Proje.ViewModels;

namespace Web_Programming_Proje.Controllers;

public class AuthController : Controller
{
    // Define a constant for the "Customer" role
    private readonly string CUSTOMER_ROLE = "Customer";

    // Declare the necessary services for user authentication and authorization
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly StoreDbContext _context;

    // Constructor to inject dependencies for SignInManager, UserManager, and DbContext
    public AuthController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager, StoreDbContext storeDbContext)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _context = storeDbContext;
    }

    // Display the login view
    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    // Log out the user and redirect to the Home page
    [HttpGet]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }

    // Display the registration view
    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    // Handle the login POST request
    [HttpPost]
    public async Task<IActionResult> Login(ViewModelUser loginRequest)
    {
        if (ModelState.IsValid)
        {
            // Attempt to find the user by username
            var user = await _userManager.FindByNameAsync(loginRequest.UserName);
            if (user != null)
            {
                // Attempt to sign in the user with the provided password
                var result = await _signInManager.PasswordSignInAsync(user, loginRequest.Password, false, false);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home"); // Redirect to Home page on successful login
                }
                else
                {
                    // Add an error if login fails
                    ModelState.AddModelError(String.Empty, "Invalid user credentials");
                }
            }
            else
            {
                // Add an error if the user does not exist
                ModelState.AddModelError(String.Empty, "There is no such a user in the system");
            }
        }
        return View(loginRequest);
    }

    // Handle the register POST request
    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel registerRequest)
    {
        if (ModelState.IsValid)
        {
            // Check if the username is already taken
            var user = await _userManager.FindByNameAsync(registerRequest.UserName);
            if (user != null)
            {
                ModelState.AddModelError(String.Empty, "This username is already registered");
                return View(registerRequest); // Return to the registration page with an error message
            }

            // Manually hash the password for saving in the database (if needed for additional encryption)
            var passwordCoder = new PasswordHasher<object>();
            string encodedPassword = passwordCoder.HashPassword(null!, registerRequest.Password);

            // Create a new user object with the registration details
            var newUser = new User
            {
                Name = registerRequest.Name,
                Surname = registerRequest.Surname,
                UserName = registerRequest.UserName,
                Password = encodedPassword
            };

            // Add the new user to the database
            _context.Users.Add(newUser);
            _context.SaveChanges();

            // Create an IdentityUser for ASP.NET Identity system
            var identityUser = new IdentityUser(registerRequest.UserName);
            var registerResult = await _userManager.CreateAsync(identityUser, registerRequest.Password);

            if (registerResult.Succeeded)
            {
                // Add the new user to the "Customer" role
                await _userManager.AddToRoleAsync(identityUser, CUSTOMER_ROLE);
                return RedirectToAction("Login", "Auth"); // Redirect to login page after successful registration
            }
            else
            {
                // Throw an exception if user registration fails unexpectedly
                throw new Exception("Unexpected exception while registering user");
            }
        }
        return View(registerRequest); // Return to the registration page if the model is not valid
    }
}
