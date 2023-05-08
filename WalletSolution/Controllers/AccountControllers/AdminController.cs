 using Azure;
using DataAccess.Models;
using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AccountsService;
using AccountsService.Interfaces;
using AccountsService.Models;
using CloudinaryDotNet.Actions;
using API.DTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using UtilityService.Interfaces;
using UtilityService;
using SendGrid;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using DataAccess;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin, SuperAdmin")]
    public class AdminController : ControllerBase
    {

        private readonly UserManager<User> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IConfiguration _configuration;
        private readonly IUserAccountService userService;
        private readonly IAdminAccountService adminService;
        private readonly IAuthentication authentication;
        private readonly ResponseObj response;

        public AdminController(
            IAuthentication authentication, ResponseObj response,
            IUserAccountService userService, IAdminAccountService adminService, UserManager<User> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this._configuration = configuration;
            this.adminService = adminService;
            this.authentication = authentication;
            this.response= response;
            
        }

        [HttpPost]
        [Route("create-currency")]
        public async Task<IActionResult> CreateCurrency([FromForm] CurrencyDTO currency)
        {
            try
            {
                string logoUrl= "" ;
                if (currency.Logo!=null)
                {
                    logoUrl = await userService.SaveGetPhotoUrl(currency.Logo);
                }

                var newcurrency= new Currency { 
                        CurrencyCode=currency.CurrencyCode,
                        Name=currency.Name,
                        IsDefault=currency.IsDefault,
                        LogoUrl = logoUrl
                };

                var result = await adminService.CreateCurrency(newcurrency);

                response.Message = "Currency Created Successfully";
                response.Code = 200;
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.IsSuccess= false; 
                response.Message = "Could not create New Currency";
                return Forbid(response.Message);
            }
        }


        [HttpGet]
        [Route("get/all-users")]
        public async Task<IActionResult> GetAllUsersAsync()
        {
            try
            {
                var users = await userManager.Users.ToListAsync();

                return Ok(users);
            }
            catch (Exception ex)
            {
                throw new Exception("Could not retrieve all users");
            }
        }

        [HttpPatch]
        [Route("activate-user/{userId}")]
        public async Task<IActionResult> ActivateUser(string userId)
        {
            try
            {
                var user = await userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    response.Code = 400;

                    response.Message = "Failed. Cannot create role.";
                    //_logger.LogError($"Cannot create the role {name}. Failed.");
                    return BadRequest(response);
                }
                var deactivateuser = adminService.ActivateUser(userId);
                response.IsSuccess = true;
                response.Code = 200;
                response.Message = "User deactivated";
                return Ok(response);
            }
            catch (Exception e)
            {
                response.Message = e.StackTrace.ToString();

                return Forbid(response.Message);

            }
        }

        [HttpPatch]
        [Route("deactivate-user/{userId}")]
        public async Task<IActionResult> DeactivateUser(string userId)
        {
            try
            {
                var user = await userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    response.Code = 400;

                    response.Message = "Failed. Cannot create role.";
                    //_logger.LogError($"Cannot create the role {name}. Failed.");
                    return BadRequest(response);
                }
                var deactivateuser = adminService.DeactivateUser(userId);
                response.IsSuccess = true;
                response.Message = "User deactivated";
                return Ok(response);
            }
            catch (Exception e)
            {
                response.Message = e.StackTrace.ToString();

                return Forbid(response.Message);

            }
        }


        #region
        //[HttpPost]
        //[Route("register")]
        //public async Task<IActionResult> RegisterWithProfilePhoto([FromForm] RegisterUserWithProfilePhoto modelwithfile)
        //{
        //    var model = userService.ExtractModel(modelwithfile);
        //    var userExists = await userService.FindByEmailAsync(model.Email);
        //    if (userExists != null)
        //        return StatusCode(StatusCodes.Status500InternalServerError, new { Status = "Error", Message = "User already exists!" });

        //    var userprofileurl = await userService.SaveGetProfilePhotoUrl(modelwithfile.file);
        //    var registerusermodel = new RegisterUser
        //    {
        //        Email = modelwithfile.Email,
        //        Username = modelwithfile.Username,
        //        Password = modelwithfile.Password,
        //        ProfileUrl = userprofileurl
        //    };
        //    var user = await userService.CreateUser(model);

        //    return Ok(new { Status = "Success", Message = "User created successfully!" });
        //}
#endregion
    }
}
