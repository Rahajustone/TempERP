using System;
using System.Collections.Generic;
using System.Text;
using Samr.ERP.Core.Stuff;

namespace Samr.ERP.Core.ViewModels.Handbook.Nationality
{
    public class EditNationalityViewModel : NationalityViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string CreatedUserName => Extension.FullNameToString(LastName, FirstName, MiddleName);

        public bool IsActive { get; set; }
        public string CreatedAt { get; set; }
    }
}
