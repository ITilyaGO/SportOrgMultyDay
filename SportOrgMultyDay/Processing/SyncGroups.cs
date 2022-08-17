using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SportOrgMultyDay.Processing.Parsing.ParseBase;
using static SportOrgMultyDay.Processing.Parsing.ParseGroup;

namespace SportOrgMultyDay.Processing
{
    public static class SyncGroups
    {
        public static string SyncByFields(JToken jBase) //Синхронизируем настройки рангов групп
        {
            int currentRaceI = CurrentRaceID(jBase);
            string msgLog = "Синхронизация групп...";
            msgLog += $"Текущий день: {currentRaceI + 1}\n";
            JArray races = Races(jBase);

            JArray groupsCurr = Groups(races[currentRaceI]);
            for (int g = 0; g < groupsCurr.Count; g++) //Перечисляем группы текущего дня
            {
                JToken rankingThis = PGRanking(groupsCurr[g]); 
                string gFromID = PGId(groupsCurr[g]);
                msgLog += $"\n  Копирование - Из:{PGName(groupsCurr[g])} {gFromID}  В:";
                for (int r = 0; r < races.Count; r++)  //Перечисляем Races для копирования настроек групп
                {
                    if (r == currentRaceI) continue; //Пропускаем race из которого копируем
                    JToken race = races[r];
                    
                    JArray groups = Groups(race);
                    for (int g2 = 0; g2 < groups.Count; g2++) //Группы другого дня
                    {
                        string g2ToID = PGId(groups[g2]);
                        if (gFromID == g2ToID) //Ищем нужную группу
                        {
                            groups[g2]["ranking"] = rankingThis.DeepClone(); //Копируем
                            msgLog += $"Д:{r+1} Г:[{PGName(groups[g2])}] ";
                            break;
                        }
                    }
                }
            }
            return msgLog;

        }

    }
}
