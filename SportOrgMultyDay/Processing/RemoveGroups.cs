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
using static SportOrgMultyDay.Processing.Logger;

namespace SportOrgMultyDay.Processing
{
    public static class RemoveGroups
    {
        public static string RemoveGroupsIfNotInList(JToken race, string groupsList)
        {
            string log = string.Empty;
            log += "Удаление групп, не входящих в список...\n";
            string[] selectedGroupNames = groupsList.Split('\n', StringSplitOptions.RemoveEmptyEntries);

            JArray persons = PBPersons(race);
            JArray groups = PBGroups(race);
            log += $"  Группы: {groups.Count}  Участники: {persons.Count}\n";

            List<JToken> groupsToRemove = [];
            foreach (JToken group in groups)
            {
                if (!selectedGroupNames.Contains(PGName(group)))
                    groupsToRemove.Add(group);
            }
            log += $"  Групп к удалению: {groupsToRemove.Count}\n";
            log += $"  Удаляем группы: {string.Join(", ", groupsToRemove.Select(g => PGName(g)))}\n";
            ILookup<string, JToken> groupId_persons = persons.ToLookup(p => PPGroupId(p));

            foreach (JToken group in groupsToRemove)
            {
                log += $"    - Удаляем группу: {PGName(group)}... ";
                IEnumerable<JToken> personsInGroup = groupId_persons[PGId(group)];
                foreach (JToken person in personsInGroup)
                    persons.Remove(person);
                groups.Remove(group);
                log += $" ОК. Удалено участников: {personsInGroup.Count()}\n";
            }
            log += $"  Осталось групп: {groups.Count} Участников: {persons.Count}\n";
            log += "Завершено\n";
            return log;
        }

        public static string GetGroups(JToken race)
        {
            try
            {
                return string.Join("\n", PBGroups(race).Select(group => PGName(group)));
            }
            catch (Exception ex)
            {
                LogError("243fawdesf6j4", ex);
                return $"Ошибка {ex.Message}";
            }

        }

    }
}
