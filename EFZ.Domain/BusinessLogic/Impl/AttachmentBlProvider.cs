using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using EFZ.Core.Entities.Dao;
using EFZ.Core.Validation.Impl;
using EFZ.Core.Validation.Interfaces;
using EFZ.Domain.BusinessLogic.Interface;
using EFZ.Entities.Entities;
using Microsoft.EntityFrameworkCore;

namespace EFZ.Domain.BusinessLogic.Impl
{
    public class AttachmentBlProvider : IAttachmentBlProvider
    {
        private readonly ICommonDao<Attachment> _attachmentDao;

        public AttachmentBlProvider(IBaseDaoFactory daoFactory)
        {
            _attachmentDao = daoFactory.GetDao<Attachment>();
        }
        public Attachment GetSingle(long id)
        {
            return _attachmentDao.GetSingle(t => t.Id.Equals(id));
        }

        public IList<Attachment> GetCollection()
        {
            return _attachmentDao.GetCollection(t=> !t.IsCompleted, true, true, Including).ToList();
        }

        private IQueryable<Attachment> Including(IQueryable<Attachment> databaseContent)
        {
            return databaseContent.Include(t => t.Delivery).ThenInclude(o => o.Order).ThenInclude(c => c.Customer);
        }

      
        public void AddItem(Attachment entity)
        {
            _attachmentDao.AddItem(entity);
        }

        public void DeleteAttachment(Attachment entityModel)
        {
            _attachmentDao.DeleteItem(entityModel);
        }

        public IValidationResult UploadAttachment(MemoryStream memoryStream, Attachment entity)
        {
            if (entity is null) throw new ArgumentNullException(nameof(entity));

            entity.ServerFileName= Guid.NewGuid().ToString();
            _attachmentDao.AddItem(entity);

            try
            {
                string filename = $"{entity.ServerFileName}";
                string path = "Data/Attachments/";

                if (!Directory.Exists(path)) Directory.CreateDirectory(path);

                var file = new FileStream($"{path}{filename}", FileMode.Create, FileAccess.Write);
                memoryStream.WriteTo(file);
                file.Close();
                memoryStream.Close();
            }
            catch (Exception e)
            {
                _attachmentDao.DeleteItem(entity);
                throw e;
            }
            return ValidationResult.ResultOk();

        }

        public FileStream GetFileForDownload(string serverFileName)
        {
            if (string.IsNullOrEmpty(serverFileName)) return null;
            var templateStream =
                System.IO.File.OpenRead(
                    @"Data/Attachments/"+serverFileName);

            return templateStream;

        }
    }
}