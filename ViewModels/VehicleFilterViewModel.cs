using Web_Programming_Proje.Models;

namespace Web_Programming_Proje.ViewModels
{
    public class VehicleFilterViewModel
    {
        public long? SelectedCategoryID { get; set; }  
        public List<Category> Categories { get; set; } = new List<Category>();
        public string SearchQuery { get;set; } = string.Empty;
        public List<Product> Vehicles { get; set; } = new List<Product>();
    }

}