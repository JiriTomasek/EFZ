namespace EFZ.Core.Entities.Dao
{
    public interface IBaseDaoFactory
    {
        ICommonDao<T> GetDao<T>() where T : class;
    }
 
}