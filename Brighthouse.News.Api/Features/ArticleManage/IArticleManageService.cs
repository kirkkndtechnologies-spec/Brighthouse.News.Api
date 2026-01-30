using Brighthouse.News.Api.Application.Dto;
using Brighthouse.News.Api.Models;

namespace Brighthouse.News.Api.Features.ArticleManage
{
    public interface IArticleManageService
    {

        public Task<BrighthouseResponse> AddArticleAsync(ArticleAddDto input);

        public Task<BrighthouseResponse> UpdateArticleAsync(ArticleUpdateDto input);

        public Task<BrighthouseResponse> DeleteArticleAsync(ArticleDeleteDto input);

    }
}
