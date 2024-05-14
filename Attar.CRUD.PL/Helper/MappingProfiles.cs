using Attar.CRUD.DAL.Entities;
using Attar.CRUD.PL.ViewModels;
using AutoMapper;

namespace Attar.CRUD.PL.Helper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<DepartmentViewModel, Department>().ReverseMap();
            CreateMap<EmployeeViewModel, Employee>().ReverseMap();
        }
    }
}
