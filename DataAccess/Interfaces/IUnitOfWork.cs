using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Interfaces
{
    //public interface IUnitOfWork : IDisposable
    //{
    //    IRepository<TEntity> Repository<TEntity>() where TEntity : class;
    //    void Save();
    //    Task SaveAsync();
    //}

    public interface IUnitOfWork : IDisposable
    {
        IUserRepository Users { get; }
        ICurrencyRepository Currencies { get; }
        IWalletTransactionRepository WalletTransactions { get; }
        IUserWalletRepository UserWallets { get; }

        Task<int> CompleteAsync();
    }
}
