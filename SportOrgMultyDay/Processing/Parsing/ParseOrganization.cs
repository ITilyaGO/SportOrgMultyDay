using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static SportOrgMultyDay.Processing.Logger;

namespace SportOrgMultyDay.Processing.Parsing
{
    public static class ParseOrganization
    {
        public static string POId(JToken organization)
        {
            try
            {
                return (string)organization["id"];
            }
            catch (Exception ex) { LogError("af6vl2dcad", ex); }
            return null;
        }
        public static string POName(JToken organization)
        {
            try
            {
                if (organization == null) return null;
                return (string)organization["name"];
            }
            catch (Exception ex) { LogError("86flvuwcq", ex); }
            return null;
        }
        public static JToken FOById(string id, JArray organizations)
        {
            for (int i = 0; i < organizations.Count; i++)
                if (id == POId(organizations[i])) return organizations[i];
            return null;
        }

        public static Dictionary<string, string> DictOIdOrgName(JArray orgs)
        {
            Dictionary<string, string> orgDict = new();
            try
            {
                foreach (JToken org in orgs)
                    orgDict.Add(POId(org), POName(org));
            }
            catch (Exception ex) { LogError("3g7k6jdasdsaaj", ex); }
            return orgDict;
        }
        public static Dictionary<string, string> DictOOrgNameId(JArray orgs)
        {
            Dictionary<string, string> orgDict = new();
            try
            {
                foreach (JToken org in orgs)
                    orgDict.Add(POName(org), POId(org));
            }
            catch (Exception ex) { LogError("3g7k6jdsakj", ex); }
            return orgDict;
        }
    }
}
