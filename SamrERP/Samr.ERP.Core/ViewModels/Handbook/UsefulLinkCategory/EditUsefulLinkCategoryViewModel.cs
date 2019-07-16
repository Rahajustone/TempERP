using System;

namespace Samr.ERP.Core.ViewModels.UsefulLink.UsefulLinkCategory
{
    public class EditUsefulLinkCategoryViewModel : UsefulLinkCategoryViewModel
    {
        public bool IsActive { get; set; }

        public string CreatedUserName { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
