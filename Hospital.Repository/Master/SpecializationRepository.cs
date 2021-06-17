using Hospital.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace Hospital.Repository
{
    public class SpecializationRepository : ISpecializationRepository
    {
        private readonly HospitalDbContext _appDBContext;

        public SpecializationRepository(HospitalDbContext context)
        {
            _appDBContext = context;
        }
        public IEnumerable<Specialization> GetSpecializationDetails()
        {
            return _appDBContext.Specializations;
        }

        public Specialization GetSpecializationsById(int Id)
        {
            return _appDBContext.Specializations.Find(Id);
        }

        public Specialization Add(Specialization model)
        {
            _appDBContext.Specializations.Add(model);
            _appDBContext.SaveChanges();
            return model;
        }

        public Specialization Update(Specialization model)
        {
            _appDBContext.Entry(model).State = EntityState.Modified;
            _appDBContext.SaveChanges();
            return model;
        }

        public bool Delete(int Id)
        {
            bool result = false;
            var specialization = _appDBContext.Specializations.Find(Id);
            if (specialization != null)
            {
                _appDBContext.Entry(specialization).State = EntityState.Deleted;
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
