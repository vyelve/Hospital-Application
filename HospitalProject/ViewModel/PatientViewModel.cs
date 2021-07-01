using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HospitalProject.ViewModel
{
    public class PatientViewModel
    {
        public int PatientID { get; set; }
        public string PatientNumber { get; set; }

        [Display(Name = "First Name")]
        [Required(ErrorMessage = "First Name Required")]
        public string FirstName { get; set; }

        [Display(Name = "Middle Name")]
        [Required(ErrorMessage = "Middle Name Required")]
        public string MiddleName { get; set; }

        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "Last Name Required")]
        public string LastName { get; set; }

        public string PatientFullName { get; set; }
        public string Gender { get; set; }

        [Required(ErrorMessage = "Gender Required")]
        public GenderEnum GenderEnum { get; set; }

        [Required(ErrorMessage = "Phone No Required")]
        [RegularExpression(@"^(\d{10})$", ErrorMessage = "Wrong Phone No")]
        public long PhoneNumber { get; set; }

        [Display(Name = "Date of Birth")]
        [Required(ErrorMessage = "Date of Birth Required")]
        public string DOB { get; set; }
        public int Age { get; set; }

        public string MaritalStatus { get; set; }

        [Display(Name = "Marital Status")]
        [Required(ErrorMessage = "Marital Status Required")]
        public MaritalStatusEnum MaritalStatusEnum { get; set; }

        [Display(Name = "Emergency Contact Person")]
        [Required(ErrorMessage = "Emergency Contact Person Required")]
        public string EmergencyContactPerson { get; set; }

        [Display(Name = "Emergency Contact Number")]
        [RegularExpression(@"^(\d{10})$", ErrorMessage = "Wrong Phone No")]
        [Required(ErrorMessage = "Emergency Contact Number Required")]
        public long EmergencyContactNumber { get; set; }

        [Display(Name = "Address")]
        [Required(ErrorMessage = "Address Required")]
        public string Address { get; set; }

        [Display(Name = "Medical History")]
        [Required(ErrorMessage = "Medical History Required")]
        public string MedicalHistory { get; set; }

        [Display(Name = "Doctor")]
        [Required(ErrorMessage = "Doctor Required")]
        public int DoctorId { get; set; }
        public string DoctorName { get; set; }

        [Display(Name = "Has Insurance")]
        public bool HasInsurance { get; set; }

        [Display(Name = "Insurance Number")]
        public string Insuranceid { get; set; }

        [Display(Name = "Is Admitted")]
        public bool HasAdmitted { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedAt { get; set; }
        public string Message { get; set; }
        public IEnumerable<PatientViewModel> TblPatientDetails { get; set; }
    }

    public enum MaritalStatusEnum
    {
        Single = 1,
        Married = 2,
        Divorced = 3,
        Separated = 4,
        Widowed = 5
    }
}
