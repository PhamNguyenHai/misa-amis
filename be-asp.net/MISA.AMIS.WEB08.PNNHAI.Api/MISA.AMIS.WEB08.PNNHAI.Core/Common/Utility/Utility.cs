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
        /// <summary>
        /// Hàm thực hiện lấy ra giá trị của enum từ description
        /// </summary>
        /// <param name="enumType">type của enum</param>
        /// <param name="description">giá trị description</param>
        /// <returns></returns>
        /// Author: PNNHai
        /// Date:
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
