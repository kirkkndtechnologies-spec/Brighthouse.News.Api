using Brighthouse.News.Api.Application.Dto;
using Brighthouse.News.Api.Infrastructure.Repositories;
using Brighthouse.News.Api.Models;
using Mapster;

namespace Brighthouse.News.Api.Features.Lookups
{
    public class LookupService(ILogger<LookupService> logger,
        INewsRepository newsRepository) : ILookupService
    {
        private readonly ILogger<LookupService> _logger = logger;
        private readonly INewsRepository _newsRepository = newsRepository;

        /// <summary>
        /// Get a list of <see cref="Author"/>s.
        /// </summary>
        /// <returns>
        /// Generic brighthouse wrapper responses.
        /// </returns>
        public async Task<BrighthouseResponse> GetAuthorsAsync()
        {
            var response = new BrighthouseResponse();

            try
            {
                _logger.LogInformation($"Start getting a list of authors");

                var authors = await _newsRepository.GetAuthorsAsync();

                _logger.LogInformation($"End getting a list of authors : {authors.Count} have been retrieved");

                response.Success = true;
                response.Message = "Authors have been successfully retrieved";
                response.Data = authors.Adapt<List<AuthorLookupDto>>();
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "The authors could not be retrieved because of an internal server error";
                _logger.LogError(ex, "The authors could not be retrieved because of an internal server error");
            }

            return response;
        }
    }
}
