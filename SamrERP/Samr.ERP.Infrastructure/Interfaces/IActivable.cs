using System;
using System.Collections.Generic;
using System.Text;

namespace Samr.ERP.Infrastructure.Interfaces
{
    public interface IActivable
    {
        bool IsActive { get; set; }
    }
}
