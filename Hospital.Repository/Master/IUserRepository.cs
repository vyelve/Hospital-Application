using Hospital.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hospital.Repository
{
    public interface IUserRepository
    {
        IEnumerable<User> GetUserDetails();
        User GetUserById(int Id);
        User Add(User model);
        User Update(User model);
        bool Delete(int Id);
    }
}
