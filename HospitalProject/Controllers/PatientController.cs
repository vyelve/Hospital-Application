using Hospital.Entities;
using Hospital.Repository;
using HospitalProject.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace HospitalProject.Controllers
{
    public class PatientController : Controller
    {
        private readonly IPatientRepository _patientRepository;
        private readonly IUserRepository _userRepository;
        private readonly IDesignationRepository _designationRepository;

        public PatientController(IPatientRepository patientRepository, IUserRepository userRepository,
            IDesignationRepository designationRepository)
        {
            _patientRepository = patientRepository;
            _userRepository = userRepository;
            _designationRepository = designationRepository;
        }

        private PatientViewModel BindData()
        {
            var _userList = _userRepository.GetUserDetails().Where(whr => whr.IsActive == true).ToList();
            var _designation = _designationRepository.GetDesignation().Where(whr => whr.IsActive == true).ToList();

            var users = (from u in _userList
                         join d in _designation on u.DesignationId equals d.DesignationID
                         where d.DesignationName == "Doctors"
                         select new { UserID = u.UserID, Name = u.FirstName + " " + u.LastName }).ToList();

            ViewBag.DoctorDropdown = new SelectList(users, "UserID", "Name");

            var viewModel = new PatientViewModel
            {
                TblPatientDetails = _patientRepository.GetPatientDetails().Select(sel => new PatientViewModel
                {
                    PatientID = sel.PatientID,
                    PatientNumber = sel.PatientNumber,
                    FirstName = sel.FirstName,
                    MiddleName = sel.MiddleName,
                    LastName = sel.LastName,
                    PatientFullName = sel.FirstName + " " + sel.MiddleName + " " + sel.LastName,
                    Gender = sel.Gender,
                    PhoneNumber = sel.PhoneNumber,
                    DOB = sel.DOB.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                    Age = sel.Age,
                    MaritalStatus = sel.MaritalStatus,
                    EmergencyContactPerson = sel.EmergencyContactPerson,
                    EmergencyContactNumber = sel.EmergencyContactNumber,
                    Address = sel.Address,
                    MedicalHistory = sel.MedicalHistory,
                    DoctorId = sel.DoctorId,
                    DoctorName = users.Where(whr => whr.UserID == sel.DoctorId).Select(sel => sel.Name).FirstOrDefault(),
                    HasInsurance = sel.HasInsurance,
                    Insuranceid = sel.Insuranceid,
                    HasAdmitted = sel.HasAdmitted,
                    CreatedBy = sel.CreatedBy,
                    CreatedAt = sel.CreatedAt,
                    ModifiedBy = sel.ModifiedBy,
                    ModifiedAt = sel.ModifiedAt
                }).OrderBy(ord => ord.PatientID).ToList()
            };

            return viewModel;
        }

        public IActionResult Index()
        {
            var model = BindData();
            return View(model);
        }

        [HttpPost]
        public ActionResult CreatePatientDetails(string patientModel)
        {
            var model = Newtonsoft.Json.JsonConvert.DeserializeObject<PatientViewModel>(patientModel);
            if (ModelState.IsValid)
            {
                var patient = new Patient
                {
                    FirstName = model.FirstName,
                    MiddleName = model.MiddleName,
                    LastName = model.LastName,
                    Gender = Enum.GetName(typeof(GenderEnum), Convert.ToInt32(model.Gender)),
                    PhoneNumber = model.PhoneNumber,
                    DOB = Convert.ToDateTime(model.DOB),
                    MaritalStatus = Enum.GetName(typeof(MaritalStatusEnum), Convert.ToInt32(model.MaritalStatus)),
                    EmergencyContactPerson = model.EmergencyContactPerson,
                    EmergencyContactNumber = model.EmergencyContactNumber,
                    Address = model.Address,
                    MedicalHistory = model.MedicalHistory,
                    DoctorId = model.DoctorId,
                    HasInsurance = model.HasInsurance,
                    Insuranceid = model.Insuranceid,
                    HasAdmitted = model.HasAdmitted,
                    CreatedAt = DateTime.Now,
                    CreatedBy = "Admin"
                };
                var result = _patientRepository.Add(patient);
                if (patient.PatientID != 0)
                {
                    model.Message = "Save";
                    model.TblPatientDetails = BindData().TblPatientDetails;
                }
            }
            return Json(model);
        }

        [HttpPost]
        public ActionResult EditPatientDetails(string patientModel)
        {
            var model = Newtonsoft.Json.JsonConvert.DeserializeObject<PatientViewModel>(patientModel);
            if (ModelState.IsValid)
            {
                var patient = _patientRepository.GetPatientDetailsById(model.PatientID);
                patient.FirstName = model.FirstName;
                patient.MiddleName = model.MiddleName;
                patient.LastName = model.LastName;
                patient.Gender = Enum.GetName(typeof(GenderEnum), Convert.ToInt32(model.Gender));
                patient.PhoneNumber = model.PhoneNumber;
                patient.DOB = Convert.ToDateTime(model.DOB);
                patient.MaritalStatus = Enum.GetName(typeof(MaritalStatusEnum), Convert.ToInt32(model.MaritalStatus));
                patient.EmergencyContactPerson = model.EmergencyContactPerson;
                patient.EmergencyContactNumber = model.EmergencyContactNumber;
                patient.Address = model.Address;
                patient.MedicalHistory = model.MedicalHistory;
                patient.DoctorId = model.DoctorId;
                patient.HasInsurance = model.HasInsurance;
                patient.Insuranceid = model.Insuranceid;
                patient.HasAdmitted = model.HasAdmitted;
                patient.ModifiedAt = DateTime.Now;
                patient.ModifiedBy = "Admin";

                var result = _patientRepository.Update(patient);
                model.Message = "Update";
                model.TblPatientDetails = BindData().TblPatientDetails;
            }
            return Json(model);
        }

        [HttpPut]
        public JsonResult DeletePatient(int PatientID)
        {
            bool result = false;
            var model = new PatientViewModel();

            if (PatientID != 0)
            {
                result = _patientRepository.Delete(PatientID);
                if (result)
                {
                    model.TblPatientDetails = BindData().TblPatientDetails;
                }
            }
            return Json(model);
        }
    }
}
