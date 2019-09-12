using System;
using System.Collections.Generic;
using System.Text;
using Samr.ERP.Infrastructure.Entities.BaseObjects;

namespace Samr.ERP.Infrastructure.Entities
{
    public class NationalityLog : NationalityBaseObject
    {
        public Guid NationalityId { get; set; }
        public Nationality Nationality { get; set; }
    }
}
