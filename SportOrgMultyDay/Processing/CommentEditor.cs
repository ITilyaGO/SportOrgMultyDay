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
using static SportOrgMultyDay.Processing.Parsing.ParseCourse;
using static SportOrgMultyDay.Processing.Logger;
using static SportOrgMultyDay.Extensions.ListExtensions;
using static SportOrgMultyDay.Processing.Parsing.Things.ParseStartTime;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;

namespace SportOrgMultyDay.Processing
{
    public static class CommentEditor
    {
        public static string SetAllDaysToComment(JToken race, string groupsList, string newComment)
        {
            string log = $"Установка комментария [{newComment}] для всех участников...\n";
            JArray persons = PBPersons(race);
            JArray groups = PBGroups(race);

           
            string[] selectedGroupNames = groupsList.Split('\n', StringSplitOptions.RemoveEmptyEntries);
            List<JToken> groupsToRecomment = [];

            foreach (JToken group in groups)
            {
                if (selectedGroupNames.Contains(PGName(group)))
                    groupsToRecomment.Add(group);
            }
            log += $"  Группы: {groupsToRecomment.Count}  Участники: {persons.Count}\n";
            ILookup<string, JToken> groupId_persons = persons.ToLookup(p => PPGroupId(p));

            foreach (JToken group in groupsToRecomment)
            {
                log += $"\n\n    - Новые комменты в группе: {PGName(group)}... ";
                IEnumerable<JToken> personsInGroup = groupId_persons[PGId(group)];
                foreach (JToken person in personsInGroup)
                {
                    string personComment = PPComment(person);
                    if (string.IsNullOrEmpty(personComment) || personComment == newComment)
                        continue;
                    log += $"      \n[{PPToString(person)}] ";
                    person["comment"] = newComment;
                }
            }
            return log + "\nЗавершено\n";
        }
    }
}
