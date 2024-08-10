using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhiteLagoon.Application.Common.Interfaces;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Infrastracture.Data;

namespace WhiteLagoon.Infrastracture.Repository
{
    public class AmenityRepository : Repository<Amenity>, IAmenityRepository
    {
        private readonly ApplicationDbContext _db;
        public AmenityRepository(ApplicationDbContext db) : base(db) 
        {
            _db = db;
        
        }
        
        public void save()
        {
            _db.SaveChanges();
        }

        public void updateAmenity(Amenity amenity)
        {
            _db.Update(amenity);
        }
    }
}
