using System.Net.Mail;
using System.Net;
using EmailService.Interfaces;
using SendGrid;
using SendGrid.Helpers.Mail;
using Microsoft.Extensions.Configuration;

namespace EmailService
{
    public class SendGridEmailService : IEmailService
    {
        private readonly ISendGridClient _sendGridClient;
        private readonly IConfiguration configuration;

        public SendGridEmailService(ISendGridClient sendGridClient, IConfiguration configuration)
        {
            configuration=configuration?? throw new ArgumentNullException(nameof(configuration));
            _sendGridClient = sendGridClient;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string content)
        {
            var message = new SendGridMessage
            {
                From = new EmailAddress(configuration.GetSection("SendGrid")["SenderEmail"]),
                Subject = subject,
                PlainTextContent = content,
                HtmlContent = content
            };
            message.AddTo(new EmailAddress(toEmail));

            var response = await _sendGridClient.SendEmailAsync(message);

            if (response.StatusCode != HttpStatusCode.OK && response.StatusCode != HttpStatusCode.Accepted)
            {
                // Handle email sending failure
            }
        }
    }
}