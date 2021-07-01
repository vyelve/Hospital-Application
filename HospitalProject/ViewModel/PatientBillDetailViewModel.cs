using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HospitalProject.ViewModel
{
    public class PatientBillDetailViewModel
    {
        public int BillId { get; set; }
        public string BillNumber { get; set; }

        [Display(Name = "Patient Name")]
        [Required(ErrorMessage = "Patient Name Required")]
        public int PatientId { get; set; }
        public string PatientName { get; set; }

        [Display(Name = "Room")]
        [Required(ErrorMessage = "Room Required")]
        public int RoomId { get; set; }
        public string RoomName { get; set; }

        [Display(Name = "No. of Days Admited")]
        [Required(ErrorMessage = "No. of Days Admited Required")]
        public int No_of_Days_Admitted { get; set; }

        [Display(Name = "Room Charges")]
        [Required(ErrorMessage = "Room Charges Required")]
        public int RoomCharges { get; set; }

        [Display(Name = "Doctor")]
        [Required(ErrorMessage = "Doctor Required")]
        public int DoctorId { get; set; }
        public string DoctorName { get; set; }

        [Display(Name = "Doctor Charges")]
        [Required(ErrorMessage = "Doctor Charges Required")]
        public int DoctorCharges { get; set; }

        [Display(Name = "Medicine Bill")]
        [Required(ErrorMessage = "Medicine Bill Charges Required")]
        public int MedicineBill { get; set; }
        public int TotalBill { get; set; }
        public int PaidBill { get; set; }
        public int RemainingBill { get; set; }
        public bool IsDischarge { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedAt { get; set; }
        public string Message { get; set; }
        public IEnumerable<PatientBillDetailViewModel> TblPatientBillDetails { get; set; }

    }
}
