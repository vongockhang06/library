using library.Models;
using library.ViewModels.Account;
using library.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace library.Controllers
{
	public class AccountController : Controller
	{
		private readonly IAuthService _authService;
		public AccountController(IAuthService authService)
        {
            _authService = authService;
        }

		[HttpGet, AllowAnonymous]
		public IActionResult SignIn(string? returnUrl=null)
		{
			return View(new SignInViewModel {ReturnUrl=returnUrl});
		}
		[HttpPost, AllowAnonymous, ValidateAntiForgeryToken]
		public async Task<IActionResult> SignIn(SignInViewModel vm)
		{
			if (!ModelState.IsValid) return View(vm);
			var res = await _authService.SignInAsync(vm.Username, vm.Password);
			if (!res.IsSuccess)
			{
				ModelState.AddModelError(string.Empty, res.Error);
				return View(vm);
			}
			var claims = new List<Claim>
			{
				new Claim(ClaimTypes.NameIdentifier, res.UserId.ToString()),
                new Claim(ClaimTypes.Name, res.Username),
                new Claim(ClaimTypes.Role, res.RoleId.ToString())
            };
			var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
			await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));

			if(!string.IsNullOrEmpty(vm.ReturnUrl) && Url.IsLocalUrl(vm.ReturnUrl))
				return Redirect(vm.ReturnUrl);

			return RedirectToAction("Index", "Catalog");
		}
		[HttpPost, ValidateAntiForgeryToken]
		public async Task<IActionResult> LogOut()
		{
			await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
			return RedirectToAction("Index", "Catalog");
		}
		public IActionResult SignUp()
        {
            return View();
        }
        
		
    }
}
