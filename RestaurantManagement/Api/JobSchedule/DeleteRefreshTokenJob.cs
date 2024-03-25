using Quartz;
using RestaurantManagement.Business.JobScheduleService;

namespace RestaurantManagement.Api.JobSchedule
{
    [DisallowConcurrentExecution]
    public class DeleteRefreshTokenJob : IJob
    {
        private readonly IJobScheduleService _jobScheduleService;
        private readonly ILogger<DeleteRefreshTokenJob> _logger;
        public DeleteRefreshTokenJob(IJobScheduleService jobScheduleService, ILogger<DeleteRefreshTokenJob> logger)
        {
            _jobScheduleService = jobScheduleService;
            _logger = logger;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            _logger.LogInformation("===START=== RefreshToken to delete old logs from the database at " + DateTime.Now);
            var logsDeleted = await _jobScheduleService.DeleteRefreshTokenAfterNumDate(-30);
            _logger.LogInformation("===END=== Deleted {0} RefreshToken old logs from the database at " + DateTime.Now, logsDeleted.ToString());
        }
    }
}
