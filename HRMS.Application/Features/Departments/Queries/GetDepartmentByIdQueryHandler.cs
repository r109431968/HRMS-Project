using HRMS.Application.Interfaces;
using HRMS.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Application.Features.Departments.Queries
{
    public class GetDepartmentByIdQueryHandler : IRequestHandler<GetDepartmentByIdQuery, Department?>
    {
        public readonly IUnitOfWork _unitOfWork;

        public GetDepartmentByIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Department?> 
            Handle(GetDepartmentByIdQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Departments.GetByIdAsync(request.Id);
        }
    }
}
