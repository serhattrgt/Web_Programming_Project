using System;
using System.Collections.Generic;

namespace Web_Programming_Proje.Models;

public partial class Address
{
    public long AddressID { get; set; }

    public string? OpenAddress { get; set; } 

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
