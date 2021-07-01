using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HospitalProject.ViewModel
{
    public class PaymentDetailViewModel
    {
        public int PaymentId { get; set; }

        [Display(Name = "Bill Number")]
        [Required(ErrorMessage = "Bill Number Required")]
        public int BillId { get; set; }
        public string BillNumber { get; set; }

        [Display(Name = "Patient Name")]
        [Required(ErrorMessage = "Patient Name Required")]
        public int PatientId { get; set; }
        public string PatientName { get; set; }
        public int TotalBill { get; set; }

        [Required(ErrorMessage = "Amount Required")]
        public int Amount { get; set; }
        public string PaymentType { get; set; }

        [Display(Name = "Payment Type")]
        [Required(ErrorMessage = "Payment Type Required")]
        public PaymentTypeEnum PaymentTypeEnum { get; set; }
        public string BankName { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedAt { get; set; }
        public bool IsDischarged { get; set; }
        public string Message { get; set; }
        public IEnumerable<PaymentDetailViewModel> TblPayment { get; set; }
    }

    public enum PaymentTypeEnum
    {
        Cash = 1,
        [Display(Name = "Net Banking")]
        NetBanking = 2,
        [Display(Name = "UPI Payment")]
        UPIPay = 3,
    }
}
