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

        [HttpGet]
        public async Task<IActionResult> BookDetails(int id)
        {
            var bookdetails = await _catalogService.GetBookDetails(id);
            if (bookdetails == null) return NotFound();
            return View(bookdetails);
        }
    }
}
