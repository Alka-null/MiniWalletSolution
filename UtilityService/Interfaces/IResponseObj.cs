using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilityService.Interfaces
{
    public interface IResponseObj
    {
        public string? Message { get; set; }
        public int Code { get; set; }
        public bool IsSuccess { get; set; }
        public Object? Data { get; set; }
    }
}
