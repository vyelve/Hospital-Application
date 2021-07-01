using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Hospital.Entities
{
    [Table("PatientDetails")]
    public class Patient
    {
        [Key]
        public int PatientID { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string PatientNumber { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public long PhoneNumber { get; set; }
        public DateTime DOB { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public int Age { get; set; }
        public string MaritalStatus { get; set; }
        public string EmergencyContactPerson { get; set; }
        public long EmergencyContactNumber { get; set; }
        public string Address { get; set; }
        public string MedicalHistory { get; set; }
        public int DoctorId { get; set; }
        public bool HasInsurance { get; set; }
        public string Insuranceid { get; set; }
        public bool HasAdmitted { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedAt { get; set; }
    }
}
