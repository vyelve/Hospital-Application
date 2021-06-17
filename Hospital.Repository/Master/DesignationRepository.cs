using Hospital.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hospital.Repository
{
    public class DesignationRepository : IDesignationRepository
    {
        private readonly HospitalDbContext _appDBContext;

        public DesignationRepository(HospitalDbContext context)
        {
            _appDBContext = context;
        }

        public IEnumerable<Designation> GetDesignation()
        {
            return _appDBContext.Designations;
        }

        public Designation GetDesignationById(int Id)
        {
            return _appDBContext.Designations.Find(Id);
        }

        public Designation Add(Designation model)
        {
            _appDBContext.Designations.Add(model);
            _appDBContext.SaveChanges();
            return model;
        } 
 
        public Designation Update(Designation model)
        {
            //var designation = _appDBContext.Designations.Attach(model);
            //designation.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _appDBContext.Entry(model).State = EntityState.Modified;
            _appDBContext.SaveChanges();
            return model;
        }
        public bool Delete(int Id)
        {
            bool result = false;
            var designation = _appDBContext.Designations.Find(Id);
            if (designation != null)
            {
                //_appDBContext.Designations.Remove(designation);
                _appDBContext.Entry(designation).State = EntityState.Deleted;
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
