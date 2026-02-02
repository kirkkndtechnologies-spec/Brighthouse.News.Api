using Brighthouse.News.Api.Models;

namespace Brighthouse.News.Api.Features.Lookups
{
    public interface ILookupService
    {
        public Task<BrighthouseResponse> GetAuthorsAsync();
    }
}
