using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Brighthouse.News.Api.Infrastructure.Contexts
{
    public class SecurityDbContext(DbContextOptions<SecurityDbContext> options) : IdentityDbContext(options)
    {
    }
}
