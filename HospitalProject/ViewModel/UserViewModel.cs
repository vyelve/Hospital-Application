using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HospitalProject.ViewModel
{
    public class UserViewModel
    {
        public int UserID { get; set; }

        [Required(ErrorMessage = "First Name required")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name required")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        public string UserShortName { get; set; }

        [Required(ErrorMessage = "Email ID required")]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$",
            ErrorMessage = "Please enter a valid e-mail address")]
        [Display(Name = "Email ID")]
        public string EmailId { get; set; }
        public string Gender { get; set; }

        [Required(ErrorMessage = "Gender required")]
        public GenderEnum GenderEnum { get; set; }

        [Display(Name = "Designation")]
        [Required(ErrorMessage = "Designation required")]
        public int DesignationId { get; set; }
        public string DesignationName { get; set; }

        [Required(ErrorMessage = "Department required")]
        [Display(Name = "Department")]
        public int DepartmentID { get; set; }
        public string DepartmentName { get; set; }

        [Required(ErrorMessage = "Phone No Required")]
        [RegularExpression(@"^(\d{10})$", ErrorMessage = "Wrong Phone No")]
        public long PhoneNumber { get; set; }

        [MinLength(8, ErrorMessage = "Minimum Password must be 8 in characters")]
        [Required(ErrorMessage = "Password Required")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm Password Required")]
        [Compare("Password", ErrorMessage = "Enter Valid Password")]
        public string ConfirmPassword { get; set; }        

        [Required(ErrorMessage = "Date of Joining Required")]
        public string DOJ { get; set; }
        //public DateTime Date_of_Joining { get; set; }
        public bool IsActive { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedAt { get; set; }
        public IEnumerable<UserViewModel> TblUsers { get; set; }
        public string Message { get; set; }
    }

    public enum GenderEnum
    {
        Male = 1,
        Female = 2,
        Others = 3
    }
}
