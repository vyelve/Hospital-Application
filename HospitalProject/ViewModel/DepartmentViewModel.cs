using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HospitalProject.ViewModel
{
    public class DepartmentViewModel
    {
        public int DeptId { get; set; }

        [Required(ErrorMessage = "Department Name Required")]
        [Display(Name = "Department Name")]
        public string DepartmentName { get; set; }
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedAt { get; set; }
        public IEnumerable<DepartmentViewModel> TblDepartment { get; set; }
        public string Message { get; set; }
    }
}
