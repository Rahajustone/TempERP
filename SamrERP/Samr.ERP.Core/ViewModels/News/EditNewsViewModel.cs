using System;
using System.Collections.Generic;
using System.Text;

namespace Samr.ERP.Core.ViewModels.News
{
    public class EditNewsViewModel : NewsViewModel
    {
        public Guid NewsCategoryId { get; set; }
        public string NewsCategoryName { get; set; }
    }
}
