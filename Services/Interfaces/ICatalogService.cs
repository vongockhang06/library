using library.ViewModels.Catalog;

namespace library.Services.Interfaces
{
    public interface ICatalogService
    {
        Task<CatalogIndexViewModel> GetAllBooks(CatalogQuery query);
    }
}