using Hospital.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace Hospital.Repository
{
    public class RoomRepository : IRoomRepository
    {
        private readonly HospitalDbContext _appDBContext;

        public RoomRepository(HospitalDbContext context)
        {
            _appDBContext = context;
        }
        public IEnumerable<Room> GetRoomDetails()
        {
            return _appDBContext.Rooms;
        }

        public Room GetRoomDetailsById(int Id)
        {
            return _appDBContext.Rooms.Find(Id);
        }

        public Room Add(Room model)
        {
            _appDBContext.Rooms.Add(model);
            _appDBContext.SaveChanges();
            return model;
        }

        public Room Update(Room model)
        {
            _appDBContext.Entry(model).State = EntityState.Modified;
            _appDBContext.SaveChanges();
            return model;
        }

        public bool Delete(int Id)
        {
            bool result = false;
            var rooms = _appDBContext.Rooms.Find(Id);
            if (rooms != null)
            {
                _appDBContext.Entry(rooms).State = EntityState.Deleted;
                _appDBContext.SaveChanges();
                result = true;
            }
            return result;
        }       
    }
}
