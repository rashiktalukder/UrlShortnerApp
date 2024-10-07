namespace UrlShortener.UI.Models
{
    public class Url
    {
        public int Id { get; set; }
        public string OriginalUrl { get; set; }
        public string ShortenedUrl { get; set; }
        public string SiteName { get; set; }
    }
}
