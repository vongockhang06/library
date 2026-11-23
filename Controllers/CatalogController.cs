using library.Services.Interfaces;
using library.ViewModels.Catalog;
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
        public async Task<IActionResult>  Index([FromQuery]CatalogQuery query)
        {
            var books = await _catalogService.GetAllBooks(query);
            return View(books);
        }

    }
}
