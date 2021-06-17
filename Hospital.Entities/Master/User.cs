using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Hospital.Entities
{
    [Table("User")]
    public class User
    {
        public int UserID { get; set; }
        public string UserShortName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailId { get; set; }
        public string Gender { get; set; }
        public string Password { get; set; }
        public int DesignationId { get; set; }
        public long PhoneNumber { get; set; }
        public int DepartmentID { get; set; }
        public DateTime Date_of_Joining { get; set; }
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedAt { get; set; }
    }
}
