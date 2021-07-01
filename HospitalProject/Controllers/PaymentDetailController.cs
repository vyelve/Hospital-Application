using Hospital.Entities;
using Hospital.Repository;
using HospitalProject.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HospitalProject.Extensions;


namespace HospitalProject.Controllers
{
    public class PaymentDetailController : Controller
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IPatientBillDetailRepository _patientBillDetailRepository;
        private readonly IPatientRepository _patientRepository;

        public PaymentDetailController(IPaymentRepository paymentRepository,
            IPatientBillDetailRepository patientBillDetailRepository,
            IPatientRepository patientRepository)
        {
            _paymentRepository = paymentRepository;
            _patientBillDetailRepository = patientBillDetailRepository;
            _patientRepository = patientRepository;
        }

        private PaymentDetailViewModel BindData()
        {
            var _patientbillDetails = _patientBillDetailRepository.GetPatientBillDetails().ToList();
            var _patient = _patientRepository.GetPatientDetails().Select(sel => new
            {
                PatientId = sel.PatientID,
                PatientName = sel.FirstName + " " + sel.MiddleName + " " + sel.LastName
            }).ToList();

            ViewBag.PatientDropdown = new SelectList(_patient, "PatientId", "PatientName");
            ViewBag.BillNumberDropdown = new SelectList(_patientbillDetails.Where(whr => whr.IsDischarge == false).ToList(),
                "BillId", "BillNumber");

            string _netBanking = PaymentTypeEnum.NetBanking.ToString();
            string _upiPay = PaymentTypeEnum.UPIPay.ToString();

            var viewModel = new PaymentDetailViewModel
            {
                TblPayment = _paymentRepository.GetPaymentDetails().Select(sel =>
                new PaymentDetailViewModel
                {
                    PaymentId = sel.PaymentId,
                    BillId = sel.BillId,
                    BillNumber = _patientbillDetails.Where(whr => whr.BillId == sel.BillId).Select(sel => sel.BillNumber).FirstOrDefault(),
                    PatientId = sel.PatientId,
                    PatientName = _patient.Where(whr => whr.PatientId == sel.PatientId).Select(sel => sel.PatientName).FirstOrDefault(),
                    TotalBill = _patientbillDetails.Where(whr => whr.BillId == sel.BillId).Select(sel =>
                    sel.TotalBill - sel.PaidBill).FirstOrDefault(),
                    Amount = sel.Amount,
                    PaymentType = (sel.PaymentType != _netBanking && sel.PaymentType != _upiPay) ? sel.PaymentType : (sel.PaymentType == _netBanking) ? PaymentTypeEnum.NetBanking.GetDisplayName() : PaymentTypeEnum.UPIPay.GetDisplayName(),
                    BankName = sel.BankName,
                    IsDischarged = _patientbillDetails.Where(whr => whr.BillId == sel.BillId).Select(sel => sel.IsDischarge).FirstOrDefault(),
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
        public JsonResult GetPatientTotalBill(int BillId)
        {
            var _totalBill = _patientBillDetailRepository.GetPatientBillDetailById(BillId);
            return Json(_totalBill.TotalBill - _totalBill.PaidBill);
        }

        [HttpGet]
        public JsonResult GetPatientNameByBillId(int BillId)
        {
            var _patientList = _patientRepository.GetPatientDetails().ToList();
            var _patientBillDetails = _patientBillDetailRepository.GetPatientBillDetails().ToList();

            var _patient = (from p in _patientList
                            join pbd in _patientBillDetails on p.PatientID equals pbd.PatientId
                            where pbd.BillId == BillId
                            select new
                            {
                                PatientId = p.PatientID,
                                PatientName = p.FirstName + " " + p.MiddleName + " " + p.LastName
                            }).ToList();

            return Json(new SelectList(_patient, "PatientId", "PatientName"));
        }

        [HttpPost]
        public ActionResult CreatePaymentDetails(string paymentModel)
        {
            var model = Newtonsoft.Json.JsonConvert.DeserializeObject<PaymentDetailViewModel>(paymentModel);
            if (ModelState.IsValid)
            {
                var _payment = new Payment
                {
                    BillId = model.BillId,
                    PatientId = model.PatientId,
                    Amount = model.Amount,
                    PaymentType = Enum.GetName(typeof(PaymentTypeEnum), Convert.ToInt32(model.PaymentType)),
                    BankName = model.BankName,
                    CreatedBy = "Admin",
                    CreatedAt = DateTime.Now,
                };
                var result = _paymentRepository.Add(_payment);
                if (_payment.PaymentId != 0)
                {
                    model.Message = "Save";
                    model.TblPayment = BindData().TblPayment;
                    UpdateAmountIsDischarge(_payment.BillId, _payment.PaymentId);
                }
            }
            return Json(model);
        }

        public ActionResult EditPaymentDetails(string paymentModel)
        {
            var model = Newtonsoft.Json.JsonConvert.DeserializeObject<PaymentDetailViewModel>(paymentModel);
            if (ModelState.IsValid)
            {
                var _payment = _paymentRepository.GetPaymentDetailsById(model.PaymentId);
                _payment.BillId = model.BillId;
                _payment.PatientId = model.PatientId;
                _payment.Amount = model.Amount;
                _payment.PaymentType = Enum.GetName(typeof(PaymentTypeEnum), Convert.ToInt32(model.PaymentType));
                _payment.BankName = model.BankName;
                _payment.ModifiedBy = "Admin";
                _payment.ModifiedAt = DateTime.Now;

                var result = _paymentRepository.Update(_payment);
                model.Message = "Update";
                model.TblPayment = BindData().TblPayment;
                UpdateAmountIsDischarge(_payment.BillId, _payment.PaymentId);
            }
            return Json(model);
        }

        private void UpdateAmountIsDischarge(int BillId, int PaymentId)
        {
            var _patientBill = _patientBillDetailRepository.GetPatientBillDetailById(BillId);
            int _payment = _paymentRepository.GetPaymentDetailsById(PaymentId).Amount;

            _patientBill.TotalBill = _patientBill.TotalBill;
            _patientBill.PaidBill = _payment + _patientBill.RemainingBill;
            _patientBill.RemainingBill = (_patientBill.TotalBill - _payment) - _patientBill.RemainingBill;
            _patientBill.IsDischarge = (_patientBill.TotalBill - _payment) == 0 ? true : false;
            _patientBill.ModifiedAt = DateTime.Now;
            _patientBill.ModifiedBy = "Admin";
            _patientBillDetailRepository.UpdateAmountIsDischarge(_patientBill);
        }
    }
}
