using EFZ.Entities.Entities;

namespace EFZ.Domain.BusinessLogic.Interface
{
    public interface IEmailSendGridBlProvider
    {
        void SendInvoiceCompleteNotification(Completion completion);
    }
}