using Microsoft.AspNetCore.Mvc;

namespace library.Controllers
{
	public class AccountController : Controller
	{
		public IActionResult Login()
		{
			return View();
		}
	}
}
