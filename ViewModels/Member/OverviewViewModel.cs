namespace library.ViewModels.Member
{
    public class OverviewViewModel
    {
        public int MemberId { get; set; }
        public string FullName { get; set; } = "";
        public int ActiveLoans { get; set; }
        public int OverdueLoans { get; set; }
        public int ActiveReservations { get; set; }
        public List<LoanViewModel> Loans { get; set; } = new();
    }
}
