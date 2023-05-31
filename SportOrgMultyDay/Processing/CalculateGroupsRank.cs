using Microsoft.VisualBasic.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SportOrgMultyDay.Processing.Combine.ResultsCountInGroup;
using static SportOrgMultyDay.Processing.Parsing.ParseBase;
using static SportOrgMultyDay.Processing.Parsing.ParseResult;
using static SportOrgMultyDay.Processing.Parsing.ParseGroup;
using static SportOrgMultyDay.Processing.Parsing.ParsePerson;

namespace SportOrgMultyDay.Processing
{
    public static class CalculateGroupsRank
    {
        public static string ProcessCurrentRace(JToken jBase, string sourceGroupId)
        {
            string msgLog = "Подсчет рангов в группах текущего дня...\n";
            JToken race = PBCurrentRaceFromBase(jBase);
            Dictionary<string, int> resultsCount = GetResultsCountInGroup(race, out string log);
            msgLog += log;
            JArray groups = PBGroups(race);
            JToken sourceGroup = FGById(sourceGroupId, groups);
            if (sourceGroup == null)
            {
                msgLog += "Error: Группа не найдена\n";
                return msgLog;
            }


            JToken sourceGroupRank = PGRanking(sourceGroup);
            msgLog += "  Копируем настройки рангов...\n";
            foreach (var keyValue in resultsCount)
            {
                JToken group = FGById(keyValue.Key, groups);
                
                if (keyValue.Value < 6)
                    continue;
                if (group == null)
                {
                    msgLog += $"  Группа не найдена ID: {keyValue.Key}";
                    continue;
                }
                group["ranking"] = sourceGroupRank.DeepClone();
                msgLog += $"Ранги скопированы: {PGName(group)}\n";
            }
            msgLog += "\nЗавершено\n";



            return msgLog;
        }
    }
}
