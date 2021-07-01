using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HospitalProject.ViewModel
{
    public class RoomViewModel
    {
        public int RoomId { get; set; }

        [Display(Name = "Room Name")]
        [Required(ErrorMessage = "Room Name Required ")]
        public string RoomType { get; set; }

        [Display(Name = "Per Day Charges")]
        [Required(ErrorMessage = "Per Day Charges Required")]
        public int Per_Day_Charges { get; set; }
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedAt { get; set; }
        public string Message { get; set; }
        public IEnumerable<RoomViewModel> TblRoomDetails { get; set; }
    }
}
