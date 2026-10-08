using Newtonsoft.Json.Linq;

using static SportOrgMultyDay.Processing.Parsing.ParseBase;
using static SportOrgMultyDay.Processing.Parsing.ParseGroup;
using static SportOrgMultyDay.Processing.Parsing.ParsePerson;

namespace SportOrgMultyDay.Processing
{
    /// <summary>Участник неполной эстафетной заявки.</summary>
    public class IncompleteTeamMember
    {
        public JToken Person { get; init; }
        public JToken Group { get; init; }
        public string ApplicationId { get; init; }
        public int Stage { get; init; }
        /// <summary>Сколько участников в заявке.</summary>
        public int TeamSize { get; init; }
        /// <summary>Сколько этапов должно быть в группе (максимальный этап среди участников группы).</summary>
        public int GroupLegs { get; init; }
        public List<int> MissingStages { get; init; }
    }

    public static class IncompleteTeamsFinder
    {
        /// <summary>Эстафетные группы (__type = 3).</summary>
        public static List<JToken> RelayGroups(JToken race) =>
            PBGroups(race).Where(g => PGType(g) == DuplicateFinder.RelayGroupType).ToList();

        /// <summary>
        /// Участники заявок, в которых нет хотя бы одного этапа из 1..N, где N - максимальный этап в группе
        /// (так же определяется число этапов при присвоении номеров). Заявка без id в world_code считается одиночкой.
        /// groupId = null - все эстафетные группы.
        /// </summary>
        public static List<IncompleteTeamMember> Find(JToken race, string groupId = null)
        {
            JArray persons = PBPersons(race);
            var result = new List<IncompleteTeamMember>();
            foreach (JToken group in RelayGroups(race))
            {
                string id = PGId(group);
                if (groupId != null && id != groupId) continue;

                var members = persons.Where(p => PPGroupId(p) == id).Select(DuplicateFinder.ParseRelay).ToList();
                int legs = members.Select(m => m.Stage).DefaultIfEmpty(0).Max();
                if (legs <= 1) continue;

                // Участники без id заявки - каждый сам по себе
                var teams = members.Where(m => !string.IsNullOrEmpty(m.ApplicationId)).GroupBy(m => m.ApplicationId)
                    .Select(t => t.ToList())
                    .Concat(members.Where(m => string.IsNullOrEmpty(m.ApplicationId)).Select(m => new List<RelayMember> { m }));

                foreach (List<RelayMember> team in teams)
                {
                    var exist = team.Select(m => m.Stage).ToHashSet();
                    var missing = Enumerable.Range(1, legs).Where(s => !exist.Contains(s)).ToList();
                    if (missing.Count == 0) continue;
                    foreach (RelayMember m in team)
                        result.Add(new IncompleteTeamMember
                        {
                            Person = m.Person, Group = group, ApplicationId = m.ApplicationId, Stage = m.Stage,
                            TeamSize = team.Count, GroupLegs = legs, MissingStages = missing
                        });
                }
            }
            return result;
        }
    }
}
