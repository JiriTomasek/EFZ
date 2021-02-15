using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using EFZ.Core.Entities.Dao;
using EFZ.Domain.BusinessLogic.Interface;
using EFZ.Entities.Entities;

namespace EFZ.Domain.BusinessLogic.Impl
{
    public class InvoiceBlProvider : IInvoiceBlProvider
    {
        private readonly ICommonDao<Invoice> _invoiceDao;

        public InvoiceBlProvider(IBaseDaoFactory daoFactory)
        {
            _invoiceDao = daoFactory.GetDao<Invoice>();

        }

        public Invoice GetSingle(long id)
        {
             return _invoiceDao.GetSingle(t => t.Id.Equals(id));
        }

        public IList<Invoice> GetCollection(long? customerId = null)
        {
            return _invoiceDao.GetCollection(t => (customerId==null || t.CustomerId.Equals(customerId)) && !t.IsCompleted, true).ToList();
        }

        public void AddItem(Invoice entity)
        {
            _invoiceDao.AddItem(entity);
        }

       

        
    }
}