using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using EFZ.Core.Entities;
using EFZ.Core.NavigationProperty;

namespace EFZ.Entities.Entities
{
    public class Company
    {
        [Key]
        public Guid Id { get; set; }
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
    }
}