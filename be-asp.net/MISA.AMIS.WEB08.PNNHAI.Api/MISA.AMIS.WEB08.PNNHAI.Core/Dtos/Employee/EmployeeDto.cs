using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.WEB08.PNNHAI.Core
{
    public class EmployeeDto
    {
        public Guid EmployeeId { set; get; }
        public string EmployeeCode { set; get; }
        public string EmployeeName { set; get; }
        public bool? IsCustomer { set; get; }
        public bool? IsProvider { set; get; }
        public DateTime? DateOfBirth { set; get; }
        /// <summary>
        /// Giới tính
        /// </summary>
        public Gender Gender { set; get; }
        public Guid DepartmentId { set; get; }
        public string DepartmentName { get; set; }
        public string? PositionName { set; get; }
        public string? IdentityNumber { set; get; }
        public DateTime? IdentityIssuedDate { set; get; }
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
