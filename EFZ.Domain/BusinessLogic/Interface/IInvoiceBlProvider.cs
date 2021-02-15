using System.Collections.Generic;
using System.IO;
using EFZ.Entities.Entities;

namespace EFZ.Domain.BusinessLogic.Interface
{
    public interface IInvoiceBlProvider
    {
        Invoice GetSingle(long id);
        IList<Invoice> GetCollection(long? customerId = null);
        void AddItem(Invoice entity);

    }
}