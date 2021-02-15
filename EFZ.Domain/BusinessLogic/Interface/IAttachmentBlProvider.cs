using System.Collections.Generic;
using System.IO;
using EFZ.Core.Validation.Interfaces;
using EFZ.Entities.Entities;

namespace EFZ.Domain.BusinessLogic.Interface
{
    public interface IAttachmentBlProvider
    {

        Attachment GetSingle(long id);

        IList<Attachment> GetCollection();
        void AddItem(Attachment entity);

        void DeleteAttachment(Attachment entityModel);
        IValidationResult UploadAttachment(MemoryStream memoryStream, Attachment entity);
        FileStream GetFileForDownload(string serverFileName);
    }
}