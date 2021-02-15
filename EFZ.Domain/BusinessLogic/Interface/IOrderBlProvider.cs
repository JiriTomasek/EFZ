using System;
using System.Collections.Generic;
using EFZ.Entities.Entities;

namespace EFZ.Domain.BusinessLogic.Interface
{
    public interface IOrderBlProvider
    {
        Order GetSingle(long id);

        IList<Order> GetCollection();
        void AddItem(Order entity);
        IList<Order> GetCollectionByCustomerId(long customerId, bool all = false);
        Order GetSingleWithDeliveries(long orderId);

    }
}