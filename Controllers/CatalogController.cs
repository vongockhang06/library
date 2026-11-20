using library.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace library.Controllers
{
    public class CatalogController : Controller
    {
        private readonly ICatalogService _catalogService;
        public CatalogController(ICatalogService catalogService)
        {
            _catalogService = catalogService;
        }
        public async Task<IActionResult>  Index()
        {
            var books = await _catalogService.GetAllBooks();
            return View(books);
        }

    }
}
