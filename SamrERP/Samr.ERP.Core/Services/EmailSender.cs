using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using MimeKit.Text;
using Samr.ERP.Core.Interfaces;
using Samr.ERP.Infrastructure.Data.Contracts;
using Samr.ERP.Infrastructure.Entities;
using Samr.ERP.Infrastructure.Providers;

namespace Samr.ERP.Core.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailSettingService _emailSettingService;
        private readonly UserProvider _userProvider;
        private EmailSetting _defaultEmailSetting;

        private EmailSetting DefaultEmailSetting => _defaultEmailSetting ?? (_defaultEmailSetting = _emailSettingService.GetDefaultEmailSetting());

        public EmailSender(
            IUnitOfWork unitOfWork,
            IEmailSettingService emailSettingService,
            UserProvider userProvider
            )
        {
            _unitOfWork = unitOfWork;
            _emailSettingService = emailSettingService;
            _userProvider = userProvider;

        }


        public async Task SendEmailToEmployeeAsync(User user, string subject, string message)
        {
            await AddEmailMessageHistory(user, subject, message);
            await SendEmailAsync(user.Email, subject, message);
        }

        private async Task AddEmailMessageHistory(User destUser, string subject, string message)
        {

            var emailMessageHistory = new EmailMessageHistory()
            {
                RecieverUserId = destUser.Id,
                EmailSettingId = DefaultEmailSetting.Id,
                Subject = subject,
                Message = message,
                RecieverEMail = destUser.Email
            };
            if (_userProvider.CurrentUser == null)
            {
                var firstUser = await _unitOfWork.Users.All().FirstAsync();
                emailMessageHistory.CreatedUserId = firstUser.Id;
                _unitOfWork.EmailMessageHistories.Add(emailMessageHistory, false);
            }
            else
            {
                _unitOfWork.EmailMessageHistories.Add(emailMessageHistory);

            }

            _unitOfWork.EmailMessageHistories.Add(emailMessageHistory,false);

            await _unitOfWork.CommitAsync();
        }

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress(DefaultEmailSetting.SenderName, DefaultEmailSetting.Sender));
            emailMessage.To.Add(new MailboxAddress(email));

            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(TextFormat.Text)
            {
                Text = message
            };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(DefaultEmailSetting.MailServer, DefaultEmailSetting.MailPort, DefaultEmailSetting.EnabledSSL);
                await client.AuthenticateAsync(DefaultEmailSetting.Sender, DefaultEmailSetting.Password);
                await client.SendAsync(emailMessage);

                await client.DisconnectAsync(true);
            }
        }

    }
}
