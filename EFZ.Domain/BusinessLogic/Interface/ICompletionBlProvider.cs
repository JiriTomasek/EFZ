using System.Collections.Generic;
using System.IO;
using EFZ.Entities.Entities;

namespace EFZ.Domain.BusinessLogic.Interface
{
    public interface ICompletionBlProvider
    {
        IEnumerable<Completion> GetCollection();
        Completion GetSingle(long completionId);
        FileStream GetFileForDownload(string serverFileName);
        IEnumerable<Completion> GetCustomerCollection(long? customerId, long completionId);
    }
}