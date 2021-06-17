using Hospital.Entities;
using Hospital.Repository;
using HospitalProject.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HospitalProject.Controllers
{
    public class SpecializationController : Controller
    {
        private readonly ISpecializationRepository _specializationRepository;

        public SpecializationController(ISpecializationRepository specializationRepository)
        {
            _specializationRepository = specializationRepository;
        }

        private SpecializationViewModel BindData()
        {
            var viewModel = new SpecializationViewModel
            {
                TblSpecialization = _specializationRepository.GetSpecializationDetails().Select(sel =>
                new SpecializationViewModel
                {
                    SpecialistID = sel.SpecialistID,
                    SpecializationName = sel.SpecializationName,
                    SpecializationDescription = sel.SpecializationDescription,
                    IsActive = sel.IsActive,
                    CreatedBy = sel.CreatedBy,
                    CreatedAt = sel.CreatedAt,
                    ModifiedBy = sel.ModifiedBy,
                    ModifiedAt = sel.ModifiedAt
                }).OrderBy(ord => ord.SpecializationName).ToList()
            };
            return viewModel;
        }

        public IActionResult Index()
        {
            var model = BindData();
            return View(model);
        }

        [HttpPost]
        public ActionResult CreateSpecialization(string specializationModel)
        {
            SpecializationViewModel model = Newtonsoft.Json.JsonConvert.DeserializeObject<SpecializationViewModel>(specializationModel);
            if (ModelState.IsValid)
            {
                var _specialization = new Specialization
                {
                    SpecializationName = model.SpecializationName,
                    SpecializationDescription = model.SpecializationDescription,
                    IsActive = model.IsActive,
                    CreatedBy = "Admin",
                    CreatedAt = DateTime.Now
                };
                var result = _specializationRepository.Add(_specialization);
                if (_specialization.SpecialistID != 0)
                {
                    model.Message = "Save";
                    model.TblSpecialization = BindData().TblSpecialization;
                }
            }
            return Json(model);
        }

        [HttpPost]
        public ActionResult EditSpecialization(string specializationModel)
        {
            SpecializationViewModel model = Newtonsoft.Json.JsonConvert.DeserializeObject<SpecializationViewModel>(specializationModel);
            if (ModelState.IsValid)
            {
                var _specialization = _specializationRepository.GetSpecializationsById(model.SpecialistID);
                _specialization.SpecialistID = model.SpecialistID;
                _specialization.SpecializationName = model.SpecializationName;
                _specialization.SpecializationDescription = model.SpecializationDescription;
                _specialization.IsActive = model.IsActive;
                _specialization.ModifiedBy = "Admin";
                _specialization.ModifiedAt = DateTime.Now;

                var result = _specializationRepository.Update(_specialization);
                model.Message = "Update";
                model.TblSpecialization = BindData().TblSpecialization;
            }
            return Json(model);

        }

        public JsonResult DeleteSpecialization(int specialistID)
        {
            bool result = false;
            var model = new SpecializationViewModel();

            if (specialistID != 0)
            {
                result = _specializationRepository.Delete(specialistID);
                if (result)
                {
                    model.TblSpecialization = BindData().TblSpecialization;
                    model.Message = "Delete";
                }
            }
            return Json(model);
        }
    }
}
