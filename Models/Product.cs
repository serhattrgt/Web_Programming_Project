using System;
using System.Collections.Generic;

namespace Web_Programming_Proje.Models;

public partial class Product
{
    public long ProductID { get; set; }

    public string? ProductName { get; set; }

    public string Brand { get; set; } = null!;

    public string Model { get; set; } = null!;

    public string Color { get; set; } = null!;

    public int TopSpeed { get; set; }

    public decimal Price { get; set; }

    public int StockAmount { get; set; }

    public double FuelConsume { get; set; }

    public string Description { get; set; } = null!;

    public bool IsDeleted { get; set; }

    public string? Image { get; set; }

    public long CategoryID { get; set; }

    public virtual Category Category { get; set; } = null!;

    public virtual ICollection<OrderProduct> OrderProducts { get; set; } = new List<OrderProduct>();
}
