using Attar.CRUD.BLL.Interfaces;
using Attar.CRUD.DAL.Data;
using Attar.CRUD.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attar.CRUD.BLL.Repositories
{
    public class EmployeeRepository : GenericRepository<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(ApplicationDbContext context) : base(context)
        {
            //_context = context;
        }
        public IQueryable<Employee> searchByName(string name)
            => _context.Employees.Where(E => E.Name.ToLower().Contains(name));

    }
}
