using System;
using System.Collections.Generic;

namespace library.Models;

public partial class Loan
{
    public int LoanId { get; set; }

    public int? UserId { get; set; }

    public int? CopyId { get; set; }

    public short RenewalCount { get; set; }

    public DateTime BorrowedAt { get; set; }

    public DateTime? ReturnedAt { get; set; }

    public DateTime DueAt { get; set; }

    public int? ProcessedBy { get; set; }

    public virtual BookCopy? Copy { get; set; }

    public virtual User? ProcessedByNavigation { get; set; }

    public virtual User? User { get; set; }
}
