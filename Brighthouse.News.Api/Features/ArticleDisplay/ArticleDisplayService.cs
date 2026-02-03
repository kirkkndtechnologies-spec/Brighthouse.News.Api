using Brighthouse.News.Api.Application.Dto;
using Brighthouse.News.Api.Infrastructure.Repositories;
using Brighthouse.News.Api.Models;
using FluentValidation;
using FluentValidation.Results;
using Mapster;

namespace Brighthouse.News.Api.Features.ArticleDisplay
{
    public class ArticleDisplayService(ILogger<ArticleDisplayService> logger,
        INewsRepository newsRepository, IValidator<ArticleDetailInputDto> getValidator) : IArticleDisplayService
    {
        private readonly ILogger<ArticleDisplayService> _logger = logger;
        private readonly INewsRepository _newsRepository = newsRepository;
        private readonly IValidator<ArticleDetailInputDto> _getValidator = getValidator;

        public async Task<BrighthouseResponse> GetArticleAsync(ArticleDetailInputDto input)
        {
            var response = new BrighthouseResponse();

            try
            {
                var validationResult = await _getValidator.ValidateAsync(input);

                if (validationResult.IsValid)
                {
                    var article = await _newsRepository.GetArticleAsync(input.ArticleId);

                    if (article == null)
                    {
                        validationResult.Errors.Add(new ValidationFailure(nameof(input.ArticleId), "Article does not exist"));

                        response.Success = false;
                        response.Message = "The article could not be retrieved because of validation errors";
                        response.ValidationResult = validationResult;
                        response.Data = null;
                    }
                    else
                    {
                        response.Success = true;
                        response.Message = "Article has been successfully retrieved";
                        response.Data = article.Adapt<ArticleDetailDto>();
                    }
                }
                else
                {
                    response.Success = false;
                    response.Message = "The article could not be retrieved because of validation errors";
                    response.ValidationResult = validationResult;
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "The article could not be retrieved because of an internal server error";
                _logger.LogInformation(ex, "The article could not be retrieved because of an internal server error");
            }

            return response;
        }

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
