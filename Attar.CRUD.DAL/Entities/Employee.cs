using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Attar.CRUD.DAL.Entities
{
    public enum Gender
    {
        [EnumMember(Value = "Male")]
        Male = 1,
        [EnumMember(Value = "Female")]
        Female = 2
    }

    public enum EmpType
    {
        [EnumMember(Value = "Full-time")]
        FullTime = 1,
        [EnumMember(Value = "Part-time")]
        PartTime = 2
    }

    public class Employee : BaseEntity
    {
        public string ImageName { get; set; }
        public string Name { get; set; }

        public int? Age { get; set; }

        public string Address { get; set; }

        public decimal Salary { get; set; }

        public bool IsActive { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public DateTime HiringDate { get; set; }

        public DateTime CreationDate { get; set; } = DateTime.Now;

        public Gender Gender { get; set; }
        public EmpType EmployeeType { get; set; }
        public bool IsDeleted { get; set; }

        public int? DepartmentId { get; set; }
        public Department Department { get; set; }
    }
}
