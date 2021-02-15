using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using EFZ.Core.Enums;
using EFZ.Core.Validation.Interfaces;
using EFZ.Domain.BusinessLogic.Interface;
using EFZ.Resources;
using EFZ.WebApplication.Middleware;
using EFZ.WebApplication.Models.Attachment;
using EFZ.WebApplication.Models.Delivery;
using EFZ.WebApplication.Models.SelectModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Configuration;

namespace EFZ.WebApplication.Controllers
{
    [BaseAuthorize(PermissionItem.Admin)]
    public class AttachmentController : BaseController
    {
        private readonly ISettingsBlProvider _settingsBlProvider;
        private readonly IAttachmentBlProvider _attachmentBlProvider;

        public AttachmentController(IDeliveryBlProvider deliveryBlProvider,ISettingsBlProvider settingsBlProvider,IAttachmentBlProvider attachmentBlProvider,IConfiguration cfg, IUserBlProvider userBlProvider) : base(cfg, userBlProvider)
        {
            _settingsBlProvider = settingsBlProvider;
            _attachmentBlProvider = attachmentBlProvider;
            IndexModel = new AttachmentIndexModel();
        }
        public IActionResult Index()
        {
            var model = (AttachmentIndexModel)IndexModel;
            model.Attachments = _attachmentBlProvider.GetCollection().Select(AttachmentVm.MapToViewModel).ToList(); ;

            return View(model);
        }

     

        [HttpPost]
        public IActionResult NewAttachment(IFormFile file, AttachmentVm model)
        {
            if (file is null || !ModelState.IsValid) return BadRequest();
            model.FileName = file.FileName;
            model.FileType = file.ContentType;
            
            var entity = AttachmentVm.MapToEntityModel(model);
            

            using var memoryStream = new MemoryStream();
            file.CopyTo(memoryStream);
            IValidationResult upload;
            try
            {
                upload = _attachmentBlProvider.UploadAttachment(memoryStream, entity);
            }
            catch (Exception e)
            {
                return Conflict(e.ToString());
            }
            if (!upload.IsValid) return Conflict(); // ModelState.AddModelError("File error", upload.Message);

            return RedirectToAction("Index", "Attachment");

        }

        [HttpGet]
        public IActionResult NewAttachment()
        {
            ViewBag.Customers =
                _settingsBlProvider.GetCustomers()
                    .Select(t => new SelectModel() { label = t.Name, value = t.Id }).ToList();
            var deliveryVm = new AttachmentVm();


            return PartialView("_NewAttachment", deliveryVm);
        }


        [HttpPost]
        public IActionResult DeleteAttachment(AttachmentVm model)
        {
            var entityModel = AttachmentVm.MapToEntityModel(model);



            _attachmentBlProvider.DeleteAttachment(entityModel);


            return RedirectToAction("Index", "Attachment");
        }

        [HttpGet]
        public IActionResult DeleteAttachment(int attachmentId)
        {
            var customerVm = AttachmentVm.MapToViewModel(_attachmentBlProvider.GetSingle(attachmentId));

            return PartialView("_DeleteAttachment", customerVm);
        }

        public IActionResult DownloadFile(long attachmentId)
        {
            var entity = _attachmentBlProvider.GetSingle(attachmentId);

            var stream = _attachmentBlProvider.GetFileForDownload(entity?.ServerFileName);

            if (stream != null)
                return File(stream, "application/pdf", entity?.FileName);
            return Ok();
        }
    }
}