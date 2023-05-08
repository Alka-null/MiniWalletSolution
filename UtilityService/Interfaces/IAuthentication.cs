using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace UtilityService.Interfaces
{
    public interface IAuthentication
    {
        public Task<string> GenerateJwtToken(User User);
    }
}
