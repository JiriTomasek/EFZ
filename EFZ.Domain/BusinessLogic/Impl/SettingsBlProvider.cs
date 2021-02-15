using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using EFZ.Core;
using EFZ.Core.Entities.Dao;
using EFZ.Domain.BusinessLogic.Interface;
using EFZ.Entities.Entities;
using Microsoft.EntityFrameworkCore.Internal;

namespace EFZ.Domain.BusinessLogic.Impl
{
    public class SettingsBlProvider : ISettingsBlProvider
    {
        private readonly IJobSchedulerBlProvider _jobSchedulerBlProvider;
        private readonly ICommonDao<Customer> _customerDao;
        private readonly ICommonDao<User> _userDao;
        private readonly ICommonDao<Job> _jobDao;
        private readonly ICommonDao<JobScheduler> _jobSchedulerDao;
        private readonly ICommonDao<Company> _companyDao;

        public SettingsBlProvider(IBaseDaoFactory daoFactory, IJobSchedulerBlProvider jobSchedulerBlProvider)
        {
            _jobSchedulerBlProvider = jobSchedulerBlProvider;
            _customerDao = daoFactory.GetDao<Customer>();
            _userDao = daoFactory.GetDao<User>();

            _jobDao = daoFactory.GetDao<Job>();
            _jobSchedulerDao = daoFactory.GetDao<JobScheduler>();
            _companyDao = daoFactory.GetDao<Company>();
        }

        public void AddOrUpdateCustomer(Customer entity, string userIds)
        {
            entity = entity.Id == 0 ? _customerDao.AddItem(entity) : _customerDao.UpdateItem(entity);

            var uIds =  string.IsNullOrEmpty(userIds) ? new List<long>(): userIds.Split(',').Select(long.Parse).ToList();

            foreach (var userId in uIds)
            {
                if (entity.Users == null || entity.Users.Any(u => u.Id.Equals(userId))) continue;
                
                var user = _userDao.GetSingle(u => u.Id.Equals(userId), false);
                if (user == null) continue;

                user.CustomerId = entity.Id;
                _userDao.UpdateItem(user);
            }

            var updatedEntity = _customerDao.GetSingle(t => t.Id.Equals(entity.Id));

            foreach (var updatedEntityUser in updatedEntity.Users)
            {
                if (uIds.Any(t => t.Equals(updatedEntityUser.Id)))
                {
                    continue;
                }
                updatedEntityUser.CustomerId = null;
                _userDao.UpdateItem(updatedEntityUser);

            }
        }

        public Customer GetCustomerById(long id)
        {
            return _customerDao.GetSingle(t => t.Id.Equals(id));

        }

        public IEnumerable<Customer> GetCustomers()
        {
            return _customerDao.GetCollection(null, true);
        }

        public void DeleteCustomer(Customer entityModel)
        {
            if(entityModel != null && entityModel.Id>0)
                _customerDao.DeleteItem(entityModel);
        }

        public void UpdateJob(Job job)
        {
            _jobDao.UpdateItem(job);
            if (job.Id.Equals(Guid.Parse(Constants.JobXmlProcessingId)))
            {
                _jobSchedulerBlProvider.SetNewXmlTime(job.Length);

                if (job.IsRun)
                    _jobSchedulerBlProvider.StartXmlWorker();
                else
                    _jobSchedulerBlProvider.StopXmlWorker();
            }
            else if (job.Id.Equals(Guid.Parse(Constants.JobInvoicingCompletionId)))
            {
                _jobSchedulerBlProvider.SetNewCompletionTime(job.Length);

                if (job.IsRun)
                    _jobSchedulerBlProvider.StartCompletionWorker();
                else
                    _jobSchedulerBlProvider.StartCompletionWorker();
            }
            else if (job.Id.Equals(Guid.Parse(Constants.JobAttachmentScanProcessingId)))
            {
                _jobSchedulerBlProvider.SetNewAttachmentScanTime(job.Length);

                if (job.IsRun)
                    _jobSchedulerBlProvider.StartAttachmentScanWorker();
                else
                    _jobSchedulerBlProvider.StopAttachmentScanWorker();
            }

        }

