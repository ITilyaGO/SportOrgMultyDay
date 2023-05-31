using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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
            try
            {
                for (int i = 0; i < groups.Count; i++)
                    if (id == PGId(groups[i])) return groups[i];
            }
            catch (Exception ex) { LogError("8v264bn5ahu", ex); }
            return null;
        }
        public static JToken FGByName(string name, JArray groups)
        {
            try
            {
                for (int i = 0; i < groups.Count; i++)
                    if (name == PGName(groups[i])) return groups[i];
            }
            catch (Exception ex) { LogError("43b6fgm9yu3df", ex); }
            return null;
        }
        public static Dictionary<string, JToken> DictGIdGroup(JArray groups)
        {
            Dictionary<string, JToken> groupDict = new();
            try
            {
                foreach (JToken group in groups)
                    groupDict.Add(PGId(group), group);
            }
            catch (Exception ex) { LogError("dh37asdl83h", ex); }
            return groupDict;
        }
    }
}
