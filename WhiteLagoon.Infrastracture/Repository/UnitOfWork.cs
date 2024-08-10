using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhiteLagoon.Application.Common.Interfaces;
using WhiteLagoon.Infrastracture.Data;

namespace WhiteLagoon.Infrastracture.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        public readonly ApplicationDbContext _db;
        
        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            Villa = new VillaRepository(_db);
            VillaNumber = new VillaNumberRepository(_db);
            Amenity = new AmenityRepository(_db);
        }
        
        public IVillaRepository Villa { get; private set; }

        public IVillaNumberRepository VillaNumber { get; private set; }

        public IAmenityRepository Amenity { get; private set; }
    }
}
