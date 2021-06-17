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
    public class DesignationController : Controller
    {
        private readonly IDesignationRepository _designationRepository;

        public DesignationController(IDesignationRepository designationRepository)
        {
            _designationRepository = designationRepository;
        }

        private DesignationViewModel BindGridData()
        {
            var viewModel = new DesignationViewModel
            {
                TblDesignation = _designationRepository.GetDesignation().Select(sel =>
                new DesignationViewModel
                {
                    DesignationID = sel.DesignationID,
                    DesignationName = sel.DesignationName,
                    IsActive = sel.IsActive,
                    CreatedBy = sel.CreatedBy,
                    CreatedAt = sel.CreatedAt,
                    ModifiedBy = sel.ModifiedBy,
                    ModifiedAt = sel.ModifiedAt
                }).OrderBy(ord => ord.DesignationName).ToList()
            };
            return viewModel;
        }

        public IActionResult Index()
        {
            var model = BindGridData();
            return View(model);
        }

        [HttpPost]
        public ActionResult CreateDesignation(string designationModel)
        {
            DesignationViewModel model = Newtonsoft.Json.JsonConvert.DeserializeObject<DesignationViewModel>(designationModel);
            if (ModelState.IsValid)
            {
                var _designation = new Designation
                {
                    DesignationName = model.DesignationName,
                    IsActive = model.IsActive,
                    CreatedAt = DateTime.Now,
                    CreatedBy = "Admin"
                };
                var result = _designationRepository.Add(_designation);
                if (_designation.DesignationID != 0)
                {
                    model.Message = "Save";
                    model.TblDesignation = BindGridData().TblDesignation;
                }
            }
            return Json(model);
        }

        [HttpPost]
        public ActionResult EditDesignation(string designationModel)
        {
            DesignationViewModel model = Newtonsoft.Json.JsonConvert.DeserializeObject<DesignationViewModel>(designationModel);

            if (ModelState.IsValid)
            {
                var designation = _designationRepository.GetDesignationById(model.DesignationID);

                designation.DesignationName = model.DesignationName;
                designation.IsActive = model.IsActive;
                designation.ModifiedAt = System.DateTime.Now;
                designation.ModifiedBy = "Admin";

                var result = _designationRepository.Update(designation);
                model.Message = "Update";
                model.TblDesignation = BindGridData().TblDesignation;
            }
            return Json(model);
        }

        [HttpPut]
        public JsonResult DeleteDesignation(int DesignationId)
        {
            bool result = false;
            var model = new DesignationViewModel();

            if (DesignationId != 0)
            {
                result = _designationRepository.Delete(DesignationId);
                if (result)
                {
                    model.TblDesignation = BindGridData().TblDesignation;
                }
            }
            return Json(model);
        }
    }
}
