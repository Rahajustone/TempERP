using System;
using Newtonsoft.Json;
using Samr.ERP.Core.Enums;

namespace Samr.ERP.Core.Models
{
    public class SortDirectionTypeEnumConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            SortDirection enumValue = (SortDirection) value;
            switch (enumValue )
            {
                case SortDirection.Asc:
                    writer.WriteValue("ASC");
                    break;
                case SortDirection.Desc:
                    writer.WriteValue("DESC");
                    break;
            }
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var enumString = (string)reader.Value;

            if (enumString == "ASC")
            {
                return SortDirection.Asc;
            }
            else if (enumString == "DESC")
            {
                return SortDirection.Desc;
            }
            throw new NotSupportedException("Неизвестное значение: " + enumString);
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(string);
        }
    }
}