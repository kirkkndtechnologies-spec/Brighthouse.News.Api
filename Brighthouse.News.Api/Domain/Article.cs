using System.ComponentModel.DataAnnotations.Schema;

namespace Brighthouse.News.Api.Domain
{
    public class Article
    {

        public int Id { get; set; }

        [ForeignKey("Author")]
        public int AuthorId { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Summary { get; set; } = string.Empty;

        public string Content { get; set; } = string.Empty;

        public DateTime PublishDate { get; set; }

        public Author Author { get; set; } = default!;

    }
}
