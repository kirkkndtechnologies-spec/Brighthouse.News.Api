namespace Brighthouse.News.Api.Application.Dto
{
    public class ArticleAddDto
    {
        public int AuthorId { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Summary { get; set; } = string.Empty;

        public string Content { get; set; } = string.Empty;
    }
}
