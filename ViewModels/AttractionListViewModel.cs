namespace DiscoverYemen.ViewModels
{
    public class AttractionListViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        public string GovernorateName { get; set; } = string.Empty;
        public List<string> Categories { get; set; } = new List<string>();
    }
}