using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace Samr.ERP.Core.ViewModels.News
{
    public class EditNewsViewModel : NewsViewModel
    {
        public Guid NewsCategoryId { get; set; }
        public string NewsCategoryName { get; set; }
        public IFormFile ImageFile { get; set; }
        public string ImagePath { get; set; }
    }
}
