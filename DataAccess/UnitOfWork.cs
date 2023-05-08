using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using DataAccess.Interfaces;
using DataAccess.Repositories;

namespace DataAccess
{

    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private IUserRepository _userRepository;
        private ICurrencyRepository _currencyRepository;
        private IWalletTransactionRepository _walletTransactionRepository;
        private IUserWalletRepository _userWalletRepository;

        public UnitOfWork(AppDbContext context, IUserWalletRepository userWalletRepository, IWalletTransactionRepository walletTransactionRepository, IUserRepository userRepository, ICurrencyRepository currencyRepository)
        {
            this._userRepository = userRepository;
            this._context = context;
            this._currencyRepository = currencyRepository;
            this._walletTransactionRepository = walletTransactionRepository;
            this._userWalletRepository = userWalletRepository;
        }

        public IUserRepository Users => _userRepository ??= new UserRepository(_context);
        public ICurrencyRepository Currencies => _currencyRepository ??= new CurrencyRepository(_context);
        public IWalletTransactionRepository WalletTransactions => _walletTransactionRepository ??= new WalletTransactionRepository(_context);
        public IUserWalletRepository UserWallets => _userWalletRepository ??= new UserWalletRepository(_context);

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }

}