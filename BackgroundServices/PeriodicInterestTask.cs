using AccountsService.Interfaces;
using DataAccess.Interfaces;
using DataAccess.Models;
using EmailService.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace BackgroundServices
{
    public class PeriodicInterestTask : BackgroundService
    {
        //private readonly ILogger<PeriodicTask> _logger;

        private readonly UserManager<User> userManager;
        //private readonly RoleManager<IdentityRole> roleManager;
        private readonly IConfiguration _configuration;
        //private readonly IUserAccountService userService;
        //private readonly IAdminAccountService adminService;
        private readonly IEmailService sendGridEmailService ;
        private readonly IUnitOfWork _unitOfWork;

        public PeriodicInterestTask(
            RoleManager<IdentityRole> roleManager, UserManager<User> userManager,
            IUnitOfWork unitOfWork, IEmailService sendGridEmailService, IConfiguration configuration)
        {
            this.userManager = userManager;
            //this.roleManager = roleManager;
            this._configuration = configuration;
            //this.adminService = adminService;
            this.sendGridEmailService = sendGridEmailService ;
            this._unitOfWork = unitOfWork;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(TimeSpan.FromDays(365), stoppingToken);
                // your periodic task logic here
                double discount = 0.0375;
                var user = _unitOfWork.UserWallets.GetAll().ForEachAsync(x => x.Balance = (int)(x.Balance * discount));
                var admins = await userManager.GetUsersInRoleAsync("Admin");
                await _unitOfWork.CompleteAsync();
                //
                string subject = "Background Service for Periodic Interest";
                string content = "The Periodic Interest has been added to all customer accounts";
                foreach (var admin in admins)
                {
                    await sendGridEmailService.SendEmailAsync(admin.Email, subject, content);
                }
                //_logger.LogInformation("Periodic task running at: {time}", DateTimeOffset.Now);

            }
        }
    }
}