using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Samr.ERP.Core.ViewModels.Employee
{
    public class GetEmployeeViewModel : AllEmployeeViewModel
    {
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public string DateOfBirth { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Description { get; set; }
        public string FactualAddress { get; set; }



    }
}
