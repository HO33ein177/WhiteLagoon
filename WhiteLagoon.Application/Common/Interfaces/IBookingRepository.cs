using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhiteLagoon.Domain.Entities;

namespace WhiteLagoon.Application.Common.Interfaces
{
    public interface IBookingRepository : IRepository<Booking>
    {
        void updateBooking(Booking booking);

        void save();

        void UpdateStatus(int bookingId, string orderStatus, int villaNumber);
        
        void UpdateStripePaymentId(int bookingId, string sessionId, string PaymentIntentId);


    }
}
