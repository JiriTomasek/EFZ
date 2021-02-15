using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using System.Xml.Serialization;
using EFZ.Core.Entities;
using EFZ.Core.NavigationProperty;

namespace EFZ.Entities.Entities
{
    public class Delivery : BaseEntity
    {
        public long? CustomerId { get; set; }
        [XmlIgnore]
        public long? OrderId { get; set; }
        public string DeliveryNumber { get; set; }
        public string OrderNumber { get; set; }
        public DateTime DeliveryDate { get; set; }
        public bool IsCompleted { get; set; }
        public bool DeliveryAll { get; set; }
        [XmlIgnore]
        [NavigationProperty]
        public Order Order { get; set; }
        [XmlIgnore]
        [JsonIgnore]
        [NavigationProperty]
        public IEnumerable<OrderItem> OrderItems { get; set; }
        [XmlIgnore]
        [NavigationProperty]
        public IEnumerable<Attachment> Attachments { get; set; }

        [NotMapped]
        [XmlArray("OrderItems")]
        public OrderItem[] XmlOrderItems { get; set; }
        [XmlIgnore]
        [JsonIgnore]
        [NavigationProperty]
        public Customer Customer { get; set; }
    }
}