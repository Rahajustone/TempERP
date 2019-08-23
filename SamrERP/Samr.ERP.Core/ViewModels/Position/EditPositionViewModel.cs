using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Samr.ERP.Core.Stuff;

namespace Samr.ERP.Core.ViewModels.Position
{
    public class EditPositionViewModel : PositionViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }

        [Required]
        public Guid? DepartmentId { get; set; }

        public string DepartmentName { get; set; }
        public bool IsActive { get; set; }
        public string CreatedAt { get; set; }

        public string CreatedUserName => Extension.FullNameToString(LastName, FirstName, MiddleName);

    }
}
