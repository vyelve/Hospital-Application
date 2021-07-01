using Hospital.Entities;
using Hospital.Repository;
using HospitalProject.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HospitalProject.Controllers
{
    public class DoctorController : Controller
    {
        private readonly IDoctorRepository _doctorRepository;
        private readonly IUserRepository _userRepository;
        private readonly IDesignationRepository _designationRepository;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly ISpecializationRepository _specializationRepository;

        public DoctorController(IDoctorRepository doctorRepository,
            IUserRepository userRepository,
            IDesignationRepository designationRepository, 
            IDepartmentRepository departmentRepository,
            ISpecializationRepository specializationRepository)
        {
            _doctorRepository = doctorRepository;
            _userRepository = userRepository;
            _designationRepository = designationRepository;
            _departmentRepository = departmentRepository;
            _specializationRepository = specializationRepository;
        }

        private DoctorViewModel BindData()
        {
            var _userList = _userRepository.GetUserDetails().Where(whr => whr.IsActive == true).ToList();
            var _designation = _designationRepository.GetDesignation().Where(whr => whr.IsActive == true).ToList();
            var _department = _departmentRepository.GetDepartments().Where(whr => whr.IsActive == true).ToList();
            var _specialization = _specializationRepository.GetSpecializationDetails().Where(whr => whr.IsActive == true).ToList();

            var users = (from u in _userList
                         join d in _designation on u.DesignationId equals d.DesignationID
                         where d.DesignationName == "Doctors"
                         select new { UserID = u.UserID, Name = u.FirstName + " " + u.LastName }).ToList();

            ViewBag.DoctorDropdown = new SelectList(users, "UserID", "Name");
            ViewBag.DepartmentDropdown = new SelectList(_department, "DeptId", "DepartmentName");
            ViewBag.SpecializationDropdown = new SelectList(_specialization, "SpecialistID", "SpecializationName");

            var viewModel = new DoctorViewModel
            {
                TblDoctor = _doctorRepository.GetDoctors().Select(sel => new DoctorViewModel
                {
                    DocID = sel.DocID,
                    DoctorId = sel.DoctorId,
                    DoctorName = users.Where(whr => whr.UserID == sel.DoctorId).Select(sel => sel.Name).FirstOrDefault(),
                    DepartmentId = sel.DepartmentId,
                    DepartmentName = _department.Where(whr => whr.DeptId == sel.DepartmentId).Select(sel => sel.DepartmentName).FirstOrDefault(),
                    SpecialistName = sel.Specialist,
                    SpecialistID = _specialization.Where(whr => whr.SpecializationName == sel.Specialist).Select(sel => sel.SpecialistID).FirstOrDefault(),
                    Per_Day_Charges = sel.Per_Day_Charges,
                    DoctorSchedule = sel.DoctorSchedule,
                    IsActive = sel.IsActive,
                    CreatedBy = sel.CreatedBy,
                    CreatedAt = sel.CreatedAt,
                    ModifiedBy = sel.ModifiedBy,
                    ModifiedAt = sel.ModifiedAt
                }).OrderBy(ord => ord.DoctorName).ToList()
            };

            return viewModel;
        }

        public IActionResult Index()
        {
            var model = BindData();
            return View(model);
        }

        [HttpPost]
        public ActionResult CreateDoctor(string doctorModel)
        {
            var model = Newtonsoft.Json.JsonConvert.DeserializeObject<DoctorViewModel>(doctorModel);
            if (ModelState.IsValid)
            {
                var doctor = new Doctor
                {
                    DoctorId = model.DoctorId,
                    DepartmentId = model.DepartmentId,
                    Specialist = model.SpecialistName,
                    Per_Day_Charges = model.Per_Day_Charges,
                    DoctorSchedule = model.DoctorSchedule,
                    IsActive = model.IsActive,
                    CreatedAt = DateTime.Now,
                    CreatedBy = "Admin"
                };
                var result = _doctorRepository.Add(doctor);
                if (doctor.DocID != 0)
                {
                    model.Message = "Save";
                    model.TblDoctor = BindData().TblDoctor;
                }
            }
            return Json(model);
        }

        [HttpPost]
        public ActionResult EditDoctor(string doctorModel)
        {
            var model = Newtonsoft.Json.JsonConvert.DeserializeObject<DoctorViewModel>(doctorModel);
            if (ModelState.IsValid)
            {
                var doctor = _doctorRepository.GetDoctorById(model.DocID);
                doctor.DoctorId = model.DoctorId;
                doctor.DepartmentId = model.DepartmentId;
                doctor.Specialist = model.SpecialistName;
                doctor.Per_Day_Charges = model.Per_Day_Charges;
                doctor.DoctorSchedule = model.DoctorSchedule;
                doctor.IsActive = model.IsActive;
                doctor.ModifiedBy = "Admin";
                doctor.ModifiedAt = DateTime.Now;

                var result = _doctorRepository.Update(doctor);
                model.Message = "Update";
                model.TblDoctor = BindData().TblDoctor;
            }
            return Json(model);
        }

        [HttpPut]
        public ActionResult DeleteDoctor(int ID)
        {
            bool result = false;
            var model = new DoctorViewModel();
            if (ID != 0)
            {
                result = _doctorRepository.Delete(ID);
                if (result)
                {
                    model.TblDoctor = BindData().TblDoctor;
                    model.Message = "Delete";
                }
            }
            return Json(model);
        }
    }
}
