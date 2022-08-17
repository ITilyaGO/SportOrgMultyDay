using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SportOrgMultyDay.Processing.Logger;

namespace SportOrgMultyDay.Processing.Parsing
{
    public static class ParseResult
    {
        public static string PRId(JToken result)
        {
            try
            {
                return (string)result["id"];
            }
            catch (Exception ex) { LogError("cniow8c4j", ex); }
            return null;
        }
        public static string PRPersonId(JToken result)
        {
            try
            {
                return (string)result["person_id"];
            }
            catch (Exception ex) { LogError("h767ry44h", ex); }
            return null;
        }
    }
}
