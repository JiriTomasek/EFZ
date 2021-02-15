using System.Text.Json.Serialization;
using EFZ.Core.Entities;
using EFZ.Core.NavigationProperty;

namespace EFZ.Entities.Entities
{
    public class JobLog : BaseEntity
    {
        public int HResult { get; set; }
        [NavigationProperty]
        public JobLog InnerLog { get; set; }
        public virtual string Message { get; set; }
        public virtual string Source { get; set; }
        public virtual string StackTrace { get; set; }
        public long? InnerLogId { get; set; }
        public string Name { get; set; }
    }
}