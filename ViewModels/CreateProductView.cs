
using System.ComponentModel.DataAnnotations;

namespace Web_Programming_Proje.Models{
    public class CreateProductView{
        public long ProductID{get;set;}
        public string ProductName {get;set;}= String.Empty;
        [Required]
        public string Brand { get; set; }= String.Empty;
        [Required]
        public string Model { get; set; }= String.Empty;
        [Required]
        public string Color { get; set; }= String.Empty;
        [Required]
        public int TopSpeed { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public int StockAmount { get; set; }
        [Required]
        public double FuelConsume { get; set; }
        [Required]
        public string Description { get; set; } = String.Empty;
        [Required]
        public long CategoryID { get; set; }
        public IFormFile? FileName{get;set;}
    }
}