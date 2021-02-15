using System;
using System.IO;
using EFZ.Entities.Entities;

namespace EFZ.Domain.BusinessLogic.Interface
{
    public interface IJobSchedulerBlProvider
    {
        void SetNewXmlTime(TimeSpan time);

        void StartXmlWorker();

        void StopXmlWorker();
        void RunXmlManually();

        void SetNewCompletionTime(TimeSpan time);

        void StartCompletionWorker();

        void StopCompletionWorker();
        void RunCompletionManually();

        byte[] GetInvoicePdfDocument(Invoice invoice);
        void RunAttachmentScansManually();
        void SetNewAttachmentScanTime(TimeSpan time);
        void StartAttachmentScanWorker();
        void StopAttachmentScanWorker();
    }
}