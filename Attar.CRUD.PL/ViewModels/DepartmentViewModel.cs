using System.ComponentModel.DataAnnotations;
using System;
using Microsoft.AspNetCore.Http;
using Attar.CRUD.DAL.Entities;
using System.Collections.Generic;

namespace Attar.CRUD.PL.ViewModels
{
    public class DepartmentViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Code Is Required !!")]
        public string Code { get; set; }

        [Required(ErrorMessage = "Name Is Required !!")]
        public string Name { get; set; }

        [Display(Name = "Date Of Creation")]
        public DateTime DateOfCreation { get; set; }

        public IFormFile Image { get; set; }

        public string ImageName { get; set; }

        public ICollection<Employee> Employees { get; set; } = new HashSet<Employee>();
    }
}
