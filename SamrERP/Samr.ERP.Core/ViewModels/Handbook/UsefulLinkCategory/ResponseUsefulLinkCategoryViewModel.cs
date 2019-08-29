using System;
using Samr.ERP.Core.Stuff;

namespace Samr.ERP.Core.ViewModels.UsefulLink.UsefulLinkCategory
{
    public class ResponseUsefulLinkCategoryViewModel : RequestUsefulLinkCategoryViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string CreatedUserName => Extension.FullNameToString(LastName, FirstName, MiddleName);

        public string CreatedAt { get; set; }
    }
}
