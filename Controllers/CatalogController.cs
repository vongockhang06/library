using library.Services.Interfaces;
using library.ViewModels.Catalog;
using Microsoft.AspNetCore.Mvc;

namespace library.Controllers
{
    public class CatalogController : Controller
    {
        private readonly ICatalogService _catalogService;
		private bool IsMember => User.Identity?.IsAuthenticated == true;
		public CatalogController(ICatalogService catalogService)
        {
            _catalogService = catalogService;
        }
        [HttpGet]
        public async Task<IActionResult>  Index([FromQuery]CatalogQuery query)
        {
            var books = await _catalogService.GetAllBooks(query);
            return View(IsMember? "MemberIndex" :"Index",books);
        }

        [HttpGet]
        public async Task<IActionResult> BookDetails(int id)
        {
            var bookdetails = await _catalogService.GetBookDetails(id);
            if (bookdetails == null) return NotFound();
            return View(IsMember ? "MemberBookDetails" : "BookDetails", bookdetails);
        }
    }
}
