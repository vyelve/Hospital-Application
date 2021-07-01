using Hospital.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hospital.Repository
{
    public interface IDoctorRepository
    {
        IEnumerable<Doctor> GetDoctors();
        Doctor GetDoctorById(int Id);
        Doctor Add(Doctor model);
        Doctor Update(Doctor model);
        bool Delete(int Id);
    }
}
