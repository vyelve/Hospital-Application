using Hospital.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hospital.Repository
{
    public interface IRoomRepository
    {
        IEnumerable<Room> GetRoomDetails();
        Room GetRoomDetailsById(int Id);
        Room Add(Room model);
        Room Update(Room model);
        bool Delete(int Id);
    }
}
