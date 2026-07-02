using HRMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace HRMS.Application.Features.Departments.Queries
{
    public class GetAllDepartmentsQuery : IRequest<IEnumerable<Department>>
    {
    }
}
