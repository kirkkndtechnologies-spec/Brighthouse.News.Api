using Brighthouse.News.Api.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Brighthouse.News.Api.Infrastructure.Migrations
{
    public static class MigrationManager
    {

        public static IHost MigrateNewsDatabase(this IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                using (var appContext = scope.ServiceProvider.GetRequiredService<NewsDbContext>())
                {
                    try
                    {
                        // This is the key method to apply all pending migrations
                        appContext.Database.Migrate();
                    }
                    catch (Exception ex)
                    {
                        throw;
                    }
                }
            }

            return host;
        }

        public static IHost MigrateSecurityDatabase(this IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                using (var appContext = scope.ServiceProvider.GetRequiredService<SecurityDbContext>())
                {
                    try
                    {
                        // This is the key method to apply all pending migrations
                        appContext.Database.Migrate();
                    }
                    catch (Exception ex)
                    {
                        throw;
                    }
                }
            }

            return host;
        }

    }
}
