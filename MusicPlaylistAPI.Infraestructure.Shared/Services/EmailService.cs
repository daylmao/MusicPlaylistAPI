using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MimeKit;
using MusicPlaylistAPI.Core.Application.DTOs.Email;
using MusicPlaylistAPI.Core.Application.Interfaces.Services;
using MusicPlaylistAPI.Core.Domain.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlaylistAPI.Infraestructure.Shared.Services
{
    public class EmailService : IEmailService
    {
        private MailSettings _mailSettings;

        public EmailService(IOptions<MailSettings> mailSettings)
        {
            _mailSettings = mailSettings.Value;
        }

        public async Task EmailRequest(EmailDTO request)
        {
            var email  = new MimeMessage();
            email.Sender = MailboxAddress.Parse(_mailSettings.EmailFrom);

            email.To.Add(MailboxAddress.Parse(request.To));

            email.Subject = request.Subject;

            var builder = new BodyBuilder();

            builder.HtmlBody = request.Body;

            email.Body = builder.ToMessageBody();

            using MailKit.Net.Smtp.SmtpClient smtp = new();

            smtp.ServerCertificateValidationCallback = (s, c, h, e) => true;

            smtp.Connect(_mailSettings.SmtpHost, _mailSettings.SmtpPort, MailKit.Security.SecureSocketOptions.StartTls);

            smtp.Authenticate(_mailSettings.SmtpUser, _mailSettings.SmtpPass);

            await smtp.SendAsync(email);
            smtp.Disconnect(true);

        }
    }
}
