﻿using RestaurantManagement.Business.AuthService;
using RestaurantManagement.Business.BaseService;
using RestaurantManagement.Business.EmailCofigServices;
using RestaurantManagement.Business.FoodServices;
using RestaurantManagement.Business.FoodServices.IngredientService;

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
        }
    }
}