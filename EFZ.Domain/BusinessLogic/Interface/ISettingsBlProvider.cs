using System;
using System.Collections.Generic;
using System.IO;
using EFZ.Entities.Entities;

namespace EFZ.Domain.BusinessLogic.Interface
{
    public interface ISettingsBlProvider
    {
        void AddOrUpdateCustomer(Customer entity, string userIds);

        Customer GetCustomerById(long id);
        IEnumerable<Customer> GetCustomers();
        void DeleteCustomer(Customer entityModel);
        IEnumerable<Job> GetJobs();
        IEnumerable<JobScheduler> GetJobScheduler(int takeLast);
        void UpdateJob(Job job);
        void StartJob(Guid jobId);
        void StopJob(Guid jobId);
        Job GetSingleJob(Guid jobId);
        JobScheduler GetSingleJobScheduler(long jobId);
        Company GetCompany();
        void UpdateCompany(Company entityModel);
        void UploadXmlFile(MemoryStream memoryStream, string formFileName);
        List<string> GetXmlFilesForProcessing();
        void DeleteFile(string path);
        List<string> GetAttachmentScanFilesForProcessing();
        void UploadAttachmentScanFile(MemoryStream memoryStream, string formFileName);
    }
}