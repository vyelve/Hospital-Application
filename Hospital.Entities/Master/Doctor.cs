using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Hospital.Entities
{
    [Table("Doctor")]
    public class Doctor
    {
        [Key]
        public int DocID { get; set; }
        public int DoctorId { get; set; }
        public int DepartmentId { get; set; }
        public string Specialist { get; set; }
        public int Per_Day_Charges { get; set; }
        public string DoctorSchedule { get; set; }
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedAt { get; set; }

    }
}
