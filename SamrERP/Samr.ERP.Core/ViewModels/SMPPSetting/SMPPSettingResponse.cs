using System;
using System.Collections.Generic;
using System.Text;

namespace Samr.ERP.Core.ViewModels.SMPPSetting
{
    public class SMPPSettingResponseViewModel : SMPPSettingViewModel
    {
        public string CreatedUserName { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
