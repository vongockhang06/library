namespace library.ViewModels.Catalog
{
    public class BookCardViewModel
    {
        public int BookId { get; set; }
        public string Isbn { get; set; } = "";
        public string Authors { get; set; } = "";
        public string Title { get; set; } = "";
        public string? CoverImageURL { get; set; }
        public string Categories { get; set; } = "";
        public int TotalCopies { get; set; }
        public int AvailableCopies { get; set; }
        public bool IsAvailable => AvailableCopies > 0;
        public string AvailableText => $"{AvailableCopies} of {TotalCopies} available";
    }
}