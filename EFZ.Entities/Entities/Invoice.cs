using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using System.Xml.Serialization;
using EFZ.Core;
using EFZ.Core.Entities;
using EFZ.Core.NavigationProperty;

namespace EFZ.Entities.Entities
{
    public class Invoice : BaseEntity
    {
        public long? CustomerId { get; set; }
        [XmlIgnore]
        public Guid? CompanyId { get; set; } = Guid.Parse(Constants.CompanyId);
        [XmlIgnore]
        public long? OrderId { get; set; }
        public string OrderNumber { get; set; }
        public string InvoiceNumber { get; set; }
        public DateTime InvoiceDate { get; set; }
        public string InvoiceType { get; set; }
        [XmlIgnore]
        public long? InvoiceAddressId { get; set; }
        public double ItemsNet { get; set; }
        public double ItemsTax { get; set; }
        public double ItemsTotal { get; set; }
        public bool IsCompleted { get; set; }
        public int CompletionCounter { get; set; } = 0;

        public string InvoiceText { get; set; }

        [NavigationProperty]
        [JsonIgnore]
        public InvoiceAddress InvoiceAddress { get; set; }
        [XmlIgnore]
        public Order Order { get; set; }
        [XmlIgnore]
        [JsonIgnore]
        [NavigationProperty]
        public Customer Customer { get; set; }
        [XmlIgnore]
        [NavigationProperty]
        public Company Company { get; set; }
        
        [XmlIgnore]
        [NavigationProperty]
        [JsonIgnore]
        public IEnumerable<InvoiceItem> InvoiceItems { get; set; }
        [NotMapped]
        [XmlArray("InvoiceItems")]
        public InvoiceItem[] XmlInvoiceItems { get; set; }
    }
}