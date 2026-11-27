using library.Data;
using library.Models;
using library.Services.Interfaces;
using library.ViewModels.Member;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

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

        public async Task<ReservationResult> ReserveAsync(int memberId, int bookId)
        {
            var now = DateTime.Now;

            var member = await _context.Users.AsNoTracking()
                .Where(u => u.UserId == memberId)
                .Select(u => new { u.Stat, u.RoleId })
                .FirstOrDefaultAsync();
            if (member == null || member.Stat != "Active")
                return ReservationResult.Fail("Your account cannot place reservations.");

            var title = await _context.Books.AsNoTracking()
                .Where(b => b.BookId == bookId).Select(b => b.Title).FirstOrDefaultAsync();
            if (title == null)
                return ReservationResult.Fail("That book does not exist.");

            var alreadyPending = await _context.Reservations.AsNoTracking().AnyAsync(r =>
                r.UserId == memberId && r.BookId == bookId && r.Stat == ReservationStatus.Pending);
            if (alreadyPending)
                return ReservationResult.Fail($"You already have a pending reservation for \"{title}\".");

            var holdDays = await _context.Policies.AsNoTracking()
                .Where(p => p.RoleId == member.RoleId)
                .OrderBy(p => p.PolicyId)
                .Select(p => (int)p.ReservationHoldDays)
                .FirstOrDefaultAsync();
            if (holdDays <= 0)
                return ReservationResult.Fail("Reservations are not available for your membership type.");

            var expires = now.AddDays(holdDays);
            _context.Reservations.Add(new Reservation
            {
                UserId = memberId,
                BookId = bookId,
                ReservedAt = now,
                ExpiresAt = expires,
                Stat = ReservationStatus.Pending
            });

            try { await _context.SaveChangesAsync(); }
            catch (DbUpdateException)      // the unique index caught a simultaneous duplicate
            {
                return ReservationResult.Fail($"You already have a pending reservation for \"{title}\".");
            }

            return ReservationResult.Ok(
                $"\"{title}\" is reserved until {expires.ToString("dd MMM yyyy", CultureInfo.InvariantCulture)}. A librarian will confirm it.");
        }
    }
}
