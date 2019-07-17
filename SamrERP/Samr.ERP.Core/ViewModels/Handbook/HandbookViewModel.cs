using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Samr.ERP.Core.ViewModels.Handbook
{
    public class HandbookViewModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string DisplayName { get; set; }
        [Required]
        public string ActionName { get; set; }
        public string LastModifiedAt { get; set; }
        public Guid? LastModifiedUserId { get; set; }
        public string LastModifiedUserFullName { get; set; }
    }
}
