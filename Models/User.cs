using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Web_Programming_Proje.Models;

public partial class User
{
    public long UserID { get; set; }

    public string Name { get; set; } = null!;

    public string Surname { get; set; } = null!;

    public string UserName { get; set; } = null!;

    public string Password { get; set; } = null!;

    public long? AddressID { get; set; }

    public virtual Address Address { get; set; } = new Address();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<Role> Roles { get; set; } = new List<Role>();
}
