using System;
using System.ComponentModel.DataAnnotations;
using Samr.ERP.Infrastructure.Interfaces;

namespace Samr.ERP.Infrastructure.Entities
{
    public class Department : BaseObject, IActivable, ICreatable, ICreatableByUser
    {
        [Required]
        public string Name { get; set; }

        public Guid RootId;

        public bool IsActive { get; set; }

        public DateTime CreatedAt { get; set; }

        public Guid CreatedUserId { get; set; }
        public User User { get; set; }
    }
}
