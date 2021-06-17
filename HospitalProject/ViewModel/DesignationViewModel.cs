using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HospitalProject.ViewModel
{
    public class DesignationViewModel
    {
        public int DesignationID { get; set; }

        [Required(ErrorMessage = "Designation required")]
        [Display(Name = "Designation")]
        public string DesignationName { get; set; }
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedAt { get; set; }
        public IEnumerable<DesignationViewModel> TblDesignation { get; set; }
        public string Message { get; set; }
    }
}
