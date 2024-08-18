using Microsoft.AspNetCore.Mvc;
using WhiteLagoon.Application.Common.Interfaces;
using WhiteLagoon.Domain.Entities;

namespace WhiteLagoon.Web.Controllers
{
    public class BookingController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public BookingController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public ActionResult FinalizeBooking(int VillaId, DateOnly checkInDate, int Nights) 
        {
            Booking booking = new()
            {
                VillaId = VillaId,
                Villa = _unitOfWork.Villa.Get(u => u.Id == VillaId, includeProperties: "VillaAmenities"),
                CheckInDate = checkInDate,
                Nights = Nights,
                CheckOutDate = checkInDate.AddDays(Nights),

            };
            return View(booking);
        
            
        }
    }
}
