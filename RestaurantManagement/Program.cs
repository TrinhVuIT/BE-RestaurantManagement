using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Quartz;
using Quartz.Impl;
using RestaurantManagement.Api.Extensions;
using RestaurantManagement.Api.JobSchedule;
using RestaurantManagement.Api.MiddleWare;
using RestaurantManagement.Data;
using RestaurantManagement.Data.Entities;
using System.Reflection;
using System.Text;
using static RestaurantManagement.Commons.Constants;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews()
                .AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles);
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

// Add services to the container.
builder.Services.AddDbContext<DataContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddCors();
builder.Services.AddControllers();

//For Identity
builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<DataContext>()
    .AddDefaultTokenProviders();

// Adding Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})

// Adding Jwt Bearer
 .AddJwtBearer(options =>
 {
     options.SaveToken = true;
     options.RequireHttpsMetadata = false;
     options.TokenValidationParameters = new TokenValidationParameters()
     {
         ValidateIssuer = false,
         ValidateAudience = false,
         ValidAudience = builder.Configuration[AppSettingKeys.JWT_VALIDAUDIENCE],
         ValidIssuer = builder.Configuration[AppSettingKeys.JWT_VALIDISSUER],
         ValidateIssuerSigningKey = true,
         IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration[AppSettingKeys.JWT_SECRET]!))
     };
 });
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    //hien thi mo ta tren Swagger va su dung Enum trong mo ta cua API
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
    options.UseInlineDefinitionsForEnums();

    options.SwaggerDoc("v1", new OpenApiInfo { Title = "RestaurantHRM API", Version = "v1" });
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
    options.CustomSchemaIds(type => type.ToString());
});

builder.Services.Configure<IdentityOptions>(options =>
{
    // Default Password settings.
    options.Password.RequireDigit = false; // Không bắt phải có số
    options.Password.RequireLowercase = false; // Không bắt phải có chữ thường
    options.Password.RequireNonAlphanumeric = false; // Không bắt ký tự đặc biệt
    options.Password.RequireUppercase = false; // Không bắt buộc chữ in
    options.Password.RequiredLength = 6; // Số ký tự tối thiểu của password
    options.Password.RequiredUniqueChars = 1; // Số ký tự riêng biệt
});

builder.Services.AddAutoMapper(typeof(Program).Assembly);
builder.Services.AddTransient<ExceptionHandlerMiddleWare>();
builder.Services.AddTransient<JwtMiddleWare>();
builder.Services.ServiceRegister();
builder.Services.AddProblemDetails();

builder.Services.AddQuartz(q =>
{
    var jobKey1 = new JobKey(JobScheduleOptions.DeleteRefreshTokenJob);
    q.AddJob<DeleteRefreshTokenJob>(opts => opts.WithIdentity(jobKey1));
    q.AddTrigger(opts => opts
    .ForJob(jobKey1)
    .WithIdentity($"{jobKey1}-trigger")
    .StartNow()
    .WithCronSchedule(builder.Configuration.GetSection("JobScheduleOptions:CronSchedule").Value ?? "0 0 1 * * ?"));
});

builder.Services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);
builder.Services.AddSingleton<IScheduler>(provider =>
{
    // Khởi tạo và cấu hình IScheduler
    var schedulerFactory = new StdSchedulerFactory();
    var scheduler = schedulerFactory.GetScheduler().Result;
    scheduler.Start().Wait();
    // Đợi một khoảng thời gian để công việc chạy
    Task.Delay(TimeSpan.FromMinutes(1));

    // Dừng Scheduler
    scheduler.Shutdown();
    return scheduler;
});
builder.Services.AddHostedService<QuartzHostedService>();
builder.Services.AddSignalR();
var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseDbMigration();
app.UseDataSeeding(app.Environment.IsDevelopment());

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors(x => x
    .AllowAnyMethod()
    .AllowAnyHeader()
    .SetIsOriginAllowed(origin => true) // allow any origin
    .AllowCredentials()); // allow credentials

app.UseHttpsRedirection();
app.UseMiddleware<ExceptionHandlerMiddleWare>();
app.UseMiddleware<JwtMiddleWare>();
app.UseAuthentication();
app.UseAuthorization();

app.UseExceptionHandler(c => c.Run(async context =>
{
    var exception = context.Features
        .Get<IExceptionHandlerPathFeature>()?
        .Error;
    var response = new { error = exception?.Message };
    await context.Response.WriteAsJsonAsync(response);
}));

app.MapControllers();

app.Run();
