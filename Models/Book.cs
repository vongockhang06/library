using System;
using System.Collections.Generic;

namespace library.Models;

public partial class Book
{
    public int BookId { get; set; }

    public string Isbn { get; set; } = null!;

    public string Title { get; set; } = null!;

    public string Languages { get; set; } = null!;

    public short Edition { get; set; }

    public DateOnly? PublishedDate { get; set; }

    public string? Descriptions { get; set; }

    public string? CoverImage { get; set; }

    public virtual ICollection<BookCopy> BookCopies { get; set; } = new List<BookCopy>();

    public virtual ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();

    public virtual ICollection<Author> Authors { get; set; } = new List<Author>();

    public virtual ICollection<Category> Categories { get; set; } = new List<Category>();

    public virtual ICollection<Publisher> Publishers { get; set; } = new List<Publisher>();
}
