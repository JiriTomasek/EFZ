using System;
using System.ComponentModel.DataAnnotations;
using EFZ.Resources;

namespace EFZ.WebApplication.Models.Settings
{
    public class JobVm
    {
        public Guid Id { get; set; }
        public string JobName { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "valJobLength")]
        public TimeSpan Length { get; set; }
        public bool IsRun { get; set; }
    }
}