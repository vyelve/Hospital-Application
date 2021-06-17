using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hospital.Entities
{
    [Table("Specialization")]
    public class Specialization
    {
        [Key]
        public int SpecialistID  { get; set; }
        public string SpecializationName { get; set; }
        public string SpecializationDescription { get; set; }
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedAt { get; set; }
    }
}
