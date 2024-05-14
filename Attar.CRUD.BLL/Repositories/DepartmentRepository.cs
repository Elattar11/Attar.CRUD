using Attar.CRUD.BLL.Interfaces;
using Attar.CRUD.DAL.Data;
using Attar.CRUD.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attar.CRUD.BLL.Repositories
{
    public class DepartmentRepository : GenericRepository<Department>, IDepartmentRepository
    {
        public DepartmentRepository(ApplicationDbContext context) : base(context)
        {
            
        }
        public IQueryable<Department> searchDepartmentByName(string name)
            => _context.Departments.Where(D => D.Name.ToLower().Contains(name));
    }
}
