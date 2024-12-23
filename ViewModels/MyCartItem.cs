namespace Web_Programming_Proje.ViewModels;

public class MyCartItem{

public long ProductID { get; set; }
public string ProductName { get; set; }=string.Empty;
public decimal UnitPrice{ get; set; }
public int Quantity{ get; set; }
public decimal TotalPrice{ get; set; }


}