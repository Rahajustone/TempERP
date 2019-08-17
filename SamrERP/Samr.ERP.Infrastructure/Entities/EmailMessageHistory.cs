﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Samr.ERP.Infrastructure.Entities.BaseObjects;
using Samr.ERP.Infrastructure.Interfaces;

namespace Samr.ERP.Infrastructure.Entities
{
    public class EmailMessageHistory:CreatableByUserBaseObject,ICreatable
    {
        public Guid RecieverUserId { get; set; }
        [ForeignKey(nameof(RecieverUserId))]
        public User RecieverUser { get; set; }

        public string RecieverEMail { get; set; }

        public Guid EmailSettingId { get; set; }
        [ForeignKey(nameof(EmailSettingId))]
        public EmailSetting EmailSetting { get; set; }

        public string Subject { get; set; }

        public string Message { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
