using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using EFZ.Entities.Entities;
using EFZ.Resources;

namespace EFZ.WebApplication.Models.Completion
{
    public class CompletionVm : BaseVm
    {
        public long CustomerId { get; set; }
        public long InvoiceId { get; set; }
        public long? OrderId { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "valOrderNumber")]
        public string OrderNumber { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "valInvoiceNumber")]
        public string InvoiceNumber { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "valInvoiceDate")]
        public DateTime InvoiceDate { get; set; }
        public string CompleteFileName { get; set; }
        public string PAth { get; set; }

        public string Attachments
        {
            get
            {
                var result = string.Empty;
                foreach (var attachmentComplete in AttachmentCompletes)
                {
                    if (attachmentComplete.Attachment != null)
                        result +=
                            $"{Labels.valDelivery}: {attachmentComplete.Attachment.DeliveryNumber}, {attachmentComplete.Attachment.FileName}";
                }

                return result;
            }
        }
        public Customer Customer { get; set; }

        public IEnumerable<AttachmentComplete> AttachmentCompletes { get; set; }
    }
}