using System;
using System.Collections.Generic;

namespace Web_Programming_Proje.Models;

public partial class OrderProduct
{
    public long OrderProductID { get; set; }

    public long OrderID { get; set; }

    public long ProductID { get; set; }

    public string ProductName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }

    public decimal TotalPrice { get; set; }  

    public virtual Order Order { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}
