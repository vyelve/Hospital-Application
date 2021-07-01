using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Hospital.Entities
{
    [Table("PatientBillDetails")]
    public class PatientBillDetail
    {
        [Key]
        public int BillId { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string BillNumber { get; set; }
        public int PatientId { get; set; }
        public int RoomId { get; set; }
        public int No_of_Days_Admitted { get; set; }
        public int RoomCharges { get; set; }
        public int DoctorId { get; set; }
        public int DoctorCharges { get; set; }
        public int MedicineBill { get; set; }
        public int TotalBill { get; set; }
        public int PaidBill { get; set; }
        public int RemainingBill { get; set; }
        public bool IsDischarge { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedAt { get; set; }
    }
}
