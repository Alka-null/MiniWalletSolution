using Azure;
using DataAccess.Models;
using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AccountsService;
using AccountsService.Interfaces;
using AccountsService.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using UtilityService;

namespace API.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "SuperAdmin")]

    [Route("api/[controller]")]
    [ApiController]
    public class SuperAdminController : ControllerBase
    {

        private readonly UserManager<User> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IConfiguration _configuration;
        private readonly IUserAccountService userService;
        private readonly IAdminAccountService adminService;
        private readonly ISuperAdminAccountService superAdminService;

        public SuperAdminController(IUserAccountService userService, IAdminAccountService adminService, ISuperAdminAccountService superAdminService, UserManager<User> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this._configuration = configuration;
            this.adminService = adminService;
            this.superAdminService = superAdminService;
        }

        [HttpPost]
        [Route("register-admin")]
        public async Task<IActionResult> RegisterAdmin([FromBody] RegisterUser model)
        {
            var response = new ResponseObj();

            try
            {
                var adminExist = await userManager.FindByEmailAsync(model.Email);

                if (adminExist != null)
                {
                    response.Code = 403;

                    response.Message = "Email already exist.";

                    return BadRequest(response);
                }

                User newAdmin = new User()
                {
                    Email = model.Email,
                    UserName = model.Email,
                };

                var registerAdmin = await userManager.CreateAsync(newAdmin, model.Password);

                if (!registerAdmin.Succeeded)
                {
                    response.Code = 500;

                    response.Message = "Admin registration failed.";

                    return BadRequest(response);
                }

                var addRole = await userManager.AddToRoleAsync(newAdmin, "Admin");

                if (!addRole.Succeeded)
                {
                    await userManager.DeleteAsync(newAdmin);

                    response.Code = 500;

                    response.Message = "Admin registration failed. Could not add role.";
                    //_logger.LogError("Admin registration failed. Could not add role.");

                    return BadRequest(response);
                }

                response.IsSuccess = true;
                response.Message = "Admin registration successful.";
                //_logger.LogInformation("Admin registration successful.");

                return Ok(response);
            }
            catch (Exception ex)
            {
                //_logger.LogError($"Something went wrong: {ex}");

                response.Code = 400;

                response.Message = ex.StackTrace.ToString();

                return BadRequest(response);
            }
        }

        [HttpPost]
        [Route("create-role")]
        public async Task<ActionResult> CreateAsync(string name)
        {
            var response = new ResponseObj();
            try
            {
                //check if the role exist
                bool roleExist = await roleManager.RoleExistsAsync(name);

                if (roleExist)
                {
                    response.Code = 400;

                    response.Message = "Role already exist.";
                    //_logger.LogError($"Cannot the create role {name}. Role already exist.");
                    return BadRequest(response);
                }

                var role = await roleManager.CreateAsync(new IdentityRole(name));

                if (!role.Succeeded)
                {
                    response.Code = 400;

                    response.Message = "Failed. Cannot create role.";
                    //_logger.LogError($"Cannot create the role {name}. Failed.");
                    return BadRequest(response);
                }

                response.Message = "The role has been added successfully.";
                response.IsSuccess = true;
                response.Data = true;
                //_logger.LogInformation($"The role {name} has been added successfully");
                return Ok(role);
            }
            catch (Exception ex)
            {
                //_logger.LogError($"Something went wrong: {ex}");

                //response.Code = 403;

                response.Message = ex.StackTrace.ToString();

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
