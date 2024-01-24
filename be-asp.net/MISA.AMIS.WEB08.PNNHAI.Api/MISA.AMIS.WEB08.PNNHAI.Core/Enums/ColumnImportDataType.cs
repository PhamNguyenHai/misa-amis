using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.WEB08.PNNHAI.Core
{
    public enum ColumnImportDataType
    {
        /// <summary>
        /// Chuỗi
        /// </summary>
        String = 0,

        /// <summary>
        /// Số nguyên
        /// </summary>
        Int = 1,

        /// <summary>
        /// True/ False
        /// </summary>
        Boolean = 2,

        /// <summary>
        /// Enum
        /// </summary>
        Enum = 3,

        /// <summary>
        /// Tham chiếu tới bảng dữ liệu xác định trong Database
        /// </summary>
        ReferenceTable = 4
    }
}
