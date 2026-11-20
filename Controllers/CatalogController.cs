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
        [HttpGet]
        public async Task<IActionResult>  Index(string? search)
        {
            var books = await _catalogService.GetAllBooks(search);
            return View(books);
        }

    }
}
