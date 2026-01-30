using Brighthouse.News.Api.Application.Dto;
using Brighthouse.News.Api.Infrastructure.Repositories;
using Brighthouse.News.Api.Models;
using Mapster;

namespace Brighthouse.News.Api.Features.ArticleDisplay
{
    public class ArticleDisplayService(ILogger<ArticleDisplayService> logger,
        NewsRepository newsRepository) : IArticleDisplayService
    {
        private readonly ILogger<ArticleDisplayService> _logger = logger;
        private readonly NewsRepository _newsRepository = newsRepository;

        public async Task<BrighthouseResponse> GetArticlesAsync()
        {
            var response = new BrighthouseResponse();

            try
            {
                var articles = await _newsRepository.GetArticlesAsync();

                response.Success = true;
                response.Message = "Articles have been successfully retrieved";
                response.Data = articles.Adapt<List<ArticleDto>>();
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "The articles could not be retrieved because of an internal server error";
                _logger.LogInformation(ex, "The articles could not be retrieved because of an internal server error");
            }

            return response;
        }
    }
}
