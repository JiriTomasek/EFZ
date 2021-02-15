using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using EFZ.Core.Entities;
using EFZ.Core.NavigationProperty;

namespace EFZ.Entities.Entities
{
    public class UserRole : BaseEntity 
    {
        [NotMapped]
        public new long Id { get; set; }
        public long RoleId { get; set; }
        public long UserId { get; set; }
        public virtual User User { get; set; }
        [JsonIgnore]
        [NavigationProperty]
        public virtual Role Role { get; set; }
    }
}