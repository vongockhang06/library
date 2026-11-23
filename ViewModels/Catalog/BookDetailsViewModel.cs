namespace library.ViewModels.Catalog
{
    public class BookDetailsViewModel
    {
        public int BookId { get; set; }//Without this one it is fine to display
        //But after sign in need to once more time type the url
        // With it, we can return URL faster after sign in 
        public string Title { get; set; } = "";
        public string? Description { get; set; }
        public string Publisher { get; set; } = "";
        public string Isbn { get; set; } = "";
        public string Language { get; set; } = "";
        public DateOnly? PublishDate { get; set; }
        public int TotalCopies { get; set; }
        public int Availability { get; set; }
        public string? Shelf { get; set; }
        public string Author { get; set; } = "";
        public string Category { get; set; } ="";
        public string? CoverImageUrl { get; set; }

    }
}
