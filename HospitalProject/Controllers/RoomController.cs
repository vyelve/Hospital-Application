using Hospital.Entities;
using Hospital.Repository;
using HospitalProject.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HospitalProject.Controllers
{
    public class RoomController : Controller
    {
        private readonly IRoomRepository _roomRepository;

        public RoomController(IRoomRepository roomRepository)
        {
            _roomRepository = roomRepository;
        }

        private RoomViewModel BindData()
        {
            var viewModel = new RoomViewModel
            {
                TblRoomDetails = _roomRepository.GetRoomDetails().Select(sel => new RoomViewModel
                {
                    RoomId = sel.RoomId,
                    RoomType = sel.RoomType,
                    Per_Day_Charges = sel.Per_Day_Charges,
                    IsActive = sel.IsActive,
                    CreatedBy = sel.CreatedBy,
                    CreatedAt = sel.CreatedAt,
                    ModifiedBy = sel.ModifiedBy,
                    ModifiedAt = sel.ModifiedAt
                }).OrderBy(ord => ord.RoomId)
            };
            return viewModel;
        }

        public IActionResult Index()
        {
            var model = BindData();
            return View(model);
        }

        [HttpPost]
        public ActionResult CreateRoom(string roomModel)
        {
            var model = Newtonsoft.Json.JsonConvert.DeserializeObject<RoomViewModel>(roomModel);
            if (ModelState.IsValid)
            {
                var _room = new Room
                {
                    RoomType = model.RoomType,
                    Per_Day_Charges = model.Per_Day_Charges,
                    IsActive = model.IsActive,
                    CreatedAt = DateTime.Now,
                    CreatedBy = "Admin"
                };
                var result = _roomRepository.Add(_room);
                if (model.RoomId != 0)
                {
                    model.Message = "Save";
                    model.TblRoomDetails = BindData().TblRoomDetails;
                }
            }
            return Json(model);
        }

        [HttpPost]
        public ActionResult EditRoom(string roomModel)
        {
            var model = Newtonsoft.Json.JsonConvert.DeserializeObject<RoomViewModel>(roomModel);
            if (ModelState.IsValid)
            {
                var _room = _roomRepository.GetRoomDetailsById(model.RoomId);
                _room.RoomType = model.RoomType;
                _room.Per_Day_Charges = model.Per_Day_Charges;
                _room.IsActive = model.IsActive;
                _room.ModifiedBy = "Admin";
                _room.ModifiedAt = DateTime.Now;

                var result = _roomRepository.Update(_room);
                model.Message = "Update";
                model.TblRoomDetails = BindData().TblRoomDetails;
            }
            return Json(model);
        }
        
        [HttpPut]
        public ActionResult DeleteRoom(int ID)
        {
            bool result = false;
            var model = new RoomViewModel();
            if (ID != 0)
            {
                result = _roomRepository.Delete(ID);
                if (result)
                {
                    model.TblRoomDetails = BindData().TblRoomDetails;
                    model.Message = "Delete";
                }
            }
            return Json(model);
        }
    }
}
