using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilityService.Interfaces;

namespace UtilityService
{
    internal class DateTimeParse:IDateTimeParse
    {
        public string getYear(DateTime datetime) {

            return datetime.Year.ToString();
        }

        public string getMonth(DateTime datetime)
        {

            return datetime.Month.ToString();
        }

        public string getDay(DateTime datetime)
        {

            return datetime.Day.ToString();
        }
    }
}
