using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Samr.ERP.Infrastructure.Interfaces;

namespace Samr.ERP.Infrastructure.Entities.BaseObjects
{
    public class CreatableByUserBaseObject:BaseObject,ICreatableByUser
    {
        [Required]
        public Guid CreatedUserId { get; set; }
        [ForeignKey(nameof(CreatedUserId))]
        public User CreatedUser { get; set; }
    }
}
