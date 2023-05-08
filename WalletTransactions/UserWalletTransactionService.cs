using DataAccess;
using DataAccess.Interfaces;
using DataAccess.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using WalletTransactions.DTOs;
using WalletTransactions.Interfaces;

namespace WalletTransactions
{
    public class UserWalletTransactionService: IUserWalletTransactionService
    {
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork _unitOfWork;

        public UserWalletTransactionService(IUnitOfWork unitOfWork, IConfiguration configuration)
        //: base(unitOfWork, userManager, roleManager, configuration)
        {
            this._configuration = configuration;
            this._unitOfWork = unitOfWork;
        }

        public Task<string> topup(string userId, TopUpTransaction topup)
        {
            throw new NotImplementedException();
        }

        public async Task<string> withdraw(string userId, WithdrawTransaction withdraw)
        {
            var currencyavailable = _unitOfWork.UserWallets.GetAll().Where(x =>
            (
                x.Currency.CurrencyCode == withdraw.Currency
            ));
            if (currencyavailable == null ) throw new Exception("Currency not Available");

            var userexist = currencyavailable.Where(x =>
            (
                x.User.Id == userId
            ));
            if (userexist == null) throw new Exception("User does not exist");


            var userwallet = userexist.Where(x =>
            (
                x.Balance >= withdraw.Amount
            )).ToList().FirstOrDefault();

            if (userwallet==null) throw new Exception("Insufficient balance");

            //User has enough balance
            userwallet.Balance = userwallet.Balance - withdraw.Amount;
            _unitOfWork.UserWallets.Update(userwallet);

            var user = await _unitOfWork.Users.GetByIdAsync(userId);
            var currency = _unitOfWork.Currencies.GetAll().Where(x => x.CurrencyCode == withdraw.Currency.ToUpper())
                                                            .ToList().FirstOrDefault();
            await _unitOfWork.WalletTransactions.AddAsync(new WalletTransaction
            {
                DateOfTransaction = new DateTime(),
                Description = "Withdrawal",
                TransactionAmount = withdraw.Amount,

                User = user,
                Currency = currency
            });

            await _unitOfWork.CompleteAsync();

            return "Success";
        }
    }
}