using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HospitalProject.ViewModel
{
    public class NurseViewModel
    {
        public int NurID { get; set; }

        [Required(ErrorMessage = "Nurse Required")]
        public int NurseId { get; set; }
        public string NurseName { get; set; }        
        public string NurseType { get; set; }

        [Display(Name ="Type")]
        [Required(ErrorMessage = "Type required")]
        public NurseTypeEnum NurseTypeEnum { get; set; }
        public string ShiftType { get; set; }

        [Display(Name = "Shift Type")]
        [Required(ErrorMessage = "Shift Type required")]
        public ShifTypeEnum ShifTypeEnum { get; set; }
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedAt { get; set; }
        public IEnumerable<NurseViewModel> TblNurses { get; set; }
        public string Message { get; set; }
    }

    public enum NurseTypeEnum
    {
        Permanent = 1,
        [Display(Name = "Part Time")]
        Parttime = 2,
        Others = 3
    }

    public enum ShifTypeEnum
    {
        Morning = 1,
        Afternoon = 2,
        Night = 3
    }
}
