using Brighthouse.News.Api.Application.Dto;
using Brighthouse.News.Api.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Brighthouse.News.Api.Features.ArticleManage
{
    public static class ArticleManageEndpoint
    {

        public static void RegisterArticleManageEndpoints(this IEndpointRouteBuilder endpointRouteBuilder)
        {
            var root = endpointRouteBuilder
                .MapGroup("");

            root.MapPost(pattern: "article", handler: AddArticleAsync)
                .Produces(StatusCodes.Status500InternalServerError)
                .Accepts<ArticleAddDto>("application/json")
                .WithName("AddArticle")
                .WithTags("Brighthouse Article Management")
                .WithDescription("Add a new article")
                .RequireAuthorization();

            root.MapPut(pattern: "article", handler: UpdateArticleAsync)
                .Produces(StatusCodes.Status500InternalServerError)
                .Accepts<ArticleUpdateDto>("application/json")
                .WithName("UpdateArticle")
                .WithTags("Brighthouse Article Management")
                .WithDescription("Update an existing article")
                .RequireAuthorization();

            root.MapDelete(pattern: "article", handler: DeleteArticleAsync)
                .Produces(StatusCodes.Status500InternalServerError)
                .Accepts<ArticleDetailInputDto>("application/json")
                .WithName("DeleteArticle")
                .WithTags("Brighthouse Article Management")
                .WithDescription("Delete an existing article")
                .RequireAuthorization();
        }

        public static async Task<Results<Ok<BrighthouseResponse>, BadRequest<BrighthouseResponse>, InternalServerError>> AddArticleAsync(
              ILogger<ArticleManageService> logger, ArticleAddDto input, IArticleManageService articleDisplayService)
        {
            var response = await articleDisplayService.AddArticleAsync(input);

            if (response.Success)
            {
                return TypedResults.Ok(response);
            }
            else
            {
                if (response.ValidationResult != null && response.ValidationResult.Errors.Any())
                {
                    return TypedResults.BadRequest(response);
                }
                else
                {
                    return TypedResults.InternalServerError();
                }
            }
        }

        public static async Task<Results<Ok<BrighthouseResponse>, BadRequest<BrighthouseResponse>, InternalServerError>> UpdateArticleAsync(
              ILogger<ArticleManageService> logger, ArticleUpdateDto input, IArticleManageService articleDisplayService)
        {
            var response = await articleDisplayService.UpdateArticleAsync(input);

            if (response.Success)
            {
                return TypedResults.Ok(response);
            }
            else
            {
                if (response.ValidationResult != null && response.ValidationResult.Errors.Any())
                {
                    return TypedResults.BadRequest(response);
                }
                else
                {
                    return TypedResults.InternalServerError();
                }
            }
        }

        public static async Task<Results<Ok<BrighthouseResponse>, BadRequest<BrighthouseResponse>, InternalServerError>> DeleteArticleAsync(
              ILogger<ArticleManageService> logger, [FromBody] ArticleDetailInputDto input, IArticleManageService articleDisplayService)
        {
            var response = await articleDisplayService.DeleteArticleAsync(input);

            if (response.Success)
            {
                return TypedResults.Ok(response);
            }
            else
            {
                if (response.ValidationResult != null && response.ValidationResult.Errors.Any())
                {
                    return TypedResults.BadRequest(response);
                }
                else
                {
                    return TypedResults.InternalServerError();
                }
            }
        }
    }
}
