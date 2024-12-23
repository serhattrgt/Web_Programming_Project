using Web_Programming_Proje.Models;

namespace Web_Programming_Proje.ViewModels
{
    public class OrderDetailsViewModel{
        public long OrderID { get; set; }
        public string PaymentType { get; set; } = string.Empty;
        public DateTime OrderDate{ get; set; } 
        public bool IsDelivered { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPriceOfOrder { get; set; }
        public List<ProductDetailViewModel> ProductDetails { get; set; } = new List<ProductDetailViewModel>(); 

    }


    
}


