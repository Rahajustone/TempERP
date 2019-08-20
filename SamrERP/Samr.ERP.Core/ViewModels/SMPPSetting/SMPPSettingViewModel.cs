using System;
using System.Collections.Generic;
using System.Text;

namespace Samr.ERP.Core.ViewModels.SMPPSetting
{
    public class SMPPSettingViewModel
    {
        public Guid? Id { get; set; }
        public string Host { get; set; }
        public int PortNumber { get; set; }
        public string SystemId { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; }
    }
}
