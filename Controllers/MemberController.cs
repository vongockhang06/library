using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using library.Services.Interfaces;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore.Metadata;
namespace library.Controllers
{
    [Authorize]
    public class MemberController : Controller
    {
        private readonly IMemberService _memberService;
        public MemberController(IMemberService memberService)
        {
            _memberService = memberService;
        }

        public async  Task<IActionResult> Overview()
        {
            var memberId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var roleId = int.Parse(User.FindFirstValue(ClaimTypes.Role)!);
            var vm = await _memberService.GetOverviewAsync(memberId,roleId);
            return View(vm);
        }

        public async Task<IActionResult> MyLoans()
        {
			var memberId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
			var roleId = int.Parse(User.FindFirstValue(ClaimTypes.Role)!);
			var vm = await _memberService.GetMyLoansAsync(memberId, roleId);
            return View(vm);
		}

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Reserve(int bookid)
        {
            var memberId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var result = await _memberService.ReserveAsync(memberId, bookid);
            TempData[result.Succeeded ? "Success" : "Error"] = result.Message;
            return RedirectToAction("BookDetails", "Catalog", new { id = bookid });

        }
    }
}
