using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DinkToPdf;
using DinkToPdf.Contracts;
using EFZ.Core.Entities.Dao;
using EFZ.Core.Enums;
using EFZ.Domain.BusinessLogic;
using EFZ.Domain.BusinessLogic.Interface;
using EFZ.Entities.Entities;
using EFZ.WebApplication.Extensions;
using EFZ.WebApplication.Middleware;
using EFZ.WebApplication.Models.Invoice;
using EFZ.WebApplication.Models.Settings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualBasic;

namespace EFZ.WebApplication.Controllers
{
    [BaseAuthorize(PermissionItem.Admin)]
    public class InvoiceController : BaseController
    {
        private readonly IJobSchedulerBlProvider _jobSchedulerBlProvider;
        private readonly IInvoiceBlProvider _invoiceBlProvider;

        public InvoiceController(IJobSchedulerBlProvider jobSchedulerBlProvider,IInvoiceBlProvider invoiceBlProvider, IConfiguration cfg, IUserBlProvider userBlProvider) : base(cfg, userBlProvider)
        {
            _jobSchedulerBlProvider = jobSchedulerBlProvider;
            _invoiceBlProvider = invoiceBlProvider;
            IndexModel = new InvoiceIndexModel();
        }
        public IActionResult Index()
        {
            var model = (InvoiceIndexModel)IndexModel;
            model.Invoices = _invoiceBlProvider.GetCollection().Select(InvoiceVm.MapToViewModel).ToList();
           
            return View(model);
        }
       
        [HttpGet]
        public IActionResult InvoiceDetail(long invoiceId)
        {
            var invoiceVm = InvoiceVm.MapToViewModel(_invoiceBlProvider.GetSingle(invoiceId));


            return PartialView("_InvoiceDetail", invoiceVm);
        }

        public IActionResult DownloadFile(long invoiceId)
        {
            var invoice = _invoiceBlProvider.GetSingle(invoiceId);

            var file = _jobSchedulerBlProvider.GetInvoicePdfDocument(invoice);

           

            return File(file, "application/pdf");

        }

    }
}