using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using System.Xml.Serialization;
//DinkToPdf generate pdf
using DinkToPdf;
using DinkToPdf.Contracts;
using Disig.TimeStampClient;
using EFZ.Core;
using EFZ.Core.Entities.Dao;
using EFZ.Core.Mapping;
using EFZ.Core.Validation.Impl;
using EFZ.Core.Validation.Interfaces;
using EFZ.Database.Dao;
using EFZ.Database.DbContext;
using EFZ.Domain.BusinessLogic.Interface;
using EFZ.Domain.XmlModel;
using EFZ.Entities.Entities;
//iTextSharp for merge pdf and digital sign
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.Extensions.Logging;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Tls;
using Org.BouncyCastle.Security;
using PdfSharp.Drawing;

namespace EFZ.Domain.BusinessLogic.Impl
{
    public class JobSchedulerBlProvider :IJobSchedulerBlProvider
    {
        private readonly IConverter _converter;
        private readonly IAttachmentScanHelperProvider _attachmentScanHelper;
        private readonly IEmailSendGridBlProvider _emailSendGridBlProvider;
        private readonly ILogger _logger;

        private readonly Timer _xmlTimer;
        private readonly ICommonDao<JobScheduler> _jobScheduler;
        private readonly ICommonDao<Job> _jobDao;
        private readonly Timer _completionTimer;
        private readonly Timer _attachmentScanTimer;
        private  Task _attachmentTask;
        public JobSchedulerBlProvider(IConverter converter, ILoggerFactory loggerFactory, IAttachmentScanHelperProvider attachmentScanHelper, IEmailSendGridBlProvider emailSendGridBlProvider)
        {
            _converter = converter;
            _attachmentScanHelper = attachmentScanHelper;
            _emailSendGridBlProvider = emailSendGridBlProvider;
            _logger = loggerFactory.CreateLogger<JobSchedulerBlProvider>();

            var daoFactory=  new BaseDaoFactory(new EfzDbContextFactory().Create());

            _xmlTimer = new Timer();

            _completionTimer = new Timer();

            _attachmentScanTimer = new Timer();


            _jobDao = new BaseDaoFactory(new EfzDbContextFactory().Create()).GetDao<Job>();
            _jobScheduler = new BaseDaoFactory(new EfzDbContextFactory().Create()).GetDao<JobScheduler>();

            _attachmentTask = new Task(AttachmentScanProcessingAsync);

            SetupXmlJobTimer();
            SetupCompletionJobTimer();
            SetupAttachmentScanJobTimer();

        }

     

        public void SetNewAttachmentScanTime(TimeSpan time)
        {
            _attachmentScanTimer.Interval = time.TotalMilliseconds;
        }

       

        public void RunAttachmentScansManually()
        {
            AttachmentScanProcessingJob();
        }

        public void StartAttachmentScanWorker()
        {
            _attachmentScanTimer.Stop();
        }

        public void StopAttachmentScanWorker()
        {
            _attachmentScanTimer.Stop();
        }

        public void SetNewXmlTime(TimeSpan time)
        {
            _xmlTimer.Interval = time.TotalMilliseconds;
        }
        public void StartXmlWorker()
        {
            _xmlTimer.Start();
        }

        public void StopXmlWorker()
        {
            _xmlTimer.Stop();
        }
        public void StopCompletionWorker()
        {
            _completionTimer.Stop();
        }

        public void RunCompletionManually()
        {
            CompletionJob();
        }

        public void SetNewCompletionTime(TimeSpan time)
        {
            _completionTimer.Interval = time.TotalMilliseconds;
        }

        public void StartCompletionWorker()
        {

            _completionTimer.Start();
        }
        public void RunXmlManually()
        {
            XmlProcessingJob();
        }
        private void SetupXmlJobTimer()
        {
            _xmlTimer.Elapsed += XmlTimerOnElapsed;

            var job = _jobDao.GetSingle(t => t.Id.Equals(Guid.Parse(Constants.JobXmlProcessingId)), false);
            if (job == null) return;
            _xmlTimer.Interval = job.Length.TotalMilliseconds;

            if (job.IsRun)
                StartXmlWorker();
            else
                StopXmlWorker();
        }

        private void XmlTimerOnElapsed(object sender, ElapsedEventArgs e)
        {
            XmlProcessingJob();
        }

