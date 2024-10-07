using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using UrlShortner.API.Domain.Entities;
using UrlShortner.API.Domain.Interfaces;
using UrlShortner.API.Infrastructure.Data;

namespace UrlShortner.API.Application.Services
{
    public class UrlShortnerService : IUrlShortnerRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public UrlShortnerService(ApplicationDbContext dbContext)
        {
            this._dbContext = dbContext;
        }
        public async Task<UrlModel> CreateShortenUrl(string originalUrl)
        {
            var shortenUrl = GenerateShortUrl(originalUrl);
            var url = new UrlModel()
            {
                OriginalUrl = originalUrl,
                ShortenedUrl = "short" + shortenUrl + "short",
                SiteName = new Uri(originalUrl).Host
            };

            _dbContext.Add(url);
            await _dbContext.SaveChangesAsync();

            return url;
        }

        public async Task<List<UrlModel>> GetAll()
        {
            return await _dbContext.Urls.ToListAsync();
        }

        public async Task<UrlModel> GetByOriginalUrl(string originalUrl)
        {
            return await _dbContext.Urls.FirstOrDefaultAsync(x => x.OriginalUrl == originalUrl);
        }

        public string GenerateShortUrl(string originalUrl)
        {
            return Guid.NewGuid().ToString().Substring(0, 8);
        }
    }
}
