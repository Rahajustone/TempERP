using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Samr.ERP.Core.Models.ErrorModels;

//using NLog;

namespace Samr.ERP.Core.Stuff
{
    public static class Extension
    {
        public static Guid Increment(this Guid value)
        {
            var arr = value.ToByteArray();

            var hiEnd = new Byte[] { arr[15], arr[14], arr[13], arr[12] };
            var val = BitConverter.ToInt32(hiEnd, 0);
            var valArr = BitConverter.GetBytes(++val);

            arr[15] = valArr[0];
            arr[14] = valArr[1];
            arr[13] = valArr[2];
            arr[12] = valArr[3];
            return new Guid(arr);
        }

        public static Guid Increment(this Guid value, Byte index)
        {
            var arr = value.ToByteArray();
            arr[arr.Length - index - 1]++;
            return new Guid(arr);
        }

        public static Guid NullLast(this Guid value)
        {
            var arr = value.ToByteArray();
            arr[15] = 0;
            arr[14] = 0;
            arr[13] = 0;
            arr[12] = 0;
            arr[11] = 0;
            arr[10] = 0;
            return new Guid(arr);
        }

        public static Boolean IsEmpty(this Guid value)
        {
            return value.Equals(Guid.Empty);
        }

        private static Guid FULL_GUID = new Guid("FFFFFFFF-FFFF-FFFF-FFFF-FFFFFFFFFFFF");
        public static Boolean IsFull(this Guid value)
        {
            return value.Equals(FULL_GUID);
        }

        public static string ToStringX(this System.Enum enumerate)
        {
            
            var type = enumerate.GetType();
            var fieldInfo = type.GetField(enumerate.ToString());
            var attributes = (DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);
            return (attributes.Length > 0) ? attributes[0].Description : enumerate.ToString();
        }
        public static List<String> Descriptions(this System.Enum enumerate)
        {
            var values = System.Enum.GetValues(enumerate.GetType());
            var descriptions = new List<String>(values.Length);

            foreach (var enumVal in values)
            {
                descriptions.Add(((System.Enum)enumVal).ToStringX());
            }

            return descriptions;
        }

        public static IEnumerable<ErrorModel> ToErrorModels(this IEnumerable<IdentityError> identityErrors)
        {
            return identityErrors.Select(p => new ErrorModel(p.Description));
        }
    }
}
