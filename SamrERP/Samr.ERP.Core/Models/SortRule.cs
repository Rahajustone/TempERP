using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Samr.ERP.Core.Enums;

namespace Samr.ERP.Core.Models
{
    // [{"property":"title","direction":"ASC"}]
    public class SortRule
    {
        public string SortProperty { get; set; }

        public SortDirection SortDir { get; set; }
    }
}
