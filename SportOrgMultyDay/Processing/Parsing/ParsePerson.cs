using Newtonsoft.Json.Linq;
using SportOrgMultyDay.Processing.Parsing.Things;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static SportOrgMultyDay.Processing.Logger;


namespace SportOrgMultyDay.Processing.Parsing
{
    public static class ParsePerson
    {
        public static string PPId(JToken person)
        {
            try
            {
                return (string)person["id"];
            }
            catch (Exception ex) { LogError("yh377lal", ex); }
            return String.Empty;
        }
        public static int PPBib(JToken person)
        {
            try
            {
                return (int)person["bib"];
            }
            catch (Exception ex) { LogError("8d326vrc", ex); }
            return -1;
        }
        public static bool PPIsPersonal(JToken person)
        {
            try
            {
                return (bool)person["is_personal"];
            }
            catch (Exception ex) { LogError("23846rvcs", ex); }
            return false;
        }
        public static string PPComment(JToken person)
        {
            try
            {
                return (string)person["comment"];
            }
            catch (Exception ex) { LogError("ius6fcv32c", ex); }
            return null;
        }

        public static string PPGroupId(JToken person)
        {
            try
            {
                return (string)person["group_id"];
            }
            catch (Exception ex) { LogError("7fsaiufdw", ex); }
            return String.Empty;
        }
        public static string PPSurname(JToken person)
        {
            try
            {
                return (string)person["surname"];
            }
            catch (Exception ex) { LogError("o86vf3xxx", ex); }
            return null;
        }
        public static string PPName(JToken person)
        {
            try
            {
                return (string)person["name"];
            }
            catch (Exception ex) { LogError("pfi87v3da", ex); }
            return null;
        }

        public static string PPSurnameName(JToken person)
        {
            return PPSurname(person) + " " + PPName(person);
        }
        public static string PPOrganizationId(JToken person)
        {
            try
            {
                return (string)person["organization_id"];
            }
            catch (Exception ex) { LogError("g26felqxwq", ex); }
            return null;
        }
        public static int PPYear(JToken person)
        {
            try
            {
                return (int)person["year"];
            }
            catch (Exception ex) { LogError("8d326vrc", ex); }
            return -1;
        }
        public static int PPStartTime(JToken person)
        {
            try
            {
                return (int)person["start_time"];
            }
            catch (Exception ex) { LogError("o8sd6vcla", ex); }
            return -1;
        }
        public static int PPQual(JToken person)
        {
            try
            {
                return (int)person["qual"];
            }
            catch (Exception ex) { LogError("dqwf34g3d", ex); }
            return -1;
        }
        public static TimeSpan? PPStartTimeTS(JToken person)
        {
            try
            {
                return ParseStartTime.StartTimeToTimeSpan((int)person["start_time"]);
            }
            catch (Exception ex) { LogError("o8sd6vcla", ex); }
            return null;
        }

        public static string PPToString(JToken person)
        {
            try
            {
                return $"ID: {PPId(person)} Номер: {PPBib(person)} Имя: {PPSurname(person)} {PPName(person)}";
            }
            catch (Exception ex) { LogError("9edyug13od", ex); }
            return String.Empty;
        }

        public static int PPCardNumber(JToken person)
        {
            try
            {
                return (int)person["card_number"];
            }
            catch (Exception ex) { LogError("78gkajvydc", ex); }
            return -1;
        }
        public static List<JToken> FPAllByGroup(string groupId, JArray persons)
        {
            try
            {
                return persons.Where(p => PPGroupId(p) == groupId).ToList();
            }
            catch (Exception ex) { LogError("78gkajvydc", ex); }
            return null;
        }
        public static List<JToken> FPAllByGroup(string groupId, List<JToken> persons)
        {
            try
            {
                return persons.Where(p => PPGroupId(p) == groupId).ToList();
            }
            catch (Exception ex) { LogError("78gkajvydc", ex); }
            return null;
        }
        public static List<JToken> FPAllByOrganization(string organizationId, JArray persons)
        {
            try
            {
                return persons.Where(p => PPOrganizationId(p) == organizationId).ToList();
            }
            catch (Exception ex) { LogError("asd23hf2gs", ex); }
            return null;
        }
        public static JToken FPById(string id, JArray persons)
        {
            for (int i = 0; i < persons.Count; i++)
                if (id == PPId(persons[i])) return persons[i];
            return null;
        }
        public static JToken FPByBib(int bib, JArray persons)
        {
            for (int i = 0; i < persons.Count; i++)
                if (bib == PPBib(persons[i])) return persons[i];
            return null;
        }

        public static List<IGrouping<string, JToken>> GPByGroupId(JArray persons)
        {
            var groupIdPersons = persons.GroupBy(p => PPGroupId(p)).ToList();
            return groupIdPersons;
        }
    }
}
