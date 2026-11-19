using Microsoft.AspNetCore.Mvc;

namespace library.Controllers
{
	public class AccountController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
