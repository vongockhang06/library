using library.Services.Interfaces;
using library.Data;
using library.ViewModels.Catalog;
using Microsoft.EntityFrameworkCore;

namespace library.Services
{
    public class CatalogService:ICatalogService
    {
        private readonly AppDbContext _context;
        public CatalogService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<BookCardViewModel>> GetAllBooks()
        {
            var res = await _context.Books.AsNoTracking()
                .OrderBy(b => b.BookId)
                .Select( b=> new BookCardViewModel
                {
                    BookId = b.BookId,
                    Title = b.Title,
                    CoverImageURL = b.CoverImage
                }).ToListAsync();
            return res;

        }
    }
}
