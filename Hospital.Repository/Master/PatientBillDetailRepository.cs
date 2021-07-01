using Hospital.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace Hospital.Repository
{
    public class PatientBillDetailRepository : IPatientBillDetailRepository
    {
        private readonly HospitalDbContext _appDBContext;
        public PatientBillDetailRepository(HospitalDbContext context)
        {
            _appDBContext = context;
        }

        public IEnumerable<PatientBillDetail> GetPatientBillDetails()
        {
            return _appDBContext.PatientBillDetails;
        }

        public PatientBillDetail GetPatientBillDetailById(int Id)
        {
            return _appDBContext.PatientBillDetails.Find(Id);
        }

        public PatientBillDetail Add(PatientBillDetail model)
        {
            _appDBContext.PatientBillDetails.Add(model);
            _appDBContext.SaveChanges();
            return model;
        }

        public PatientBillDetail Update(PatientBillDetail model)
        {
            _appDBContext.Entry(model).State = EntityState.Modified;
            _appDBContext.SaveChanges();
            return model;
        }

        public void UpdateAmountIsDischarge(PatientBillDetail model)
        {
            _appDBContext.Entry(model).State = EntityState.Modified;
            _appDBContext.SaveChanges();
        }

        public bool Delete(int Id)
        {
            bool result = false;
            var _patientBill = _appDBContext.PatientBillDetails.Find(Id);
            if (_patientBill != null)
            {
                _appDBContext.Entry(_patientBill).State = EntityState.Deleted;
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
