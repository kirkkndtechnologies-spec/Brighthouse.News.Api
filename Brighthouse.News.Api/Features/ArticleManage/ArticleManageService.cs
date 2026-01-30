using Brighthouse.News.Api.Application.Dto;
using Brighthouse.News.Api.Domain;
using Brighthouse.News.Api.Features.ArticleDisplay;
using Brighthouse.News.Api.Infrastructure.Repositories;
using Brighthouse.News.Api.Models;
using FluentValidation;
using Mapster;

namespace Brighthouse.News.Api.Features.ArticleManage
{
    public class ArticleManageService(ILogger<ArticleDisplayService> logger,
        NewsRepository newsRepository, IValidator<ArticleAddDto> addValidator,
        IValidator<ArticleUpdateDto> updateValidator, IValidator<ArticleDeleteDto> deleteValidator) : IArticleManageService
    {

        private readonly ILogger<ArticleDisplayService> _logger = logger;
        private readonly NewsRepository _newsRepository = newsRepository;
        private readonly IValidator<ArticleAddDto> _addValidator = addValidator;
        private readonly IValidator<ArticleUpdateDto> _updateValidator = updateValidator;
        private readonly IValidator<ArticleDeleteDto> _deleteValidator = deleteValidator;

        public async Task<BrighthouseResponse> AddArticleAsync(ArticleAddDto input)
        {
            var response = new BrighthouseResponse();

            try
            {
                var validationResult = await _addValidator.ValidateAsync(input);

                if (validationResult.IsValid)
                {
                    var article = new Article();
                    article = input.Adapt<Article>();

                    var articleId = await _newsRepository.AddArticleAsync(article);

                    response.Success = true;
                    response.Message = "The article has been successfully added";
                    response.Data = articleId;
                }
                else
                {
                    response.Success = false; ;
                    response.Message = "The article could not be added because of validation errors";
                    response.ValidationResult = validationResult;
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "The articles could not be added because of an internal server error";
                _logger.LogInformation(ex, "The articles could not be added because of an internal server error");
            }

            return response;
        }

        public async Task<BrighthouseResponse> UpdateArticleAsync(ArticleUpdateDto input)
        {
            var response = new BrighthouseResponse();

            try
            {
                var validationResult = await _updateValidator.ValidateAsync(input);

                if (validationResult.IsValid)
                {
                    var article = new Article();
                    article = input.Adapt<Article>();

                    await _newsRepository.UpdateArticleAsync(article);

                    response.Success = true;
                    response.Message = "The article has been successfully updated";
                }
                else
                {
                    response.Success = false; ;
                    response.Message = "The article could not be updated because of validation errors";
                    response.ValidationResult = validationResult;
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "The articles could not be updated because of an internal server error";
                _logger.LogInformation(ex, "The articles could not be updated because of an internal server error");
            }

            return response;
        }

        public async Task<BrighthouseResponse> DeleteArticleAsync(ArticleDeleteDto input)
        {
            var response = new BrighthouseResponse();

            try
            {
                var validationResult = await _deleteValidator.ValidateAsync(input);

                if (validationResult.IsValid)
                {
                    await _newsRepository.DeleteArticleAsync(input.ArticleId);

                    response.Success = true;
                    response.Message = "The article has been successfully deleted";
                }
                else
                {
                    response.Success = false; ;
                    response.Message = "The article could not be deleted because of validation errors";
                    response.ValidationResult = validationResult;
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "The articles could not be deleted because of an internal server error";
                _logger.LogInformation(ex, "The articles could not be deleted because of an internal server error");
            }

            return response;
        }
    }
}
