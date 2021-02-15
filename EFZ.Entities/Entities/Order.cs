using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using System.Xml.Serialization;
using EFZ.Core.Entities;
using EFZ.Core.NavigationProperty;

namespace EFZ.Entities.Entities
{
    public class Order : BaseEntity
    {
        public long CustomerId { get; set; }
        public string OrderNumber { get; set; }

        public DateTime OrderDate { get; set; }
        
        [XmlElement(ElementName = "ItemsNet")]
        public double? TotalNet { get; set; }
        [XmlElement(ElementName = "ItemsTax")]
        public double? TotalTax { get; set; }
        [XmlElement(ElementName = "ItemsTotal")]
        public double? TotalPrice { get; set; }
        [XmlIgnore]
        [JsonIgnore]
        [NavigationProperty]
        public Customer Customer { get; set; }
        [XmlIgnore]
        [NavigationProperty]
        public IEnumerable<OrderItem> OrderItems { get; set; }
        [XmlIgnore]

        public IEnumerable<Delivery> Deliveries { get; set; }
        [NotMapped]
        [XmlArray("OrderItems")]
        public OrderItem[] XmlOrderItems { get; set; }
    }
}