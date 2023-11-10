using BajajWeb.Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace BajajWeb.DAL
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Motorcycle_characteristics>().HasKey(u => new {u.id_motorcycle_model, u.id_characteristic });
        }

        public DbSet<Models> Models { get; set; }
        public DbSet<All_characteristics> All_characteristics { get; set; }
        public DbSet<Category_characteristics> Category_characteristics { get; set; }
        public DbSet<List_of_units> List_of_units { get; set; }
        public DbSet<Marks> Marks { get; set; }
        public DbSet<Motorcycle_characteristics> Motorcycle_characteristics { get; set; }
        public DbSet<Motorcycles> Motorcycles { get; set; }
        public DbSet<Photos> Photos { get; set; }
        public DbSet<Users> Users { get; set; }
        public DbSet<Years_of_release> Years_of_release { get; set; }
        public DbSet<Cart> Cart { get; set; }
        public DbSet<Orders> Orders { get; set; }
    }
}
