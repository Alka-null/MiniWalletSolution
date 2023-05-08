using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountsService.Interfaces
{
    public interface IAdminAccountService
        //:IUserAccountService
    {
        public string DeactivateUser(string userid);
        public string ActivateUser(string userid);

        public Task<string> CreateCurrency(Currency newcurrency);
        //public Task<string> CreateUser(RegisterUser model);
    }
}
