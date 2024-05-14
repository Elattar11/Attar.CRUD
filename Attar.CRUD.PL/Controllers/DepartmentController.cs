using Attar.CRUD.BLL.Interfaces;
using Attar.CRUD.BLL.Repositories;
using Attar.CRUD.DAL.Entities;
using Attar.CRUD.PL.Helper;
using Attar.CRUD.PL.ViewModels;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Attar.CRUD.PL.Controllers
{
    [Authorize]
    public class DepartmentController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IWebHostEnvironment _env;

        public DepartmentController(
            IMapper mapper, 
            IDepartmentRepository departmentRepository, 
            IWebHostEnvironment env
            )
        {
            _mapper = mapper;
            _departmentRepository = departmentRepository;
            _env = env;
        }
        public async Task<IActionResult> Index(string searchInp)
        {
            var department = Enumerable.Empty<Department>();


            if (string.IsNullOrEmpty(searchInp))
                department = await _departmentRepository.GetAllAsync();
            else
                department = _departmentRepository.searchDepartmentByName(searchInp.ToLower());

            var mappedDep = _mapper.Map<IEnumerable<Department>, IEnumerable<DepartmentViewModel>>(department);

            return View(mappedDep);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(DepartmentViewModel departmentVM)
        {
            if (ModelState.IsValid) //Server Side Validation
            {
                departmentVM.ImageName = await DocumentSettings.UploadFile(departmentVM.Image, "images");

                var mappedDep = _mapper.Map<DepartmentViewModel, Department>(departmentVM);

                var count =  _departmentRepository.Add(mappedDep);

                
                if (count > 0)
                {
                    TempData["Message"] = "Department is created successfully";

                }
                else
                {
                    TempData["Message"] = "Error !!!";
                }
                return RedirectToAction(nameof(Index));
            }

            return View(departmentVM);

        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            return await Details(id, "Edit");
        }


        [HttpGet]
        public async Task<IActionResult> Details(int? id, string viewName = "Details")
        {
            if (!id.HasValue)
            {
                return BadRequest(); //400
            }

            var department = await _departmentRepository.GetAsync(id.Value);

            var mappedDep = _mapper.Map<Department, DepartmentViewModel>(department);

            if (department is null)
            {
                return NotFound(); //404 
            }
            if (viewName.Equals("Delete", System.StringComparison.OrdinalIgnoreCase))
            {
                TempData["ImageName"] = department.ImageName;
            }

            return View(viewName, mappedDep);


        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] int id, DepartmentViewModel departmentVM)
        {
            if (id != departmentVM.Id)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
                return View(departmentVM);

            try
            {
                departmentVM.ImageName = await DocumentSettings.UploadFile(departmentVM.Image, "images");

                var mappedEmp = _mapper.Map<DepartmentViewModel, Department>(departmentVM);
                
                _departmentRepository.Update(mappedEmp);
                return RedirectToAction(nameof(Index));
            }
            catch (System.Exception ex)
            {
                //1. Log Exception

                if (_env.IsDevelopment())
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "There is un Error");

                }

                return View(departmentVM);

            }


        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {

            return await Details(id, "Delete");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(DepartmentViewModel departmentVM)
        {
            try
            {
                departmentVM.ImageName = TempData["ImageName"] as string;
                var mappedEmp = _mapper.Map<DepartmentViewModel, Department>(departmentVM);
                var count = _departmentRepository.Delete(mappedEmp);
                if (count > 0)
                {
                    DocumentSettings.DeleteFile(departmentVM.ImageName, "images");
                    return RedirectToAction(nameof(Index));

                }
                return View(departmentVM);
            }
            catch (System.Exception ex)
            {

                if (_env.IsDevelopment())
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "There is un Error");

                }

                return View(departmentVM);
            }
        }
    }
}
