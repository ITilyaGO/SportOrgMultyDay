using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static SportOrgMultyDay.Processing.Logger;

namespace SportOrgMultyDay.Processing.Parsing
{
    public static class ParseData
    {
        public static string PDTitle(JToken data)
        {
            try
            {
                return (string)data["title"];
            }
            catch (Exception ex) { LogError("dc6foa1vc", ex); }
            return null;
        }
        public static string PDChiefReferee(JToken data)
        {
            try
            {
                return (string)data["chief_referee"];
            }
            catch (Exception ex) { LogError("d86263vc2", ex); }
            return null;
        }
        public static string PDSecretary(JToken data)
        {
            try
            {
                return (string)data["secretary"];
            }
            catch (Exception ex) { LogError("d63bf53hgh", ex); }
            return null;
        }
    }
}
