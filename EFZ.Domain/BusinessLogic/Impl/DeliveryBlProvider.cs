using System.Collections.Generic;
using System.Linq;
using EFZ.Core.Entities.Dao;
using EFZ.Domain.BusinessLogic.Interface;
using EFZ.Entities.Entities;

namespace EFZ.Domain.BusinessLogic.Impl
{
    public class DeliveryBlProvider : IDeliveryBlProvider
    {
        private readonly ICommonDao<Delivery> _deliveryDao;


        public DeliveryBlProvider(IBaseDaoFactory daoFactory)
        {
            _deliveryDao = daoFactory.GetDao<Delivery>();
        }

        public Delivery GetSingle(long id)
        {
            return _deliveryDao.GetSingle(t => t.Id.Equals(id), true);
        }

        public IList<Delivery> GetCollection(long? orderId = null)
        {
            return _deliveryDao.GetCollection(t => (orderId == null || t.OrderId.Equals(orderId)) && !t.IsCompleted , true).ToList();
        }

        public void AddItem(Delivery entity)
        {
            _deliveryDao.AddItem(entity);
        }
    }
}