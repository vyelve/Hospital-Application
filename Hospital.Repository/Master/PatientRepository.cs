using Hospital.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace Hospital.Repository
{
    public class PatientRepository : IPatientRepository
    {
        private readonly HospitalDbContext _appDBContext;

        public PatientRepository(HospitalDbContext context)
        {
            _appDBContext = context;
        }

        public IEnumerable<Patient> GetPatientDetails()
        {
            return _appDBContext.Patients;
        }

        public Patient GetPatientDetailsById(int Id)
        {
            return _appDBContext.Patients.Find(Id);
        }
        public Patient Add(Patient model)
        {
            _appDBContext.Patients.Add(model);
            _appDBContext.SaveChanges();
            return model;
        }
        public Patient Update(Patient model)
        {
            _appDBContext.Entry(model).State = EntityState.Modified;
            _appDBContext.SaveChanges();
            return model;
        }

        public bool Delete(int Id)
        {
            bool result = false;
            var patient = _appDBContext.Patients.Find(Id);
            if (patient != null)
            {
                _appDBContext.Entry(patient).State = EntityState.Modified;
                _appDBContext.SaveChanges();
                result = true;
            }
            return result;
        }        
    }
}
