using RestaurantManagement.Data.Entities;
using RestaurantManagement.Data.RequestModels.Email;

namespace RestaurantManagement.Business.EmailCofigServices
{
    public interface IEmailConfigServices
    {
        Task<bool> AddNewEmailConfig(EmailConfigRequestModel newEmail);
        Task<bool> UpdateEmailConfig(long id, EmailConfigRequestModel updateEmail);
        Task<bool> DeleteEmail(long id);
        Task<bool> SendToEmailAsync(SendToEmailRequestModel emailModel);
        Task<bool> SendEmailForgotPasswordAsync(string fullNamse, string mail, string token);
    }
}
