using Brighthouse.News.Api.Application.Dto;
using Brighthouse.News.Api.Domain;
using Brighthouse.News.Api.Features.ArticleDisplay;
using Brighthouse.News.Api.Infrastructure.Repositories;
using Brighthouse.News.Api.Models;
using FluentValidation;
using FluentValidation.Results;
using Mapster;

namespace Brighthouse.News.Api.Features.ArticleManage
{
    public class ArticleManageService(ILogger<ArticleDisplayService> logger,
        INewsRepository newsRepository, IValidator<ArticleAddDto> addValidator,
        IValidator<ArticleUpdateDto> updateValidator, IValidator<ArticleDetailInputDto> deleteValidator) : IArticleManageService
    {

        private readonly ILogger<ArticleDisplayService> _logger = logger;
        private readonly INewsRepository _newsRepository = newsRepository;
        private readonly IValidator<ArticleAddDto> _addValidator = addValidator;
        private readonly IValidator<ArticleUpdateDto> _updateValidator = updateValidator;
        private readonly IValidator<ArticleDetailInputDto> _deleteValidator = deleteValidator;

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

                    var duplicateArticle = await _newsRepository.GetArticleByTitleAndAuthorAsync(input.AuthorId, input.Title);

                    if(duplicateArticle != null)
                    {
                        validationResult.Errors.Add(new ValidationFailure(nameof(input.Title), "An article already exists for this author with this title"));

                        response.Success = false;
                        response.Message = "The article could not be added because of validation errors";
                        response.ValidationResult = validationResult;
                    }
                    else
                    {
                        var articleId = await _newsRepository.AddArticleAsync(article);

                        response.Success = true;
                        response.Message = "The article has been successfully added";
                        response.Data = articleId;
                    }
                }
                else
                {
                    response.Success = false;
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

                    var existingArticle = await _newsRepository.GetArticleAsync(input.Id);

                    if (article == null)
                    {
                        validationResult.Errors.Add(new ValidationFailure(nameof(input.Id), "Article does not exist"));

                        response.Success = false;
                        response.Message = "The article could not be updated because of validation errors";
                        response.ValidationResult = validationResult;
                    }
                    else
                    {
                        await _newsRepository.DeleteArticleAsync(input.Id);

                        response.Success = true;
                        response.Message = "The article has been successfully updated";
                    }
                }
                else
                {
                    response.Success = false;
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

        public async Task<BrighthouseResponse> DeleteArticleAsync(ArticleDetailInputDto input)
        {
            var response = new BrighthouseResponse();

            try
            {
                var validationResult = await _deleteValidator.ValidateAsync(input);

                if (validationResult.IsValid)
                {
                    var article = await _newsRepository.GetArticleAsync(input.ArticleId);

                    if(article == null)
                    {
                        validationResult.Errors.Add(new ValidationFailure(nameof(input.ArticleId), "Article does not exist"));

                        response.Success = false;
                        response.Message = "The article could not be deleted because of validation errors";
                        response.ValidationResult = validationResult;
                    }
                    else
                    {
                        await _newsRepository.DeleteArticleAsync(input.ArticleId);

                        response.Success = true;
                        response.Message = "The article has been successfully deleted";
                    }
                }
                else
                {
                    response.Success = false;
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
