using Newtonsoft.Json.Linq;

using static SportOrgMultyDay.Processing.Parsing.ParsePerson;

namespace SportOrgMultyDay.Processing
{
    public enum AutoTeamClubMode
    {
        /// <summary>Команды только из участников одного клуба.</summary>
        OnlySameClub,
        /// <summary>Сначала из одного клуба, затем из оставшихся без учета клуба.</summary>
        SameClubThenRest,
        /// <summary>Клуб не учитывается.</summary>
        IgnoreClub
    }

    public class AutoTeamOptions
    {
        public AutoTeamClubMode ClubMode { get; set; } = AutoTeamClubMode.SameClubThenRest;
        /// <summary>Сначала объединять участников с самым высоким разрядом (число: чем больше, тем сильнее).</summary>
        public bool UseRank { get; set; } = true;
        /// <summary>Подбирать близких по возрасту.</summary>
        public bool UseAge { get; set; } = true;
        /// <summary>Сначала стремиться собрать команду по заявленным этапам (из world_code), потом менять этапы остаткам.</summary>
        public bool UseDeclaredStages { get; set; } = true;
    }

    /// <summary>Автоматическая сборка полных эстафетных команд из одиночек одной группы.</summary>
    public static class AutoTeamBuilder
    {
        /// <summary>
        /// Собирает команды по legs человек. Возвращает команды, в каждой участники уже в порядке этапов (индекс = этап - 1).
        /// Не вошедшие в полные команды участники не возвращаются.
        /// </summary>
        public static List<List<JToken>> Build(IEnumerable<JToken> singles, int legs, AutoTeamOptions options)
        {
            var result = new List<List<JToken>>();
            if (legs < 2) return result;

            // Базовый порядок - по номеру заявки, затем как в базе
            var pool = singles
                .Select((p, i) => (p, i))
                .OrderBy(x => long.TryParse(DuplicateFinder.ParseRelay(x.p).ApplicationId, out long a) ? a : long.MaxValue)
                .ThenBy(x => x.i)
                .Select(x => x.p).ToList();
            var order = pool.Select((p, i) => (p, i)).ToDictionary(x => x.p, x => x.i);
            var used = new HashSet<JToken>();

            if (options.ClubMode != AutoTeamClubMode.IgnoreClub)
            {
                foreach (var club in pool.Where(p => !string.IsNullOrEmpty(PPOrganizationId(p))).GroupBy(p => PPOrganizationId(p)))
                    FormTeams(club.ToList(), legs, options, order, used, result);
            }
            if (options.ClubMode != AutoTeamClubMode.OnlySameClub)
                FormTeams(pool.Where(p => !used.Contains(p)).ToList(), legs, options, order, used, result);
            return result;
        }

        private static void FormTeams(List<JToken> bucket, int legs, AutoTeamOptions o, Dictionary<JToken, int> order,
            HashSet<JToken> used, List<List<JToken>> result)
        {
            bucket = bucket.Where(p => !used.Contains(p)).ToList();
            while (bucket.Count >= legs)
            {
                JToken seed = o.UseRank
                    ? bucket.OrderByDescending(p => PPQual(p)).ThenBy(p => order[p]).First()
                    : bucket.OrderBy(p => order[p]).First();
                var team = new List<JToken> { seed };
                bucket.Remove(seed);

                while (team.Count < legs)
                {
                    var takenStages = team.Select(p => DeclaredStage(p, legs)).Where(s => s > 0).ToHashSet();
                    IOrderedEnumerable<JToken> ranked = bucket.OrderBy(p => 0);
                    if (o.UseDeclaredStages)
                        ranked = ranked.ThenBy(p => { int s = DeclaredStage(p, legs); return s > 0 && !takenStages.Contains(s) ? 0 : 1; });
                    if (o.UseAge)
                        ranked = ranked.ThenBy(p => AgeDiff(seed, p));
                    if (o.UseRank)
                        ranked = ranked.ThenByDescending(p => PPQual(p));
                    JToken next = ranked.ThenBy(p => order[p]).First();
                    team.Add(next);
                    bucket.Remove(next);
                }

                foreach (JToken p in team) used.Add(p);
                result.Add(AssignStages(team, legs, o.UseDeclaredStages, order));
            }
        }

        private static int DeclaredStage(JToken p, int legs)
        {
            int s = DuplicateFinder.ParseRelay(p).Stage;
            return s >= 1 && s <= legs ? s : 0;
        }

        private static int AgeDiff(JToken a, JToken b)
        {
            int ya = PPYear(a), yb = PPYear(b);
            return ya > 0 && yb > 0 ? Math.Abs(ya - yb) : 999;
        }

        /// <summary>Каждый получает заявленный этап, если он свободен; остальные занимают оставшиеся этапы по порядку.</summary>
        private static List<JToken> AssignStages(List<JToken> team, int legs, bool useDeclared, Dictionary<JToken, int> order)
        {
            var slots = new JToken[legs];
            var rest = new List<JToken>();
            foreach (JToken p in team.OrderBy(p => order[p]))
            {
                int s = useDeclared ? DeclaredStage(p, legs) : 0;
                if (s > 0 && slots[s - 1] == null) slots[s - 1] = p;
                else rest.Add(p);
            }
            int ri = 0;
            for (int i = 0; i < legs && ri < rest.Count; i++)
                if (slots[i] == null) slots[i] = rest[ri++];
            return slots.ToList();
        }
    }
}
