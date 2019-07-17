using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Samr.ERP.Infrastructure.Entities.BaseObjects;
using Samr.ERP.Infrastructure.Interfaces;

namespace Samr.ERP.Infrastructure.Entities
{
    public class Handbook : BaseObject
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string DisplayName { get; set; }
        [Required]
        public string ActionName { get; set; }
        public DateTime? LastModifiedAt { get; set; }
        public Guid? LastModifiedUserId { get; set; }
        public string LastModifiedUserFullName { get; set; }
    }
}
