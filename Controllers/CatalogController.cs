using Microsoft.AspNetCore.Mvc;

namespace library.Controllers
{
    public class CatalogController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
