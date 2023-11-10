using BajajWeb.DAL.Interfaces;
using BajajWeb.Domain.Entity;
using System.Collections.Generic;
using System.Linq;

namespace BajajWeb.DAL.Repositories
{
    public class UserRepository : IBaseRepository<Users>
    {
        private readonly ApplicationDbContext _db;
        public UserRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public void Register(Users users)
        {
            _db.Users.Add(users);
            _db.SaveChanges();
        }
        public List<Users> GetAll()
        {
            return _db.Users.ToList();
        }
        public Users Update(Users entity)
        {
            _db.Users.Update(entity);
            _db.SaveChanges();
            return entity;
        }
    }
}
