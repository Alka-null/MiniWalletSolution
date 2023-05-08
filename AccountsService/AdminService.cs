using AccountsService.Interfaces;
using DataAccess.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using DataAccess.Interfaces;
using AccountsService.Models;
using Microsoft.AspNetCore.Http;
//using System.Data.Entity;

namespace AccountsService
{
    public class AdminService : AccountService, IAdminAccountService
    {
        public AdminService() { }
        private readonly UserManager<User> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork _unitOfWork;

        public AdminService(IUnitOfWork unitOfWork, UserManager<User> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        : base(unitOfWork, userManager, roleManager, configuration)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this._configuration = configuration;
            this._unitOfWork = unitOfWork;
        }

        #region
        //public async Task<string> CreateUser(RegisterUser model)
        //{
        //    var userExists = await userManager.FindByNameAsync(model.Username);
        //    if (userExists != null)
        //        return "";
        //        //return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User already exists!" });

        //    User user = new User()
        //    {
        //        Email = model.Email,
        //        SecurityStamp = Guid.NewGuid().ToString(),
        //        UserName = model.Username
        //    };
        //    var result = await userManager.CreateAsync(user, model.Password);
        //    if (!result.Succeeded)
        //        return "";
        //        //return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User creation failed! Please check user details and try again." });

        //    if (!await roleManager.RoleExistsAsync(UserRoles.Admin))
        //        await roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
        //    if (!await roleManager.RoleExistsAsync(UserRoles.User))
        //        await roleManager.CreateAsync(new IdentityRole(UserRoles.User));

        //    if (await roleManager.RoleExistsAsync(UserRoles.Admin))
        //    {
        //        await userManager.AddToRoleAsync(user, UserRoles.Admin);
        //    }

        //    return "";
        //    //return Ok(new Response { Status = "Success", Message = "User created successfully!" });
        //}
        #endregion
        public string DeactivateUser(string userid)
        {
            var user = _unitOfWork.Users.GetAll().Where(x=> x.Id == userid).FirstOrDefault();
            user.IsActive = false;
            _unitOfWork.Users.Update(user);
            return "Success";
        }

        public string ActivateUser(string userid)
        {
            var user = _unitOfWork.Users.GetAll().Where(x => x.Id == userid).FirstOrDefault();
            user.IsActive = true;
            _unitOfWork.Users.Update(user);
            return "Success";
        }

        public async Task<string> CreateCurrency(Currency newcurrency)
        {
            if (newcurrency == null) throw new ArgumentNullException();
            await _unitOfWork.Currencies.AddAsync(newcurrency);
            return "Done";
        }
    }
}
