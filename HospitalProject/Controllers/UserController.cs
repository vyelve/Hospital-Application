using Hospital.Entities;
using Hospital.Repository;
using HospitalProject.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Globalization;
using System.Linq;

namespace HospitalProject.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IDesignationRepository _designationRepository;
        private readonly INurseRepository _nurseRepository;

        public UserController(IUserRepository userRepository,
            IDepartmentRepository departmentRepository,
            IDesignationRepository designationRepository,
            INurseRepository nurseRepository)
        {
            _userRepository = userRepository;
            _departmentRepository = departmentRepository;
            _designationRepository = designationRepository;
            _nurseRepository = nurseRepository;
        }

        private UserViewModel BindData()
        {
            var departments = _departmentRepository.GetDepartments().ToList().Where(whr => whr.IsActive == true).Select(sel => sel);
            var designation = _designationRepository.GetDesignation().ToList().Where(whr => whr.IsActive == true).Select(sel => sel);

            ViewBag.DepartmentDropdown = new SelectList(departments.ToList(), "DeptId", "DepartmentName");
            ViewBag.DesignationDropdown = new SelectList(designation.ToList(), "DesignationID", "DesignationName");

            var viewModel = new UserViewModel
            {
                TblUsers = _userRepository.GetUserDetails().Select(sel =>
                new UserViewModel
                {
                    UserID = sel.UserID,
                    FirstName = sel.FirstName,
                    LastName = sel.LastName,
                    UserShortName = sel.UserShortName,
                    EmailId = sel.EmailId,
                    Gender = sel.Gender,
                    DesignationId = sel.DesignationId,
                    DesignationName = designation.Where(whr => whr.DesignationID == sel.DesignationId).Select(x => x.DesignationName).SingleOrDefault(),
                    DepartmentID = sel.DepartmentID,
                    DepartmentName = departments.Where(whr => whr.DeptId == sel.DepartmentID).Select(x => x.DepartmentName).SingleOrDefault(),
                    PhoneNumber = sel.PhoneNumber,
                    Password = sel.Password,
                    ConfirmPassword = sel.Password,
                    DOJ = sel.Date_of_Joining.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                    IsActive = sel.IsActive,
                    CreatedBy = sel.CreatedBy,
                    CreatedAt = sel.CreatedAt,
                    ModifiedBy = sel.ModifiedBy,
                    ModifiedAt = sel.ModifiedAt,

                }).OrderBy(ord => ord.DesignationName).ToList()
            };
            return viewModel;
        }

        public IActionResult Index()
        {
            var model = BindData();
            return View(model);
        }

        [HttpPost]
        public ActionResult CreateUser(string userModel)
        {
            UserViewModel model = Newtonsoft.Json.JsonConvert.DeserializeObject<UserViewModel>(userModel);
            if (ModelState.IsValid)
            {
                var _user = new User
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    UserShortName = model.UserShortName,
                    EmailId = model.EmailId,
                    Gender = Enum.GetName(typeof(GenderEnum), Convert.ToInt32(model.Gender)),
                    DesignationId = model.DesignationId,
                    PhoneNumber = model.PhoneNumber,
                    DepartmentID = model.DepartmentID,
                    Date_of_Joining = Convert.ToDateTime(model.DOJ),
                    Password = model.Password,
                    IsActive = model.IsActive,
                    CreatedAt = DateTime.Now,
                    CreatedBy = "Admin"
                };
                var result = _userRepository.Add(_user);
                if (_user.UserID != 0)
                {
                    model.Message = "Save";
                    model.TblUsers = BindData().TblUsers;
                }
            }
            return Json(model);
        }

        [HttpPost]
        public ActionResult EditUser(string userModel)
        {
            UserViewModel model = Newtonsoft.Json.JsonConvert.DeserializeObject<UserViewModel>(userModel);
            if (ModelState.IsValid)
            {
                var user = _userRepository.GetUserById(model.UserID);
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.UserShortName = model.UserShortName;
                user.EmailId = model.EmailId;
                user.Gender = Enum.GetName(typeof(GenderEnum), Convert.ToInt32(model.Gender));
                user.DesignationId = model.DesignationId;
                user.PhoneNumber = model.PhoneNumber;
                user.DepartmentID = model.DepartmentID;
                user.Date_of_Joining = Convert.ToDateTime(model.DOJ);
                user.IsActive = model.IsActive;
                user.ModifiedBy = "Admin";
                user.ModifiedAt = DateTime.Now;

                var result = _userRepository.Update(user);
                model.Message = "Update";
                model.TblUsers = BindData().TblUsers;
            }
            return Json(model);
        }

        [HttpPut]
        public JsonResult DeleteUser(int UserId)
        {
            bool result = false;
            var model = new UserViewModel();

            if (UserId != 0)
            {
                result = _userRepository.Delete(UserId);
                if (result)
                {
                    model.TblUsers = BindData().TblUsers;
                    model.Message = "Delete";
                }
            }
            return Json(model);
        }

        [HttpGet]
        public JsonResult ValidatedNurseUserActive(int UserId)
        {
            var nurse = _nurseRepository.GetNurseDetails().Where(whr => whr.NurseId == UserId && whr.IsActive == true).Select(sel => sel).SingleOrDefault();
            var model = new UserViewModel();
            if (nurse != null)
            {
                if (nurse.IsActive)
                {
                    model.Message = "User is Mapped";
                }
            }
            else
            {
                model.Message = "User is not Mapped";
            }
            return Json(model);
        }
    }
}
