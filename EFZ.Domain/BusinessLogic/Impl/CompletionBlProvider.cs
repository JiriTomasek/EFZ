using System.Collections.Generic;
using System.IO;
using EFZ.Core.Entities.Dao;
using EFZ.Domain.BusinessLogic.Interface;
using EFZ.Entities.Entities;

namespace EFZ.Domain.BusinessLogic.Impl
{
    public class CompletionBlProvider : ICompletionBlProvider
    {
        private readonly ICommonDao<Completion> _completionDao;

        public CompletionBlProvider(IBaseDaoFactory daoFactory)
        {
            _completionDao = daoFactory.GetDao<Completion>();
        }
        public IEnumerable<Completion> GetCollection()
        {
            return _completionDao.GetCollection(null, true);
        }

        public Completion GetSingle(long completionId)
        {
            return _completionDao.GetSingle(t => t.Id.Equals(completionId));
        }

        public FileStream GetFileForDownload(string serverFileName)
        {
            if (string.IsNullOrEmpty(serverFileName)) return null;
            var templateStream =
                System.IO.File.OpenRead(
                    @"Data/CompletionStorage/" + serverFileName);

            return templateStream;
        }

        public IEnumerable<Completion> GetCustomerCollection(long? customerId, long completionId)
        { 
            return _completionDao.GetCollection(t=> t.CustomerId.Equals(customerId) && (completionId <= 0 || t.Id.Equals(completionId)), true);
        }
    }
}