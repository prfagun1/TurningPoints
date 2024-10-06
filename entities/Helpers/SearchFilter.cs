namespace entities.Helpers
{
    public class SearchFilter
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public string? SortField { get; set; }
        public Enumerators.SortOrder SortType { get; set; }
        public Dictionary<string, object>? Entity { get; set; }
    }
}
