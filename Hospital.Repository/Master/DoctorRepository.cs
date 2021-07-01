using Hospital.Entities;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace Hospital.Repository
{
    public class DoctorRepository : IDoctorRepository
    {
        private readonly HospitalDbContext _appDBContext;

        public DoctorRepository(HospitalDbContext context)
        {
            _appDBContext = context;
        }

        public IEnumerable<Doctor> GetDoctors()
        {
            return _appDBContext.Doctors;
        }
        
        public Doctor GetDoctorById(int Id)
        {
            return _appDBContext.Doctors.Find(Id);
        }

        public Doctor Add(Doctor model)
        {
            _appDBContext.Doctors.Add(model);
            _appDBContext.SaveChanges();
            return model;
        }  

        public Doctor Update(Doctor model)
        {
            _appDBContext.Entry(model).State = EntityState.Modified;
            _appDBContext.SaveChanges();
            return model;
        }

        public bool Delete(int Id)
        {
            bool result = false;
            var _doctor = _appDBContext.Doctors.Find(Id);
            if (_doctor != null)
            {
                _appDBContext.Entry(_doctor).State = EntityState.Deleted;
                _appDBContext.SaveChanges();
                result = true;
            }
            return result;
        }

    }
}
