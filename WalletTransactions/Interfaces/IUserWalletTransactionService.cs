using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WalletTransactions.DTOs;

namespace WalletTransactions.Interfaces
{
    public interface IUserWalletTransactionService
    {
        public Task<string> withdraw(string userId, WithdrawTransaction withdraw);
        public Task<string> topup(string userId, TopUpTransaction topup);
    }
}
