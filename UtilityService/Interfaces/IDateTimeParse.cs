using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilityService.Interfaces
{
    public interface IDateTimeParse
    {
        public string getYear(DateTime datetime);

        public string getMonth(DateTime datetime);

        public string getDay(DateTime datetime);
    }
}
