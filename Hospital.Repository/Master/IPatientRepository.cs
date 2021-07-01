using Hospital.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hospital.Repository
{
    public interface IPatientRepository
    {
        IEnumerable<Patient> GetPatientDetails();
        Patient GetPatientDetailsById(int Id);
        Patient Add(Patient model);
        Patient Update(Patient model);
        bool Delete(int Id);
    }
}
