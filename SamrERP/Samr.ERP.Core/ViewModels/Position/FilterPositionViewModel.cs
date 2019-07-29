using System;
using System.Collections.Generic;
using System.Text;
using Samr.ERP.Core.ViewModels.Handbook;

namespace Samr.ERP.Core.ViewModels.Position
{
    public class FilterPositionViewModel : FilterHandbookViewModel
    {
        public Guid? DepartmentId { get; set; }
    }
}
