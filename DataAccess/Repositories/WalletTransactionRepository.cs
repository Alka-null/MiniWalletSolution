﻿using DataAccess.Interfaces;
using DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class WalletTransactionRepository : Repository<WalletTransaction>, IWalletTransactionRepository
    {
        public WalletTransactionRepository(AppDbContext context)
            : base(context)
        {
        }
    }
}
