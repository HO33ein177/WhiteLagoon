using Microsoft.AspNetCore.Mvc;
using System.Net;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Infrastracture.Data;

namespace WhiteLagoon.Web.Controllers
{
    public class VillaController : Controller
    {
        private readonly ApplicationDbContext _db;
        public VillaController(ApplicationDbContext db) {
            _db = db;
        }
        public IActionResult Index()
        {
            var villas = _db.Villas.ToList();
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
                _db.Villas.Add(villaObj);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                return View(villaObj);
            }
         
        }

        public IActionResult Update(int villaId)
        {
            Villa? villaObj = _db.Villas.FirstOrDefault(v => v.Id == villaId);
            
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
                _db.Villas.Update(villaObj);
                _db.SaveChanges();
                TempData["success"] = "The Villa was successfully updated"; 
                return RedirectToAction("Index");
            }

            TempData["error"] = "The villa couldn't be updated";
            return View(villaObj);
        }


        public IActionResult Delete(int villaId)
        {
            Villa? villaObj = _db.Villas.FirstOrDefault(v => v.Id == villaId);

            if (villaObj == null)
            {
                return RedirectToAction("Error", "Home");
            }

            return View(villaObj);

        }


        [HttpPost]
        public IActionResult Delete(Villa villaObj)
        {
            Villa? villa = _db.Villas.FirstOrDefault(v => v.Id == villaObj.Id);
            if (villa is not null)
            {
                _db.Villas.Remove(villa);
                _db.SaveChanges();
                TempData["success"] = "The villa has been successfully deleted";
                return RedirectToAction("Index");
            }
            TempData["error"] = "The villa couldn't be deleted";
            return View();
        }
    }
}
