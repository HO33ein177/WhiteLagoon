using Microsoft.AspNetCore.Mvc;
using WhiteLagoon.Application.Common.Interfaces;
using WhiteLagoon.Domain.Entities;

namespace WhiteLagoon.Web.Controllers
{
    public class AmenityController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public AmenityController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

        }

        public IActionResult Index()
        {
            var amenities = _unitOfWork.Amenity.GetAll();
            return View(amenities);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Amenity amenityObj)
        {
            if (amenityObj.Name == amenityObj.Description)
            {
                ModelState.AddModelError("", "The name and description can not be the same ");
            }

            if (ModelState.IsValid)
            {
               


                _unitOfWork.Amenity.add(amenityObj);
                _unitOfWork.Amenity.save();
                return RedirectToAction("Index");
            }
            else
            {
                return View(amenityObj);
            }

        }

        public IActionResult Update(int amenityId)
        {
            Amenity? amenityObj = _unitOfWork.Amenity.Get(v => v.Id == amenityId);

            if (amenityObj == null)
            {
                return RedirectToAction("Error", "Home");
            }

            return View(amenityObj);

        }


        [HttpPost]
        public IActionResult Update(Amenity amenityObj)
        {
            if (ModelState.IsValid && amenityObj.Id > 0)
            {
                
                _unitOfWork.Amenity.updateAmenity(amenityObj);
                _unitOfWork.Amenity.save();
                TempData["success"] = "The Villa was successfully updated";
                return RedirectToAction("Index");
            }

            TempData["error"] = "The villa couldn't be updated";
            return View(amenityObj);
        }


        public IActionResult Delete(int villaId)
        {
            Amenity? amenityObj = _unitOfWork.Amenity.Get(v => v.Id == villaId);

            if (amenityObj == null)
            {
                return RedirectToAction("Error", "Home");
            }

            return View(amenityObj);

        }


        [HttpPost]
        public IActionResult Delete(Amenity amenityObj)
        {
            Amenity? amenity = _unitOfWork.Amenity.Get(v => v.Id == amenityObj.Id);
            if (amenity is not null)
            {
                
                _unitOfWork.Amenity.remove(amenity);
                _unitOfWork.Amenity.save();
                TempData["success"] = "The villa has been successfully deleted";
                return RedirectToAction("Index");
            }
            TempData["error"] = "The villa couldn't be deleted";
            return View();
        }
    }
}
