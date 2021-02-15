using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using EFZ.Core.Entities;
using EFZ.Core.NavigationProperty;

namespace EFZ.Entities.Entities
{
    public class JobScheduler : BaseEntity
    {
        public Guid JobId { get; set; }
        public DateTime JobTime { get; set; }
        public TimeSpan JobLength { get; set; }
        public bool Status { get; set; }

        [NavigationProperty]
        public IEnumerable<JobLog> Logs { get; set; }
        
        [NavigationProperty]
        [JsonIgnore]
        public Job Job { get; set; }
    }
}