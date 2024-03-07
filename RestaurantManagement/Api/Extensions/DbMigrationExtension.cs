using Microsoft.EntityFrameworkCore;
using RestaurantManagement.Data;

namespace RestaurantManagement.Api.Extensions
{
    public static class DbMigrationExtension
    {
        public static void UseDbMigration(this IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>()!.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<DataContext>();
                context.Database.Migrate();
            }
        }

        public static void UseDataSeeding(this IApplicationBuilder app, bool isDevelopment = false)
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>()!.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<DataContext>();
                if (isDevelopment)
                {
                    DataSeeding.DevelopementSeed(context);
                }
                else
                {
                    DataSeeding.ProductionSeed(context);
                }
            }
        }
    }
}
