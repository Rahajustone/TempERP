using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Samr.ERP.Infrastructure.Interfaces;

namespace Samr.ERP.Infrastructure.Entities
{
    public class Nationality : BaseObject, IActivable, ICreatable
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public Guid CreateUserId { get; set; }
        public User User { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
