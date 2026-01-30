using Brighthouse.News.Api.Application.Dto;
using Brighthouse.News.Api.Domain;
using Mapster;

namespace Brighthouse.News.Api.Application.Mapping
{
    public class NewsMapsterConfiguration : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Article, ArticleDto>()
                    .Map(dest => dest.Id, src => src.Id)
                    .Map(dest => dest.Title, src => src.Title)
                    .Map(dest => dest.Summary, src => src.Summary)
                    .Map(dest => dest.Author, src => $"{src.Author.FirstName} {src.Author.LastName}")
                    .Map(dest => dest.PublishDate, src => src.PublishDate);
        }
    }
}
