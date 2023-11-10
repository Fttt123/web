using BajajWeb.DAL.Interfaces;
using BajajWeb.Domain.Entity;
using System.Collections.Generic;
using System.Linq;

namespace BajajWeb.DAL.Repositories
{
    public class OrderRepository : IBaseRepository<Orders>
    {
        private readonly ApplicationDbContext _context;
        public OrderRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public List<Orders> GetAll()
        {
            return _context.Orders.ToList();
        }
        public void Register(Orders entity)
        {
            _context.Orders.Add(entity);
            _context.SaveChanges();
        }
        public Orders Update(Orders entity)
        {
            throw new System.NotImplementedException();
        }
    }
}
