using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace EFZ.Core.Entities.Dao
{
    public interface ICommonDao<TEntity>
    {
        void SaveChanges();
        IEnumerable<TEntity> AddRangeItems(IEnumerable<TEntity> entities);
        TEntity AddItem(TEntity entity);

        Task<TEntity> AddItemAsync(TEntity entity);

        Task<IdentityResult> CreateAsync(TEntity entity);

        void DeleteRange(IEnumerable<TEntity> deleteList);

        void DeleteItem(TEntity entity);

        Task<IdentityResult> DeleteAsync(TEntity entity);

        IEnumerable<TEntity> UpdateRangeItems(IEnumerable<TEntity> entities);
        TEntity UpdateItem(TEntity businessObject);

        public delegate IQueryable<TEntity> Including(IQueryable<TEntity> databaseContent);

        TEntity GetSingle(Func<TEntity, bool> condition, bool withIncludeRelation = true, bool asNoTracking = true, Including including = null);
        IEnumerable<TEntity> GetCollection(Func<TEntity, bool> condition = null, bool withIncludeRelation = false,
            bool asNoTracking = true, Including including = null);

        Task<TEntity> GetSingleAsync(Expression<Func<TEntity, bool>> condition,
            CancellationToken cancellationToken = default(CancellationToken), bool withIncludeRelation = true,
            bool asNoTracking = true);
    }
}
