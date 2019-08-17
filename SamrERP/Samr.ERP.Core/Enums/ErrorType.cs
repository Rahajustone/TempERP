using System.ComponentModel;

namespace Samr.ERP.Core.Enums
{
    public enum ErrorCode
    {
        [Description("invalid login or pass")]
        InvalidLoginPass = 1000,
        [Description("account or employee is locked")]
        AccountOrEmployeeLocked,
        [Description("email must be unique")]
        EmailMustBeUnique,
        [Description("phone must be unique")]
        PhoneMustBeUnique,
        [Description("passport must be unique")]
        PassportMustBeUnique

    }
}