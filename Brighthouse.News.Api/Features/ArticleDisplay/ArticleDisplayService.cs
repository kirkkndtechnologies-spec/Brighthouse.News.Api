using Brighthouse.News.Api.Application.Dto;
using Brighthouse.News.Api.Domain;
using Brighthouse.News.Api.Infrastructure.Repositories;
using Brighthouse.News.Api.Models;
using FluentValidation;
using FluentValidation.Results;
using Mapster;
using System.Diagnostics.Tracing;

namespace Brighthouse.News.Api.Features.ArticleDisplay
{
    public class ArticleDisplayService(ILogger<ArticleDisplayService> logger,
        INewsRepository newsRepository, IValidator<ArticleDetailInputDto> getValidator) : IArticleDisplayService
    {
        private readonly ILogger<ArticleDisplayService> _logger = logger;
        private readonly INewsRepository _newsRepository = newsRepository;
        private readonly IValidator<ArticleDetailInputDto> _getValidator = getValidator;

        /// <summary>
        /// Get a list of <see cref="Article"/>s
        /// </summary>
        /// <returns>
        /// List of <see cref="Article"/>s
        /// </returns>
        public async Task<BrighthouseResponse> GetArticleAsync(ArticleDetailInputDto input)
        {
            var response = new BrighthouseResponse();

            try
            {
                _logger.LogInformation($"Start the valiation before attempting to get the article details");

                var validationResult = await _getValidator.ValidateAsync(input);

                if (validationResult.IsValid)
                {
                    _logger.LogInformation($"Check if the article exists");

                    var article = await _newsRepository.GetArticleAsync(input.ArticleId);

                    if (article == null)
                    {
                        _logger.LogInformation($"Article with {input.ArticleId} does not exist");

                        validationResult.Errors.Add(new ValidationFailure(nameof(input.ArticleId), "Article does not exist"));

                        response.Success = false;
                        response.Message = "The article could not be retrieved because of validation errors";
                        response.ValidationResult = validationResult;
                        response.Data = null;
                    }
                    else
                    {
                        _logger.LogInformation($"Valiation passed so get the article details");

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
                _logger.LogError(ex, "The article could not be retrieved because of an internal server error");
            }

            return response;
        }

        /// <summary>
        /// Get the details for the selected article
        /// </summary>
        /// <param name="input">The id of the article</param>
        /// <returns>
        /// The details of the selected <see cref="Article"/>
        /// </returns>
        public async Task<BrighthouseResponse> GetArticlesAsync()
        {
            var response = new BrighthouseResponse();

            try
            {
                _logger.LogInformation($"Start getting a list of articles");

                var articles = await _newsRepository.GetArticlesAsync();

                _logger.LogInformation($"End getting a list of articles : {articles.Count} have been retrieved");

                response.Success = true;
                response.Message = "Articles have been successfully retrieved";
                response.Data = articles.Adapt<List<ArticleDto>>();
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "The articles could not be retrieved because of an internal server error";
                _logger.LogError(ex, "The articles could not be retrieved because of an internal server error");
            }

            return response;
        }

    }
}
