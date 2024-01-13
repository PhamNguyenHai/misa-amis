using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.WEB08.PNNHAI.Core
{
    public enum FilterType
    {
        TextFilterType,
        SelectionFilterTye,
        ComparableFilterType,
    }

    public enum TextFilterType
    {
        /// <summary>
        /// Với các cột dữ liệu kiểu text
        /// </summary>
        StartWith = 0,

        /// <summary>
        /// Với các cột dữ liệu kiểu Date
        /// </summary>
        EndWith = 1,

        /// <summary>
        /// Với các cột dữ liệu kiểu số
        /// </summary>
        Contain = 2,

        /// <summary>
        /// Với các cột dữ liệu lựa chọn (vd: department)
        /// </summary>
        NotContain = 3,

        /// <summary>
        /// Với các cột dữ liệu kiểu text
        /// </summary>
        Equal = 4,

        /// <summary>
        /// Với các cột dữ liệu kiểu Date
        /// </summary>
        NotEqual = 5,
    }

    public enum ComparableFilterType
    {
        /// <summary>
        ///             >
        /// </summary>
        Greater = 0,

        /// <summary>
        ///             <
        /// </summary>
        Less = 1,

        /// <summary>
        ///             >=
        /// </summary>
        GreaterOrEqual = 2,

        /// <summary>
        ///             <=
        /// </summary>
        LessOrEqual = 3,

        /// <summary>
        ///             =
        /// </summary>
        Equal = 4,

        /// <summary>
        ///             <>
        /// </summary>
        NotEqual = 5,
    }

    public enum SelectionFilterTye
    {
        Equal = 0,
        NotEqual = 1,
    }
}