        public void StartJob(Guid jobId)
        {
            if (jobId.Equals(Guid.Parse(Constants.JobXmlProcessingId)))
                _jobSchedulerBlProvider.StartXmlWorker();
            else if (jobId.Equals(Guid.Parse(Constants.JobInvoicingCompletionId)))
                _jobSchedulerBlProvider.StartCompletionWorker();
            else if (jobId.Equals(Guid.Parse(Constants.JobInvoicingCompletionId)))
                _jobSchedulerBlProvider.StartAttachmentScanWorker();

            UpdateJobStatus(jobId, true);

        }

        public void StopJob(Guid jobId)
        {
            if (jobId.Equals(Guid.Parse(Constants.JobXmlProcessingId)))
                _jobSchedulerBlProvider.StopXmlWorker();
            else if (jobId.Equals(Guid.Parse(Constants.JobInvoicingCompletionId)))
                _jobSchedulerBlProvider.StopCompletionWorker();
            else if (jobId.Equals(Guid.Parse(Constants.JobInvoicingCompletionId)))
                _jobSchedulerBlProvider.StopAttachmentScanWorker();

            UpdateJobStatus(jobId, false);
        }

        public Job GetSingleJob(Guid jobId)
        {
            return _jobDao.GetSingle(t => t.Id.Equals(jobId));
        }

        public JobScheduler GetSingleJobScheduler(long jobId)
        {
            return _jobSchedulerDao.GetSingle(t => t.Id.Equals(jobId));
        }

        public Company GetCompany()
        {
            return _companyDao.GetSingle(t => t.Id.Equals(Guid.Parse(Constants.CompanyId)));
        }

        public void UpdateCompany(Company entityModel)
        {
            _companyDao.UpdateItem(entityModel);
        }

        public void UploadXmlFile(MemoryStream memoryStream, string formFileName)
        {
            try
            {
                
                string path = @"Data/XmlProcessing/";

                if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                if(File.Exists(path + formFileName)) return;

                var file = new FileStream($"{path}{formFileName}", FileMode.Create, FileAccess.Write);
                memoryStream.WriteTo(file);
                file.Close();
                memoryStream.Close();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<string> GetXmlFilesForProcessing()
        {
            var list = new List<string>();

            var path = "Data/XmlProcessing/";
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);

            foreach (string file in Directory.EnumerateFiles(
                @"Data/XmlProcessing/",
                "*.xml",
                SearchOption.TopDirectoryOnly)
            )
            {
                list.Add(file);
                // do something

            }

            return list;
        }

        public void DeleteFile(string path)
        {
            if(string.IsNullOrEmpty(path))
                return;


            if (File.Exists(path)) File.Delete(@path);
        }

        public List<string> GetAttachmentScanFilesForProcessing()
        {
            var list = new List<string>();

            var path = "Data/AttachmentScans/";
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);

            foreach (string file in Directory.EnumerateFiles(
                @"Data/AttachmentScans/",
                "*.*",
                SearchOption.TopDirectoryOnly)
            )
            {
                list.Add(file);
                // do something

            }

            return list;
        }

        public void UploadAttachmentScanFile(MemoryStream memoryStream, string formFileName)
        {
            try
            {
                string path = @"Data/AttachmentScans/";

                if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                if (File.Exists(path + formFileName)) return;

                var file = new FileStream($"{path}{formFileName}", FileMode.Create, FileAccess.Write);
                memoryStream.WriteTo(file);
                file.Close();
                memoryStream.Close();
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        private void UpdateJobStatus(Guid jobId, bool status)
        {
            var job = _jobDao.GetSingle(t => t.Id.Equals(jobId));
            job.IsRun = status;

            _jobDao.UpdateItem(job);
        }

        public IEnumerable<Job> GetJobs()
        {
            return _jobDao.GetCollection(null, true);
        }
        public IEnumerable<JobScheduler> GetJobScheduler(int takeLast)
        {
            return _jobSchedulerDao.GetCollection(null,true).TakeLast(takeLast).OrderByDescending(t=>t.JobTime);
        }

    }
}