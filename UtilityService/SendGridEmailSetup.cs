using EmailService;
using EmailService.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SendGrid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilityService
{
    public static class SendGridEmailSetup
    {
        public static void SetupSendGridEmail(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<ISendGridClient>(new SendGridClient(configuration.GetSection("SendGrid")["ApiKey"]));
            services.AddScoped<IEmailService, SendGridEmailService>();

        }
    }
}
