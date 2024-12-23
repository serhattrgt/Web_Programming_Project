namespace Web_Programming_Proje.ViewModels
{ 
    public class OrderViewModel
{
    public long OrderID{ get; set; }
    public DateTime OrderDate { get; set; }
    public bool IsDelivered { get; set; }
    public int Quantity { get; set; }
    public decimal TotalPrice { get; set; }
    public List<string> ProductNames { get; set; } = new List<string>();
}


}