        private void XmlProcessingJob()
        {
            var jobScheduler = new JobScheduler { JobTime = DateTime.Now, JobId = Guid.Parse(Constants.JobXmlProcessingId) };
            var sw = new Stopwatch();
            sw.Start();
            var path = "Data/XmlProcessing/";
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            var filePaths = Directory.GetFiles(@"Data/XmlProcessing/");
            var status = true;
            var jobLogs = new List<JobLog>();
            foreach (var filePath in filePaths)
            {
                if (GetDataFromXml(filePath, jobLogs) && status)
                    status = true;
                else
                {
                    status = false;
                }
            }
            sw.Stop();
            jobScheduler.Status = status;
            jobScheduler.Logs = jobLogs;
            jobScheduler.JobLength = sw.Elapsed;
            _jobScheduler.AddItem(jobScheduler);
        }
        private bool GetDataFromXml(string fileName, List<JobLog> jobLogs)
        {
            var invoiceDao = new BaseDaoFactory(new EfzDbContextFactory().Create()).GetDao<Invoice>();
            var customerDao = new BaseDaoFactory(new EfzDbContextFactory().Create()).GetDao<Customer>();
            var orderDao = new BaseDaoFactory(new EfzDbContextFactory().Create()).GetDao<Order>();
            var deliveryDao = new BaseDaoFactory(new EfzDbContextFactory().Create()).GetDao<Delivery>();

            var status = false;

            var stream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            try
            {
                var serializer = new XmlSerializer(typeof(XmlModel.XmlModel));

                serializer.UnknownNode += new XmlNodeEventHandler(serializer_UnknownNode);
                serializer.UnknownAttribute += new
                    XmlAttributeEventHandler(serializer_UnknownAttribute);

                var xml = (XmlModel.XmlModel) serializer.Deserialize(stream);


                if (xml.Invoices != null)
                {
                    foreach (var invoice in xml.Invoices)
                    {
                        if (invoiceDao.GetSingle(t => t.OrderNumber.Equals(invoice.OrderNumber)) != null)
                        {
                            jobLogs.Add(GetLogException(fileName, new Exception($"Invoice with order number:{invoice.OrderNumber} existing.")));
                            continue;
                        }
                        var orderId = orderDao.GetSingle(t => t.OrderNumber.Equals(invoice.OrderNumber), false)?.Id;
                        invoice.OrderId = orderId;
                        invoice.InvoiceItems = invoice.XmlInvoiceItems?.ToList();
                        invoiceDao.AddItem(invoice);

                    }
                }

                if (xml.Orders != null)
                {
                    foreach (var order in xml.Orders)
                    {
                        if (orderDao.GetSingle(t => t.OrderNumber.Equals(order.OrderNumber)) != null)
                        {

                            jobLogs.Add(GetLogException(fileName, new Exception($"Order with order number:{order.OrderNumber} existing.")));
                            continue;
                        }
                        order.OrderItems = order.XmlOrderItems?.ToList();
                        var entity = orderDao.AddItem(order);
                        var invoice = invoiceDao.GetSingle(t => t.OrderNumber.Equals(entity.OrderNumber), false);
                        if (invoice.OrderId != entity.Id)
                        {
                            invoice.OrderId = entity.Id;
                            invoiceDao.UpdateItem(invoice);
                        }

                        var delivery = deliveryDao.GetSingle(t => t.OrderNumber.Equals(entity.OrderNumber), false);
                        if (delivery.OrderId != entity.Id)
                        {
                            delivery.OrderId = entity.Id;
                            deliveryDao.UpdateItem(delivery);
                        }
                    }
                }

                if (xml.Deliveries != null)
                {
                    foreach (var delivery in xml.Deliveries)
                    {
                        if (deliveryDao.GetSingle(t => t.OrderNumber.Equals(delivery.OrderNumber)) != null)
                        {

                            jobLogs.Add(GetLogException(fileName, new Exception($"Delivery with order number:{delivery.OrderNumber} existing.")));
                            continue;
                        }
                        var orderId = orderDao.GetSingle(t => t.OrderNumber.Equals(delivery.OrderNumber), false)?.Id;
                        delivery.OrderId = orderId;
                        deliveryDao.AddItem(delivery);
                    }
                }

                status = true;
                stream.Dispose();
            }
            catch (Exception e)
            {
                jobLogs.Add(GetLogException(fileName,e));
                stream.Dispose();
            }
            finally
            {

                stream.Dispose();
                if (status)
                    if (File.Exists(fileName)) File.Delete(@fileName);


            }

            return status;

        }

