using System;
using System.Collections.Generic;

namespace library.Models;

public partial class BookCopy
{
    public int CopyId { get; set; }

    public int? BookId { get; set; }

    public string ShelfLocation { get; set; } = null!;

    public string CopyCondition { get; set; } = null!;

    public string Stat { get; set; } = null!;

    public virtual Book? Book { get; set; }

    public virtual ICollection<Loan> Loans { get; set; } = new List<Loan>();

    public virtual ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
}
