using AccountsService.Interfaces;
using DataAccess.Interfaces;
using DataAccess;
using DataAccess.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
//using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
//using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AccountsService.Models;
using Microsoft.AspNetCore.Http;
using CloudinaryDotNet.Actions;
using CloudinaryDotNet;

namespace AccountsService
{
    public class UserService: AccountService, IUserAccountService
    {
        public UserService() { }
        private readonly UserManager<User> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork _unitOfWork;
        private readonly Cloudinary _cloudinary;

        public UserService( Cloudinary cloudinary, IUnitOfWork unitOfWork, UserManager<User> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
            :base(unitOfWork, userManager, roleManager, configuration)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this._configuration = configuration;
            this._unitOfWork = unitOfWork;
            this._cloudinary = cloudinary;
        }
        public async Task<string> CreateUser(RegisterUser model) {
            var walletTransactions = await CreateDefaultUserWallets();

            User user = new User()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username,
                IsActive = true,
                UserWallets = walletTransactions
            };

            var result = await userManager.CreateAsync(user, model.Password);
            //await _unitOfWork.Users.AddAsync(user);

            if (!result.Succeeded)
                throw new Exception("User Creation Failed");
                //return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User creation failed! Please check user details and try again." });

            //return Ok(new Response { Status = "Success", Message = "User created successfully!" });
            return "Success";
        }

        public async Task<List<UserWallet>> CreateDefaultUserWallets()
        {
            var walletTransactions =  _unitOfWork.Currencies.GetAll().Where(x => x.IsDefault == true)
                                .Select(p => new UserWallet()
                                {
                                    DateCreated = new DateTime(),
                                    Currency = p
                                }).ToList();

            return walletTransactions;
        }
        #region
        //public RegisterUser ExtractModel(RegisterUserWithProfilePhoto modelwithfile)
        //{
        //    return new RegisterUser
        //    {
        //        Email = modelwithfile.Email,
        //        Password = modelwithfile.Password,
        //        Username = modelwithfile.Username

        //    };
        //    //throw new NotImplementedException();
        //}
        #endregion
        public async Task<string> SaveGetPhotoUrl(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return "No file selected";
            }

            var result = await _cloudinary.UploadAsync(new ImageUploadParams
            {
                File = new FileDescription(file.FileName, file.OpenReadStream())
            });

            return result.SecureUrl.AbsoluteUri;
        }

    }
}
