using Hospital.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hospital.Repository
{
    public interface IDesignationRepository
    {
        IEnumerable<Designation> GetDesignation();
        Designation GetDesignationById(int Id);
        Designation Add(Designation model);
        Designation Update(Designation model);
        bool Delete(int Id);
    }
}
