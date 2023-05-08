using Azure;
using DataAccess.Models;
using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AccountsService;
using AccountsService.Interfaces;
using AccountsService.Models;
using UtilityService.Interfaces;
using UtilityService;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly UserManager<User> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IConfiguration _configuration;
        private readonly IUserAccountService userService;
        private readonly IAuthentication authentication;
        private readonly ResponseObj response;
        public UserController(
                    IUserAccountService userService, IAuthentication authentication, ResponseObj response,
                    UserManager<User> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this._configuration = configuration;
            this.userService = userService;
            this.response = response;
            this.authentication = authentication;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromForm] RegisterUserWithProfilePhoto model)
        {
            try
            {
                var userExists = await userManager.FindByEmailAsync(model.Email);
                if (userExists != null) throw new Exception("user already exists");

                var createuserwallets = await userService.CreateDefaultUserWallets();

                var profileUrl = userService.SaveGetPhotoUrl(model.file);

                User user = new User()
                {
                    Email = model.Email,
                    SecurityStamp = Guid.NewGuid().ToString(),
                    UserName = model.Username,
                    IsActive = true,
                    UserWallets = createuserwallets
                };

                var result = await userManager.CreateAsync(user, model.Password);

                response.Code= 200;
                response.IsSuccess= true;
                response.Message = "User created successfully!";
                return Ok(response);

            }
            catch (Exception ex)
            {
                response.Code = 403;

                response.Message = ex.StackTrace.ToString();

                return Forbid(response.Message);
            }

        }

        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public async Task<ActionResult> LoginAsync([FromBody] LoginUser model)
        {
            var response = new ResponseObj();

            try
            {
                var existingUser = await userManager.FindByEmailAsync(model.Email);

                if (existingUser == null)
                {
                    response.Code = 400;
                    response.IsSuccess = false;
                    response.Message = "Invalid credential.";
                    //_logger.LogError("Invalid Credential.");
                    return BadRequest(response);
                };

                var IsPasswordValid = await userManager.CheckPasswordAsync(existingUser, model.Password);

                if (!IsPasswordValid)
                {
                    response.Code = 403;

                    response.Message = "Invalid credential.";
                    //_logger.LogError("Invalid Credential.");
                    return Forbid(response.Message);
                };

                var token = await authentication.GenerateJwtToken(existingUser);

                response.IsSuccess = true;
                response.Message = "Login was successful";
                response.Data = new { Token = token };
                return Ok(response);
            }
            catch (Exception ex)
            {
                //_logger.LogError($"Something went wrong: {ex}");

                response.Code = 403;

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
