using System;
using System.Collections.Generic;

namespace library.Models;

public partial class Policy
{
    public short PolicyId { get; set; }

    public short? RoleId { get; set; }

    public short LoanDays { get; set; }

    public short MaxLoans { get; set; }

    public short MaxRenewals { get; set; }

    public decimal FinePerDay { get; set; }

    public short ReservationHoldDays { get; set; }

    public virtual Role? Role { get; set; }
}
