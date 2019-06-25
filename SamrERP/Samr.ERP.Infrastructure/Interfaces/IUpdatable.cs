using System;
using System.Collections.Generic;
using System.Text;

namespace Samr.ERP.Infrastructure.Interfaces
{
    public interface IUpdatable
    {
        DateTime UpdatedAt { get; set; }
    }
}
