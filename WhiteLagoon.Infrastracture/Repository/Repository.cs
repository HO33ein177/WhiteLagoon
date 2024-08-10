using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using WhiteLagoon.Application.Common.Interfaces;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Infrastracture.Data;

namespace WhiteLagoon.Infrastracture.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _db;
        internal DbSet<T> dbSet;
        public Repository(ApplicationDbContext db)
        {
            _db = db;
            dbSet = _db.Set<T>();
        }
        public void add(T villaEntity)
        {
            dbSet.Add(villaEntity);
        }

        public IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter = null, string? inlcudeProperties = null)
        {
            IQueryable<T> query = dbSet;
            if (filter is not null)
            {
                query = query.Where(filter);
            }

            if (!string.IsNullOrEmpty(inlcudeProperties))
            {
                foreach (var includeProp in inlcudeProperties
                    .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }
            return query.ToList();
        }

        public T Get(Expression<Func<T, bool>>? filter, string? inlcudeProperties = null)
        {
            IQueryable<T> query = dbSet;
            if (filter is not null)
            {
                query = query.Where(filter);
            }

            if (!string.IsNullOrEmpty(inlcudeProperties))
            {
                foreach (var includeProp in inlcudeProperties
                    .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }
            return query.FirstOrDefault();
        }
        
        public bool Any(Expression<Func<T,bool>> filter) {  
            return dbSet.Any(filter); 
        }

        public void remove(T villaEntity)
        {
            dbSet.Remove(villaEntity);
        }

        
    }
}
