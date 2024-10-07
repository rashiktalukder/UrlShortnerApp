using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Text.Json.Serialization;
using UrlShortener.UI.Models;

namespace UrlShortener.UI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _configuration;

        public HomeController(ILogger<HomeController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> ShortenUrl(string originalUrl)
        {
            if (string.IsNullOrEmpty(originalUrl))
            {
                ViewBag.Message = "Please provide a valid URL.";
                return View("Index");
            }

            //var scheme = HttpContext.Request.Scheme; // "http" or "https"
            //var host = HttpContext.Request.Host; //localhost or www
            var host = _configuration["apiUrl"];

            var apiUrl = $"{host}/api/url/shorten";

            using (var client = new HttpClient())
            {
                var response = await client.PostAsync($"{apiUrl}?originalUrl={originalUrl}", null);
                if (response.IsSuccessStatusCode)
                {
                    var shortUrl = await response.Content.ReadAsStringAsync();
                    var urlObj = JsonConvert.DeserializeObject<Url>(shortUrl);
                    ViewBag.ShortUrl = urlObj.ShortenedUrl;
                }
            }
            return View("Index");
        }
        public async Task<IActionResult> DisplayUrls()
        {
            //var scheme = HttpContext.Request.Scheme; // "http" or "https"
            var host = _configuration["apiUrl"]; //localhost or www

            var apiUrl = $"{host}/api/url/urls";
            using (var client = new HttpClient())
            {
               var response = await client.GetAsync(apiUrl);
                if (response.IsSuccessStatusCode)
                {
                    var urls = await response.Content.ReadFromJsonAsync<List<Url>>();
                    return View(urls);
                }
                else
                {
                    ViewBag.Message = "Error retrieving URLs.";
                }
            }
            return View(new List<Url>());
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
