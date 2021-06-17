using Hospital.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace Hospital.Repository
{
    public class NurseRepository : INurseRepository
    {
        private readonly HospitalDbContext _appDBContext;

        public NurseRepository(HospitalDbContext context)
        {
            _appDBContext = context;
        }

        public IEnumerable<Nurse> GetNurseDetails()
        {
            return _appDBContext.Nurses;
        }

        public Nurse GetNurseDetailsById(int Id)
        {
            return _appDBContext.Nurses.Find(Id);
        }

        public Nurse Add(Nurse model)
        {
            _appDBContext.Nurses.Add(model);
            _appDBContext.SaveChanges();
            return model;
        }

        public Nurse Update(Nurse model)
        {
            //var _nurse = _appDBContext.Nurses.Attach(model);
            //_nurse.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _appDBContext.Entry(model).State = EntityState.Modified;
            _appDBContext.SaveChanges();
            return model;
        }
        public bool Delete(int Id)
        {
            bool result = false;
            var _nurse = _appDBContext.Nurses.Find(Id);
            if (_nurse != null)
            {
                //_appDBContext.Nurses.Remove(_nurse);
                _appDBContext.Entry(_nurse).State = EntityState.Deleted;
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
