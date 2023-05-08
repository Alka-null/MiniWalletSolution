using AccountsService;
using AccountsService.Interfaces;
using BackgroundServices;
using DataAccess;
using DataAccess.Interfaces;
using DataAccess.Models;
using DataAccess.Repositories;
using Firebase.Auth;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Configuration;
using System.Text;
using UtilityService;
using UtilityService.Interfaces;
//using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//builder.Services.AddSingleton<IConfiguration>(Configuration);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "Your API", Version = "v1" }));

var configurationBuilder = new ConfigurationBuilder()
    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

var configuration = configurationBuilder.Build();
builder.Services.AddSingleton(configuration);

var connectionString = configuration.GetConnectionString("DefaultConnection");

//Firebase storage options
builder.Services.SetupFirebase(configuration);

//Setup Cloudinary Storage
builder.Services.SetupCloudinary(configuration);

//sendGrid
builder.Services.SetupSendGridEmail(configuration);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));


builder.Services.AddAuthentication(configuration);
builder.Services.AddAuthorization(configuration);


builder.Services.AddIdentity<DataAccess.Models.User, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
                        .AddEntityFrameworkStores<AppDbContext>();


builder.Services.AddAppServices();
builder.Services.AddSingleton<ISeedSuperAdmin, SeedSuperAdmin>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
