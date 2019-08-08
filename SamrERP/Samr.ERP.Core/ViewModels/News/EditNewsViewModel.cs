using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Http;
using Samr.ERP.Core.ViewModels.Employee;

namespace Samr.ERP.Core.ViewModels.News
{
    public class EditNewsViewModel : NewsViewModel
    {
        public Guid NewsCategoryId { get; set; }
        public string NewsCategoryName { get; set; }
        public IFormFile ImageFile { get; set; }
        public string ImagePath { get; set; }
        public bool IsActive { get; set; }
        public string CreatedUserName { get; set; }
        public string CreatedUserId { get; set; }

        public MiniProfileViewModel Author { get; set; }
    }
}
