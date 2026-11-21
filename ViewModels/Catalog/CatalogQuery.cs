namespace library.ViewModels.Catalog
{
    public class CatalogQuery
    {
        public string? Q { get; set; }
        public int? AuthorId { get; set; }
        public int? CategoryId { get; set; }
        public string? Language { get; set; }
    }
}