        private JobLog GetLogException(string fileName, Exception exception)
        {
            var jobLog = MappingUtils.MapToNew<Exception, JobLog>(exception);
            jobLog.Message = exception.Message.ToString();
            jobLog.Name = fileName;
            if (exception.InnerException != null)
            {
                jobLog.InnerLog = GetLogException(fileName, exception.InnerException);
            }

            return jobLog;

        }


        private void serializer_UnknownNode(object sender, XmlNodeEventArgs e)
        {
            Console.WriteLine("Unknown Node:" + e.Name + "\t" + e.Text);
        }

        private void serializer_UnknownAttribute
            (object sender, XmlAttributeEventArgs e)
        {
            System.Xml.XmlAttribute attr = e.Attr;
            Console.WriteLine("Unknown attribute " +
                              attr.Name + "='" + attr.Value + "'");
        }

        private void SetupCompletionJobTimer()
        {
            _completionTimer.Elapsed += CompletionTimerOnElapsed;

            var job = _jobDao.GetSingle(t => t.Id.Equals(Guid.Parse(Constants.JobInvoicingCompletionId)), false);
            if (job == null) return;
            _completionTimer.Interval = job.Length.TotalMilliseconds;

            if (job.IsRun)
                StartCompletionWorker();
            else
                StopCompletionWorker();
        }



        private void CompletionTimerOnElapsed(object sender, ElapsedEventArgs e)
        {
            CompletionJob();
        }

        private void CompletionJob()
        {
            var jobScheduler = new JobScheduler { JobTime = DateTime.Now, JobId = Guid.Parse(Constants.JobInvoicingCompletionId) };
            var sw = new Stopwatch();
            sw.Start();
            var status = true;
            var jobLogs = new List<JobLog>();
            status = InvoiceCompletion(jobLogs);

            sw.Stop();
            jobScheduler.Status = status;
            jobScheduler.Logs = jobLogs;
            jobScheduler.JobLength = sw.Elapsed;
            _jobScheduler.AddItem(jobScheduler);
        }

        private bool InvoiceCompletion(List<JobLog> jobLogs)
        {
            int trials = 2;
            bool status = true;
            var invoiceDao = new BaseDaoFactory(new EfzDbContextFactory().Create()).GetDao<Invoice>();
            var completionDao = new BaseDaoFactory(new EfzDbContextFactory().Create()).GetDao<Completion>();
            var attachmentDao = new BaseDaoFactory(new EfzDbContextFactory().Create()).GetDao<Attachment>();
            var deliveryDao = new BaseDaoFactory(new EfzDbContextFactory().Create()).GetDao<Delivery>();

            var customerDao = new BaseDaoFactory(new EfzDbContextFactory().Create()).GetDao<Customer>();

            var path = @"Data/CompletionStorage/";


            var invoices = invoiceDao.GetCollection(t => !t.IsCompleted, false);
            foreach (var invoice in invoices)
            {

                try
                {
                    var serverFilename = Guid.NewGuid().ToString();
                    var attachmentCompletes = new List<AttachmentComplete>();

                    var attachments = attachmentDao.GetCollection(t => t.OrderNumber.Equals(invoice.OrderNumber));
                    if (attachments == null || !attachments.Any())
                    {
                        ++invoice.CompletionCounter;
                        if (invoice.CompletionCounter < trials)
                            continue;
                    }
                    var invoiceIncluded = invoiceDao.GetSingle(t => t.Id.Equals(invoice.Id));
                    var invoicePdfDocumentArray = GetInvoicePdfDocument(invoiceIncluded);
                    var attachmentFiles = GetAttachmentsFile(attachments);

                    if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                    CompletionPdf(invoicePdfDocumentArray, attachmentFiles, $"{path}{serverFilename}");

                    if(attachmentFiles!=null && attachmentFiles.Any())
                        attachmentCompletes.AddRange(attachments.Select(t=> new AttachmentComplete(){AttachmentId = t.Id}));

                    var completion = new Completion()
                    {
                        CustomerId = invoice.CustomerId,
                        InvoiceId = invoice.Id,
                        OrderId = invoice.OrderId,
                        OrderNumber = invoice.OrderNumber,
                        InvoiceNumber = invoice.InvoiceNumber,
                        InvoiceDate = invoice.InvoiceDate,
                        CompleteFileName = $"{invoice.InvoiceNumber}.pdf",
                        ServerFileName = serverFilename,
                        AttachmentCompletes = attachmentCompletes

                    };
                    completionDao.AddItem(completion);
                    invoice.IsCompleted = true;

                    foreach (var attachment in attachments) attachment.IsCompleted = true;

                    var deliveries = deliveryDao.GetCollection(t => t.OrderNumber.Equals(invoice.OrderNumber));
                    foreach (var delivery in deliveries)
                    {
                        delivery.IsCompleted = true;
                    }

                    attachmentDao.UpdateRangeItems(attachments);
                    deliveryDao.UpdateRangeItems(deliveries);

                    var customer = customerDao.GetSingle(t => t.Id.Equals(completion.CustomerId));
                    completion.Customer = customer;
                    _emailSendGridBlProvider.SendInvoiceCompleteNotification(completion);

                }
                catch (Exception e)
                {
                    status = false;
                    jobLogs.Add(GetLogException(invoice.InvoiceNumber, e));
                }

               
            }

            try
            {
                invoiceDao.UpdateRangeItems(invoices);
            }
            catch (Exception e)
            {
                status = false;
                jobLogs.Add(GetLogException("InvoiceCompleteUpdateRange", e));
            }

            return status;

        }

