using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static SportOrgMultyDay.Processing.Parsing.ParseBase;
using static SportOrgMultyDay.Processing.Parsing.ParsePerson;
using static SportOrgMultyDay.Processing.Parsing.ParseGroup;
using static SportOrgMultyDay.Processing.Parsing.ParseOrganization;
using static SportOrgMultyDay.Processing.Parsing.ParseData;

namespace SportOrgMultyDay.Processing.SFRSmartTerminal
{
    public static class ExportStartTimes
    {

        public static string ToSFRSmartTerminal(JToken jBase)
        {
            int currentRaceI = CurrentRaceID(jBase);
            string msgLog = "Экспорт стартовых минут...\n";
            msgLog += $"Текущий день: {currentRaceI + 1}\n";
            JToken race = CurrentRaceFromBase(jBase);
            JArray persons = Persons(race);
            JArray groups = Groups(race);
            JToken data = Data(race);
            JArray organizations = Organizations(race);
            string outStr = GetHead(data);
            for (int p = 0; p < persons.Count; p++)
            {
                JToken person = persons[p];


                outStr += GetPersonLine(p, person, groups, organizations);
            }
            return outStr;
        }

        private static string StartTimeToString(int startTime)
        {
            TimeSpan dt = TimeSpan.FromMilliseconds(startTime);
            return dt.ToString();
        }

        private static string GetHead(JToken data)
        {
            string outStr = "";
            outStr += $"h000000\tv5.01\t1\t1\t0\t{PDChiefReferee(data)}\t{PDSecretary(data)}\n";
            outStr += $"t000000\t{PDTitle(data)}\n";
            return outStr;
        }

        private static string GetPersonLine(int lineId, JToken person, JArray groups, JArray organizations)
        {
            string groupName = "none";
            string organizationName = "none";

            string pgId = PPGroupId(person);
            if (pgId != null)
            {
                JToken group = FGById(pgId, groups);
                groupName = PGName(group);
            }

            string poId = PPOrganizationId(person);
            if (poId != null)
            {
                JToken organization = FOById(poId, organizations);
                organizationName = POName(organization);
            }

            string line = "";
            line += cCode(lineId) + Tabs(1); //id Строки
            line += PPBib(person) + Tabs(1); //Номер
            line += groupName + Tabs(1); //Имя группы
            line += PPSurname(person) + " " + PPName(person) + Tabs(2);// ФИ участника
            //  line += person["organization_id"] + Tabs(1);
            line += organizationName;//Команда
            line += "" + Tabs(1); //Должны быть первые две буквы коменды, но мне лень
            line += PPYear(person) + Tabs(3); //Год рождения
            line += 0 + Tabs(7); //Стартовый взнос?
            line += StartTimeToString(PPStartTime(person)) + Tabs(3);//Время старта + окончание
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
