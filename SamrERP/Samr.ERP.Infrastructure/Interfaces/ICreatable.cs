using System;
using System.Collections.Generic;
using System.Text;

namespace Samr.ERP.Infrastructure.Interfaces
{
    public interface ICreatable
    {
        public DateTime CreatedAt { get; set; }
    }
}
