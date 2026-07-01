using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Domain.Entities
{
    public class Employee
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public DateTime DateOfJoining { get; set; }
        public decimal Salary { get; set; }

        // Foreign Key
        public int DepartmentId { get; set; }

        // Navigation Property (EF Core ke liye, relationship establish karta hai)
        public Department? Department { get; set; }
    }
}
