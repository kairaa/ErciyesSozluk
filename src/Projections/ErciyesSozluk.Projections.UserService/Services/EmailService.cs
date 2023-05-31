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
            client.Host = configuration["Host"];
            client.Port = 587;

            // setup Smtp authentication
            System.Net.NetworkCredential credentials =
                new System.Net.NetworkCredential(configuration["EmailAddress"], configuration["Password"]);
            client.UseDefaultCredentials = false;
            client.Credentials = credentials;

            MailMessage msg = new MailMessage();
            msg.From = new MailAddress(configuration["EmailAddress"]);
            msg.To.Add(new MailAddress(toEmailAddress));

            msg.Subject = "Onay Maili";
            msg.IsBodyHtml = true;

            msg.Body = $"<body><div style=\"border: 1px solid; padding: 10px; text-align: center\"><h1>Hoşgeldin!</h1><p>Başarılı bir şekilde kaydın gerçekleşti! Uygulama üzerinde fikirlerini paylaşabilmek için son bir adım kaldı. </p><p>Aşağıdaki butona tıklayarak üyeliğini onaylayabilirsin.</p><form method=\"post\" action=\"{content}\"><button style=\"padding: 10px;\" type=\"submit\" onclick='fetch(\"{content}\", {{method: \"POST\"}});console.log(\"aaa ol aaa\")'>Üyeliği Başlat</button></form></div></body>";

            //msg.Body = $"<form method=\"POST\"><button style=\"padding: 10px;\" type=\"submit\" onclick=\"() => {{console.log(\"aaa ol\"); fetch}}\"> </button></form>";

            //msg.Body = GenerateBodyString(content);

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

        private string GenerateBodyString(string content)
        {
            string beginning = "<html><head></head><body><div style=\"border: 1px solid; padding: 10px; text-align: center\"><h1>Hoşgeldin!</h1><p>Başarılı bir şekilde kaydın gerçekleşti! Uygulama üzerinde fikirlerini paylaşabilmek için son bir adım kaldı. </p><p>Aşağıdaki butona tıklayarak üyeliğini onaylayabilirsin.</p>";
            
            string jsFunction = "() => {console.log(\"neden olmayyy\");fetch(" + content + ", { method: \"POST\""  + ")}; window.open(\"http://localhost:5206/login\");";
            string formBegin = "<form method=\"post\" action=\"\"><button style=\"padding: 10px;\" type=\"submit\"";
            string formFunction = "onclick=\"" + jsFunction + "\">";
            string formEnd = "Üyeliği Başlat</button></form>";
            //string formTag = $"<form method=\"post\" action=\"\"><button style=\"padding: 10px;\" type=\"submit\" onclick=\"window.open({content},'_blank');'\">Üyeliği Başlat</button></form>";
            string end = "</div></body></html>";
            return string.Concat(beginning, formBegin, formFunction, formEnd, end);
        }
    }
}
