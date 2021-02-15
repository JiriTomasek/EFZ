using System.Collections.Generic;
using System.Linq;
using EFZ.Domain.BusinessLogic.Interface;
using EFZ.Entities.Entities;
using EFZ.Resources;
using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;
using Attachment = EFZ.Entities.Entities.Attachment;

namespace EFZ.Domain.BusinessLogic.Impl
{
    public class EmailSendGridBlProvider : IEmailSendGridBlProvider
    {
        private readonly IConfiguration _configuration;

        public EmailSendGridBlProvider(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void SendInvoiceCompleteNotification(Completion completion)
        {
            var apiKey = _configuration.GetSection("SENDGRID_API_KEY").Value;
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("no-reply@efz.cz", "EFZ - Elektronická fakturace zákazníkem");
            var url = string.Format(LabelsMessages.valCompletionUrl,completion.Id);
            string subject, htmlContent;
            var recipients = new List<EmailAddress>();
            if (completion.Customer?.Users != null)
                recipients.AddRange(completion.Customer.Users.Select(t => new EmailAddress(t.Email, t.Name)).ToList());


            subject = $"Nová faktura: {completion.OrderNumber}";
                htmlContent = string.Format(LabelsMessages.valCompletationContent,
                                        completion.OrderNumber,
                                        url, LabelsMessages.valCompletionUrlName);

            var plainText = "";
            var msg = MailHelper.CreateSingleEmailToMultipleRecipients(from, recipients, subject, plainText, htmlContent);

            client.SendEmailAsync(msg);
        }
    }
}