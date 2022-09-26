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
        public static int PRResultMsec(JToken result)
        {
            try
            {
                return (int)result["result_msec"];
            }
            catch (Exception ex) { LogError("ajksdh2pdf", ex); }
            return -1;
        }
        public static int PRBib(JToken result)
        {
            try
            {
                return (int)result["bib"];
            }
            catch (Exception ex) { LogError("hgdudj3b76h", ex); }
            return -1;
        }
    }
}
