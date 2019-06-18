using System.ComponentModel;

namespace Samr.ERP.Infrastructure.Enums
{
    public enum GenderType
    {
        [Description("не задан")]
        Unknown,
        [Description("мужской")]
        Male,
        [Description("женский")]
        Female
    }
}
