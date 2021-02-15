using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using EFZ.Core.Entities;
using EFZ.Core.NavigationProperty;

namespace EFZ.Entities.Entities
{
    public class Customer : BaseEntity
    {
        [Required]
        public string Name { get; set; }

        [Required]
        // ReSharper disable once InconsistentNaming
        public string IC { get; set; }

        // ReSharper disable once InconsistentNaming
        public string DIC { get; set; }

        public long? AddressId { get; set; }

        [NavigationProperty]
        [JsonIgnore]
        public Address Address { get; set; }

        public IList<Order> Orders { get; set; }
        public IList<Invoice> Invoices { get; set; }
        public IList<Completion> Completions { get; set; }

        [NavigationProperty]
        [JsonIgnore]
        public IList<User> Users { get; set; }
    }
}