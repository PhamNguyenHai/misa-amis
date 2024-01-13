using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.WEB08.PNNHAI.Core
{
    public static class BaseAuditAttributeValidationConst
    {
        // no more than current date
        public const string CREATED_DATE_NO_MORE_THAN_CURRENT_DATE = "Ngày tạo không được vượt quá ngày hiện tại !";
        public const string MODIFIED_DATE_NO_MORE_THAN_CURRENT_DATE = "Ngày cập nhật không được vượt quá ngày hiện tại !";

        // invalid length
        public const string CREATED_BY_NO_MORE_THAN_MAX_LENGTH = "Tên người tạo tối đa 255 kí tự !";
        public const string MODIFIED_BY_NO_MORE_THAN_MAX_LENGTH = "Tên người cập nhật tối đa 255 kí tự !";
    }
}
