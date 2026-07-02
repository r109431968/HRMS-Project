using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Application.Features.Departments.Commands
{
    public class DeleteDepartmentCommand : IRequest<bool>
    {
        public int Id { get; set; }

        public DeleteDepartmentCommand(int id) 
        {
            Id = id;
        }
    }
}
