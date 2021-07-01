using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HospitalProject.ViewModel
{
    public class SpecializationViewModel
    {
        public int SpecialistID { get; set; }

        [Display(Name = "Specialization")]
        [Required(ErrorMessage = "Specialization Required")]
        public string SpecializationName { get; set; }
        public string SpecializationDescription { get; set; }
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedAt { get; set; }
        public string  Message { get; set; }
        public IEnumerable<SpecializationViewModel> TblSpecialization { get; set; }
    }
}
