using System;
using System.Collections.Generic;
using System.Text;
using Samr.ERP.Infrastructure.Entities;

namespace Samr.ERP.Infrastructure.Interfaces
{
    public interface ICreatableByUser
    {
        Guid CreatedUserId { get; set; }
        User CreatedUser { get; set; }

    }
}
