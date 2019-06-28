using System;
using System.ComponentModel.DataAnnotations;
using Samr.ERP.Infrastructure.Entities.BaseObjects;

namespace Samr.ERP.Infrastructure.Entities
{
    public class Gender : BaseObject
    {
        [StringLength(16)]
        public string Name { get; set; }
    }
}
