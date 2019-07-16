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
        public string ActionName { get; set; }
        public string CreatedUserName { get; set; }
        public DateTime LastEditedAt { get; set; }
    }
}
