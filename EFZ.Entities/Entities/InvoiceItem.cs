using System;
using System.Net.Mime;
using System.Xml.Serialization;
using EFZ.Core.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace EFZ.Entities.Entities
{
    public class InvoiceItem : BaseEntity
    {
        [XmlIgnore]
        public long InvoiceId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        [XmlElement(ElementName = "QtyOrdered")]
        public long Quantity { get; set; }
        public double UnitPrice { get; set; }
        [XmlElement(ElementName = "UnitDiscountPercentage")]
        public double Discount { get; set; }
        public double TaxRate { get; set; }
        public double TotalNet { get; set; }
        public double TotalTax { get; set; }


    }
}