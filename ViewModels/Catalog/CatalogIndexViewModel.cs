using Microsoft.AspNetCore.Mvc.Rendering;

namespace library.ViewModels.Catalog
{
    public class CatalogIndexViewModel
    {
        public CatalogQuery Query { get; set; } = new();
        public List<BookCardViewModel> Books { get; set; } = new();
        public List<SelectListItem> Authors { get; set; } = new();
        public List<SelectListItem> Languages { get; set; } = new();
        public List<SelectListItem> Categories { get; set; } = new();
    }
}
