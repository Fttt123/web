using BajajWeb.DAL.Interfaces;
using BajajWeb.Domain.Entity;
using System.Collections.Generic;
using System.Linq;

namespace BajajWeb.DAL.Repositories
{
    public class CartRepository : IBaseRepository<Cart>
    {
        private readonly ApplicationDbContext _context;
        public CartRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public List<Cart> GetAll()
        {
            return _context.Cart.ToList();
        }
        public void Register(Cart entity)
        {
            _context.Cart.Add(entity);
            _context.SaveChanges();
        }
        public Cart Update(Cart entity)
        {
            _context.Cart.Update(entity);
            _context.SaveChanges();
            return entity;
        }
    }
}
