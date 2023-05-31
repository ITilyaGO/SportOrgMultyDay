using Newtonsoft.Json.Linq;
using SportOrgMultyDay.Data.Combine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SportOrgMultyDay.Processing.Parsing.ParseBase;
using static SportOrgMultyDay.Processing.Parsing.ParseResult;
using static SportOrgMultyDay.Processing.Parsing.ParseGroup;
using static SportOrgMultyDay.Processing.Parsing.ParsePerson;

namespace SportOrgMultyDay.Processing.Combine
{
    public static class ResultsCountInGroup
    {
        public static Dictionary<string, int> GetResultsCountInGroup(JToken race, out string msgLog)
        {
            msgLog = "  Подсчет количества результатов в группах...\n";
            JArray raceResults = PBResults(race);
            JArray persons = PBPersons(race);
            JArray groups = PBGroups(race);
            List<PersonGroupResult> personGroupResults = new();
            
            msgLog += "    Генерация словаря результатов участников...\n";
            Dictionary<string, JToken> dictPersonIdResults = DictRIdPerson(raceResults, out List<string> duplicates);
            msgLog += String.Join("\n", duplicates.Select(dup => FPById(dup, persons)).Select(pers => $"      Дубликат результата: Номер: {PPBib(pers)} Id: {PPId(pers)} Имя: {PPName(pers)}\n"));
            
            Dictionary<string, JToken> dictGroupIdGroup = DictGIdGroup(groups);

            msgLog += "    Объединение групп, результатов и участников...\n";
            for (int i = 0; i < persons.Count; i++)
            {
                JToken person = persons[i];
                string personId = PPId(person);
                string groupId = PPGroupId(person);
                if (personId != null && dictPersonIdResults.TryGetValue(personId,out JToken result))
                    if (groupId != null && dictGroupIdGroup.TryGetValue(groupId, out JToken group))
                        personGroupResults.Add(new(person, group, result));
                    else
                        msgLog += $"      Группа не найдена: {PPToString(person)}\n";
                else
                    msgLog += $"      Результат не найден: {PPToString(person)}\n";
            }

            Dictionary<string, int> resultsCountInGroup = new();

            msgLog += "    Подсчет результатов в группах...\n";
            var groupedPGR = personGroupResults.GroupBy(pgr => pgr.GroupID);
            foreach (var group in groupedPGR)
            {
                int resultsCount = 0;
                foreach (var pgr in group)
                    if (pgr.ResultPlace != -1)
                        resultsCount++;
                msgLog += $"      Группа: {PGName(FGById(group.Key, groups))} Валидные результаты:{resultsCount}\n";
                resultsCountInGroup.Add(group.Key, resultsCount);
            }
            msgLog += $"  Посчитано групп: {resultsCountInGroup.Count}\n";
            return resultsCountInGroup;
        }
    }
}
