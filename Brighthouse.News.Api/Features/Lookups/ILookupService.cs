using Brighthouse.News.Api.Models;

namespace Brighthouse.News.Api.Features.Lookups
{
    /// <summary>
    /// Contract for the lookup service
    /// </summary>
    public interface ILookupService
    {
        /// <summary>
        /// Get a list of <see cref="Author"/>s.
        /// </summary>
        /// <returns>
        /// Generic brighthouse wrapper responses.
        /// </returns>
        public Task<BrighthouseResponse> GetAuthorsAsync();
    }
}
