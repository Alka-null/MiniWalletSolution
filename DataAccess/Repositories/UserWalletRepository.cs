using DataAccess.Interfaces;
using DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class UserWalletRepository : Repository<UserWallet>, IUserWalletRepository
    {
        public UserWalletRepository(AppDbContext context)
            : base(context)
        {
        }
    }
}
