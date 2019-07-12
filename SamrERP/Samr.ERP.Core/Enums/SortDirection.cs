using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Samr.ERP.Core.Models;

namespace Samr.ERP.Core.Enums
{
    [JsonConverter(typeof(SortDirectionTypeEnumConverter))]
    public enum SortDirection
    {
        Ascending,
        Descending
    }
}
