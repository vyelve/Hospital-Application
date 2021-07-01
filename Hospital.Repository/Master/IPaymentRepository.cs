using Hospital.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hospital.Repository
{
    public interface IPaymentRepository
    {
        IEnumerable<Payment> GetPaymentDetails();
        Payment GetPaymentDetailsById(int Id);
        Payment Add(Payment model);
        Payment Update(Payment model);
    }
}
