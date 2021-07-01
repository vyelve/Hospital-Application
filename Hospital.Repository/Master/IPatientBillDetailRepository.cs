using Hospital.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hospital.Repository
{
    public interface IPatientBillDetailRepository
    {
        IEnumerable<PatientBillDetail> GetPatientBillDetails();
        PatientBillDetail GetPatientBillDetailById(int Id);
        PatientBillDetail Add(PatientBillDetail model);
        PatientBillDetail Update(PatientBillDetail model);
        bool Delete(int Id);
        void UpdateAmountIsDischarge(PatientBillDetail model);
    }
}
