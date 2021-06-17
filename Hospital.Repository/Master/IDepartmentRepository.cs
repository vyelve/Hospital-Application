using Hospital.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hospital.Repository
{
    public interface IDepartmentRepository
    {
        IEnumerable<Department> GetDepartments();
        Department GetDepartmentById(int Id);
        Department Add(Department model);
        Department Update(Department model);
        bool Delete(int Id);
    }
}
