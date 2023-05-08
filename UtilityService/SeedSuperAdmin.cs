using AccountsService.Interfaces;
using AccountsService.Models;
using AccountsService;
using DataAccess.Interfaces;
using DataAccess.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UtilityService.Interfaces;

namespace UtilityService
{
    
    public class SeedSuperAdmin: ISeedSuperAdmin
    {
        private readonly UserManager<User> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IConfiguration configuration;
        private readonly IUserAccountService userService;
        private readonly IAdminAccountService adminService;
        private readonly ISuperAdminAccountService superAdminService;

        public SeedSuperAdmin(IUserAccountService userService, IAdminAccountService adminService, ISuperAdminAccountService superAdminService, UserManager<User> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.configuration = configuration;
            this.adminService = adminService;
            this.superAdminService = superAdminService;

            CreateSuperAdmin();
        }
        private async void CreateSuperAdmin()
        {
            var userExists = await userManager.FindByNameAsync(configuration["SuperAdmin:Username"]);
            if (userExists != null)
                return ;
            //return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User already exists!" });

            User user = new User()
            {
                Email = configuration["SuperAdmin:Email"],
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = configuration["SuperAdmin:UserName"]
            };
            var result = await userManager.CreateAsync(user, configuration["SuperAdmin:Password"]);
            if (!result.Succeeded)
                throw new Exception("Failed to Create Super Admin");
            //return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User creation failed! Please check user details and try again." });

            if (!await roleManager.RoleExistsAsync(UserRoles.SuperAdmin))
                await roleManager.CreateAsync(new IdentityRole(UserRoles.SuperAdmin));
            if (!await roleManager.RoleExistsAsync(UserRoles.User))
                await roleManager.CreateAsync(new IdentityRole(UserRoles.User));

            if (await roleManager.RoleExistsAsync(UserRoles.SuperAdmin))
            {
                await userManager.AddToRoleAsync(user, UserRoles.SuperAdmin);
            }

            return ;
            //return Ok(new Response { Status = "Success", Message = "User created successfully!" });
        }

    }
}