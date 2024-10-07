using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Policy;
using UrlShortner.API.Domain.Entities;
using UrlShortner.API.Domain.Interfaces;
using UrlShortner.API.Infrastructure.Data;

namespace UrlShortener.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UrlController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IUrlShortnerRepository _urlShortnerRepository;

        public UrlController(ApplicationDbContext dbContext, IUrlShortnerRepository urlShortnerRepository)
        {
            this._dbContext = dbContext;
            this._urlShortnerRepository = urlShortnerRepository;
        }

        [HttpPost("shorten")]
        public async Task<IActionResult> ShortenUrl(string originalUrl)
        {
            if (string.IsNullOrWhiteSpace(originalUrl))
            {
                return BadRequest("Url can not be Empty!");
            }

            var isExistUrl = await _urlShortnerRepository.GetByOriginalUrl(originalUrl);
            if (isExistUrl is not null)
            {
                return Ok(isExistUrl);
            }

            var url = await _urlShortnerRepository.CreateShortenUrl(originalUrl);

            return Ok(url);
        }

        [HttpGet("urls")]
        public async Task<IActionResult> GetUrls()
        {
            var urlList = await _urlShortnerRepository.GetAll();
            return Ok(urlList);
        }
    }
}
