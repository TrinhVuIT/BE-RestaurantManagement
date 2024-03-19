using RestaurantManagement.Business.AddressService;
using RestaurantManagement.Business.AuthService;
using RestaurantManagement.Business.BaseService;
using RestaurantManagement.Business.EmailCofigServices;
using RestaurantManagement.Business.FileUploadService;
using RestaurantManagement.Business.FoodServices;
using RestaurantManagement.Business.FoodServices.IngredientDetailService;
using RestaurantManagement.Business.FoodServices.IngredientService;
using RestaurantManagement.Business.FoodServices.RecipeService;
using RestaurantManagement.Business.OrderServices;
using RestaurantManagement.Business.OrderServices.OrderDetailService;
using RestaurantManagement.Business.OrderServices.StockInDetailService;
using RestaurantManagement.Business.OrderServices.StockInService;
using RestaurantManagement.Business.OrderServices.StockOutDetailService;
using RestaurantManagement.Business.OrderServices.StockOutService;
using RestaurantManagement.Business.OrderServices.SupplierService;
using RestaurantManagement.Business.PurchaseOrderService;
using RestaurantManagement.Business.PurchaseOrderService.PurchaseOrderFoodService;

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
            services.AddScoped<IStockInService, StockInService>();
            services.AddScoped<IStockInDetailService, StockInDetailService>();
            services.AddScoped<IStockOutService, StockOutService>();
            services.AddScoped<IStockOutDetailService, StockOutDetailService>();
            services.AddScoped<IPurchaseOrderService, PurchaseOrderService>();
            services.AddScoped<IPurchaseOrderFoodService, PurchaseOrderFoodService>();
            services.AddScoped<IFileUploadService, FileUploadService>();
            services.AddScoped<IAddressService , AddressService>();
        }
    }
}
