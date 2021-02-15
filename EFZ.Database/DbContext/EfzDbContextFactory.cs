using EFZ.Core.Database.DbContextFactory;
using Microsoft.EntityFrameworkCore;

namespace EFZ.Database.DbContext
{
    public class EfzDbContextFactory : AbstractDbContextFactory<EfzDbContext>
    {

        protected override EfzDbContext CreateNewInstance(
            DbContextOptions<EfzDbContext> options)
        {
            return new EfzDbContext(options);
        }
    }
}
