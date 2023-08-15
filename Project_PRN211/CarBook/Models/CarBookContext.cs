using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CarBook.Models
{
    public class CarBookContext : IdentityDbContext
    {
        public CarBookContext(DbContextOptions options) : base(options)
        {
        }

        public List<AppUser> AppUsers { get; set; }
        public DbSet<Car> Cars { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Property> Properties { get; set; }
        public DbSet<Feature> Features { get; set; }
    }
}
