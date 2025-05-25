using Newtonsoft.Json.Linq;
using SportOrgMultyDay.Data.SportOrg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SportOrgMultyDay.Processing.Parsing.ParseBase;
using static SportOrgMultyDay.Processing.Parsing.ParsePerson;
namespace SportOrgMultyDay.Processing
{
    public static class PersonListReplacer
    {
        public static string ReplacePersonsListInOtherDays(JToken jBase, bool copyPersons, bool copyOrganizations, bool copyGroups)
        {
            string msgLog = "Замена списков участников... \n";
            int currentRaceI = CurrentRaceID(jBase);
            JArray races = PBRaces(jBase);
            JToken currRace = PBCurrentRace(races, currentRaceI);

            JArray persons = PBPersons(currRace);
            JArray organizations = PBOrganizations(currRace);
            JArray groups = PBGroups(currRace);

            for (int i = 0; i < races.Count; i++)
            {
                msgLog += $"    Д{i + 1}:";
                if (i == currentRaceI)
                {
                    msgLog += "  Пропущен - текуший. \n";
                    continue;
                }
                JToken race = races[i];
                int beforCount = PBPersons(race).Count;
                int beforCountOrg = PBOrganizations(race).Count;
                int beforCountGrp = PBGroups(race).Count;

                if (copyPersons)
                    race["persons"] = persons.DeepClone();
                if (copyOrganizations)
                    race["organizations"] = organizations.DeepClone();
                if (copyGroups)
                    race["groups"] = groups.DeepClone();

                msgLog += "  Скопировано.";
                if (copyPersons)
                    msgLog += $" Кол-во участников [{beforCount} > {PBPersons(race).Count}]";
                if (copyOrganizations)
                    msgLog += $" Команды [{beforCountOrg} > {PBOrganizations(race).Count}]";
                if (copyGroups)
                    msgLog += $" Группы [{beforCountGrp} > {PBGroups(race).Count}]";
                msgLog += "\n";
            }
            msgLog += "Завершено.\n";

            return msgLog;
        }
    }
}
