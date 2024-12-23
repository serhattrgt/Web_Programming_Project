// Import necessary namespaces for MVC functionality
using Microsoft.AspNetCore.Mvc;

namespace Web_Programming_Proje.Controllers;

// HomeController handles the main page (home page) requests
public class HomeController : Controller
{
    // The Index action method is responsible for returning the main view (home page)
    public IActionResult Index()
    {
        return View();  // Return the default view for the home page
    }
}
