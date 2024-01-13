using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.WEB08.PNNHAI.Core.Consts
{
    public static class DepartmentAttributeValidationConst
    {
        // required
        public const string DEPARTMENT_ID_REQUIRED = "Mã định danh phòng ban không được để trống !";
        public const string DEPARTMENT_CODE_REQUIRED = "Mã phòng ban không được để trống !";
        public const string DEPARTMENT_NAME_REQUIRED = "Tên phòng ban không được để trống !";

        // invalid length
        public const string DEPARTMENT_CODE_NO_MORE_THAN_MAX_LENGTH = "Mã phòng ban tối đa 20 kí tự !";
        public const string DEPARTMENT_NAME_NO_MORE_THAN_MAX_LENGTH = "Tên phòng ban tối đa 255 kí tự !";
    }
}
