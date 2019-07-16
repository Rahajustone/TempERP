using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Samr.ERP.Core.ViewModels.Handbook
{
    public class HandbookViewModel
    {
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string DisplayName { get; set; }
        [Required]
        public string ActionName { get; set; }
        public string CreatedUserName { get; set; }
        public string LastEditedAt { get; set; }
    }
}
