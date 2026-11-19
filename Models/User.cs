using System;
using System.Collections.Generic;

namespace library.Models;

public partial class User
{
    public int UserId { get; set; }

    public string UserName { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public string Email { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public string Stat { get; set; } = null!;

    public short? RoleId { get; set; }

    public virtual ICollection<Loan> LoanProcessedByNavigations { get; set; } = new List<Loan>();

    public virtual ICollection<Loan> LoanUsers { get; set; } = new List<Loan>();

    public virtual ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();

    public virtual Role? Role { get; set; }
}
