using Hospital.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace Hospital.Repository
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly HospitalDbContext _appDBContext;
        public PaymentRepository(HospitalDbContext context)
        {
            _appDBContext = context;
        }

        public IEnumerable<Payment> GetPaymentDetails()
        {
            return _appDBContext.PaymentDetails;
        }

        public Payment GetPaymentDetailsById(int Id)
        {
            return _appDBContext.PaymentDetails.Find(Id);
        }
        
        public Payment Add(Payment model)
        {
            _appDBContext.PaymentDetails.Add(model);
            _appDBContext.SaveChanges();
            return model;
        }

        public Payment Update(Payment model)
        {
            _appDBContext.Entry(model).State = EntityState.Modified;
            _appDBContext.SaveChanges();
            return model;
        }
    }
}
