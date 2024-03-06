using RestaurantManagement.Business.AuthService;
using RestaurantManagement.Business.BaseService;

namespace RestaurantManagement.Api.Extensions
{
    public static class ServiceRegisterExtension
    {
        public static void ServiceRegister(this IServiceCollection services)
        {
            services.AddTransient<IAuthService, AuthService>();
            services.AddScoped<IBaseService, BaseService>();
        }
    }
}
