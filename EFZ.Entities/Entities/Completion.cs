using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata;
using System.Text.Json.Serialization;
using EFZ.Core.Entities;
using EFZ.Core.NavigationProperty;

namespace EFZ.Entities.Entities
{
    public class Completion : BaseEntity
    {

        public long? CustomerId { get; set; }
        public long InvoiceId { get; set; }
        public long? OrderId { get; set; }
        public string OrderNumber { get; set; }
        public string InvoiceNumber { get; set; }
        public DateTime InvoiceDate { get; set; }
        public string CompleteFileName { get; set; }
        public string ServerFileName { get; set; }
        public Order Order { get; set; }
        
        [NavigationProperty]
        public Customer Customer { get; set; }

        [JsonIgnore]
        [NavigationProperty]
        public Invoice Invoice { get; set; }
        [NavigationProperty]
        public IEnumerable<AttachmentComplete> AttachmentCompletes { get; set; }
    }
}