using WhiteLagoon.Domain.Entities;
using System.Linq;

namespace WhiteLagoon.Web.ViewModel
{
    public class HomeVm
    {
        public IEnumerable<Villa> VillasList { get; set; }

        public DateOnly CheckInDate { get; set; }

        public DateOnly? CheckOutDate { get; set; }
        
        public int Nights { get; set; }
    }
}
