using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using EFZ.Core.Entities;
using EFZ.Core.NavigationProperty;

namespace EFZ.Entities.Entities
{
    public class AttachmentComplete : BaseEntity
    {
        [NotMapped]
        public new long Id { get; set; }
        public long AttachmentId { get; set; }

        public long CompletionId { get; set; }
        [JsonIgnore]
        [NavigationProperty]
        public Attachment Attachment { get; set; }
        public  Completion Completion { get; set; }
    }
}