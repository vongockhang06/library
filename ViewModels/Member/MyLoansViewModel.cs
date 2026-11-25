namespace library.ViewModels.Member
{
	public class MyLoansViewModel
	{
		public int MemberId { get; set; }
		public string FullName { get; set; } = "";
		public List<LoanViewModel> Loans { get; set; } = new();
	}
}
