using library.Data;
using library.Services.Interfaces;
using library.ViewModels.Member;
using Microsoft.EntityFrameworkCore;

namespace library.Services
{
    public class MemberService:IMemberService
    {
        private readonly AppDbContext _context;
        public MemberService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<OverviewViewModel> GetOverviewAsync(int memberId, int roleId)
        {
       
            var now = DateTime.Now;           

            var maxRenewals = await _context.Policies.AsNoTracking()
                .Where(p => p.RoleId == roleId)
                .OrderBy(p => p.PolicyId)
                .Select(p => (int)p.MaxRenewals)
                .FirstOrDefaultAsync();           

            var loans = await _context.Loans.AsNoTracking()
                .Where(l => l.UserId == memberId
                            && l.ReturnedAt == null          
                            && l.Copy != null && l.Copy.Book != null)
                .OrderBy(l => l.DueAt)                   
                .Select(l => new LoanViewModel
                {
                    LoanId = l.LoanId,
                    CopyId = l.CopyId.Value,             
                    BookId = l.Copy!.Book!.BookId,
                    BookTitle = l.Copy.Book.Title,
                    CoverImage = l.Copy.Book.CoverImage,
                    BorrowedAt = l.BorrowedAt,
                    DueAt = l.DueAt,
                    RenewalsUsed = l.RenewalCount,
                    MaxRenewals = maxRenewals,
                    IsOverdue = l.DueAt < now
                })
                .ToListAsync();

            var fullName = await _context.Users.AsNoTracking()
                .Where(u => u.UserId == memberId)
                .Select(u => u.Name)                      
                .FirstOrDefaultAsync() ?? "";

            var reservations = await _context.Reservations.AsNoTracking()
                .CountAsync(r => r.UserId == memberId && r.Stat == "Ready");   

            return new OverviewViewModel
            {
                MemberId = memberId,
                FullName = fullName,
                ActiveLoans = loans.Count,
                OverdueLoans = loans.Count(l => l.IsOverdue),    
                ActiveReservations = reservations,
                Loans = loans
            };
        }
    
        public async Task<MyLoansViewModel> GetMyLoansAsync(int memberId, int roleId)
        {
			var now = DateTime.Now;

			var maxRenewals = await _context.Policies.AsNoTracking()
				.Where(p => p.RoleId == roleId)
				.OrderBy(p => p.PolicyId)
				.Select(p => (int)p.MaxRenewals)
				.FirstOrDefaultAsync();

			var loans = await _context.Loans.AsNoTracking()
				.Where(l => l.UserId == memberId
							&& l.ReturnedAt == null
							&& l.Copy != null && l.Copy.Book != null)
				.OrderBy(l => l.DueAt)
				.Select(l => new LoanViewModel
				{
					LoanId = l.LoanId,
					CopyId = l.CopyId.Value,
					BookId = l.Copy!.Book!.BookId,
					BookTitle = l.Copy.Book.Title,
					CoverImage = l.Copy.Book.CoverImage,
					BorrowedAt = l.BorrowedAt,
					DueAt = l.DueAt,
					RenewalsUsed = l.RenewalCount,
					MaxRenewals = maxRenewals,
					IsOverdue = l.DueAt < now
				})
				.ToListAsync();

			var fullName = await _context.Users.AsNoTracking()
				.Where(u => u.UserId == memberId)
				.Select(u => u.Name)
				.FirstOrDefaultAsync() ?? "";

			return new MyLoansViewModel
			{
				FullName=fullName,
                MemberId=memberId,
                Loans=loans
			};
		}
    }
}
