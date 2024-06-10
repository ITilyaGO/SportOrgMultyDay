using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static SportOrgMultyDay.Processing.Logger;
namespace SportOrgMultyDay.Processing.Parsing
{
    public static class ParseCourse
    {
        public static string PCName(JToken cource)
        {
            try
            {
                return (string)cource["name"];
            }
            catch (Exception ex) { LogError("weoifhjwd", ex); }
            return null;
        }
        public static string PCId(JToken cource)
        {
            try
            {
                return (string)cource["id"];
            }
            catch (Exception ex) { LogError("sdkjqwkldk", ex); }
            return null;
        }
        public static JArray PCControls(JToken cource)
        {
            try
            {
                return (JArray)cource["controls"];
            }
            catch (Exception ex) { LogError("aslef34g567j", ex); }
            return null;
        }
        public static int PCCorridor(JToken cource)
        {
            try
            {
                return (int)cource["corridor"];
            }
            catch (Exception ex) { LogError("oi82316cvdiow", ex); }
            return -1;
        }

        public static JToken FCById(string id, JArray courses)
        {
            try
            {
                for (int i = 0; i < courses.Count; i++)
                    if (id == PCId(courses[i])) return courses[i];
            }
            catch (Exception ex) { LogError("skjdgqlwdtg", ex); }
            return null;
        }
    }
}
