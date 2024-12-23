using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web_Programming_Proje.Data;
using Web_Programming_Proje.Models;
using Web_Programming_Proje.ViewModels;

namespace Web_Programming_Proje.Controllers
{
    // VehicleController handles CRUD operations for vehicles/products
    public class VehicleController : Controller
    {
        private readonly StoreDbContext _context;

        // Constructor to initialize the DbContext
        public VehicleController(StoreDbContext storeDbContext)
        {
            _context = storeDbContext;
        }

        // GET method to retrieve all vehicles with optional search and category filters, and pagination
        [HttpGet]
        public IActionResult AllVehicles(string searchQuery, long? categoryID, int page = 1)
        {
            const int PageSize = 10;  // Set the page size for pagination

            // Fetch all categories for filtering purposes
            var categories = _context.Categories.ToList();

            // Start with all products that are not deleted
            var vehicles = _context.Products.AsQueryable().Where(p => p.IsDeleted != true);

            // Apply search query if provided
            vehicles = string.IsNullOrEmpty(searchQuery)
                ? vehicles
                : vehicles.Where(v => v.ProductName!.ToLower().Contains(searchQuery.ToLower()));

            // Apply category filter if categoryID is provided
            if (categoryID != null)
            {
                vehicles = vehicles.Where(v => v.CategoryID == categoryID);
            }

            // Calculate total vehicles and pages for pagination
            int totalVehicles = vehicles.Count();
            int totalPages = (int)Math.Ceiling(totalVehicles / (double)PageSize);

            // Apply pagination
            vehicles = vehicles.Skip((page - 1) * PageSize).Take(PageSize);

            // Prepare the view model to pass to the view
            var viewModel = new VehicleFilterViewModel
            {
                Categories = categories,
                SelectedCategoryID = categoryID,
                Vehicles = vehicles.ToList(),
                SearchQuery = searchQuery
            };

            // Pass current page and total pages to the view for pagination display
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;

            return View(viewModel);
        }

        // GET method to display the details of a specific vehicle
        public IActionResult Details(int itemid)
        {
            // Fetch the product details by its ID
            var vehicle = _context.Products.FirstOrDefault(product => product.ProductID == itemid);
            return View(vehicle);
        }

        // GET method to show the vehicle creation form (only accessible by Admin)
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST method to handle the creation of a new vehicle
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(CreateProductView product)
        {
            if (ModelState.IsValid)
            {
                // Check if the product already exists in the database
                var newProductName = product.Brand + " " + product.Model;
                var controlProduct = _context.Products.FirstOrDefault(p => p.ProductName == newProductName);

                if (controlProduct != null && controlProduct.IsDeleted != true)
                {
                    ModelState.AddModelError("", "Böyle araç daha önce kaydedilmiş");
                    return View(product);
                }

                // Handle file upload (image) for the product
                if (product.FileName != null && product.FileName.Length > 0)
                {
                    var fileName = Path.GetFileName(product.FileName.FileName); // Get the file name
                    var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");

                    // Ensure the directory exists
                    if (!Directory.Exists(uploadPath))
                    {
                        Directory.CreateDirectory(uploadPath);
                    }

                    var filePath = Path.Combine(uploadPath, fileName);
                    // Save the file to the server
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await product.FileName.CopyToAsync(stream);
                    }

                    // Fetch the category of the product
                    var category = _context.Categories.FirstOrDefault(c => c.CategoryID == product.CategoryID);

                    // Create a new product object and save it to the database
                    var newProduct = new Product
                    {
                        ProductName = newProductName,
                        Brand = product.Brand,
                        Model = product.Model,
                        Price = product.Price,
                        StockAmount = product.StockAmount,
                        Color = product.Color,
                        TopSpeed = product.TopSpeed,
                        FuelConsume = product.FuelConsume,
                        Description = product.Description,
                        Category = category!,
                        Image = "/uploads/" + fileName // Store the file path
                    };

                    _context.Products.Add(newProduct);
                    _context.SaveChanges();

                    // Redirect to the Create page after successful creation
                    return RedirectToAction("Create", "Vehicle");
                }
                else
                {
                    ModelState.AddModelError("", "Dosya yükleme başarısız.");
                }
            }
            return View(product);
        }

        // GET method to display the edit form for a specific vehicle (only accessible by Admin)
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(int itemid)
        {
            var product = _context.Products.FirstOrDefault(p => p.ProductID == itemid);

            var productView = new CreateProductView
            {
                ProductID = product!.ProductID,
                ProductName = product.ProductName,
                Brand = product.Brand,
                Model = product.Model,
                Color = product.Color,
                TopSpeed = product.TopSpeed,
                Price = product.Price,
                StockAmount = product.StockAmount,
                FuelConsume = product.FuelConsume,
                Description = product.Description,
                CategoryID = product.CategoryID,
            };

            return View(productView);
        }

        // POST method to handle the editing of an existing vehicle
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Edit(CreateProductView productRequest)
        {
            if (ModelState.IsValid)
            {
                // Fetch the product to edit
                var editProduct = _context.Products.FirstOrDefault(p => p.ProductID == productRequest.ProductID);
                if (editProduct == null)
                {
                    ModelState.AddModelError("", "Böyle bir ürün bulunamadı");
                }

                // Handle file upload if a new file is provided
                if (productRequest.FileName != null && productRequest.FileName.Length > 0)
                {
                    var fileName = Path.GetFileName(productRequest.FileName.FileName);
                    var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");

                    // Ensure the directory exists
                    if (!Directory.Exists(uploadPath))
                    {
                        Directory.CreateDirectory(uploadPath);
                    }

                    var filePath = Path.Combine(uploadPath, fileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        productRequest.FileName.CopyToAsync(stream);
                    }

                    // Update the product image
                    editProduct!.Image = "/uploads/" + fileName;
                }

                // Update product properties
                var category = _context.Categories.FirstOrDefault(c => c.CategoryID == productRequest.CategoryID);
                var editProductName = productRequest.Brand + " " + productRequest.Model;

                editProduct!.ProductName = editProductName;
                editProduct.Brand = productRequest.Brand;
                editProduct.Model = productRequest.Model;
                editProduct.Color = productRequest.Color;
                editProduct.TopSpeed = productRequest.TopSpeed;
                editProduct.Price = productRequest.Price;
                editProduct.StockAmount = productRequest.StockAmount;
                editProduct.FuelConsume = productRequest.FuelConsume;
                editProduct.Description = productRequest.Description;
                editProduct.Category = category!;
                _context.SaveChanges();

                return View();
            }

            if (!ModelState.IsValid)
            {
                // Log errors if model validation fails
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine(error.ErrorMessage);
                }
                return View(productRequest);
            }
            return View(productRequest);
        }

        // Method to mark a vehicle as deleted (soft delete)
        public async Task<ActionResult> Delete(long itemid)
        {
            var willRemoveProduct = await _context.Products.FirstOrDefaultAsync(p => p.ProductID == itemid);

            if (willRemoveProduct == null)
            {
                return NotFound(); // Return 404 if the product is not found
            }

            // Mark the product as deleted
            willRemoveProduct.IsDeleted = true;
            await _context.SaveChangesAsync();

            // Redirect to the list of all vehicles after deletion
            return RedirectToAction("AllVehicles", "Vehicle");
        }
    }
}
