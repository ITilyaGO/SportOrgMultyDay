using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static SportOrgMultyDay.Processing.Logger;

namespace SportOrgMultyDay.Processing.Parsing
{
    public static class ParseGroup
    {
        public static JToken PGRanking(JToken group)
        {
            try
            {
                return group["ranking"];
            }
            catch (Exception ex) { LogError("o8sd6vcla", ex); }
            return null;
        }
        public static string PGName(JToken group)
        {
            try
            {
                return (string)group["name"];
            }
            catch (Exception ex) { LogError("sd8g6vp23", ex); }
            return null;
        }
        public static string PGId(JToken group)
        {
            try
            {
                return (string)group["id"];
            }
            catch (Exception ex) { LogError("a03297vcr", ex); }
            return null;
        }
        public static JToken FGById(string id, JArray groups)
        {
            for (int i = 0; i < groups.Count; i++)
                if (id == PGId(groups[i])) return groups[i];
            return null;
        }
    }
}
