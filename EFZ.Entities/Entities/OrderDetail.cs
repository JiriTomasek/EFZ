using System.Xml.Serialization;
using EFZ.Core.Entities;

namespace EFZ.Entities.Entities
{
    public class OrderItem : BaseEntity
    {
        public long OrderId { get; set; }
        public long? DeliveryId { get; set; }
        [XmlElement(ElementName = "Name")]
        public string ProductName { get; set; }
        [XmlElement(ElementName = "QtyOrdered")]
        public long Quantity { get; set; }
        public double UnitPrice { get; set; }
        [XmlElement(ElementName = "UnitDiscountPercentage")]
        public double Discount { get; set; }
        public double TaxRate { get; set; }
        public double TotalNet { get; set; }
        public double TotalTax { get; set; }
        public long Delivered { get; set; }
        public Order Order { get; set; }
        public Delivery Delivery { get; set; }
    }
}