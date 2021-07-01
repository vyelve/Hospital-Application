using Hospital.Entities;
using Hospital.Repository;
using HospitalProject.Extensions;
using HospitalProject.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Linq;

namespace HospitalProject.Controllers
{
    public class NurseController : Controller
    {
        private readonly INurseRepository _nurseRepository;
        private readonly IUserRepository _userRepository;
        private readonly IDesignationRepository _designationRepository;

        public NurseController(INurseRepository nurseRepository,
            IUserRepository userRepository,
            IDesignationRepository designationRepository)
        {
            _nurseRepository = nurseRepository;
            _userRepository = userRepository;
            _designationRepository = designationRepository;
        }

        private NurseViewModel BindData()
        {
            var _userList = _userRepository.GetUserDetails().Where(whr => whr.IsActive == true).ToList();
            var _designation = _designationRepository.GetDesignation().Where(whr => whr.IsActive == true).ToList();

            var users = (from u in _userList
                         join d in _designation on u.DesignationId equals d.DesignationID
                         where d.DesignationName == "Nurses"
                         select new { UserID = u.UserID, Name = u.FirstName + " " + u.LastName }).ToList();

            ViewBag.UserDropdown = new SelectList(users.ToList(), "UserID", "Name");

            string nursetype = NurseTypeEnum.Parttime.ToString();

            var viewModel = new NurseViewModel
            {
                TblNurses = _nurseRepository.GetNurseDetails().Select(sel =>
                new NurseViewModel
                {
                    NurID = sel.NurID,
                    NurseId = sel.NurseId,
                    NurseName = users.Where(whr => whr.UserID == sel.NurseId).Select(x => x.Name).FirstOrDefault(),
                    NurseType = sel.NurseType == nursetype ? NurseTypeEnum.Parttime.GetDisplayName() : sel.NurseType,
                    ShiftType = sel.ShiftType,
                    IsActive = sel.IsActive,
                    CreatedBy = sel.CreatedBy,
                    CreatedAt = sel.CreatedAt,
                    ModifiedBy = sel.ModifiedBy,
                    ModifiedAt = sel.ModifiedAt
                }).OrderBy(ord => ord.NurseName).ToList()
            };

            return viewModel;
        }

        public IActionResult Index()
        {
            var model = BindData();
            return View(model);
        }

        [HttpPost]
        public ActionResult CreateNurse(string nurseModel)
        {
            var model = Newtonsoft.Json.JsonConvert.DeserializeObject<NurseViewModel>(nurseModel);
            if (ModelState.IsValid)
            {
                var _nurse = new Nurse
                {
                    NurseId = model.NurseId,
                    NurseType = Enum.GetName(typeof(NurseTypeEnum), Convert.ToInt32(model.NurseType)),
                    ShiftType = Enum.GetName(typeof(ShifTypeEnum), Convert.ToInt32(model.ShiftType)),
                    IsActive = model.IsActive,
                    CreatedAt = DateTime.Now,
                    CreatedBy = "Admin"
                };
                var result = _nurseRepository.Add(_nurse);
                if (_nurse.NurID != 0)
                {
                    model.Message = "Save";
                    model.TblNurses = BindData().TblNurses;
                }
            }
            return Json(model);
        }

        [HttpPost]
        public ActionResult EditNurse(string nurseModel)
        {
            var model = Newtonsoft.Json.JsonConvert.DeserializeObject<NurseViewModel>(nurseModel);
            if (ModelState.IsValid)
            {
                var nurse = _nurseRepository.GetNurseDetailsById(model.NurID);
                nurse.NurseId = model.NurseId;
                nurse.NurseType = Enum.GetName(typeof(NurseTypeEnum), Convert.ToInt32(model.NurseType));
                nurse.ShiftType = Enum.GetName(typeof(ShifTypeEnum), Convert.ToInt32(model.ShiftType));
                nurse.IsActive = model.IsActive;
                nurse.ModifiedBy = "Admin";
                nurse.ModifiedAt = DateTime.Now;

                var result = _nurseRepository.Update(nurse);
                model.Message = "Update";
                model.TblNurses = BindData().TblNurses;
            }
            return Json(model);
        }

        [HttpPut]
        public JsonResult DeleteNurse(int nurseId)
        {
            bool result = false;
            var model = new NurseViewModel();

            if (nurseId != 0)
            {
                result = _nurseRepository.Delete(nurseId);
                if (result)
                {
                    model.TblNurses = BindData().TblNurses;
                    model.Message = "Delete";
                }
            }
            return Json(model);
        }
    }
}
