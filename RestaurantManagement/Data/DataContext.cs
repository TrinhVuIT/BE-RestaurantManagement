using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using RestaurantManagement.Data.Entities;
using RestaurantManagement.Data.Entities.Address;

namespace RestaurantManagement.Data
{
    public class DataContext : IdentityDbContext<User>
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
        public DbSet<EmailConfig> EmailConfig { get; set; }
        public DbSet<Food> Food { get; set; }
        public DbSet<Ingredient> Ingredient { get; set; }
        public DbSet<PurchaseOrder> PurchaseOrder { get; set; }
        public DbSet<Recipe> Recipe { get; set; }
        public DbSet<StockOut> StockOut { get; set; }
        public DbSet<StockIn> StockIn { get; set; }
        public DbSet<Supplier> Supplier { get; set; }
        public DbSet<Order> Order { get; set; }
        public DbSet<IngredientDetail> IngredientDetail { get; set; }
        public DbSet<OrderDetail> OrderDetail { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();
            base.OnConfiguring(optionsBuilder);
        }
        public override int SaveChanges()
        {
            var httpContextAccessor = this.GetService<IHttpContextAccessor>();
            // get entries that are being Added or Updated
            var modifiedEntries = ChangeTracker.Entries()
                    .Where(x => x.State == EntityState.Added || x.State == EntityState.Modified);

            foreach (var entry in modifiedEntries)
            {
                // try and convert to an Auditable Entity
                var entity = entry.Entity as Commons.BaseEntityCommons;
                // call PrepareSave on the entity, telling it the state it is in
                entity?.PrepareSave(httpContextAccessor, entry.State);
            }

            var result = base.SaveChanges();
            return result;
        }
        public async Task<int> SaveChangesAsync()
        {
            var httpContextAccessor = this.GetService<IHttpContextAccessor>();
            // get entries that are being Added or Updated
            var modifiedEntries = ChangeTracker.Entries()
                    .Where(x => x.State == EntityState.Added || x.State == EntityState.Modified);

            foreach (var entry in modifiedEntries)
            {
                // try and convert to an Auditable Entity
                var entity = entry.Entity as Commons.BaseEntityCommons;
                // call PrepareSave on the entity, telling it the state it is in
                entity?.PrepareSave(httpContextAccessor, entry.State);
            }

            var result = await base.SaveChangesAsync();
            return result;

        }
    }
}
