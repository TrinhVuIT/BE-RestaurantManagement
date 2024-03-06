using RestaurantManagement.Data.Entities;
using RestaurantManagement.Data.ResponseModels.User;

namespace RestaurantManagement.Business.BaseService
{
    public interface IBaseService
    {
        Task<TokenModel> GenerateTokenUser(User user, IList<string> roles);
        Task<ApiResponse> ReNewToken(TokenModel model);
    }
}
