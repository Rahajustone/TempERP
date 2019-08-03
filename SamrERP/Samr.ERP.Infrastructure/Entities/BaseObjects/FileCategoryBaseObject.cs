using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Samr.ERP.Infrastructure.Interfaces;

namespace Samr.ERP.Infrastructure.Entities.BaseObjects
{
    public class FileCategoryBaseObject : CreatableByUserBaseObject, IActivable, ICreatable
    {
        [StringLength(64)]
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