        private void CompletionPdf(byte[] invoiceMemoryStream, List<FileStream> attachmentFiles, string fileName)
        {
            var reader = new PdfReader(invoiceMemoryStream);
            var document = new Document(reader.GetPageSizeWithRotation(1));
            var pdfMerge = new MemoryStream();
            var pdfCopyProvider = new PdfCopy(document, pdfMerge);

            document.Open();

            var pages = new List<PdfImportedPage>();
            GetAllPages(reader, pdfCopyProvider, pages);
            foreach (var attachmentReader in attachmentFiles.Select(attachmentFile => new PdfReader(attachmentFile)))
            {
                GetAllPages(attachmentReader, pdfCopyProvider, pages);
                attachmentReader.Close();
            }

            foreach (var pdfImportedPage in pages)
            {
                pdfCopyProvider.AddPage(pdfImportedPage);

            }
            document.Close();
            reader.Close();
            pdfCopyProvider.Close();

            pdfMerge.Seek(0, SeekOrigin.Begin);

            var readerWithoutSign = new PdfReader(pdfMerge);
            var finalOutput = new FileStream(fileName, FileMode.Create, FileAccess.Write);
            
            
            var tsa = new TsaClientBouncyCastle("https://freetsa.org/tsr");
            int contentEstimated = (int)pdfMerge.Length;

            var st = PdfStamper.CreateSignature(readerWithoutSign, finalOutput, '\0', null, true);

            var sap = st.SignatureAppearance;

            var cert = new Cert("EFZ.pfx", "Lea10985");

            sap.SetCrypto(cert.Akp, cert.Chain, null, PdfSignatureAppearance.SelfSigned);
            sap.Reason = "Archived digital signature";
            sap.Contact = "EFZ";
            sap.Location = "EFZ";

            sap.CertificationLevel = PdfSignatureAppearance.CERTIFIED_NO_CHANGES_ALLOWED;

            sap.SetCrypto(null, cert.Chain, null, PdfSignatureAppearance.VerisignSigned);
            
            var dic = new PdfSignature(PdfName.AdobePpklite, PdfName.AdbePkcs7Detached);
            dic.Put(PdfName.TYPE, PdfName.Stamp);
            dic.Reason = sap.Reason;
            dic.Location = sap.Location;
            dic.Contact = sap.Contact;
            dic.Date = new PdfDate(sap.SignDate);
            sap.CryptoDictionary = dic;

            var exc = new Dictionary<PdfName, int>();
            exc[PdfName.Contents] = contentEstimated * 2 + 2;
            sap.PreClose(new Hashtable(exc));

            var sgn = new PdfPkcs7(cert.Akp, cert.Chain, null, "SHA1", false);
            var data = sap.RangeStream;
            var messageDigest = DigestUtilities.GetDigest("SHA1");
            byte[] buf = new byte[8192];
            int n;
            while ((n = data.Read(buf, 0, buf.Length)) > 0)
            {
                messageDigest.BlockUpdate(buf, 0, n);
            }
            byte[] tsImprint = new byte[messageDigest.GetDigestSize()];
            messageDigest.DoFinal(tsImprint, 0);
            var cal = DateTime.UtcNow;
            byte[] tsToken = tsa.GetTimeStampToken(null, tsImprint);

            byte[] ocsp = null;
            if (cert.Chain.Length >= 2)
            {
                String url = PdfPkcs7.GetOcspurl(cert.Chain[0]);
                if (url != null && url.Length > 0)
                    ocsp = new OcspClientBouncyCastle(cert.Chain[0], cert.Chain[1], url).GetEncoded();
            }
            byte[] sh = sgn.GetAuthenticatedAttributeBytes(tsImprint, cal, ocsp);
            sgn.Update(sh, 0, sh.Length);

            byte[] encodedSig = sgn.GetEncodedPkcs7(tsImprint, cal, tsa, ocsp);
            if (contentEstimated + 2 < encodedSig.Length)
                throw new Exception("Not enough space");

            byte[] paddedSig = new byte[contentEstimated];
            Array.Copy(encodedSig, 0, paddedSig, 0, encodedSig.Length);


            var dic2 = new PdfDictionary();
            dic2.Put(PdfName.Contents, new PdfString(paddedSig).SetHexWriting(true));

            sap.Close(dic2);

            finalOutput.Close();
            finalOutput.Dispose();

            
        }

      
        private void GetAllPages(PdfReader reader, PdfCopy pdfCopyProvider, List<PdfImportedPage> pages)
        {

            for (int i = 1; i <= reader.NumberOfPages; i++)
            {
                pages.Add(pdfCopyProvider.GetImportedPage(reader, i));
            }

        }

