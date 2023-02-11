namespace RPGOnline.Application.DTOs.Requests.Forum
{
    public class SearchPostRequest
    {
        public int Page { get; set; }
        public string? Category { get; set; }
        public string? Search { get; set; }
        public bool OnlyFollowed { get; set; } = false;
        public bool OnlyFavourite { get; set; } = false;
    }
}
