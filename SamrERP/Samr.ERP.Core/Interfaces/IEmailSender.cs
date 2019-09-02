using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Samr.ERP.Infrastructure.Entities;

namespace Samr.ERP.Core.Interfaces
{
    public interface IEmailSender
    {
        Task SendEmailToUserAsync(User user,string subject,string message, bool hideMessage);
    }
}
