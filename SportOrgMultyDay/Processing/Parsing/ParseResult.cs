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
        public static int PRcardNumber(JToken result)
        {
            try
            {
                return (int)result["card_number"];
            }
            catch (Exception ex) { LogError("cniow8c4j", ex); }
            return 0;

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
        public static int PRPlace(JToken result)
        {
            try
            {
                return (int)result["place"];
            }
            catch (Exception ex) { LogError("843gu89diuq", ex); }
            return -1;
        }
        public static int PRStatus(JToken result)
        {
            try
            {
                return (int)result["status"];
            }
            catch (Exception ex) { LogError("iew7vdcadjqw", ex); }
            return -1;
        }
        public static Dictionary<string, JToken> DictRIdPerson(JArray results, out List<string> duplicates)
        {
            duplicates = new();
            Dictionary<string, JToken> resultDict = new();
            try
            {
                foreach (JToken result in results)
                {
                    string personId = PRPersonId(result);
                    if (personId == null)
                    {
                        continue;
                    }
                    if (resultDict.ContainsKey(personId))
                    {
                        int placeNew = PRPlace(result);
                        int placeOld = PRPlace(resultDict[personId]);
                        if (placeNew != -1 && placeNew < placeOld)
                            resultDict[personId] = result;
                        duplicates.Add(personId);
                    }
                    else
                    {
                        resultDict.Add(personId, result);
                    }
                }
            }
            catch (Exception ex) { LogError("8238gaigfdq", ex); }
            return resultDict;
        }
    }
}
