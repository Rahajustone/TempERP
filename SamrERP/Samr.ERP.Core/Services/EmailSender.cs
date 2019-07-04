﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MimeKit;
using MimeKit.Text;
using Samr.ERP.Core.Interfaces;
using Samr.ERP.Infrastructure.Data.Contracts;
using Samr.ERP.Infrastructure.Entities;

namespace Samr.ERP.Core.Services
{
    public class EmailSender:IEmailSender
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailSettingService _emailSettingService;
        private readonly EmailSetting _emailSetting;
        public EmailSender(
            IUnitOfWork unitOfWork,
            IEmailSettingService emailSettingService

            )
        {
            _unitOfWork = unitOfWork;
            _emailSettingService = emailSettingService;

            _emailSetting = _emailSettingService.GetDefaultEmailSetting();
        }

     
        public async Task SendEmailToEmployeeAsync(Employee employee,string subject,string message)
        {
            await SendEmailAsync(employee.Email, subject, message);
        }

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress(_emailSetting.SenderName, _emailSetting.Sender));
            emailMessage.To.Add(new MailboxAddress(email));

            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(TextFormat.Text)
            {
                Text = message
            };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(_emailSetting.MailServer, _emailSetting.MailPort, false);
                await client.AuthenticateAsync(_emailSetting.Sender, _emailSetting.Password);
                await client.SendAsync(emailMessage);
               
                await client.DisconnectAsync(true);
            }
        }
        
    }
}
