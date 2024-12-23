using System;
using System.Collections.Generic;

namespace Web_Programming_Proje.Models;

public partial class Order
{
    public long OrderID { get; set; }

    public long UserID { get; set; }

    public long PaymentID { get; set; }

    public DateTime OrderDate { get; set; }

    public DateTime DeliveryDate { get; set; }

    public string ShippingAddress { get; set; } = null!;

    public bool IsDelivered { get; set; } 

    public virtual ICollection<OrderProduct> OrderProducts { get; set; } = new List<OrderProduct>();

    public virtual Payment Payment { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
