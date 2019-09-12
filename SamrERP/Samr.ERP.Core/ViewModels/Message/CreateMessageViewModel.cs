using System;

namespace Samr.ERP.Core.ViewModels.Message
{
    public class CreateMessageViewModel
    {
        public string Title { get; set; }
        public string Body { get; set; }
        public Guid? ReceiverUserId { get; set; }
    }
}
