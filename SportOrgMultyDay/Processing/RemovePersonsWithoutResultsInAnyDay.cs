using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;

using static SportOrgMultyDay.Processing.Parsing.ParseBase;
using static SportOrgMultyDay.Processing.Parsing.ParsePerson;
using static SportOrgMultyDay.Processing.Parsing.ParseResult;

namespace SportOrgMultyDay.Processing
{
    public static class RemovePersonsWithoutResultsInAnyDay
    {
        private const int StatusDidNotStart = 13;

        public static string Remove(JObject jbase)
        {
            string msgLog = "Удаление участников, не стартовавших ни в один день...\n";
            JArray races = PBRaces(jbase);

            HashSet<int> bibsWithResults = new();
            for (int i = 0; i < races.Count; i++)
            {
                JArray persons = PBPersons(races[i]);
                JArray results = PBResults(races[i]);
                HashSet<string> personIdsWithResult = results
                    .Where(r => PRStatus(r) != StatusDidNotStart)
                    .Select(r => PRPersonId(r))
                    .Where(id => id != null)
                    .ToHashSet();

                foreach (JToken person in persons)
                    if (personIdsWithResult.Contains(PPId(person)))
                        bibsWithResults.Add(PPBib(person));
            }

            int removedCount = 0;
            int removedResultsCount = 0;
            for (int i = 0; i < races.Count; i++)
            {
                JArray persons = PBPersons(races[i]);
                JArray results = PBResults(races[i]);
                msgLog += $"День:{i + 1}\n";
                for (int p = persons.Count - 1; p >= 0; p--)
                {
                    JToken person = persons[p];
                    if (!bibsWithResults.Contains(PPBib(person)))
                    {
                        msgLog += $"  Удален - {PersonToString.BibNameComment(person)}\n";
                        string personId = PPId(person);
                        for (int r = results.Count - 1; r >= 0; r--)
                        {
                            if (PRPersonId(results[r]) == personId)
                            {
                                results[r].Remove();
                                removedResultsCount++;
                            }
                        }
                        person.Remove();
                        removedCount++;
                    }
                }
            }
            msgLog += $"Удалено записей участников: {removedCount}\n";
            msgLog += $"Удалено результатов: {removedResultsCount}\n";
            return msgLog;
        }
    }
}
