using Brighthouse.News.Api.Domain;
using Microsoft.EntityFrameworkCore;

namespace Brighthouse.News.Api.Infrastructure.Contexts
{
    public class NewsDbContext(DbContextOptions<NewsDbContext> options) : DbContext(options)
    {
        public DbSet<Article> Articles { get; set; }

        public DbSet<Author> Authors { get; set; }

    }
}
