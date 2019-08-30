using System;
using System.ComponentModel.DataAnnotations;
using Samr.ERP.Core.Stuff;

namespace Samr.ERP.Core.ViewModels.Handbook.NewCategories
{
    public class ResponseNewsCategoryViewModel : RequestNewsCategoryViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string CreatedUserName => Extension.FullNameToString(LastName, FirstName, MiddleName);
        public string CreatedAt { get; set; }
    }
}
