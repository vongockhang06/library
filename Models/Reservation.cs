using System;
using System.Collections.Generic;

namespace library.Models;

public partial class Reservation
{
    public int ReservationId { get; set; }

    public int? UserId { get; set; }

    public int? BookId { get; set; }

    public int? FullfilledCopyId { get; set; }

    public DateTime ReservedAt { get; set; }

    public DateTime ExpiresAt { get; set; }

    public string Stat { get; set; } = null!;

    public virtual Book? Book { get; set; }

    public virtual BookCopy? FullfilledCopy { get; set; }

    public virtual User? User { get; set; }
}
