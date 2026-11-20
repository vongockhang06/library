using library.Services.Interfaces;
using library.Data;
using library.ViewModels.Catalog;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.IdentityModel.Tokens;

namespace library.Services
{
    public class CatalogService:ICatalogService
    {
        private readonly AppDbContext _context;
        public CatalogService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<BookCardViewModel>> GetAllBooks(string? search)
        {
            var query = _context.Books.AsNoTracking().AsQueryable();
            if (!search.IsNullOrEmpty())
            {
                query = query.Where(b => 
                    b.Isbn.Contains(search) ||
                    b.Title.Contains(search) ||
                    b.Authors.Any(a => a.AuthorName.Contains(search))
                );
            }
            var res = await query
                .OrderBy(b => b.BookId)
                .Select( b=> new BookCardViewModel
                {
                    Isbn = b.Isbn,
                    Title = b.Title,
                    CoverImageURL = b.CoverImage,
                    Authors = string.Join(", ", b.Authors.Select(a => a.AuthorName))
                }).ToListAsync();
            return res;

        }
    }
}
