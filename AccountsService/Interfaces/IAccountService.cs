using AccountsService.Models;
using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountsService.Interfaces
{
    public interface IAccountService
    {
        public Task<List<User>> FindByNameAsync(string Username);
        public Task<User> FindByEmailAsync(string email);
        public Task<Object> Login(LoginUser model);
    }
}
