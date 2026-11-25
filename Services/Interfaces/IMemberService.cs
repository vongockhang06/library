using library.ViewModels.Member;

namespace library.Services.Interfaces
{
    public interface IMemberService
    {
        Task<OverviewViewModel> GetOverviewAsync(int memberId, int roleId);
		Task<MyLoansViewModel> GetMyLoansAsync(int memberId, int roleId);

	}
}
