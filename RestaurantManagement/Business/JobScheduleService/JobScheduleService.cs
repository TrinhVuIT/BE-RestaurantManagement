
using RestaurantManagement.Commons;
using RestaurantManagement.Data;
using RestaurantManagement.Data.Entities;

namespace RestaurantManagement.Business.JobScheduleService
{
    public class JobScheduleService : IJobScheduleService
    {
        public readonly DataContext _context;
        public JobScheduleService(DataContext context) {  _context = context; }
        public async Task<int> DeleteRefreshTokenAfterNumDate(int numDay)
        {
            var refreshToken = _context.RefreshToken.Where(x => x.ExpiredAt < DateTime.Now.AddDays(numDay)).ToList();

            var amount = refreshToken?.Count() ?? 0;
            if(amount > 0)
            {
                _context.RefreshToken.RemoveRange(refreshToken!);
                var logInfo = new LogsSystem
                {
                    LogName = Constants.JobScheduleOptions.DeleteRefreshTokenJob,
                    LogAmount = amount,
                    LogDescriptions = "Delete RefreshToken After Reaching Date (" + numDay + ") by auto system Cron Schedule",
                    NguoiTao = Constants.JobScheduleOptions.NameSystemJob,
                    NguoiCapNhat = Constants.JobScheduleOptions.NameSystemJob,
                };
                _context.LogsSystem.Add(logInfo);
                await _context.SaveChangesAsync();
            }

            return amount;
        }
    }
}
