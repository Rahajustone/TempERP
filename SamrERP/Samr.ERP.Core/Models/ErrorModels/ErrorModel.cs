using System;
using Samr.ERP.Core.Enums;

namespace Samr.ERP.Core.Models.ErrorModels
{
    public class ErrorModel
    {
        public ErrorType Code { get; set; }
        public String Description { get; set; }

        public ErrorModel()
        {
            
        }

        public ErrorModel(string description)
        {
            Description = description;
        }
    }
}
