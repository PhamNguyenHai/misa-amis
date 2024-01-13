using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.WEB08.PNNHAI.Core
{
    public enum Gender
    {
        /// <summary>
        /// Giới tính nam
        /// </summary>
        [Description("Nam")]
        Male = 0,

        /// <summary>
        /// Giới tính nữ
        /// </summary>
        [Description("Nữ")]
        Female = 1,

        /// <summary>
        /// Giới tính khác
        /// </summary>
        [Description("Khác")]
        Other = 2,
    }
}
