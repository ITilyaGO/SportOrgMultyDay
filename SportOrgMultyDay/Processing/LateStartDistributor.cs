using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using static SportOrgMultyDay.Processing.Parsing.ParseBase;
using static SportOrgMultyDay.Processing.Parsing.ParseGroup;
using static SportOrgMultyDay.Processing.Parsing.ParsePerson;
using static SportOrgMultyDay.Processing.Parsing.Things.ParseStartTime;

namespace SportOrgMultyDay.Processing
{
    // Находит "дозаявленных" участников (start_time == 0, но заявленных на день по
    // комментарию "C:N") и расставляет им стартовое время внутри их группы:
    // определяется наиболее частый интервал между уже стартующими участниками группы,
    // выбирается случайное место в группе, весь стартовый коридор от этого места
    // сдвигается на интервал, а освободившийся слот отдаётся новому участнику.
    public static class LateStartDistributor
    {
        public class LateEntry
        {
            public int DayNumber; // 1-based, соответствует "C:N" в комментарии
            public int Bib;
            public string Description;
        }

        private static readonly Regex LineRegex = new(@"^\s*(\d+)\s*:\s*(\d+)", RegexOptions.Compiled);

        public static List<LateEntry> FindLateEntries(JObject jBase, bool allDays)
        {
            List<LateEntry> result = new();
            if (jBase == null)
                return result;

            JArray races = PBRaces(jBase);
            if (races == null || races.Count == 0)
                return result;

            int currentRaceId = CurrentRaceID(jBase);
            IEnumerable<int> raceIndexes = allDays ? Enumerable.Range(0, races.Count) : new[] { currentRaceId };

            foreach (int raceIndex in raceIndexes)
            {
                if (raceIndex < 0 || raceIndex >= races.Count)
                    continue;

                JToken race = races[raceIndex];
                JArray persons = PBPersons(race);
                JArray groups = PBGroups(race);
                if (persons == null)
                    continue;

                Dictionary<string, JToken> groupById = groups != null ? DictGIdGroup(groups) : new Dictionary<string, JToken>();

                foreach (JToken person in persons)
                {
                    if (PPStartTime(person) != 0)
                        continue;
                    if (!RemoveExtraPersons.RunPersonInDay(person, raceIndex))
                        continue;

                    string groupName = groupById.TryGetValue(PPGroupId(person), out JToken group) ? PGName(group) : "?";
                    result.Add(new LateEntry
                    {
                        DayNumber = raceIndex + 1,
                        Bib = PPBib(person),
                        Description = $"{PPSurnameName(person)} ({groupName})",
                    });
                }
            }

            return result.OrderBy(e => e.DayNumber).ThenBy(e => e.Bib).ToList();
        }

        public static string FormatEntries(List<LateEntry> entries)
        {
            StringBuilder sb = new();
            foreach (LateEntry entry in entries)
                sb.AppendLine($"{entry.DayNumber}:{entry.Bib}  # {entry.Description}");
            return sb.ToString();
        }

        public static string Distribute(JObject jBase, string rawText)
        {
            StringBuilder log = new();
            if (jBase == null)
            {
                log.AppendLine("База не загружена.");
                return log.ToString();
            }

            JArray races = PBRaces(jBase);
            if (races == null || races.Count == 0)
            {
                log.AppendLine("Нет дней соревнований.");
                return log.ToString();
            }

            HashSet<string> processed = new();
            string[] lines = (rawText ?? "").Replace("\r", "").Split('\n');

            foreach (string rawLine in lines)
            {
                string line = rawLine.Trim();
                if (line.Length == 0 || line.StartsWith("#"))
                    continue;

                Match match = LineRegex.Match(line);
                if (!match.Success)
                {
                    log.AppendLine($"Не удалось разобрать строку \"{line}\" (ожидается формат \"день:номер\")");
                    continue;
                }

                int dayNumber = int.Parse(match.Groups[1].Value);
                int bib = int.Parse(match.Groups[2].Value);
                int raceIndex = dayNumber - 1;

                if (raceIndex < 0 || raceIndex >= races.Count)
                {
                    log.AppendLine($"День {dayNumber} не существует, номер {bib} пропущен");
                    continue;
                }

                JToken race = races[raceIndex];
                JArray persons = PBPersons(race);
                JArray groups = PBGroups(race);
                JToken person = persons != null ? FPByBib(bib, persons) : null;

                if (person == null)
                {
                    log.AppendLine($"День {dayNumber}: участник с номером {bib} не найден, пропущен");
                    continue;
                }

                string personKey = raceIndex + ":" + PPId(person);
                if (!processed.Add(personKey))
                {
                    log.AppendLine($"День {dayNumber}: номер {bib} уже обработан в этом запуске, пропущен");
                    continue;
                }

                if (PPStartTime(person) != 0)
                {
                    log.AppendLine($"День {dayNumber}: номер {bib} уже имеет стартовое время, пропущен");
                    continue;
                }

                JToken group = groups != null ? FGById(PPGroupId(person), groups) : null;
                if (group == null)
                {
                    log.AppendLine($"День {dayNumber}: номер {bib} - не найдена группа участника, пропущен");
                    continue;
                }

                int corridor = PGStartCorridor(group);
                if (corridor <= 0)
                {
                    log.AppendLine($"День {dayNumber}: у группы \"{PGName(group)}\" не задан стартовый коридор, номер {bib} пропущен");
                    continue;
                }

                InsertPerson(race, person, group, corridor, dayNumber, bib, log);
            }

            return log.ToString();
        }

