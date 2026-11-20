namespace library.ViewModels.Catalog
{
    public class BookCardViewModel
    {
        public string Isbn { get; set; } = "";
        public string Authors { get; set; } = "";
        public string Title { get; set; } = "";
        public string? CoverImageURL { get; set; }
    }
}