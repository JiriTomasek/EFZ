using System.Collections.Generic;
using EFZ.Entities.Entities;

namespace EFZ.Domain.BusinessLogic.Interface
{
    public interface IDeliveryBlProvider
    {
        Delivery GetSingle(long id);

        IList<Delivery> GetCollection(long? orderId = null);
        void AddItem(Delivery entity);
    }
}