using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;

namespace Samr.ERP.Core.Stuff
{
    public class DateTimeFormatter : IValueConverter<DateTime, string>
    {
        public string Convert(DateTime sourceMember, ResolutionContext context) => sourceMember.ToString("dd-MM-yyyy");
    }
}
