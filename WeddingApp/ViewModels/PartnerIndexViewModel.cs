using WeddingApp.Models;

namespace WeddingApp.ViewModels
{
    public class PartnerIndexViewModel
    {
        public IEnumerable<Partner> Partners { get; set; } = [];
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public string SortBy { get; set; } = "name";
        public string SortDir { get; set; } = "asc";

        public string FlipDir => SortDir == "asc" ? "desc" : "asc";

        public string SortIcon(string column) =>
            SortBy != column ? "↕" : SortDir == "asc" ? "↑" : "↓";
    }
}
