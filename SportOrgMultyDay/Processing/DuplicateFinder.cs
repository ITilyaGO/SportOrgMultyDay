using Newtonsoft.Json.Linq;
using System.Globalization;

using static SportOrgMultyDay.Processing.Parsing.ParseBase;
using static SportOrgMultyDay.Processing.Parsing.ParseGroup;
using static SportOrgMultyDay.Processing.Parsing.ParsePerson;

namespace SportOrgMultyDay.Processing
{
    /// <summary>Набор участников, считающихся одним и тем же человеком (фамилия + имя + дата рождения).</summary>
    public class DuplicateSet
    {
        public string Key { get; init; }
        public List<JToken> Persons { get; init; } = new();
        /// <summary>Хотя бы один из дублей находится в эстафетной группе (__type = 3).</summary>
        public bool HasRelay { get; init; }
        public string Title { get; init; }
    }

    /// <summary>Участник эстафеты с разобранным world_code (id заявки - номер этапа).</summary>
    public class RelayMember
    {
        public JToken Person { get; init; }
        public string ApplicationId { get; init; }
        public int Stage { get; init; }
    }

    public static class DuplicateFinder
    {
        public const int RelayGroupType = 3;

        public static string BirthDate(JToken person)
        {
            string raw = person["birth_date"]?.ToString();
            if (!string.IsNullOrWhiteSpace(raw) && DateTime.TryParse(raw, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dt))
                return dt.ToString("dd.MM.yyyy");
            int year = PPYear(person);
            return year > 0 ? year.ToString() : string.Empty;
        }

        private static string Norm(string s) =>
            (s ?? string.Empty).Trim().ToLowerInvariant().Replace('ё', 'е');

        public static bool IsReserv(JToken person) => (PPSurname(person) ?? string.Empty).StartsWith("_Резерв");

        /// <summary>Ищет дубликаты в гонке. Дубликат - совпадение фамилии, имени и даты рождения (если даты нет - года) в любых группах.</summary>
        public static List<DuplicateSet> Find(JToken race, bool yearOnly = false)
        {
            JArray persons = PBPersons(race);
            JArray groups = PBGroups(race);
            var relayGroupIds = groups.Where(g => PGType(g) == RelayGroupType).Select(g => PGId(g)).ToHashSet();

            return persons
                .Where(p => !IsReserv(p))
                .GroupBy(p => $"{Norm(PPSurname(p))}|{Norm(PPName(p))}|{(yearOnly ? PPYear(p).ToString() : BirthDate(p))}")
                .Where(g => g.Count() > 1)
                .Select(g => new DuplicateSet
                {
                    Key = g.Key,
                    Persons = g.ToList(),
                    HasRelay = g.Any(p => relayGroupIds.Contains(PPGroupId(p))),
                    Title = $"{PPSurname(g.First())} {PPName(g.First())}"
                })
                .OrderBy(s => s.Title)
                .ToList();
        }

        public static RelayMember ParseRelay(JToken person)
        {
            string[] parts = (PPWorldCode(person) ?? string.Empty).Split('-');
            int stage = 0;
            if (parts.Length == 2)
                int.TryParse(parts[1], out stage);
            return new RelayMember
            {
                Person = person,
                ApplicationId = parts.Length == 2 ? parts[0] : string.Empty,
                Stage = stage
            };
        }

        /// <summary>Все участники той же группы с тем же id заявки, по этапам. Без id заявки возвращается только сам участник.</summary>
        public static List<RelayMember> RelayTeam(JToken person, JArray persons)
        {
            RelayMember self = ParseRelay(person);
            if (string.IsNullOrEmpty(self.ApplicationId))
                return new List<RelayMember> { self };
            string groupId = PPGroupId(person);
            return persons
                .Where(p => PPGroupId(p) == groupId)
                .Select(ParseRelay)
                .Where(m => m.ApplicationId == self.ApplicationId)
                .OrderBy(m => m.Stage)
                .ToList();
        }

        public static bool IsRelayGroup(JToken race, string groupId)
        {
            JToken group = FGById(groupId, PBGroups(race));
            return group != null && PGType(group) == RelayGroupType;
        }

        /// <summary>
        /// Выбирает дубль, которого нужно оставить. Приоритет: больше участников в заявке
        /// (для не эстафетных - 1), затем больший номер заявки (позже сделана), затем более поздняя запись в базе.
        /// </summary>
        public static JToken ChooseKeeper(DuplicateSet set, JToken race)
        {
            JArray persons = PBPersons(race);
            JArray groups = PBGroups(race);
            var relayGroupIds = groups.Where(g => PGType(g) == RelayGroupType).Select(g => PGId(g)).ToHashSet();
            var indexInBase = persons.Select((p, i) => (p, i)).ToDictionary(x => x.p, x => x.i);

            return set.Persons
                .OrderByDescending(p => relayGroupIds.Contains(PPGroupId(p)) ? RelayTeam(p, persons).Count : 1)
                .ThenByDescending(p => long.TryParse(ParseRelay(p).ApplicationId, out long app) ? app : -1)
                .ThenByDescending(p => indexInBase.TryGetValue(p, out int i) ? i : -1)
                .First();
        }

        /// <summary>
        /// Авто-пометка: в каждом наборе остается один дубль (см. ChooseKeeper), остальные помечаются.
        /// Если ignoreDifferentGroups - остается по одному дублю в каждой группе набора (лишние заявки внутри группы помечаются).
        /// </summary>
        public static HashSet<JToken> AutoMark(IEnumerable<DuplicateSet> sets, JToken race, bool ignoreDifferentGroups)
        {
            var result = new HashSet<JToken>();
            foreach (DuplicateSet set in sets)
            {
                IEnumerable<List<JToken>> parts = ignoreDifferentGroups
                    ? set.Persons.GroupBy(p => PPGroupId(p)).Select(g => g.ToList())
                    : new[] { set.Persons };
                foreach (List<JToken> part in parts)
                {
                    JToken keeper = ChooseKeeper(new DuplicateSet { Key = set.Key, Persons = part, HasRelay = set.HasRelay, Title = set.Title }, race);
                    foreach (JToken p in part)
                        if (!ReferenceEquals(p, keeper)) result.Add(p);
                }
            }
            return result;
        }

        /// <summary>Удаляет участников из гонки. Результаты не трогает.</summary>
        public static int Remove(IEnumerable<JToken> toRemove)
        {
            int count = 0;
            foreach (JToken p in toRemove.ToList())
            {
                p.Remove();
                count++;
            }
            return count;
        }
    }
}
