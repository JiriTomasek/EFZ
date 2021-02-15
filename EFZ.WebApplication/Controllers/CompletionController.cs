using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EFZ.Core.Enums;
using EFZ.Core.Mapping;
using EFZ.Domain.BusinessLogic.Interface;
using EFZ.Entities.Entities;
using EFZ.WebApplication.Middleware;
using EFZ.WebApplication.Models.Completion;
using EFZ.WebApplication.Models.Invoice;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace EFZ.WebApplication.Controllers
{
    [BaseAuthorize(PermissionItem.Member)]
    public class CompletionController : BaseController
    {
        private readonly IEmailSendGridBlProvider _emailSendGridBlProvider;
        private readonly ICompletionBlProvider _completionBlProvider;

        public CompletionController(IEmailSendGridBlProvider emailSendGridBlProvider,ICompletionBlProvider completionBlProvider,IConfiguration cfg, IUserBlProvider userBlProvider) : base(cfg, userBlProvider)
        {
            _emailSendGridBlProvider = emailSendGridBlProvider;
            _completionBlProvider = completionBlProvider;
            IndexModel = new CompletionIndexModel();
        }
        public IActionResult Index()
        {
            var model = (CompletionIndexModel)IndexModel;
            model.Completions = _completionBlProvider.GetCollection()
                .Select(t => MappingUtils.MapToNew<Completion, CompletionVm>(t)).ToList();


            return View(model);
        }
        public IActionResult Customer(long completionId = 0)
        {
            var user = UserBlProvider.GetUser(User);
            var customerId = user.CustomerId;
            var model = (CompletionIndexModel)IndexModel;

            var collection = _completionBlProvider.GetCustomerCollection(customerId, completionId);

            if (customerId != null)
                model.Completions = collection
                    .Select(t => MappingUtils.MapToNew<Completion, CompletionVm>(t)).ToList();


            //if (collection.Count() == 1)
            //    _emailSendGridBlProvider.SendInvoiceCompleteNotification(collection.FirstOrDefault());
            
            return View(model);
        }

        public IActionResult DownloadFile(long completionId)
        {
                var entity = _completionBlProvider.GetSingle(completionId);

            var stream = _completionBlProvider.GetFileForDownload(entity?.ServerFileName);

            if (stream != null)
                return File(stream, "application/pdf", entity?.CompleteFileName);
            return Ok();
        }
    }
}