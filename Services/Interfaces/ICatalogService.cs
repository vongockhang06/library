using library.ViewModels.Catalog;

namespace library.Services.Interfaces
{
    public interface ICatalogService
    {
        Task<List<BookCardViewModel>> GetAllBooks();
    }
}