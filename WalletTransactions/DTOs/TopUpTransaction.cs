﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WalletTransactions.DTOs
{
    public class TopUpTransaction
    {
        public int Amount { get; set; }
        public string Currency { get; set; }

    }
}
