using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;
using EFZ.Core.Entities.Dao;
using EFZ.Domain.BusinessLogic.Interface;
using EFZ.Entities.Entities;
using Microsoft.EntityFrameworkCore;

namespace EFZ.Domain.BusinessLogic.Impl
{
    public class OrderBlProvider : IOrderBlProvider
    {
        private readonly ICommonDao<Order> _orderDao;

        public OrderBlProvider(IBaseDaoFactory daoFactory)
        {
            _orderDao = daoFactory.GetDao<Order>();
        }
        public Order GetSingle(long id)
        {
            return _orderDao.GetSingle(t => t.Id.Equals(id));
        }

        public IList<Order> GetCollection()
        {
            return _orderDao.GetCollection(null, true).ToList();
        }

        public void AddItem(Order entity)
        {
            _orderDao.AddItem(entity);
        }

        public IList<Order> GetCollectionByCustomerId(long customerId, bool all)
        {
            
            var list = all ? _orderDao.GetCollection(t=> t.CustomerId.Equals(customerId), true, true, Including).ToList() : _orderDao.GetCollection(t => t.CustomerId.Equals(customerId) && (!t.Deliveries?.Any(x => x.DeliveryAll) ?? true), true, true, Including).ToList();
            return list;
        }

        private IQueryable<Order> Including(IQueryable<Order> databaseContent)
        {
            return databaseContent.Include(t => t.Deliveries);
        }

        public Order GetSingleWithDeliveries(long orderId)
        {
            return _orderDao.GetSingle(t => t.Id.Equals(orderId), true, true, Including);
        }
    }
}