
using api.Model;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Data  {
    public class AppDbContext : DbContext {
        public AppDbContext(DbContextOptions dbContextOptions) : base(dbContextOptions){

        }

        public DbSet<User> Users { get; set; }
        public DbSet<RegisterUser> RegisterUsers { get; set; }
    }
}