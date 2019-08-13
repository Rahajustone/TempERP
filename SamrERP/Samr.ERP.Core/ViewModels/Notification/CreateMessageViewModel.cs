﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Samr.ERP.Core.ViewModels.Notification
{
    public class CreateMessageViewModel
    {
        public string Title { get; set; }
        public string Body { get; set; }
        public Guid? ReceiverUserId { get; set; }
    }
}
