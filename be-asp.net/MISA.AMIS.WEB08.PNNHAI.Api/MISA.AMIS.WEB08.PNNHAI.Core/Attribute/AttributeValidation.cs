using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.WEB08.PNNHAI.Core
{
    /// <summary>
    /// Custom attribute validate không cho nhập ngày lớn hơn ngày hiện tại
    /// </summary>
    public class NoMoreThanCurrentDate : ValidationAttribute
    {
        public string ErrorMessage { get; set; } = Core.Resources.AppResource.InputDateNoMoreThanCurrentDateDefault;

        /// <summary>
        /// Hàm xử lý date có lớn hơn ngày hiện tại ko
        /// </summary>
        /// <param name="value">giá trị date</param>
        /// <param name="validationContext"></param>
        /// <returns>trạng thái success hoặc không</returns>
        /// Author: PNNHai
        /// Date:
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            DateTime currentDate = DateTime.Now.Date;

            if (value is DateTime dateValue && dateValue.Date > currentDate)
            {
                return new ValidationResult(ErrorMessage);
            }

            return ValidationResult.Success;
        }
    }
}
