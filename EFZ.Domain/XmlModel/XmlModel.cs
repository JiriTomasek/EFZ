using System.Collections.Generic;
using System.Xml.Serialization;
using EFZ.Entities.Entities;

namespace EFZ.Domain.XmlModel
{
    public class XmlModel
    {

        [XmlArray("Invoices")]
        public Invoice[] Invoices { get; set; }

        [XmlArray("Orders")]
        public Order[] Orders { get; set; }
        [XmlArray("Deliveries")]
        public Delivery[] Deliveries { get; set; }
    }
}