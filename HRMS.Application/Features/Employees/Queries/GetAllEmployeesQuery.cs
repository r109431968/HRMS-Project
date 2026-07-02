using HRMS.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Application.Features.Employees.Queries
{
    public class GetAllEmployeesQuery : IRequest<IEnumerable<Employee>>
    {
    }
}
