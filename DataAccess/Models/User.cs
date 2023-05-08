using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models
{
    public class User: IdentityUser
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; }
        public DateTime DateCreated { get; set; }

        public ICollection<WalletTransaction> WalletTransactions { get; set; }
        public ICollection<UserWallet> UserWallets { get; set; }

    }
}
