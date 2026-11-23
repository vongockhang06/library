using library.Services.Interfaces;
using library.Data;
using library.ViewModels.Catalog;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Mvc.Rendering;
using library.Models;

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
            IQueryable<Book> books = _context.Books.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(query.Q))
            {
                var q = query.Q.Trim();
                books = books.Where(b =>
                    b.Isbn.Contains(q) ||
                    b.Title.Contains(q) ||
                    b.Authors.Any(a => a.AuthorName.Contains(q)));
            }

            if (query.AuthorId.HasValue)
                books = books.Where(b => b.Authors.Any(a => a.AuthorId == query.AuthorId.Value));

            if (query.CategoryId.HasValue)
                books = books.Where(b => b.Categories.Any(c => c.CategoryId == query.CategoryId.Value));

            if (!string.IsNullOrWhiteSpace(query.Language))
                books = books.Where(b => b.Languages == query.Language);

            var availability = query.Availability?.Trim().ToLowerInvariant();
            if (availability == "available")
                books = books.Where(b => b.BookCopies.Any(c => c.Stat == "Available"));
            else if (availability == "unavailable")
                books = books.Where(b => !b.BookCopies.Any(c => c.Stat == "Available"));

            books = query.Sort?.Trim().ToLowerInvariant() switch
            {
                "title" => books.OrderBy(b => b.Title).ThenBy(b => b.BookId),
                "available" => books.OrderByDescending(b => b.BookCopies.Count(c => c.Stat == "Available"))
                                    .ThenBy(b => b.Title),
                _ => books.OrderBy(b => b.BookId)
            };

            var res = await books.Select(b => new BookCardViewModel
            {
                BookId = b.BookId,
                Isbn = b.Isbn,
                Title = b.Title,
                CoverImageURL = b.CoverImage,
                Authors = string.Join(" · ", b.Authors.Select(a => a.AuthorName)),
                Categories = string.Join(" · ", b.Categories.Select(c => c.CategoryName)),
                TotalCopies = b.BookCopies.Count(),
                AvailableCopies = b.BookCopies.Count(c => c.Stat == "Available")
            }).ToListAsync();

            var categories = await _context.Categories.AsNoTracking()
                .OrderBy(c => c.CategoryName)
                .Select(c => new SelectListItem(c.CategoryName, c.CategoryId.ToString()))
                .ToListAsync();

            var authors = await _context.Authors.AsNoTracking()
                .OrderBy(a => a.AuthorName)
                .Select(a => new SelectListItem(a.AuthorName, a.AuthorId.ToString()))
                .ToListAsync();

            var languages = await _context.Books.AsNoTracking()
                .Where(b => b.Languages != null)
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
                Languages = languages,
                Categories = categories
            };
        }

        public async Task<BookDetailsViewModel> GetBookDetails(int id)//book id
        {
            var bookdetails = await _context.Books.AsNoTracking()
                .Where(b => b.BookId == id)
                .Select(b => new BookDetailsViewModel
                {
                    BookId = b.BookId,
                    Title = b.Title,
                    Isbn = b.Isbn,
                    Description = b.Descriptions,
                    Publisher = string.Join(" · ", b.Publishers.Select(p => p.PublisherName)),
                    Language = b.Languages,
                    PublishDate = b.PublishedDate,
                    Shelf = b.BookCopies
                            .Where(c => c.Stat == "Available")   
                            .OrderBy(c => c.CopyId)                         
                            .Select(c => c.ShelfLocation)                    
                            .FirstOrDefault(),                               
                    CoverImageUrl = b.CoverImage,
                    Author = string.Join(" · ", b.Authors.Select(a => a.AuthorName)),
                    Category = string.Join(" · ", b.Categories.Select(c => c.CategoryName)),
                    TotalCopies = b.BookCopies.Count(),
                    Availability = b.BookCopies.Count(c => c.Stat == "Available")
                })
                .FirstOrDefaultAsync();
            return bookdetails;
        }
    }
}
