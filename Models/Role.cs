using System;
using System.Collections.Generic;

namespace library.Models;

public partial class Role
{
    public short RoleId { get; set; }

    public string RoleName { get; set; } = null!;

    public virtual ICollection<Policy> Policies { get; set; } = new List<Policy>();

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
