using System.Collections.Generic;
using EFZ.WebApplication.Models.Order;

namespace EFZ.WebApplication.Models.Attachment
{
    public class AttachmentIndexModel : BaseIndexModel
    {
        public IList<AttachmentVm> Attachments { get; set; }
    }
}