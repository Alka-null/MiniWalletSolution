using AccountsService;
using AccountsService.Interfaces;
using BackgroundServices;
using DataAccess;
using DataAccess.Interfaces;
using DataAccess.Repositories;
using Microsoft.Extensions.DependencyInjection;
using UtilityService.Interfaces;
using WalletTransactions;
using WalletTransactions.Interfaces;

namespace UtilityService
{
    public static class AppServicesContainer
    {
        public static void AddAppServices(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ICurrencyRepository, CurrencyRepository>();
            services.AddScoped<IWalletTransactionRepository, WalletTransactionRepository>();
            services.AddScoped<IUserWalletRepository, UserWalletRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddHostedService<PeriodicInterestTask>();

            services.AddScoped<IUserAccountService, UserService>();
            services.AddScoped<IAdminAccountService, AdminService>();
            //services.AddScoped<ISuperAdminAccountService, SuperAdminService>();

            services.AddScoped<IDateTimeParse, DateTimeParse>();
            services.AddScoped<IAuthentication, Authentication>();
            services.AddScoped<IUserWalletTransactionService, UserWalletTransactionService>();
            services.AddScoped<IResponseObj, ResponseObj>();

            services.Configure<SuperAdminService>(options => options.CreateUser());
        }

    }
}