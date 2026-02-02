using Brighthouse.News.Api.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Brighthouse.News.Api.Features.Lookups
{
    public static class LookupEndpoint
    {
        public static void RegisterLookupEndpoints(this IEndpointRouteBuilder endpointRouteBuilder)
        {
            var root = endpointRouteBuilder
            .MapGroup("lookups");

            root.MapGet(pattern: "/authors", handler: GetAuthorsAsync)
                    .Produces(StatusCodes.Status500InternalServerError)
                    .WithName("GetAuthors")
                    .WithTags("Brighthouse Lookups")
                    .WithDescription("Get the lookup values");

        }

        public static async Task<Results<Ok<BrighthouseResponse>, BadRequest<BrighthouseResponse>, InternalServerError>> GetAuthorsAsync(
               [FromServices] ILogger<LookupService> logger, ILookupService lookupService)
        {
            var response = await lookupService.GetAuthorsAsync();

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
