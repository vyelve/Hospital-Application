using Hospital.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hospital.Repository
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly HospitalDbContext _appDBContext;

        public DepartmentRepository(HospitalDbContext context)
        {
            _appDBContext = context;
        }

        public IEnumerable<Department> GetDepartments()
        {
            return _appDBContext.Departments;
        }
        public Department GetDepartmentById(int Id)
        {
            return _appDBContext.Departments.Find(Id);
        }

        public Department Add(Department model)
        {
            _appDBContext.Departments.Add(model);
            _appDBContext.SaveChanges();
            return model;
        }

        public Department Update(Department model)
        {
            //var department = _appDBContext.Departments.Attach(model);
            //department.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _appDBContext.Entry(model).State = EntityState.Modified;
            _appDBContext.SaveChanges();
            return model;
        }

        public bool Delete(int Id)
        {
            bool result = false;
            var department = _appDBContext.Departments.Find(Id);
            if (department != null)
            {
                //_appDBContext.Departments.Remove(department);
                _appDBContext.Entry(department).State = EntityState.Deleted;
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
