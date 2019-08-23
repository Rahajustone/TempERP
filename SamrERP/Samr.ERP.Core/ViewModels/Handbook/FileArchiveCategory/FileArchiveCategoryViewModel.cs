using System;
using Samr.ERP.Core.Stuff;

namespace Samr.ERP.Core.ViewModels.Handbook.FileArchiveCategory
{
    public class FileArchiveCategoryViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string CreatedUserName => Extension.FullNameToString(LastName, FirstName, MiddleName);
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string CreatedAt { get; set; }
    }
}
