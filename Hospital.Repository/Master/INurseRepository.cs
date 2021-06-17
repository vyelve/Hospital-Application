using Hospital.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hospital.Repository
{
    public interface INurseRepository
    {
        IEnumerable<Nurse> GetNurseDetails();
        Nurse GetNurseDetailsById(int Id);
        Nurse Add(Nurse model);
        Nurse Update(Nurse model);
        bool Delete(int Id);
    }
}
