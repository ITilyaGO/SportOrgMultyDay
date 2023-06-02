using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportOrgMultyDay.Processing.Parsing.Things
{
    public static class ParseStartTime
    {
        public static string StartTimeToString(int startTime)
        {
            TimeSpan dt = TimeSpan.FromMilliseconds(startTime);
            return StartTimeToString(dt);
        }
        public static string StartTimeToString(TimeSpan startTime)
        {
            if (startTime.Days >= 1)
                startTime -= TimeSpan.FromDays(startTime.Days);
            return startTime.ToString();
        }
    }
}
