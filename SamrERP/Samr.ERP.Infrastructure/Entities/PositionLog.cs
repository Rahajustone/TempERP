using System;
using System.Collections.Generic;
using System.Text;
using Samr.ERP.Infrastructure.Entities.BaseObjects;

namespace Samr.ERP.Infrastructure.Entities
{
    public class PositionLog : PositionBaseObject
    {
        public Guid PositionId { get; set; }
        public Position Position { get; set; }
    }
}
