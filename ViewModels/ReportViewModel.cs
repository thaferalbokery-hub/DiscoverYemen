namespace DiscoverYemen.ViewModels
{
    public class ReportViewModel
    {
        public int TotalUsers { get; set; }
        public int TotalGovernorates { get; set; }
        public int TotalAttractions { get; set; }
        public int TotalHotels { get; set; }
        public int TotalRestaurants { get; set; }
        public int TotalEvents { get; set; }
        public int TotalReviews { get; set; }
        public int TotalBookings { get; set; }
        public List<GovernorateAttractionCount> AttractionsByGovernorate { get; set; } = new();
    }

    public class GovernorateAttractionCount
    {
        public string GovernorateName { get; set; } = string.Empty;
        public int Count { get; set; }
    }
}