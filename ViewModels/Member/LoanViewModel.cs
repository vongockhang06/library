using System.Globalization;

namespace library.ViewModels.Member
{
    //"Overview" and "My loans" use the same loan view model because My loans just
    //requires extra field "Borrow at"
    public class LoanViewModel
    {
        public int LoanId { get; set; }
        public int CopyId { get; set; }
        public int BookId { get; set; }
        public string BookTitle { get; set; } = "";
        public string? CoverImage { get; set; }     
        public DateTime BorrowedAt { get; set; }    //Used by My Loans only     
        public DateTime DueAt { get; set; }
        public int RenewalsUsed { get; set; }
        public int MaxRenewals { get; set; }              
        public bool IsOverdue { get; set; }               

        // "Overdue · 24 Oct 2026, 10:00"  /  "Due 03 Nov 2026, 15:00"
        public string DueLabel => (IsOverdue ? "Overdue · " : "Due ")
            + DueAt.ToString("dd MMM yyyy, HH:mm", CultureInfo.InvariantCulture);
    }
}
