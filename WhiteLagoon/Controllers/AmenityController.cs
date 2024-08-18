using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WhiteLagoon.Application.Common.Interfaces;
using WhiteLagoon.Application.Utility;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Web.ViewModel;

namespace WhiteLagoon.Web.Controllers
{
    [Authorize(Roles =SD.Role_Admin)]
    public class AmenityController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public AmenityController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            var amenities = _unitOfWork.Amenity.GetAll(includeProperties: "Villa");
            return View(amenities);
        }

        public IActionResult Create()
        {
            AmenityVm vm = new AmenityVm()
            {
                AmenityList = _unitOfWork.Amenity.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString(),
                }),
            };


            return View(vm);
        }

        [HttpPost]
        public IActionResult Create(AmenityVm amenityVmObj)
        {

            //bool duplicateVillaNumber = _unitOfWork.Amenity.Any(u => u.Villa_Number == amenityVmObj.Amenity.Villa_Number);

            //if (duplicateVillaNumber)
            //{
            //    TempData["warning"] = "The Villa Number already exists";
            //}



            if (ModelState.IsValid )
            {
                _unitOfWork.Amenity.add(amenityVmObj.Amenity);
                _unitOfWork.Amenity.save();



                return RedirectToAction("Index");
            }

            amenityVmObj.AmenityList = _unitOfWork.Amenity.GetAll().Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString(),
            });

            return View(amenityVmObj);


        }

        public IActionResult Update(int Id)
        {
            AmenityVm vm = new AmenityVm()
            {
                AmenityList = _unitOfWork.Amenity.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString(),
                }),
                Amenity = _unitOfWork.Amenity.Get(u => u.Id == Id)
            };

            if (vm.Amenity == null)
            {
                return RedirectToAction("Error", "Home");
            }

            return View(vm);
        }



        [HttpPost]
        public IActionResult Update(AmenityVm amenityVm)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Amenity.updateAmenity(amenityVm.Amenity);
                _unitOfWork.Amenity.save();
                TempData["success"] = "The Villa Number was successfully updated";
                return RedirectToAction("Index");
            }

            amenityVm.AmenityList = _unitOfWork.Amenity.GetAll().Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString(),
            });

            TempData["error"] = "The villa couldn't be updated";
            return View(amenityVm);
        }


        public IActionResult Delete(int Id)
        {
            AmenityVm vm = new AmenityVm()
            {
                AmenityList = _unitOfWork.Amenity.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString(),
                }),
                Amenity = _unitOfWork.Amenity.Get(u => u.Id ==Id)
            };

            if (vm.Amenity == null)
            {
                return RedirectToAction("Error", "Home");
            }

            return View(vm);
        }


        [HttpPost]
        public IActionResult Delete(AmenityVm amenityObj)
        {
            Amenity? objFromDb = _unitOfWork.Amenity.Get(u => u.Id == amenityObj.Amenity.Id);

            if (objFromDb is not null)
            {
                _unitOfWork.Amenity.remove(objFromDb);
                _unitOfWork.Amenity.save();
                TempData["success"] = "The Villa Number has been deletd successfully";

                return RedirectToAction("Index");
            }
            TempData["error"] = "The Villa Number Could not be deleted";
            return View();
        }
    }
}
