using library.Services.Interfaces;
using library.Data;
using library.ViewModels.Catalog;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace library.Services
{
    public class CatalogService:ICatalogService
    {
        private readonly AppDbContext _context;
        public CatalogService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<CatalogIndexViewModel> GetAllBooks(CatalogQuery query)
        {
            var books = _context.Books.AsNoTracking().AsQueryable();
            var q = query.Q;
            var language = query.Language;
            var authorid = query.AuthorId;
            var categoryid = query.CategoryId;
            var availability = query.Availability;
            // By Q first
            if (!q.IsNullOrEmpty())
            {
                books = books.Where(b =>
                    b.Isbn.Contains(q) ||
                    b.Title.Contains(q) ||
                    b.Authors.Any(a => a.AuthorName.Contains(q)));
            }
            //Filter by authorid
            if (authorid.HasValue)
            {
                books = books.Where(b =>
                    b.Authors.Any(a => a.AuthorId == authorid));
            }
            //Filter by categoryid
            if (categoryid.HasValue) 
            {
                books = books.Where(b =>
                    b.Categories.Any(a => a.CategoryId == categoryid));
            }
            // Filter by language
            if (!language.IsNullOrEmpty())
            {
                books = books.Where(b =>
                    b.Languages == language
                );
            }
            // Filter by availability
            if (availability == "Available")
                books = books.Where(b => b.BookCopies.Any(c => c.Stat == "Available"));
            else if (query.Availability == "Unavailable")
                books = books.Where(b => !b.BookCopies.Any(c => c.Stat == "Available"));

            // Sort
            var option = query.Sort;
            if (option=="Title")
            {
                books=books.OrderBy(b=>b.Title);
            }
            else if (option == "Availability")
            {
                books = books.OrderByDescending(b => b.BookCopies
                                            .Count(c => c.Stat == "Available"))
                                            .ThenBy(b => b.Title);
            }
            else
            {
                books = books.OrderBy(b => b.BookId);

            }
            // Build Search Query
            var res = await books
                .Select(b =>new BookCardViewModel
                {
                    Isbn = b.Isbn,
                    Title = b.Title,
                    CoverImageURL = b.CoverImage,
                    Authors = string.Join(", ", b.Authors.Select(a => a.AuthorName)),
                    TotalCopies = b.BookCopies.Count(),
                    AvailableCopies = b.BookCopies.Count(c => c.Stat == "Available")
                }).ToListAsync();

            // Build dropdowns
            var categories = await _context.Categories.AsNoTracking()
                .OrderBy(c=>c.CategoryId)
                .Select(c => new SelectListItem(c.CategoryName, c.CategoryId.ToString()))
                .ToListAsync();

            var authors = await _context.Authors.AsNoTracking()
                .OrderBy(a => a.AuthorId)
                .Select(a => new SelectListItem(a.AuthorName, a.AuthorId.ToString()))
                .ToListAsync();

            var langauges = await _context.Books.AsNoTracking()
                .Select(b => b.Languages)
                .Distinct()
                .OrderBy(l => l)
                .Select(l => new SelectListItem(l, l))
                .ToListAsync();

            return new CatalogIndexViewModel
            {
                Query = query,
                Books = res,
                Authors = authors,
                Languages = langauges,
                Categories = categories
            };
        }
    }
}
