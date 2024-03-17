using RestaurantManagement.Business.AuthService;
using RestaurantManagement.Business.BaseService;
using RestaurantManagement.Business.EmailCofigServices;
using RestaurantManagement.Business.FoodServices;
using RestaurantManagement.Business.FoodServices.IngredientDetailService;
using RestaurantManagement.Business.FoodServices.IngredientService;
using RestaurantManagement.Business.FoodServices.RecipeService;
using RestaurantManagement.Business.OrderServices;
using RestaurantManagement.Business.OrderServices.OrderDetailService;
using RestaurantManagement.Business.OrderServices.SupplierService;

namespace RestaurantManagement.Api.Extensions
{
    public static class ServiceRegisterExtension
    {
        public static void ServiceRegister(this IServiceCollection services)
        {
            services.AddTransient<IAuthService, AuthService>();
            services.AddScoped<IBaseService, BaseService>();
            services.AddScoped<IEmailConfigServices, EmailConfigServices>();
            services.AddScoped<IFoodService, FoodService>();
            services.AddScoped<IIngredientService, IngredientService>();
            services.AddScoped<IIngredientDetailService, IngredientDetailService>();
            services.AddScoped<IRecipeService, RecipeService>();
            services.AddScoped<ISupplierService, SupplierService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IOrderDetailService, OrderDetailService>();
        }
    }
}
