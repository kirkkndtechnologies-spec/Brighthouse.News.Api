namespace Brighthouse.News.Api.Application.Dto
{
    public class ArticleDto
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Summary { get; set; } = string.Empty;

        public string Author { get; set; } = string.Empty;

        public DateTime PublishDate { get; set; }
    }
}
