using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WhiteLagoon.Application.Common.Interfaces;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Infrastracture.Data;
using WhiteLagoon.Infrastracture.Repository;
using WhiteLagoon.Web.ViewModel;

namespace WhiteLagoon.Web.Controllers
{
    public class VillaNumberController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public VillaNumberController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            var villas = _unitOfWork.VillaNumber.GetAll(includeProperties: "Villa");
            return View(villas);
        }

        public IActionResult Create()
        {
            VillaNumberVm vm = new VillaNumberVm()
            {
                VillaList = _unitOfWork.Villa.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString(),
                }),
            };


            return View(vm);
        }

        [HttpPost]
        public IActionResult Create(VillaNumberVm villaNumberVmObj)
        {

            bool duplicateVillaNumber = _unitOfWork.VillaNumber.Any(u => u.Villa_Number == villaNumberVmObj.VillaNumber.Villa_Number);
            
            if (duplicateVillaNumber)
            {
                TempData["warning"] = "The Villa Number already exists";
            }
            
            
            
            if (ModelState.IsValid && !duplicateVillaNumber)
            {
                _unitOfWork.VillaNumber.add(villaNumberVmObj.VillaNumber);
                _unitOfWork.VillaNumber.saveVillaNumber();



                return RedirectToAction("Index");
            }

            villaNumberVmObj.VillaList = _unitOfWork.Villa.GetAll().Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString(),
            });
             
                return View(villaNumberVmObj);
            

        }

        public IActionResult Update(int villaId)
        {
            VillaNumberVm vm = new VillaNumberVm()
            {
                VillaList = _unitOfWork.Villa.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString(),
                }),
                VillaNumber = _unitOfWork.VillaNumber.Get(u => u.Villa_Number == villaId)
            };

            if (vm.VillaNumber == null)
            {
                return RedirectToAction("Error", "Home");
            }

            return View(vm);
        }



        [HttpPost]
        public IActionResult Update(VillaNumberVm villaNumberVm)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.VillaNumber.updateVillaNumber(villaNumberVm.VillaNumber);
                _unitOfWork.VillaNumber.saveVillaNumber();
                TempData["success"] = "The Villa Number was successfully updated";
                return RedirectToAction("Index");
            }

            villaNumberVm.VillaList = _unitOfWork.Villa.GetAll().Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString(),
            });

            TempData["error"] = "The villa couldn't be updated";
            return View(villaNumberVm);
        }


        public IActionResult Delete(int villaId)
        {
            VillaNumberVm vm = new VillaNumberVm()
            {
                VillaList = _unitOfWork.Villa.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString(),
                }),
                VillaNumber = _unitOfWork.VillaNumber.Get(u => u.Villa_Number == villaId)
            };

            if (vm.VillaNumber == null)
            {
                return RedirectToAction("Error", "Home");
            }

            return View(vm);
        }


        [HttpPost]
        public IActionResult Delete(VillaNumberVm villaObj)
        {
            VillaNumber? objFromDb = _unitOfWork.VillaNumber.Get(u => u.Villa_Number == villaObj.VillaNumber.Villa_Number);

            if(objFromDb is not null)
            {
                _unitOfWork.VillaNumber.remove(objFromDb);
                _unitOfWork.VillaNumber.saveVillaNumber();
                TempData["success"] = "The Villa Number has been deletd successfully";

                return RedirectToAction("Index");
            }
            TempData["error"] = "The Villa Number Could not be deleted";
            return View();
        }
    }
}
