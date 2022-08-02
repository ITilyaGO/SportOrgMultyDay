using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static SportOrgMultyDay.Processing.Parsing.ParseBase;
using static SportOrgMultyDay.Processing.Parsing.ParsePerson;

namespace SportOrgMultyDay.Processing
{
    public static class ExportStartTimes
    {
        public static string ToSFRSmartTerminal(JToken jBase)
        {
            string msgLog = "Экспорт стартовых минут...\n";
            msgLog += $"Текущий день: {CurrentRaceID(jBase) + 1}\n";
            JArray persons = PersonsCurRace(jBase);

            string outStr = GetHead();
            for (int p = 0; p < persons.Count; p++)
            {
                JToken preson = persons[p];
                outStr += GetPersonLine(p, preson);
            }
            return outStr;
        }

        private static string StartTimeToString(int startTime)
        {
            TimeSpan dt = TimeSpan.FromMilliseconds(startTime);
            return dt.ToString();
        }

        private static string GetHead()
        {
            string outStr = "";
            outStr += $"h000000\tv5.01\t1\t1\t0\tSportOrg\tMultyDay\n";
            outStr += $"t000000\tName\n";
            return outStr;
        }

        private static string GetPersonLine(int lineId,JToken person)
        {
            string line = "";
            line += cCode(lineId) + Tabs(1);
            line += PPBib(person) + Tabs(1);
            //  line += person["group_id"] + Tabs(1);
            line += "group_id" + Tabs(1);
            line += PPSurname(person) + " " + PPName(person) + Tabs(2);
          //  line += person["organization_id"] + Tabs(1);
            line += "org" + Tabs(1);
            line += "or" + Tabs(1);
            line += PPYear(person) + Tabs(3);
            line += 0 + Tabs(7);
            line += StartTimeToString(PPStartTime(person)) + Tabs(3);
            return line + "\n";
        }

        private static string Tabs(int count)
        {
            string s = "";
            for (int i = 0; i < count; i++)
                s += "\t";
            return s;
        }

        private static string cCode(int id)
        {
            string s = "c";
            string sId = id.ToString();
            int zeroToAdd = 6 - sId.Length;
            for (int i = 0; i < zeroToAdd; i++)
                s += "0";
            s += sId;
            return s;
        }
    }
}
