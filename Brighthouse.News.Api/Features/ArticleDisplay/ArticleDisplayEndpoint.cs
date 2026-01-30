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
                .MapGroup(string.Empty);

            root.MapGet(pattern: "articles", handler: GetArticlesAsync)
                .Produces(StatusCodes.Status500InternalServerError)
                .WithName("GetArticles")
                .WithTags("Brighthouse Articles Display")
                .WithDescription("Get the stored the articles");

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
    }
}
