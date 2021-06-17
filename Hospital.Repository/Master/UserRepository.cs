using Hospital.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hospital.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly HospitalDbContext _appDBContext;

        public UserRepository(HospitalDbContext context)
        {
            _appDBContext = context;
        }

        public IEnumerable<User> GetUserDetails()
        {
            return _appDBContext.Users;
        }

        public User GetUserById(int Id)
        {
            return _appDBContext.Users.Find(Id);
        }

        public User Add(User model)
        {
            _appDBContext.Users.Add(model);
            _appDBContext.SaveChanges();
            return model;
        }
        public User Update(User model)
        {
            //var user = _appDBContext.Users.Attach(model);
            //user.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _appDBContext.Entry(model).State = EntityState.Modified;
            _appDBContext.SaveChanges();
            return model;
        }

        public bool Delete(int Id)
        {
            bool result = false;
            var user = _appDBContext.Users.Find(Id);
            if (user != null)
            {
                //_appDBContext.Users.Remove(user);
                _appDBContext.Entry(user).State = EntityState.Deleted;
                _appDBContext.SaveChanges();
                result = true;
            }
            else
            {
                result = false;
            }
            return result;
        }
    }
}
