using Brighthouse.News.Api.Domain;
using Brighthouse.News.Api.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Brighthouse.News.Api.Infrastructure.Repositories
{
    public class NewsRepository(NewsDbContext dbContext) : INewsRepository
    {
        private readonly NewsDbContext _dbContext = dbContext;

        public async Task<List<Article>> GetArticlesAsync()
        {
            return await _dbContext.Articles.Include(s => s.Author).ToListAsync();
        }

        public async Task<Article?> GetArticleAsync(int id)
        {
            return await _dbContext.Articles.FirstOrDefaultAsync(f => f.Id == id);
        }

        public async Task<Article?> GetArticleByTitleAndAuthorAsync(int authorId, string title)
        {
            return await _dbContext.Articles.FirstOrDefaultAsync(f => f.AuthorId == authorId && f.Title == title);
        }

        public async Task<int> AddArticleAsync(Article article)
        {
            article.PublishDate = DateTime.Now;

            await _dbContext.Articles.AddAsync(article);
            await _dbContext.SaveChangesAsync();

            return article.Id;
        }

        public async Task UpdateArticleAsync(Article article)
        {
            var articleToUpdate = _dbContext.Articles.FirstOrDefault(f => f.Id == article.Id);

            if (articleToUpdate != null)
            {
                articleToUpdate.AuthorId = article.Id;
                articleToUpdate.Title = article.Title;
                articleToUpdate.Summary = article.Summary;
                articleToUpdate.Content = article.Content;
                articleToUpdate.PublishDate = article.PublishDate;

                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task DeleteArticleAsync(int id)
        {
            var article = _dbContext.Articles.FirstOrDefault(f => f.Id == id);

            if (article != null)
            {
                _dbContext.Articles.Remove(article);

                await _dbContext.SaveChangesAsync();
            }
        }

    }
}
