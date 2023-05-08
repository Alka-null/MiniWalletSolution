using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models
{
    public class WalletTransaction
    {
        public string Description { get; set; }
        public DateTime DateOfTransaction { get; set; }
        public int TransactionAmount { get; set; }

        public User User { get; set; }
        public Currency Currency { get; set; }
    }
}
