using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RestaurantManagement.Data.Entities;
using RestaurantManagement.Data.Entities.Address;

namespace RestaurantManagement.Data
{
    public class DataContext : IdentityDbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            
        }
        public DbSet<User> ApplicationUser { get; set; }
        public DbSet<ListOfCountries> Countries { get; set; }
        public DbSet<Provinces> Provinces { get; set; }
        public DbSet<Districts> Districts { get; set; }
        public DbSet<Wards> Wards { get; set; }
        public DbSet<RefreshToken> RefreshToken { get; set; }
    }
}
