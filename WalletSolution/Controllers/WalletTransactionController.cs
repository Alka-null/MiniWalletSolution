using Azure;
using DataAccess.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AccountsService;
using AccountsService.Interfaces;
using AccountsService.Models;
using WalletTransactions.DTOs;
using System.Security.Claims;
using API.DTOs;
using Microsoft.EntityFrameworkCore;
using DataAccess.Interfaces;
using UtilityService.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using CloudinaryDotNet.Actions;
using SendGrid;
using UtilityService;
using WalletTransactions.Interfaces;
using WalletTransactions;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalletTransactionController : ControllerBase
    {

        private readonly UserManager<User> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IConfiguration _configuration;
        private readonly IUserAccountService userService;
        private readonly IAdminAccountService adminService;
        private readonly ISuperAdminAccountService superAdminService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDateTimeParse dateTimeParse;
        private readonly ResponseObj response;
        private readonly IUserWalletTransactionService userWalletTransactionService;

        public WalletTransactionController(
            IUserWalletTransactionService userWalletTransactionService,
            ResponseObj response, IDateTimeParse dateTimeParse, IUnitOfWork unitOfWork, 
            IUserAccountService userService, IAdminAccountService adminService, ISuperAdminAccountService superAdminService, UserManager<User> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this._configuration = configuration;
            this.adminService = adminService;
            this.superAdminService = superAdminService;
            this._unitOfWork = unitOfWork;
            this.dateTimeParse = dateTimeParse;
            this.response = response;
            this.userWalletTransactionService= userWalletTransactionService;

        }


        [HttpGet]
        [Route("all-transactions")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "SuperAdmin")]
        public IActionResult GetAllTransactionsByDateAndYear([FromQuery] FilterDto filterDto)
        {
            var response = new ResponseObj();
            if (!ModelState.IsValid)
            {
                response.IsSuccess = false;
                response.Message = "Bad Request";
                response.Code = 400;
            };
            var query = _unitOfWork.WalletTransactions.GetAll()
                .Where(m => m.DateOfTransaction == filterDto.Date.Date) // Filter by date
                .Where(m => dateTimeParse.getYear(m.DateOfTransaction) == filterDto.Year) // Filter by year
                .Where(m => dateTimeParse.getMonth(m.DateOfTransaction) == filterDto.Month) // Filter by year
                .Where(m => dateTimeParse.getYear(m.DateOfTransaction) == filterDto.Day); // Filter by year

            var totalItems = query.Count();
            var itemsPerPage = filterDto.PageSize;
            var totalPages = (int)Math.Ceiling(totalItems / (double)itemsPerPage);
            var pageNumber = filterDto.PageNumber < 1 ? 1 : filterDto.PageNumber;
            var items = query.Skip((pageNumber - 1) * itemsPerPage).Take(itemsPerPage).ToList();

            return Ok(new
            {
                TotalItems = totalItems,
                TotalPages = totalPages,
                PageNumber = pageNumber,
                ItemsPerPage = itemsPerPage,
                Items = items,
            });
        }

        [HttpPost]
        [Route("withdraw")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "RegularUser")]
        public async Task<IActionResult> WithdrawFromWallet([FromBody] WithdrawTransaction model)
        {
            try
            {
                
                var loggedInUserId = HttpContext.User.Claims.ToList().FirstOrDefault(x => x.Type == "Id").Value;

                if (string.IsNullOrEmpty(loggedInUserId.ToString()))
                {
                    response.Code = 400;

                    response.Message = "Invalid user.";
                    //_logger.LogError("Invalid user.");

                    return BadRequest(response);
                }

                //_logger.LogInformation($"Loggedin user identity id : {loggedInUserId}.");

                var withdraw = await userWalletTransactionService.withdraw(loggedInUserId, model);

                if (withdraw == null) throw new Exception("Couldn't make transaction");               

                    response.Message = "Deposit successully completed.";
                    response.IsSuccess = true;
                    response.Data = withdraw;
                    return Ok(response);

            }
            catch (Exception ex)
            {
                //_logger.LogError($"Something went wrong: {ex}");

                response.Message = ex.StackTrace.ToString();

                return Forbid(response.Message);
            }
        }

        [HttpPost]
        [Route("deposit")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "RegularUser")]
        public async Task<IActionResult> DepositToWallet([FromBody] TopUpTransaction model)
        {
            try
            {

                var loggedInUserId = HttpContext.User.Claims.ToList().FirstOrDefault(x => x.Type == "Id").Value;

                if (string.IsNullOrEmpty(loggedInUserId.ToString()))
                {
                    response.Code = 400;

                    response.Message = "Invalid user.";
                    //_logger.LogError("Invalid user.");

                    return BadRequest(response);
                }

                //_logger.LogInformation($"Loggedin user identity id : {loggedInUserId}.");

                var topup = await userWalletTransactionService.topup(loggedInUserId, model);

                if (topup == null) throw new Exception("Couldn't make transaction");

                response.Message = "Deposit successully completed.";
                response.IsSuccess = true;
                response.Data = topup;
                return Ok(response);

            }
            catch (Exception ex)
            {
                //_logger.LogError($"Something went wrong: {ex}");

                response.Message = ex.StackTrace.ToString();

                return Forbid(response.Message);
            }
        }


    }



}

