using System;
using System.Collections.Generic;

namespace Web_Programming_Proje.Models;

public partial class Role
{
    public long RoleID { get; set; }

    public string RoleName { get; set; } = null!;

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
