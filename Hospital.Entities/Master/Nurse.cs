using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Hospital.Entities
{
    [Table("Nurse")]
    public class Nurse
    {
        [Key]
        public int NurID { get; set; }
        public int NurseId { get; set; }
        public string NurseType { get; set; }
        public string ShiftType { get; set; }
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedAt { get; set; }

    }
}
