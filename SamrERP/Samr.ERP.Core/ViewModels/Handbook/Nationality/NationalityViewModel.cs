using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Samr.ERP.Core.ViewModels.Handbook.Nationality
{
    public class NationalityViewModel
    {
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
