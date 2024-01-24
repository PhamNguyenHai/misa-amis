using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.WEB08.PNNHAI.Core
{
    public class EmployeeExcelImportRespondDto : ExcelImportRespondedDto
    {
        public string? EmployeeCode { set; get; }
        public string? EmployeeName { set; get; }
        public string? DateOfBirth { set; get; }
        public string? Gender { set; get; }
        public string? DepartmentId { set; get; }
        public string? PositionName { set; get; }
        public string? IdentityNumber { set; get; }
        public string? IdentityIssuedDate { set; get; }
        public string? IdentityIssuedPlace { set; get; }
        public string? Address { set; get; }
        public string? PhoneNumber { set; get; }
        public string? LandlineNumber { set; get; }
        public string? Email { set; get; }
        public string? BankNumber { set; get; }
        public string? BankName { set; get; }
        public string? BankBranch { set; get; }
    }
}
