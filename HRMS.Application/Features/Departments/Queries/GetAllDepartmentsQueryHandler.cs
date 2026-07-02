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
    public class GetAllDepartmentsQueryHandler : IRequestHandler<GetAllDepartmentsQuery, IEnumerable<Department>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAllDepartmentsQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IEnumerable<Department>> 
            Handle(GetAllDepartmentsQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Departments.GetAllAsync();
        }
    }
}
