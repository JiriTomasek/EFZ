using System.ComponentModel.DataAnnotations;
using EFZ.Core.Entities;

namespace EFZ.Entities.Entities
{
    public class Address : BaseEntity
    {
       
        public string StreetName { get; set; }

        public string StreetNumber { get; set; }

        public string AlternativeStreetNumber { get; set; }
        public string PostalCode { get; set; }

        public string City { get; set; }

        public string State { get; set; }
    }
}