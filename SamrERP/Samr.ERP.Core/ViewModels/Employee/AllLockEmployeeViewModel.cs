using System;
using System.Collections.Generic;
using System.Text;

namespace Samr.ERP.Core.ViewModels.Employee
{
    public class AllLockEmployeeViewModel : AllEmployeeViewModel
    {
        public string HireDate { get; set; }
        public string LockDate { get; set; }
        public string LockReason { get; set; }
    }
}
