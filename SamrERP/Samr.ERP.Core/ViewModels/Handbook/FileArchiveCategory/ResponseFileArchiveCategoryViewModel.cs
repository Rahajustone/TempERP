using Samr.ERP.Core.Staff;

namespace Samr.ERP.Core.ViewModels.Handbook.FileArchiveCategory
{
    public class ResponseFileArchiveCategoryViewModel : RequestFileArchiveCategoryViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string CreatedUserName => Extension.FullNameToString(LastName, FirstName, MiddleName);

        public string CreatedAt { get; set; }
    }
}
