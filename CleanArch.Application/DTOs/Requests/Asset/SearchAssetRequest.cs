namespace RPGOnline.Application.DTOs.Requests.Asset
{
    public class SearchAssetRequest
    {
        public int Page { get; set; }
        public string? KeyValueName { get; set; }
        public string? Search { get; set; }
        public string PrefferedLanguage { get; set; } = null!;
        public bool? SortingByDate { get; set; } = false;
        public bool? SortingByLikes { get; set; } = false;
        public bool? IfOnlyMyAssets { get; set; } = false;
    }
}
