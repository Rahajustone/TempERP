using System;
using Samr.ERP.Core.Enums;
using Samr.ERP.Core.Stuff;

namespace Samr.ERP.Core.Models.ErrorModels
{
    public class ErrorModel
    {
        public ErrorCode Code { get; set; }
        public String Description { get; set; }

        public ErrorModel()
        {
            
        }

        public ErrorModel(string description)
        {
            Description = description;
        }
        public ErrorModel(string code, string description)
        {
            Code = Enum.Parse<ErrorCode>(code);
            Description = description;
        }
        public ErrorModel(ErrorCode code)
        {
            Code = code;
            Description = code.ToStringX();
        }
        
    }
}
