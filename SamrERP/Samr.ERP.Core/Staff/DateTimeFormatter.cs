using System;
using AutoMapper;

namespace Samr.ERP.Core.Staff
{
    public class DateTimeFormatter : IValueConverter<DateTime, string>
    {
        public string Convert(DateTime sourceMember, ResolutionContext context) => sourceMember.ToString("dd-MM-yyyy");
    }
}
