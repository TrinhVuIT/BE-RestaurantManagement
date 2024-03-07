using Microsoft.EntityFrameworkCore;
using RestaurantManagement.Data.Entities;

namespace RestaurantManagement.Data
{
    public partial class DataSeeding
    {
        static private int retryForAvailability = 0;
        public static void DevelopementSeed(DataContext context)
        {
            ILoggerFactory loggerFactory = new LoggerFactory();
            try
            {
                //context.Database.Migrate();
                context.Database.OpenConnection();

                //if (!context.EmailConfig.Any())
                //{
                //    context.EmailConfig.AddRange(ListEmailSettings());
                //    context.SaveChanges();
                //}
                context.Database.CloseConnection();
            }
            catch (Exception ex)
            {
                if (retryForAvailability < 10)
                {
                    context.Database.CloseConnection();
                    retryForAvailability++;
                    var log = loggerFactory.CreateLogger<DataSeeding>();
                    log.LogError(ex.Message);
                    DevelopementSeed(context);
                }
            }
        }
        static IEnumerable<EmailConfig> ListEmailSettings()
        {
            return new List<EmailConfig>()
            {
                new EmailConfig() {
                    Email = "test@gmail.com",
                    Password = "ahmx yvat wnou fghi",
                    Host = "smtp.gmail.com",
                    Port = 587,
                    Tittle = "HRM Management Software PhuongTrinh"
                }
            };
        }
    }
}
