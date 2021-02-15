using System.Collections.Generic;

namespace EFZ.WebApplication.Models.Invoice
{
    public class InvoiceIndexModel : BaseIndexModel
    {

        public IList<InvoiceVm> Invoices { get; set; }
    }
}