using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportOrgMultyDay
{
    public class BaseParseException : Exception
    {
        public BaseParseException(string message)
    : base(message)
        {
        }
    }
}
