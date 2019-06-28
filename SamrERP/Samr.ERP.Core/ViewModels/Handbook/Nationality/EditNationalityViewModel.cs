using System;
using System.Collections.Generic;
using System.Text;

namespace Samr.ERP.Core.ViewModels.Handbook.Nationality
{
    public class EditNationalityViewModel : NationalityViewModel
    {
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedUserName { get; set; }
    }
}
