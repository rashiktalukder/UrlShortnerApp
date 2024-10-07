using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UrlShortner.API.Domain.Entities;

namespace UrlShortner.API.Domain.Interfaces
{
    public interface IUrlShortnerRepository
    {
        Task<UrlModel> GetByOriginalUrl(string originalUrl);
        Task<List<UrlModel>> GetAll();
        Task<UrlModel> CreateShortenUrl(string originalUrl);
    }
}
