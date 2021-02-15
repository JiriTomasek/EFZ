using EFZ.Core.Database.Dao;
using EFZ.Core.Entities;
using EFZ.Core.Entities.Dao;
using EFZ.Database.DbContext;

namespace EFZ.Database.Dao
{
    public class BaseDaoFactory : IBaseDaoFactory
    {
        private readonly EfzDbContext _context;

        public BaseDaoFactory(EfzDbContext context)
        {
            _context = context;
        }

        public ICommonDao<T> GetDao<T>() where T : class
        {
            return new CommonDao<T>(_context);
        }


    }
}
