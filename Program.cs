using Serilog.Events;
using Serilog;
using NikuAPI.IRepository;
using NikuAPI.Repository;
using NikuAPI.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.OpenApi.Models;
using NikuAPI.Helper;
using Newtonsoft.Json;
using FluentValidation;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Host.UseSerilog();

builder.Services.AddControllers().AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);

builder.Services.AddScoped<IGoodTypeRepository, GoodTypeRepository>();
builder.Services.AddScoped<IGoodRepository, GoodRepository>();
builder.Services.AddScoped<IBoneRepository, BoneRepository>();
builder.Services.AddScoped<IImitationRepository, ImmitaionRepository>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IDateTimeRepository, DateTimeRepository>();
builder.Services.AddScoped<IQuoteRepository, QuoteRepository>();
builder.Services.AddScoped<IKioskRepository, KioskRepository>();
builder.Services.AddScoped<IStoreRepository, StoreRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddValidatorsFromAssemblyContaining<Program>();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "NIKU API", Version = "v1" });
    c.AddSecurityDefinition("basic", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "basic",
        In = ParameterLocation.Header,
        Description = "Basic Authorization header using the Bearer scheme."
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                          new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "basic"
                                }
                            },
                            new string[] {}
                    }
                });
});

builder.Services.AddAuthentication("BasicAuthentication")
               .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);

builder.Services.AddCors(
    options => options.AddPolicy("AllowAll", p => p.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//}

app.UseSwagger();
app.UseSwaggerUI();
app.UseCors("AllowAll");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

var path = AppContext.BaseDirectory;
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .MinimumLevel.Override("Microsoft.EntityFrameworkCore.Database.Command", LogEventLevel.Warning)
    .MinimumLevel.Override("AspNetCore.HealthChecks.UI", LogEventLevel.Warning)
    .MinimumLevel.Override("HealthChecks", LogEventLevel.Warning)
    .Filter.ByExcluding("RequestPath like '/health%' or contextType='HealthChecksDb' or options='StoreName=HealthChecksUI' or contextType='DatabaseContext'")
    .Filter.ByExcluding(c => c.Properties.Any(p => p.Value.ToString().Contains("swagger")))
    .Filter.ByExcluding(c => c.Properties.Any(p => p.Value.ToString().Contains("health")))
    .WriteTo.File(
    path: $"{path}\\logs\\log-.log",
    outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u3}] {Message:1}{NewLine}{Exception}",
    rollingInterval: RollingInterval.Day,
    restrictedToMinimumLevel: LogEventLevel.Information
    )
    .CreateLogger();

try
{
    Log.Information("Starting up");

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Host terminated unexpectedly");
}
finally
{
    Log.Information("Shutting down");
    Log.CloseAndFlush();
}
