using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using EFZ.Core.Entities;
using EFZ.Core.NavigationProperty;

namespace EFZ.Entities.Entities
{
    public class Role : BaseEntity
    {
        public ICollection<UserRole> UserRoles { get; } = (ICollection<UserRole>)new List<UserRole>();
        [Required]

        public string Name { get; set; }
        public string NormalizedName { get; set; }

    }
}