using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Samr.ERP.Infrastructure.Interfaces;

namespace Samr.ERP.Infrastructure.Entities
{
    public class National : BaseObject, IChangeable
    {
        [Required]
        public string Name { get; set; }

        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }

        [Required]
        public Guid CreateUserId { get; set; }
    }
}
