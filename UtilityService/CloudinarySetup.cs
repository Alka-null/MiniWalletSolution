using CloudinaryDotNet;
using Firebase.Auth.Providers;
using Firebase.Storage;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Principal;

namespace UtilityService
{
    public static class CloudinarySetup
    {
        public static void SetupCloudinary(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton(new Account(
                configuration["Cloudinary:CloudName"],
                configuration["Cloudinary:ApiKey"],
                configuration["Cloudinary:ApiSecret"]
                ));

            services.AddScoped<Cloudinary>();
        }

    }
}