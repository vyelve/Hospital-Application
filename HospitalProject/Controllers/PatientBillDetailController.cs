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
    public class PatientBillDetailController : Controller
    {
        private readonly IPatientBillDetailRepository _patientBillDetailRepository;
        private readonly IUserRepository _userRepository;
        private readonly IPatientRepository _patientRepository;
        private readonly IRoomRepository _roomRepository;
        private readonly IDoctorRepository _doctorRepository;
        private readonly IDesignationRepository _designationRepository;
        private readonly IPaymentRepository _paymentRepository;

        public PatientBillDetailController(IPatientBillDetailRepository patientBillDetailRepository,
            IUserRepository userRepository,
            IPatientRepository patientRepository,
            IRoomRepository roomRepository,
            IDoctorRepository doctorRepository,
            IDesignationRepository designationRepository,
            IPaymentRepository paymentRepository)
        {
            _patientBillDetailRepository = patientBillDetailRepository;
            _userRepository = userRepository;
            _patientRepository = patientRepository;
            _roomRepository = roomRepository;
            _doctorRepository = doctorRepository;
            _designationRepository = designationRepository;
            _paymentRepository = paymentRepository;
        }

        private PatientBillDetailViewModel BindData()
        {
            var _userList = _userRepository.GetUserDetails().Where(whr => whr.IsActive == true).ToList();
            var _designation = _designationRepository.GetDesignation().Where(whr => whr.IsActive == true).ToList();
            var _roomList = _roomRepository.GetRoomDetails().Where(whr => whr.IsActive == true).ToList();
            var _patient = _patientRepository.GetPatientDetails().ToList();
            var _payment = _paymentRepository.GetPaymentDetails().ToList();

            var users = (from u in _userList
                         join d in _designation on u.DesignationId equals d.DesignationID
                         where d.DesignationName == "Doctors"
                         select new
                         {
                             UserID = u.UserID,
                             Name = u.FirstName + " " + u.LastName
                         }).ToList();

            var patient = _patient.Select(sel => new
            {
                PatientId = sel.PatientID,
                PatientName = sel.FirstName + " " + sel.MiddleName + " " + sel.LastName
            }).ToList();

            var patientDD = (from p in patient
                             join pb in _patientBillDetailRepository.GetPatientBillDetails() on p.PatientId equals pb.PatientId
                             where pb.IsDischarge == false
                             select new
                             {
                                 PatientId = p.PatientId,
                                 PatientName = p.PatientName
                             }).ToList();

            ViewBag.DoctorDropdown = new SelectList(users, "UserID", "Name");
            ViewBag.RoomDropdown = new SelectList(_roomList, "RoomId", "RoomType");
            ViewBag.PatientDropdown = new SelectList(patient, "PatientId", "PatientName");

            var viewModel = new PatientBillDetailViewModel
            {
                TblPatientBillDetails = _patientBillDetailRepository.GetPatientBillDetails().Select(sel =>
                new PatientBillDetailViewModel
                {
                    BillId = sel.BillId,
                    BillNumber = sel.BillNumber,
                    PatientId = sel.PatientId,
                    PatientName = patient.Where(whr => whr.PatientId == sel.PatientId).Select(sel => sel.PatientName).FirstOrDefault(),
                    RoomId = sel.RoomId,
                    RoomName = _roomList.Where(whr => whr.RoomId == sel.RoomId).Select(sel => sel.RoomType).FirstOrDefault(),
                    RoomCharges = sel.RoomCharges,
                    No_of_Days_Admitted = sel.No_of_Days_Admitted,
                    DoctorId = sel.DoctorId,
                    DoctorName = users.Where(whr => whr.UserID == sel.DoctorId).Select(sel => sel.Name).FirstOrDefault(),
                    DoctorCharges = sel.DoctorCharges,
                    MedicineBill = sel.MedicineBill,
                    TotalBill = sel.TotalBill,
                    PaidBill = _payment.Where(whr => whr.BillId == sel.BillId && whr.PatientId == sel.PatientId).Select(sel => sel.Amount).Sum(),
                    RemainingBill = sel.RemainingBill,
                    IsDischarge = sel.IsDischarge,
                    CreatedBy = sel.CreatedBy,
                    CreatedAt = sel.CreatedAt,
                    ModifiedBy = sel.ModifiedBy,
                    ModifiedAt = sel.ModifiedAt
                }).OrderBy(ord => ord.BillNumber)
            };
            return viewModel;
        }

        public IActionResult Index()
        {
            var model = BindData();
            return View(model);
        }

        [HttpGet]
        public JsonResult GetRoomCharges(int ID)
        {
            var _roomCharges = _roomRepository.GetRoomDetailsById(ID).Per_Day_Charges;
            return Json(_roomCharges);
        }

        [HttpGet]
        public JsonResult GetDoctorCharges(int ID)
        {
            var _doctorCharges = _doctorRepository.GetDoctorById(ID);
            return Json(_doctorCharges.Per_Day_Charges);
        }

        [HttpGet]
        public JsonResult GetDoctorByPatientId(int ID)
        {
            var _userList = _userRepository.GetUserDetails().Where(whr => whr.IsActive == true).ToList();
            var _patient = _patientRepository.GetPatientDetails().ToList();

            var users = (from u in _userList
                         join p in _patient on u.UserID equals p.DoctorId
                         where p.PatientID == ID
                         select new
                         {
                             UserID = u.UserID,
                             Name = u.FirstName + " " + u.LastName
                         }).ToList();

            return Json(new SelectList(users, "UserID", "Name"));
        }

        [HttpPost]
        public ActionResult CreatePatientBillDetail(string patientBillModel)
        {
            var model = Newtonsoft.Json.JsonConvert.DeserializeObject<PatientBillDetailViewModel>(patientBillModel);
            if (ModelState.IsValid)
            {
                var _patientBill = new PatientBillDetail
                {
                    PatientId = model.PatientId,
                    RoomId = model.RoomId,
                    No_of_Days_Admitted = model.No_of_Days_Admitted,
                    RoomCharges = model.RoomCharges,
                    DoctorId = model.DoctorId,
                    DoctorCharges = model.DoctorCharges,
                    MedicineBill = model.MedicineBill,
                    TotalBill = model.TotalBill,
                    PaidBill = model.PaidBill,
                    RemainingBill = model.RemainingBill,
                    IsDischarge = model.IsDischarge,
                    CreatedAt = DateTime.Now,
                    CreatedBy = "Admin"
                };

                var result = _patientBillDetailRepository.Add(_patientBill);
                if (_patientBill.BillId != 0)
                {
                    model.Message = "Save";
                    model.TblPatientBillDetails = BindData().TblPatientBillDetails;
                }
            }
            return Json(model);
        }

        [HttpPost]
        public ActionResult EditPatientBillDetail(string patientBillModel)
        {
            var model = Newtonsoft.Json.JsonConvert.DeserializeObject<PatientBillDetailViewModel>(patientBillModel);
            if (ModelState.IsValid)
            {
                var _patientBill = _patientBillDetailRepository.GetPatientBillDetailById(model.BillId);
                _patientBill.PatientId = model.PatientId;
                _patientBill.RoomId = model.RoomId;
                _patientBill.No_of_Days_Admitted = model.No_of_Days_Admitted;
                _patientBill.RoomCharges = model.RoomCharges;
                _patientBill.DoctorId = model.DoctorId;
                _patientBill.DoctorCharges = model.DoctorCharges;
                _patientBill.MedicineBill = model.MedicineBill;
                _patientBill.TotalBill = model.TotalBill;
                _patientBill.PaidBill = model.PaidBill;
                _patientBill.RemainingBill = model.RemainingBill;
                _patientBill.IsDischarge = model.IsDischarge;
                _patientBill.ModifiedAt = DateTime.Now;
                _patientBill.ModifiedBy = "Admin";

                var result = _patientBillDetailRepository.Update(_patientBill);
                model.Message = "Update";
                model.TblPatientBillDetails = BindData().TblPatientBillDetails;
            }
            return Json(model);
        }

        [HttpPut]
        public ActionResult DeletePatientBillDetail(int ID)
        {
            bool result = false;
            var model = new PatientBillDetailViewModel();
            if (ID != 0)
            {
                result = _patientBillDetailRepository.Delete(ID);
                if (result)
                {
                    model.TblPatientBillDetails = BindData().TblPatientBillDetails;
                    model.Message = "Delete";
                }
            }
            return Json(model);
        }
    }
}