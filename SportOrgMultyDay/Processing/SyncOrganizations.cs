using Newtonsoft.Json.Linq;
using SportOrgMultyDay.Data.SportOrg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SportOrgMultyDay.Processing.Parsing.ParseBase;
using static SportOrgMultyDay.Processing.Parsing.ParseOrganization;
namespace SportOrgMultyDay.Processing
{
    public static class SyncOrganizations
    {
        public static string SyncNames(JToken jBase)
        {
            int currentRaceI = CurrentRaceID(jBase);
            string msgLog = "Синхронизация коллективов...";
            msgLog += $"Текущий день: {currentRaceI + 1}\n";
            JArray races = PBRaces(jBase);

            JArray orgsCurr = PBOrganizations(races[currentRaceI]);
            for (int g = 0; g < orgsCurr.Count; g++) //Перечисляем организаций текущего дня
            {
                JToken orgFrom = orgsCurr[g];
                string oFromId = POId(orgFrom);
                string oFromName = POName(orgFrom);
                msgLog += $"\n  Копирование - Из:{oFromName} {oFromId}  В:";
                for (int r = 0; r < races.Count; r++)  //Перечисляем Races для копирования настроек групп
                {
                    if (r == currentRaceI) continue; //Пропускаем race из которого копируем
                    JToken race = races[r];
                    JArray orgs = PBOrganizations(race);

                    JToken org = FOById(oFromId, orgs);
                    if (org == null)
                    {
                        msgLog += $"Д:{r + 1} О:[Коллектив не найден] ";
                    }else
                    {
                        msgLog += $"Д:{r + 1} О:[{POName(org)}] ";
                        org["name"] = oFromName;
                    }
                }
            }
            return msgLog;
        }
    }
}