        private List<FileStream> GetAttachmentsFile(IEnumerable<Attachment> attachments)
        {
            var result = new List<FileStream>();
            if (attachments == null || !attachments.Any())
                return result;

            foreach (var attachment in attachments)
            {
                if (string.IsNullOrEmpty(attachment.ServerFileName)) continue;

                result.Add(System.IO.File.OpenRead(
                    @"Data/Attachments/" + attachment.ServerFileName));
                    
            }

            return result;

        }

        private void ByteToFileStream(byte[] dataArray, string fileName)
        {
            using FileStream
                fileStream = new FileStream(fileName, FileMode.Create, FileAccess.Write);
            
            foreach (var t in dataArray)
            {
                fileStream.WriteByte(t);
            }

            fileStream.Seek(0, SeekOrigin.Begin);

            for (var i = 0; i < fileStream.Length; i++)
            {
                if (dataArray[i] == fileStream.ReadByte()) continue;

                _logger.LogWarning(101, "Error writing data.");
                Console.WriteLine("Error writing data.");
                return ;
            }
            fileStream.Close();

        }

        public byte[] StreamToArray(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }

        private MemoryStream ByteToMemoryStream(byte[] dataArray)
        {
            var memoryStream = new MemoryStream();

            foreach (var t in dataArray)
            {
                memoryStream.WriteByte(t);
            }

            memoryStream.Seek(0, SeekOrigin.Begin);

            for (var i = 0; i < memoryStream.Length; i++)
            {
                if (dataArray[i] == memoryStream.ReadByte()) continue;

                _logger.LogWarning(101, "Error writing data.");
                Console.WriteLine("Error writing data.");
                return null;
            }
            return memoryStream;

        }

        public byte[] GetInvoicePdfDocument(Invoice invoice)
        {
            var globalSettings = new GlobalSettings
            {

                ColorMode = ColorMode.Color,
                Orientation = Orientation.Portrait,
                PaperSize = PaperKind.A4,
                Margins = new MarginSettings { Top = 10 },
                DocumentTitle = $"Faktura - {invoice.InvoiceNumber}"
            };

            var objectSettings = new ObjectSettings
            {
                PagesCount = true,
                HtmlContent = HtmlStringBuilder.HtmlInvoiceDocument(invoice),
                WebSettings = { DefaultEncoding = "utf-8", UserStyleSheet = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "css/site.css") },
                HeaderSettings = { FontName = "Calibri", FontSize = 10, Right = "Page [page] of [toPage]", Line = true },
                FooterSettings = { FontName = "Calibri", FontSize = 10, Line = true, Center = $"Faktura - {invoice.InvoiceNumber}" }
            };

            var pdf = new HtmlToPdfDocument()
            {
                GlobalSettings = globalSettings,
                Objects = { objectSettings }
            };

            var file = _converter.Convert(pdf);
            return file;


        }

        
        private void SetupAttachmentScanJobTimer()
        {
            _attachmentScanTimer.Elapsed += AttachmentScanTimerOnElapsed;

            var job = _jobDao.GetSingle(t => t.Id.Equals(Guid.Parse(Constants.JobAttachmentScanProcessingId)), false);
            if (job == null) return;
            _xmlTimer.Interval = job.Length.TotalMilliseconds;

            if (job.IsRun)
                StartXmlWorker();
            else
                StopXmlWorker();
        }

