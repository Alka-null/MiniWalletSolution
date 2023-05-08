using AccountsService.Interfaces;
using AccountsService.Models;
using DataAccess;
using DataAccess.Interfaces;
using DataAccess.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AccountsService
{
    public class AccountService : IAccountService
    {
        public AccountService() { }

        private readonly UserManager<User> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork _unitOfWork;
        public AccountService(IUnitOfWork unitOfWork, UserManager<User> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this._configuration = configuration;
            this._unitOfWork = unitOfWork;
        }
        public async Task<List<User>> FindByNameAsync(string Username)
        {

            //List<User> users = userManager.FindByNameAsync
            var users = _unitOfWork.Users.GetAll().Where(x=> x.UserName == Username).ToList();
            return users;
        }
        public async Task<User> FindByEmailAsync(string email)
        {

            User user = await userManager.FindByEmailAsync(email);
            return user;
        }
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public virtual async Task<string> CreateUser()
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            return "";
        }

        public async Task<Object> Login(LoginUser model)
        {
            var user = await userManager.FindByEmailAsync(model.Email);
            if (user != null && await userManager.CheckPasswordAsync(user, model.Password))
            {
                var userRoles = await userManager.GetRolesAsync(user);

                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }

                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

                var token = new JwtSecurityToken(
                    issuer: _configuration["JWT:ValidIssuer"],
                    audience: _configuration["JWT:ValidAudience"],
                    expires: DateTime.Now.AddHours(3),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                    );

                return new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo
                };
            } 
  
            return new { };
        }

        Task<Object> IAccountService.Login(LoginUser model)
        {
            throw new NotImplementedException();
        }
    }
}
