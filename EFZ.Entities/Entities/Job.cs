using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using EFZ.Core.Entities;
using EFZ.Core.NavigationProperty;

namespace EFZ.Entities.Entities
{
    public class Job
    {
        [Key]
        public Guid Id { get; set; }
        public string JobName { get; set; }
        public TimeSpan Length { get; set; }
        public bool IsRun { get; set; }

        [NavigationProperty]
        [JsonIgnore]
        public IEnumerable<JobScheduler> JobSchedulers { get; set; }
    }
}