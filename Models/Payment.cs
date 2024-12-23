using System;
using System.Collections.Generic;

namespace Web_Programming_Proje.Models;

public partial class Payment
{
    public long PaymentID { get; set; }

    public string PaymentType { get; set; } = null!;

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
