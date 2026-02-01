using Brighthouse.News.Api.Domain;

namespace Brighthouse.News.Api.Infrastructure.Repositories
{
    public interface INewsRepository
    {

        public Task<List<Article>> GetArticlesAsync();

        public Task<Article?> GetArticleAsync(int id);

        public Task<Article?> GetArticleByTitleAndAuthorAsync(int authorId, string title);

        public Task<int> AddArticleAsync(Article article);

        public Task UpdateArticleAsync(Article article);

        public Task DeleteArticleAsync(int articleId);

    }
}