        private void AttachmentScanTimerOnElapsed(object sender, ElapsedEventArgs e)
        {
            AttachmentScanProcessingJob();
        }

        private void AttachmentScanProcessingJob()
        {

            if(_attachmentTask.Status == TaskStatus.Created)
                _attachmentTask.Start();
            else if (_attachmentTask.IsCompleted)
            {
                _attachmentTask = new Task(AttachmentScanProcessingAsync);
                _attachmentTask.Start();
            }
        }

        private void AttachmentScanProcessingAsync()
        {
            var jobScheduler = new JobScheduler { JobTime = DateTime.Now, JobId = Guid.Parse(Constants.JobAttachmentScanProcessingId) };
            var sw = new Stopwatch();
            sw.Start();
            var path = "Data/AttachmentScans/";
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            var filePaths = Directory.GetFiles(@"Data/AttachmentScans/");
            var status = true;
            var jobLogs = new List<JobLog>();
            foreach (var filePath in filePaths)
            {
                if (ProcessAttachmentScans(filePath, jobLogs) && status)
                    status = true;
                else
                {
                    status = false;
                }
            }
            sw.Stop();
            jobScheduler.Status = status;
            jobScheduler.Logs = jobLogs;
            jobScheduler.JobLength = sw.Elapsed;
            _jobScheduler.AddItem(jobScheduler);
        }

        private bool ProcessAttachmentScans(string filePath, List<JobLog> jobLogs)
        {

            var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);

            var fileName = Path.GetFileNameWithoutExtension(filePath);

            var fileNameWithExtension = Path.GetFileName(filePath);

            var deliveryNumberString = string.Empty;

            var status = false;
            try
            {
                var deliveryNumbers = _attachmentScanHelper.GetBarcodeFromStream(stream);
                var deliveryDao = new BaseDaoFactory(new EfzDbContextFactory().Create()).GetDao<Delivery>();
                foreach (var deliveryNumber in deliveryNumbers)
                {
                    var delivery = deliveryDao.GetSingle(t => t.DeliveryNumber.Equals(deliveryNumber));
                    if (delivery != null)
                    {


                        deliveryNumberString = delivery.DeliveryNumber;
                        var pdfStream = _attachmentScanHelper.ConvertImageToPdf(stream);
                        var entity = new Attachment()
                        {
                            OrderNumber = delivery.OrderNumber,
                            DeliveryId = delivery.Id,
                            DeliveryNumber = delivery.DeliveryNumber,
                            FileType = "pdf",
                            FileName = $"{fileName}.pdf"
                        };

                        UploadAttachment(pdfStream, entity);
                        status = true;
                        break;
                    }
                }
               

              

               

            }
            catch (Exception e)
            {
                jobLogs.Add(GetLogException(fileNameWithExtension, e));
                stream.Dispose();
            }
            finally
            {

                stream.Dispose();
                if (status)
                {
                    if (File.Exists(filePath)) File.Delete(@filePath);
                    jobLogs.Add(GetLogException(fileNameWithExtension, new Exception($"Add as attachment for delivery: {deliveryNumberString}")));
                }
                else
                {
                    jobLogs.Add(GetLogException(fileNameWithExtension, new Exception("Barcode not exist or not match to any delivery number.")));
                }


            }

            return status;
        }


        private IValidationResult UploadAttachment(MemoryStream memoryStream, Attachment entity)
        {
            var attachmentDao = new BaseDaoFactory(new EfzDbContextFactory().Create()).GetDao<Attachment>();

            if (entity is null) throw new ArgumentNullException(nameof(entity));

            entity.ServerFileName = Guid.NewGuid().ToString();
            attachmentDao.AddItem(entity);

            try
            {
                string filename = $"{entity.ServerFileName}";
                string path = "Data/Attachments/";

                if (!Directory.Exists(path)) Directory.CreateDirectory(path);

                var file = new FileStream($"{path}{filename}", FileMode.Create, FileAccess.Write);
                memoryStream.WriteTo(file);
                file.Close();
                memoryStream.Close();
            }
            catch (Exception e)
            {
                attachmentDao.DeleteItem(entity);
                throw e;
            }
            return ValidationResult.ResultOk();

        }



    }
}