using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.WEB08.PNNHAI.Core
{
    public static class Utility
    {
        public static string GetDescriptionFromEnumValue(Enum value)
        {
            DescriptionAttribute attribute = value.GetType()
                .GetField(value.ToString())
                .GetCustomAttributes(typeof(DescriptionAttribute), false)
                .SingleOrDefault() as DescriptionAttribute;
            return attribute == null ? value.ToString() : attribute.Description;
        }

        public static T GetEnumValueFromDescription<T>(string description)
        {
            var type = typeof(T);
            if (!type.IsEnum)
                throw new ArgumentException();
            FieldInfo[] fields = type.GetFields();
            var field = fields
                            .SelectMany(f => f.GetCustomAttributes(
                                typeof(DescriptionAttribute), false), (
                                    f, a) => new { Field = f, Att = a })
                            .Where(a => ((DescriptionAttribute)a.Att)
                                .Description == description).SingleOrDefault();
            return field == null ? default(T) : (T)field.Field.GetRawConstantValue();
        }

        public static object GetEnumValueFromDescription(Type enumType, string description)
        {
            foreach (var field in enumType.GetFields(BindingFlags.Public | BindingFlags.Static))
            {
                if (Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) is DescriptionAttribute attribute)
                {
                    if (attribute.Description == description)
                    {
                        return field.GetValue(null);
                    }
                }
                else
                {
                    if (field.Name == description)
                    {
                        return field.GetValue(null);
                    }
                }
            }

            return null;
        }
    }
}
