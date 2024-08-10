using Microsoft.AspNetCore.Mvc;
using System.Net;
using WhiteLagoon.Application.Common.Interfaces;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Infrastracture.Data;

namespace WhiteLagoon.Web.Controllers
{
    public class VillaController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        
        public VillaController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment) {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;

        }
        
        public IActionResult Index()
        {
            var villas = _unitOfWork.Villa.GetAll();
            return View(villas);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Villa villaObj)
        {
            if(villaObj.Name == villaObj.Description)
            {
                ModelState.AddModelError("", "The name and description can not be the same ");      
            }

            if (ModelState.IsValid)
            {
                if(villaObj.Image is not null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(villaObj.Image.FileName);
                    string imagePath = Path.Combine(_webHostEnvironment.WebRootPath, @"images\VillaImage");

                    using var fileStream = new FileStream(Path.Combine(imagePath, fileName), FileMode.Create);
                    villaObj.Image.CopyTo(fileStream);

                    villaObj.ImageUrl = @"\images\VillaImage\" + fileName;
                }
                else
                {
                    villaObj.ImageUrl = "https://placehold.co/600";
                }


                _unitOfWork.Villa.add(villaObj);
                _unitOfWork.Villa.save();
                return RedirectToAction("Index");
            }
            else
            {
                return View(villaObj);
            }
         
        }

        public IActionResult Update(int villaId)
        {
            Villa? villaObj = _unitOfWork.Villa.Get(v => v.Id == villaId);
            
            if (villaObj == null) { 
                return RedirectToAction("Error","Home");
            }

            return View(villaObj);

         }


        [HttpPost]
        public IActionResult Update(Villa villaObj)
        {
            if (ModelState.IsValid && villaObj.Id > 0)
            {
                if (villaObj.Image is not null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(villaObj.Image.FileName);
                    string imagePath = Path.Combine(_webHostEnvironment.WebRootPath, @"images\VillaImage");

                    if (!string.IsNullOrEmpty(villaObj.ImageUrl))
                    {
                        var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, villaObj.ImageUrl.TrimStart('\\'));

                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    using var fileStream = new FileStream(Path.Combine(imagePath, fileName), FileMode.Create);
                    villaObj.Image.CopyTo(fileStream);

                    villaObj.ImageUrl = @"\images\VillaImage\" + fileName;
                }
                


                _unitOfWork.Villa.updateVilla(villaObj);
                _unitOfWork.Villa.save();
                TempData["success"] = "The Villa was successfully updated"; 
                return RedirectToAction("Index");
            }

            TempData["error"] = "The villa couldn't be updated";
            return View(villaObj);
        }


        public IActionResult Delete(int villaId)
        {
            Villa? villaObj = _unitOfWork.Villa.Get(v => v.Id == villaId);

            if (villaObj == null)
            {
                return RedirectToAction("Error", "Home");
            }

            return View(villaObj);

        }


        [HttpPost]
        public IActionResult Delete(Villa villaObj)
        {
            Villa? villa = _unitOfWork.Villa.Get(v => v.Id == villaObj.Id);
            if (villa is not null)
            {
                if (!string.IsNullOrEmpty(villa.ImageUrl))
                {
                    var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, villa.ImageUrl.TrimStart('\\'));

                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }


                _unitOfWork.Villa.remove(villa);
                _unitOfWork.Villa  .save();
                TempData["success"] = "The villa has been successfully deleted";
                return RedirectToAction("Index");
            }
            TempData["error"] = "The villa couldn't be deleted";
            return View();
        }
    }
}