        private static void InsertPerson(JToken race, JToken person, JToken group, int corridor, int dayNumber, int bib, StringBuilder log)
        {
            JArray allPersons = PBPersons(race);
            JArray groups = PBGroups(race);
            string groupId = PGId(group);

            Dictionary<string, int> corridorByGroupId = new();
            foreach (JToken g in groups)
            {
                string gId = PGId(g);
                if (!corridorByGroupId.ContainsKey(gId))
                    corridorByGroupId[gId] = PGStartCorridor(g);
            }

            List<JToken> groupStarted = FPAllByGroup(groupId, allPersons)
                .Where(p => PPStartTime(p) != 0)
                .OrderBy(p => PPStartTime(p))
                .ToList();

            if (groupStarted.Count == 0)
            {
                log.AppendLine($"День {dayNumber}: у группы \"{PGName(group)}\" нет ни одного участника со стартовым временем - распределение невозможно, номер {bib} пропущен");
                return;
            }

            TimeSpan interval;
            if (groupStarted.Count == 1)
            {
                interval = TimeSpan.FromMinutes(1);
                log.AppendLine($"День {dayNumber}: у группы \"{PGName(group)}\" только один участник со временем, интервал взят по умолчанию (1 минута)");
            }
            else
            {
                interval = MostCommonInterval(groupStarted);
            }

            int insertPosition = Random.Shared.Next(groupStarted.Count + 1);
            double targetTimeMs = insertPosition < groupStarted.Count
                ? PPStartTime(groupStarted[insertPosition])
                : PPStartTime(groupStarted[^1]) + interval.TotalMilliseconds;

            List<JToken> toShift = allPersons
                .Where(p => PPStartTime(p) != 0
                         && PPStartTime(p) >= targetTimeMs
                         && corridorByGroupId.TryGetValue(PPGroupId(p), out int pCorridor) && pCorridor == corridor)
                .ToList();

            foreach (JToken p in toShift)
                p["start_time"] = PPStartTime(p) + interval.TotalMilliseconds;

            person["start_time"] = targetTimeMs;

            TimeSpan targetTimeSpan = StartTimeToTimeSpan((int)targetTimeMs);
            log.AppendLine($"День {dayNumber}: номер {bib} ({PPSurnameName(person)}) поставлен на {StartTimeToString(targetTimeSpan)}, интервал группы {interval}, сдвинуто следом {toShift.Count} участников коридора {corridor}");
        }

        private static TimeSpan MostCommonInterval(List<JToken> sortedStarted)
        {
            Dictionary<double, int> counts = new();
            for (int i = 1; i < sortedStarted.Count; i++)
            {
                double diff = PPStartTime(sortedStarted[i]) - PPStartTime(sortedStarted[i - 1]);
                if (diff < 0)
                    continue;
                counts[diff] = counts.TryGetValue(diff, out int c) ? c + 1 : 1;
            }

            if (counts.Count == 0)
                return TimeSpan.FromMinutes(1);

            int maxCount = counts.Values.Max();
            double bestDiff = counts.Where(kv => kv.Value == maxCount).Select(kv => kv.Key).Min();
            return TimeSpan.FromMilliseconds(bestDiff);
        }
    }
}
