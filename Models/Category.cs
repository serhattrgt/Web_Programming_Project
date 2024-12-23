using System;
using System.Collections.Generic;

namespace Web_Programming_Proje.Models;

public partial class Category
{
    public long CategoryID { get; set; }

    public string CategoryName { get; set; } = null!;

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
