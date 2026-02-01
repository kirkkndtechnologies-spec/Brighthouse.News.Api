using Brighthouse.News.Api.Application.Dto;
using Brighthouse.News.Api.Models;

namespace Brighthouse.News.Api.Features.ArticleDisplay
{
    public interface IArticleDisplayService
    {
        public Task<BrighthouseResponse> GetArticlesAsync();

        public Task<BrighthouseResponse> GetArticleAsync(ArticleDetailInputDto input);

    }
}
