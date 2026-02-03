using Brighthouse.News.Api.Application.Dto;
using Brighthouse.News.Api.Models;

namespace Brighthouse.News.Api.Features.ArticleDisplay
{
    public interface IArticleDisplayService
    {
        /// <summary>
        /// Get a list of <see cref="Article"/>s
        /// </summary>
        /// <returns>
        /// List of <see cref="Article"/>s
        /// </returns>
        public Task<BrighthouseResponse> GetArticlesAsync();

        /// <summary>
        /// Get the details for the selected article
        /// </summary>
        /// <param name="input">The id of the article</param>
        /// <returns>
        /// The details of the selected <see cref="Article"/>
        /// </returns>
        public Task<BrighthouseResponse> GetArticleAsync(ArticleDetailInputDto input);

    }
}
