using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HospitalProject.ViewModel
{
    public class DoctorViewModel
    {
        public int DocID { get; set; }

        [Display(Name = "Doctor")]
        [Required(ErrorMessage = "Doctor Required")]
        public int DoctorId { get; set; }
        public string DoctorName { get; set; }

        [Display(Name = "Department")]
        [Required(ErrorMessage = "Department Required")]
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }

        [Display(Name = "Specialization")]
        [Required(ErrorMessage = "Specialization Required")]
        public int SpecialistID { get; set; }
        public string SpecialistName { get; set; }

        [Display(Name = "Per Day Charges")]
        [Required(ErrorMessage = "Per Day Charges Required")]
        public int Per_Day_Charges { get; set; }

        [Display(Name = "Doctor Schedule")]
        [Required(ErrorMessage = "Doctor Schedule Required")]
        public string DoctorSchedule { get; set; }
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedAt { get; set; }
        public string Message { get; set; }
        public IEnumerable<DoctorViewModel> TblDoctor { get; set; }
    }
}
