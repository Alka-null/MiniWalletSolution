using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models
{
    public class Currency
    {
        public string Name { get; set; }
        public string CurrencyCode { get; set; }
        public bool IsDefault { get; set; }
        public string? LogoUrl { get; set; }
    }
}
