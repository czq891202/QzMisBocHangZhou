using System;

namespace QzMisBocHangZhou.DAL
{
    public static class ConvertEx
    {
        public static T To<T>(object value)
        {
            return To<T>(value, default(T));
        }


        public static T To<T>(object value, T defaultValue)
        {
            if (Convert.IsDBNull(value) || value == null) return defaultValue;

            if (value is T) return (T)value;

            var type = typeof(T);
            return (T)ChangeTypeEx(value, type);
        }

        private static object ChangeTypeEx(object value, Type conversionType)
        {
            if (conversionType.IsEnum)
            {
                return ConvertEnum(value, conversionType);
            }
            else
            {
                return Convert.ChangeType(value, conversionType);
            }
        }

        private static object ConvertEnum(object value, Type conversionType)
        {
            if (value is string)
            {
                return Enum.Parse(conversionType, value as string);
            }
            else
            {
                return Enum.ToObject(conversionType, value);
            }
        }
    }
}
