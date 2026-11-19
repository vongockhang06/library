using System;
using System.Collections.Generic;

namespace library.Models;

public partial class Author
{
    public short AuthorId { get; set; }

    public string AuthorName { get; set; } = null!;

    public virtual ICollection<Book> Books { get; set; } = new List<Book>();
}
