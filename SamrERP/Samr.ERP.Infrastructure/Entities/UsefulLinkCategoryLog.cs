using System;
using System.Collections.Generic;
using System.Text;
using Samr.ERP.Infrastructure.Entities.BaseObjects;

namespace Samr.ERP.Infrastructure.Entities
{
    public class UsefulLinkCategoryLog : UsefulLinkCategoryBaseObject
    {
        public Guid UsefulLinkCategoryId { get; set; }
        public UsefulLinkCategory UsefulLinkCategory { get; set; }
    }
}
