using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models
{
    public class UserWallet
    {
        public int Balance { get; set; }
        public DateTime DateCreated { get; set; }

        public User User { get; set; }
        public Currency Currency { get; set; }
    }
}
