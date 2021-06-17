using Hospital.Entities;
using Hospital.Repository;
using HospitalProject.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace HospitalProject.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly IDepartmentRepository _departmentRepository;

        public DepartmentController(IDepartmentRepository departmentRepository)
        {
            _departmentRepository = departmentRepository;
        }

        private DepartmentViewModel BindGridData()
        {
            var viewModel = new DepartmentViewModel
            {
                TblDepartment = _departmentRepository.GetDepartments().Select(sel =>
                new DepartmentViewModel
                {
                    DeptId = sel.DeptId,
                    DepartmentName = sel.DepartmentName,
                    IsActive = sel.IsActive,
                    CreatedBy = sel.CreatedBy,
                    CreatedAt = sel.CreatedAt,
                    ModifiedBy = sel.ModifiedBy,
                    ModifiedAt = sel.ModifiedAt
                }).OrderBy(ord => ord.DepartmentName).ToList()
            };
            return viewModel;
        }

        public IActionResult Index()
        {
            var model = BindGridData();
            return View(model);
        }

        [HttpPost]
        public ActionResult CreateDepartment(string departmentModel)
        {
            DepartmentViewModel model = Newtonsoft.Json.JsonConvert.DeserializeObject<DepartmentViewModel>(departmentModel);
            if (ModelState.IsValid)
            {
                var _department = new Department
                {
                    DepartmentName = model.DepartmentName,
                    IsActive = model.IsActive,
                    CreatedAt = System.DateTime.Now,
                    CreatedBy = "Admin"
                };
                var result = _departmentRepository.Add(_department);
                if (_department.DeptId != 0)
                {
                    model.Message = "Save";
                    model.TblDepartment = BindGridData().TblDepartment;
                }
            }
            return Json(model);
        }

        //[HttpGet]
        //public ViewResult EditDepartment(int Id)
        //{
        //    var department = _departmentRepository.GetDepartmentById(Id);
        //    var departmentViewModel = new DepartmentViewModel
        //    {
        //        DeptId = department.DeptId,
        //        DepartmentName = department.DepartmentName,
        //        IsActive = department.IsActive
        //    };

        //    return View(departmentViewModel);
        //}

        [HttpPost]
        public ActionResult EditDepartment(string departmentModel)
        {
            DepartmentViewModel model = Newtonsoft.Json.JsonConvert.DeserializeObject<DepartmentViewModel>(departmentModel);

            if (ModelState.IsValid)
            {
                var department = _departmentRepository.GetDepartmentById(model.DeptId);

                department.DepartmentName = model.DepartmentName;
                department.IsActive = model.IsActive;
                department.ModifiedAt = System.DateTime.Now;
                department.ModifiedBy = "Admin";

                var result = _departmentRepository.Update(department);
                model.Message = "Update";
                model.TblDepartment = BindGridData().TblDepartment;
            }
            return Json(model);
        }

        [HttpPut]
        public JsonResult DeleteDepartment(int DeptId)
        {
            bool result = false;
            var model = new DepartmentViewModel();

            if (DeptId != 0)
            {
                result = _departmentRepository.Delete(DeptId);
                if (result)
                {
                    model.TblDepartment = BindGridData().TblDepartment;
                }
            }
            return Json(model);
        }
    }
}
