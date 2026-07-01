using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Domain.Entities
{
    public class Department
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        // Navigation Property — ek Department mein multiple Employees ho sakte hain
        public ICollection<Employee> Employees { get; set; } = new List<Employee>();
    }
}
