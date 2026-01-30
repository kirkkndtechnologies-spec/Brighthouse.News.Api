using Brighthouse.News.Api.Models;

namespace Brighthouse.News.Api.Features.ArticleDisplay
{
    public interface IArticleDisplayService
    {
        public Task<BrighthouseResponse> GetArticlesAsync();
    }
}
