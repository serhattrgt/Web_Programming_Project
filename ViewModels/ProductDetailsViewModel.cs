namespace Web_Programming_Proje.ViewModels
{
    public class ProductDetailViewModel
    {
        public string ProductName { get; set; } = string.Empty; 
        public decimal ProductPrice { get; set; }
        public int ProductQuantity { get; set; }
        public decimal TotalPriceOfProduct => ProductPrice * ProductQuantity;
    }
}