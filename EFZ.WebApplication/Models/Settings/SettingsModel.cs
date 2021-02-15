using System.Collections.Generic;
using EFZ.Entities.Entities;
using Microsoft.Extensions.FileSystemGlobbing.Internal;

namespace EFZ.WebApplication.Models.Settings
{
    public class SettingsModel : BaseIndexModel
    {
        public IList<UserVm> Users { get; set; } = new List<UserVm>();
        public IList<CustomerVm> Customers { get; set; } = new List<CustomerVm>();

        public IList<Job> Jobs { get; set; } = new List<Job>();

        public IList<JobScheduler> JobSchedulers { get; set; } = new List<JobScheduler>();

        public CompanyVm Company { get; set; }
        public List<string> XmlFiles { get; set; } = new List<string>();
        public List<string> AttachmentScansFiles { get; set; } = new List<string>();
    }
}