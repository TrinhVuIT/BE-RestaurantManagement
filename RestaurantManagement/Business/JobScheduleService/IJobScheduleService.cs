namespace RestaurantManagement.Business.JobScheduleService
{
    public interface IJobScheduleService
    {
        Task<int> DeleteRefreshTokenAfterNumDate(int numDay);
    }
}
