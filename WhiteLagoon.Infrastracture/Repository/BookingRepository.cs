using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhiteLagoon.Application.Common.Interfaces;
using WhiteLagoon.Application.Utility;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Infrastracture.Data;

namespace WhiteLagoon.Infrastracture.Repository
{
    public class BookingRepository : Repository<Booking>, IBookingRepository
    {
        private readonly ApplicationDbContext _db;
        public BookingRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;

        }

        public void save()
        {
             _db.SaveChanges();       
        }

        public void updateBooking(Booking entity)
        {
            _db.Update(entity);
        }

        public void UpdateStatus(int bookingId, string orderStatus, int villaNumber=0)
        {
            var bookingFromDb = _db.Bookings.FirstOrDefault(m => m.Id == bookingId);
            if (bookingFromDb is not null)
            {
                bookingFromDb.Status = orderStatus;
                if(orderStatus == SD.StatusCheckedIn)
                {
                    bookingFromDb.VillaNumber = villaNumber;
                    bookingFromDb.ActualCheckInDate = DateTime.Now;
                }
                
                if(orderStatus == SD.StatusCompleted)
                {
                    bookingFromDb.ActualCheckoutDate = DateTime.Now;
                }
                
            }
        }

        public void UpdateStripePaymentId(int bookingId, string sessionId, string PaymentIntentId)
        {
            var bookingFromDb = _db.Bookings.FirstOrDefault(m =>m.Id == bookingId);
            if (bookingFromDb is not null) 
            {
                if (!string.IsNullOrEmpty(sessionId))
                {
                    bookingFromDb.StripeSessionId = sessionId;
                }

                if (!string.IsNullOrEmpty(PaymentIntentId))
                { 
                    bookingFromDb.StripePaymentIntentId = PaymentIntentId;
                    bookingFromDb.PaymentDate = DateTime.Now;
                    bookingFromDb.IsPaymentSuccessful = true;
                
                }
            }
        }
    }
}
