using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Text.Json.Serialization;
using EFZ.Core.Entities;
using EFZ.Core.NavigationProperty;

namespace EFZ.Entities.Entities
{
    public class User : BaseEntity, IIdentity
    {
        public long? CustomerId { get; set; }
        public string UserName { get; set; }
        public string NormalizedUserName { get; internal set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public string PasswordHash { get; set; }
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public DateTimeOffset? LockoutEnd { get; set; }
        public bool LockoutEnabled { get; set; }
        public int AccessFailedCount { get; set; }
        [NavigationProperty]
        
        public ICollection<UserRole> UserRoles { get; } = (ICollection<UserRole>)new List<UserRole>();

        [JsonIgnore]
        [NavigationProperty]
        public Customer Customer { get; set; }

        public string AuthenticationType { get; set; }
        public bool IsAuthenticated { get; set; }
        public string Name { get; set; }
    }
}