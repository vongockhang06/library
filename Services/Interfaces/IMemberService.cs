using library.ViewModels.Member;
using System.Threading.Tasks;

namespace library.Services.Interfaces
{
    public interface IMemberService
    {
        Task<OverviewViewModel> GetOverviewAsync(int memberId, int roleId);
		Task<MyLoansViewModel> GetMyLoansAsync(int memberId, int roleId);
        Task<ReservationResult> ReserveAsync(int memberId, int bookId);

    }
}
