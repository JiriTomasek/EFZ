using EFZ.Core.Entities;

namespace EFZ.Entities.Entities
{
    public class Attachment : BaseEntity
    {
        public long? DeliveryId { get; set; }
        public string DeliveryNumber { get; set; }
        public string OrderNumber { get; set; }
        public string FileName { get; set; }
        public string FileType { get; set; }
        public string ServerFileName { get; set; }
        public bool IsCompleted { get; set; }
        public Delivery Delivery { get; set; }
    }
}