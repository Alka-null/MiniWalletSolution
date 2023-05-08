using AccountsService.Interfaces;
using DataAccess;
using DataAccess.Interfaces;
using DataAccess.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountsService
{
    public class SuperAdminService : AccountService, ISuperAdminAccountService
    {
        public SuperAdminService() { }
        private readonly UserManager<User> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork _unitOfWork;

        public SuperAdminService(IUnitOfWork unitOfWork, UserManager<User> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        :base(unitOfWork, userManager, roleManager, configuration)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this._configuration = configuration;
            this._unitOfWork = unitOfWork;
        }
        #region
        //public override async Task<string> CreateUser()
        //{
        //    #region
        //    //User user = new User()
        //    //{
        //    //    Email = model.Email,
        //    //    SecurityStamp = Guid.NewGuid().ToString(),
        //    //    UserName = model.Username
        //    //};
        //    //var result = await userManager.CreateAsync(user, model.Password);
        //    //if (!result.Succeeded)
        //    //    return "";
        //    //    //return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User creation failed! Please check user details and try again." });

        //    //if (!await roleManager.RoleExistsAsync(UserRoles.Admin))
        //    //    await roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
        //    //if (!await roleManager.RoleExistsAsync(UserRoles.User))
        //    //    await roleManager.CreateAsync(new IdentityRole(UserRoles.User));

        //    //if (await roleManager.RoleExistsAsync(UserRoles.Admin))
        //    //{
        //    //    await userManager.AddToRoleAsync(user, UserRoles.Admin);
        //    //}

        //    //return "";
        //    #endregion
        //    var UserName = _configuration["SuperAdmin:UserName"];
        //    var Email = _configuration["SuperAdmin:Email"];
        //    var Password = _configuration["SuperAdmin:Email"];

        //    var superAdminRole = await roleManager.FindByNameAsync(UserName);
        //    if (superAdminRole == null)
        //    {
        //        throw new Exception("Role Does not Exist");
        //        // Handle error
        //    }

        //    var superAdminUser = await userManager.FindByEmailAsync(Email);
        //    if (superAdminUser == null)
        //    {
        //        superAdminUser = new User
        //        {
        //            UserName = UserName,
        //            Email = Email,
        //            EmailConfirmed = true,
        //            LockoutEnabled = false
        //        };
        //        var result = await userManager.CreateAsync(superAdminUser, Password);
        //        if (!result.Succeeded)
        //        {
        //            throw new Exception("Failed to Create Super Admin");
        //            // Handle error
        //        }
        //    }

        //    if (!await userManager.IsInRoleAsync(superAdminUser, superAdminRole.Name))
        //    {
        //        var result = await userManager.AddToRoleAsync(superAdminUser, superAdminRole.Name);
        //        if (!result.Succeeded)
        //        {
        //            throw new Exception("Failed to Add Super Admin to Super Admin Role");
        //            // Handle error
        //        }
        //    }

        //    return "Ok";
        //}
        #endregion
    }
}
