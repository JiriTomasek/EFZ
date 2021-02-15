using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using EFZ.Core.Entities.Dao;
using EFZ.Core.NavigationProperty;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
namespace EFZ.Core.Database.Dao
{
    public class CommonDao<TEntity> : ICommonDao<TEntity> where TEntity : class
    {
        public DbContext Context;
        public  CommonDao(DbContext context)
        {
            Context = context;
        }

        #region ICommonDao
        public IEnumerable<TEntity> AddRangeItems(IEnumerable<TEntity> entities)
        {
            try
            {
                var table = GetTable();
                table.AddRange(entities);
                
                SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(
                    $"An error occured while AddItem '{typeof(TEntity)}'. {ex.Message}",
                    ex.InnerException);
            }

            return entities;
        }
       

        public async Task<TEntity> AddItemAsync(TEntity entity)
        {
            try
            {

                var table = GetTable();
                await table.AddAsync(entity);

                SaveChanges();
                
            }
            catch (Exception ex)
            {
                throw new Exception(
                    $"An error occured while AddItem '{typeof(TEntity)}'. {ex.Message}",
                    ex.InnerException);
            }

            return entity;
        }

        public IEnumerable<TEntity> UpdateRangeItems(IEnumerable<TEntity> entities)
        {
            try
            {
                var table = GetTable();

                table.UpdateRange(entities);


                SaveChanges();

            }
            catch (Exception ex)
            {
                throw new Exception(
                    $"An error occured while UpdateItem '{typeof(TEntity)}'. {ex.Message}",
                    ex.InnerException);
            }

            return entities;
        }
        public TEntity UpdateItem(TEntity entity)
        {
            try
            {
                
                ClearEntityTracking(entity);

                //ReMapNavigationProperty(entity);


                var table = GetTable();

                table.Update(entity);
                
                SaveChanges();

            }
            catch (Exception ex)
            {
                throw new Exception(
                    $"An error occured while UpdateItem '{typeof(TEntity)}'. {ex.Message}",
                    ex.InnerException);
            }

            return entity;
        }

      

        protected void ClearEntityTracking<TXEntity>(TXEntity entity) where TXEntity : class
        {
            Context.Set<TXEntity>().Local.Clear();
        }



        public async Task<IdentityResult> CreateAsync(TEntity entity)
        {
            try
            {
                var table = GetTable();

                await table.AddAsync(entity);

                SaveChanges();


                return IdentityResult.Success;
            }
            catch (Exception ex)
            {
                return IdentityResult.Failed(new IdentityError { Description = $"Could not insert entity {typeof(TEntity)}. {ex.Message}" });
                
            }

        }
        public async Task<IdentityResult> DeleteAsync(TEntity entity)
        {
            try
            {
                
                var table = GetTable();
                DeleteItem(entity);
                
                return IdentityResult.Success;
            }
            catch (Exception ex)
            {
                return IdentityResult.Failed(new IdentityError { Description = $"Could not insert entity {typeof(TEntity)}. {ex.Message}" });

            }

        }

        public void DeleteItem(TEntity item)
        {
            var table = GetTable();

            //table.Attach(item);

            table.Remove(item);

            SaveChanges();
        }
        public void DeleteRange(IEnumerable<TEntity> deleteList)
        {
            var list = deleteList as TEntity[] ?? deleteList.ToArray();
            if (!list.Any())
            {
                return;
            }

            foreach (var entity in list)
            {
                DeleteItem(entity);
            }
        }

        protected IQueryable<TEntity> GetInclude(IQueryable<TEntity> databaseContent)
        {
            var includes = EntityNavigationHelper.GetNavigations<TEntity>();

            if (includes != null)
            {
                databaseContent = includes(databaseContent);
            }

            return databaseContent;
        }

      

        protected DbSet<TEntity> GetTable()
        {
            return Context.Set<TEntity>();
        }

       
        public void SaveChanges()
        {
            Context.SaveChanges();
        }


        public TEntity AddItem(TEntity entity)
        {
            try
            {
                var table = GetTable();
                table.Add(entity);

                SaveChanges();

            }
            catch (Exception ex)
            {
                throw new Exception(
                    $"An error occured while AddItem '{typeof(TEntity)}'. {ex.Message}",
                    ex.InnerException);
            }
            return entity;
        }
        public TEntity GetSingle(Func<TEntity, bool> condition, bool withIncludeRelation = true,
            bool asNoTracking = true, ICommonDao<TEntity>.Including including = null)
        {
            try
            {
                var databaseContent = GetTable().Cast<TEntity>();
                if (asNoTracking)
                    databaseContent = databaseContent.AsNoTracking();

                if (withIncludeRelation)
                    databaseContent = GetInclude(databaseContent);

                if (including != null)
                {
                    databaseContent = including(databaseContent);
                }
                return !databaseContent.Any() ? null : databaseContent.FirstOrDefault(condition);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public IEnumerable<TEntity> GetCollection(Func<TEntity, bool> condition = null,
            bool withIncludeRelation = false, bool asNoTracking = true,
            ICommonDao<TEntity>.Including including = null)
        {
            try
            {
                var databaseContent = GetTable().Cast<TEntity>();
                if (asNoTracking)
                    databaseContent = databaseContent.AsNoTracking();

                if (withIncludeRelation)
                    databaseContent = GetInclude(databaseContent);

                if (including != null)
                {
                    databaseContent = including(databaseContent);
                }

                if (!databaseContent.Any())
                {
                    return new List<TEntity>();
                }
                else
                {
                    return condition == null ? databaseContent.ToList() : databaseContent.Where(condition).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }

        }
        public async Task<TEntity> GetSingleAsync(Expression<Func<TEntity, bool>> condition, CancellationToken cancellationToken = default(CancellationToken), bool withIncludeRelation = true, bool asNoTracking = true)
        {
            cancellationToken.ThrowIfCancellationRequested();
            try
            {
                var databaseContent = GetTable().Cast<TEntity>();
                if (asNoTracking)
                    databaseContent = databaseContent.AsNoTracking();

                if (withIncludeRelation)
                    databaseContent = GetInclude(databaseContent);

                if (!databaseContent.Any())
                {
                    return null;
                }
                else
                {
                    return await databaseContent.FirstOrDefaultAsync(condition, cancellationToken);
                }

            }
            catch (Exception)
            {
                return null;
            }
        }

      
        #endregion
    }
}
