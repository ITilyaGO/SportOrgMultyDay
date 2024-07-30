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
        public static string ReplacePersonsListInOtherDays(JToken jBase)
        {
            string msgLog = "Замена списков участников... \n";
            int currentRaceI = CurrentRaceID(jBase);
            JArray races = PBRaces(jBase);
            JToken currRace = PBCurrentRace(races, currentRaceI);
            JArray persons = PBPersons(currRace);
            JArray organizations = PBOrganizations(currRace);

            for (int i = 0; i < races.Count; i++)
            {
                msgLog += $"    Д{currentRaceI+1}:";
                if (i == currentRaceI)
                {
                    msgLog += "  Пропущен - текуший. \n";
                    continue;
                }
                JToken race = races[i];
                int beforCount = PBPersons(race).Count;
                int beforCountOrg = PBOrganizations(race).Count;
                race["persons"] = persons.DeepClone();
                race["organizations"] = organizations.DeepClone();
                msgLog += $"  Скопировано. Кол-во участников [{beforCount} > {PBPersons(race).Count}]. Команды [{beforCountOrg} > {PBOrganizations(race).Count}] \n";
            }


            msgLog += "Завершено.\n";

            return msgLog;
        }
    }
}
