using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Samr.ERP.Core.ViewModels.Employee
{
    public class AllEmployeeViewModel
    {
        public Guid Id { get; set; }
        public string PhotoPath { get; set; }
        public string FullName { get; set; }
        public string Department { get; set; }
        public string Position { get; set; }
        public bool HasUser { get; set; }
        public Guid? UserId { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
    }
}
