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
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Attar.CRUD.PL.Controllers
{
    [Authorize]
    public class EmployeeController : Controller
    {
        private readonly IEmployeeRepository _employeeRepo;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _env;

        public EmployeeController(
            IEmployeeRepository employeeRepo,
            IMapper mapper,
            IWebHostEnvironment env
            )
        {
            _employeeRepo = employeeRepo;
            _mapper = mapper;
            _env = env;
        }
        public async Task<IActionResult> Index(string searchInp)
        {
            var employees = Enumerable.Empty<Employee>();
            

            if (string.IsNullOrEmpty(searchInp))
                employees = await _employeeRepo.GetAllAsync();
            else
                employees = _employeeRepo.searchByName(searchInp.ToLower());

            var mappedEmp = _mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeViewModel>>(employees);

            return View(mappedEmp);
        }

        [HttpGet]
        public async Task<IActionResult> Search(string searchInp)
        {
            var employees = Enumerable.Empty<Employee>();


            if (string.IsNullOrEmpty(searchInp))
                employees = await _employeeRepo.GetAllAsync();
            else
                employees = _employeeRepo.searchByName(searchInp.ToLower());

            var mappedEmp = _mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeViewModel>>(employees);

            return PartialView("EmployeeSearchTablePartial", mappedEmp);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(EmployeeViewModel employeeVM)
        {
            if (ModelState.IsValid) //Server Side Validation
            {
                employeeVM.ImageName = await DocumentSettings.UploadFile(employeeVM.Image, "images");

                var mappedEmp = _mapper.Map<EmployeeViewModel, Employee>(employeeVM);

                var count = _employeeRepo.Add(mappedEmp);

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

            return View(employeeVM);

        }

        [HttpGet]
        public async Task<IActionResult> Details(int? id, string viewName = "Details")
        {
            if (!id.HasValue)
            {
                return BadRequest(); //400
            }

            var employee = await _employeeRepo.GetAsync(id.Value);

            var mappedEmp = _mapper.Map<Employee, EmployeeViewModel>(employee);

            if (employee is null)
            {
                return NotFound(); //404 
            }
            if (viewName.Equals("Delete", System.StringComparison.OrdinalIgnoreCase))
            {
                TempData["ImageName"] = employee.ImageName;
            }

            return View(viewName, mappedEmp);


        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            return await Details(id, "Edit");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] int id, EmployeeViewModel employeeVM)
        {
            if (id != employeeVM.Id)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
                return View(employeeVM);

            try
            {
                employeeVM.ImageName = await DocumentSettings.UploadFile(employeeVM.Image, "images");

                var mappedEmp = _mapper.Map<EmployeeViewModel, Employee>(employeeVM);
                _employeeRepo.Update(mappedEmp);

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
                    ModelState.AddModelError(string.Empty, "There is un Error !!!");

                }

                return View(employeeVM);

            }


        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {

            return await Details(id, "Delete");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(EmployeeViewModel employeeVM)
        {
            try
            {
                employeeVM.ImageName = TempData["ImageName"] as string;
                var mappedEmp = _mapper.Map<EmployeeViewModel, Employee>(employeeVM);
                var count = _employeeRepo.Delete(mappedEmp);

                if (count > 0)
                {
                    DocumentSettings.DeleteFile(employeeVM.ImageName, "images");
                    return RedirectToAction(nameof(Index));

                }
                return View(employeeVM);
            }
            catch (System.Exception ex)
            {

                if (_env.IsDevelopment())
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "There is un error");

                }

                return View(employeeVM);
            }
        }
    }
}
