using System;
using System.Collections.Generic;
using System.Text;

namespace Samr.ERP.Core.ViewModels.UsefulLink
{
    public class UsefulLinkLogViewModel
    {
        public Guid Id { get; set; }
        public Guid UsefulLinkCategoryId { get; set; }
        public string UsefulLinkCategoryName { get; set; }
        public string Url { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public string CreatedAt { get; set; }
        public string CreatedUserName { get; set; }
    }
}
