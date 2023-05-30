using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace ErciyesSozluk.Projections.UserService.Services
{
    public class EmailService
    {
        private readonly IConfiguration configuration;
        private readonly ILogger<EmailService> logger;

        public EmailService(IConfiguration configuration, ILogger<EmailService> logger)
        {
            this.configuration = configuration;
            this.logger = logger;
        }

        public string GenerateConfirmationLink(Guid confirmationId)
        {
            var baseUrl = configuration["ConfirmationLinkBase"] + confirmationId;

            return baseUrl;
        }

        public Task SendEmail(string toEmailAddress, string content)
        {
            //send email process

            SmtpClient client = new SmtpClient();
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.EnableSsl = true;
            client.Host = "smtp.office365.com";
            client.Port = 587;

            // setup Smtp authentication
            System.Net.NetworkCredential credentials =
                new System.Net.NetworkCredential("erusozluk@outlook.com", "Aa7waw37x.");
            client.UseDefaultCredentials = false;
            client.Credentials = credentials;

            MailMessage msg = new MailMessage();
            msg.From = new MailAddress("erusozluk@outlook.com");
            msg.To.Add(new MailAddress(toEmailAddress));

            msg.Subject = "Onay Maili";
            msg.IsBodyHtml = true;
            msg.Body = string.Format($"<html><head></head><body>Al sana link: <b>{content}</b></body>");

            try
            {
                client.Send(msg);
            }
            catch (Exception ex)
            {
            }


            logger.LogInformation($"Email sent to {toEmailAddress} with content {content}");

            return Task.CompletedTask;
        }
    }
}
