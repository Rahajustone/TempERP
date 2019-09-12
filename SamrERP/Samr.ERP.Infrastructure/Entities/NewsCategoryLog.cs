using System;
using System.Collections.Generic;
using System.Text;
using Samr.ERP.Infrastructure.Entities.BaseObjects;

namespace Samr.ERP.Infrastructure.Entities
{
    public class NewsCategoryLog : NewsCategoryBaseObject
    {
        public Guid NewsCategoryId { get; set; }
        public NewsCategory NewsCategory { get; set; }
    }
}
