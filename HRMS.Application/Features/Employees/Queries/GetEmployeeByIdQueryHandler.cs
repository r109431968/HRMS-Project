using HRMS.Application.Interfaces;
using HRMS.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Application.Features.Employees.Queries
{
    public class GetEmployeeByIdQueryHandler : IRequestHandler<GetEmployeeByIdQuery, Employee?>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetEmployeeByIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Employee?> Handle(
            GetEmployeeByIdQuery request,
            CancellationToken cancellationToken)
        {
            return await _unitOfWork.Employees.GetByIdAsync(request.Id);
        }
    }
}
