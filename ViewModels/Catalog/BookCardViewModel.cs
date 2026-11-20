namespace library.ViewModels.Catalog
{
    public class BookCardViewModel
    {
        public int BookId { get; set; }
        public string Title { get; set; } = "";
        public string? CoverImageURL { get; set; }
    }
}