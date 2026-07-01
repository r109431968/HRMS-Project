using HRMS.Application.Interfaces;
using HRMS.Domain.Entities;
using HRMS.Infrastructure.Persistence;
using HRMS.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Infrastructure.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly HRMSDbContext _context;
        private IGenericRepository<Employee>? _employees;
        private IGenericRepository<Department>? _departments;

        public UnitOfWork(HRMSDbContext context) 
        {
            _context = context;
        }
        public IGenericRepository<Employee> Employees => 
            _employees ??= new GenericRepository<Employee>(_context);

        public IGenericRepository<Department> Departments =>
            _departments ??= new GenericRepository<Department>(_context);

        public void Dispose()
        {
            _context.Dispose();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
