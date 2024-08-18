using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using System.Diagnostics;
using WhiteLagoon.Application.Common.Interfaces;
using WhiteLagoon.Models;
using WhiteLagoon.Web.ViewModel;

namespace WhiteLagoon.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            HomeVm homeVm = new()
            {
                VillasList = _unitOfWork.Villa.GetAll(includeProperties: "VillaAmenities"),
                CheckInDate = DateOnly.FromDateTime(DateTime.Now),
                Nights = 1
            };
            return View(homeVm);
        }


        [HttpPost]
        public IActionResult GetVillasByDate(int nights, DateOnly checkInDate) 
        {
            var villaList = _unitOfWork.Villa.GetAll(includeProperties: "VillaAmenities").ToList();

            foreach (var villa in villaList) 
            {
                if(villa.Id %2 == 0)
                {
                    villa.IsAvailable = false ;
                }
            }

            HomeVm homeVM = new()
            {
                CheckInDate = checkInDate,
                VillasList = villaList,
                Nights = nights
            };

            return PartialView("_VillaList",homeVM);    
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View();
        }
    }
}
