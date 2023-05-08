using AccountsService.Models;
using DataAccess.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountsService.Interfaces
{
    public interface IUserAccountService: IAccountService
    {
        public Task<List<UserWallet>> CreateDefaultUserWallets();
        public Task<string> SaveGetPhotoUrl(IFormFile modelwithfile);
    }
}
