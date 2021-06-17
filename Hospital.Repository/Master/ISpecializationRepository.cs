using Hospital.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hospital.Repository
{
    public interface ISpecializationRepository
    {
        IEnumerable<Specialization> GetSpecializationDetails();
        Specialization GetSpecializationsById(int Id);
        Specialization Add(Specialization model);
        Specialization Update(Specialization model);
        bool Delete(int Id);
    }
}
