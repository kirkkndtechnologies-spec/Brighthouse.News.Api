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

        public async Task<BrighthouseResponse> GetAuthorsAsync()
        {
            var response = new BrighthouseResponse();

            try
            {
                var authors = await _newsRepository.GetAuthorsAsync();

                response.Success = true;
                response.Message = "Authors have been successfully retrieved";
                response.Data = authors.Adapt<List<AuthorLookupDto>>();
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "The authors could not be retrieved because of an internal server error";
                _logger.LogInformation(ex, "The authors could not be retrieved because of an internal server error");
            }

            return response;
        }
    }
}
