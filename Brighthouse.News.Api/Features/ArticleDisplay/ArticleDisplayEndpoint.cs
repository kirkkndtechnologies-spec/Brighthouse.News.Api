using Brighthouse.News.Api.Application.Dto;
using Brighthouse.News.Api.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Brighthouse.News.Api.Features.ArticleDisplay
{
    public static class ArticleDisplayEndpoint
    {
        public static void RegisterArticleDisplayEndpoints(this IEndpointRouteBuilder endpointRouteBuilder)
        {
            var root = endpointRouteBuilder
                .MapGroup("articles");

            root.MapGet(pattern: "/", handler: GetArticlesAsync)
                .Produces(StatusCodes.Status500InternalServerError)
                .WithName("GetArticles")
                .WithTags("Brighthouse Articles Display")
                .WithDescription("Get the stored the articles");

            root.MapGet(pattern: "{articleId}", handler: GetArticleAsync)
                .Produces(StatusCodes.Status500InternalServerError)
                .WithName("GetArticle")
                .WithTags("Brighthouse Articles Display")
                .WithDescription("Get the article details");

        }
        
        public static async Task<Results<Ok<BrighthouseResponse>, BadRequest<BrighthouseResponse>, InternalServerError>> GetArticlesAsync(
              [FromServices] ILogger<ArticleDisplayService> logger, IArticleDisplayService articleDisplayService)
        {
            var response = await articleDisplayService.GetArticlesAsync();

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

        public static async Task<Results<Ok<BrighthouseResponse>, BadRequest<BrighthouseResponse>, InternalServerError>> GetArticleAsync(
               [FromServices] ILogger<ArticleDisplayService> logger, [FromQuery] int articleId, IArticleDisplayService articleDisplayService)
        {
            var response = await articleDisplayService.GetArticleAsync(new ArticleDetailInputDto { ArticleId = articleId });

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
