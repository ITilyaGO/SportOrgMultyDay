using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

using static SportOrgMultyDay.Processing.Parsing.ParseBase;
using static SportOrgMultyDay.Processing.Parsing.ParsePerson;
using static SportOrgMultyDay.Processing.Parsing.ParseResult;
using static SportOrgMultyDay.Processing.Parsing.Things.ParseStartTime;

namespace SportOrgMultyDay.Processing
{
    // Сравнивает время старта участника (сначала смотрим на время старта, записанное в его
    // результате, если его там нет - берём стартовую минуту участника) со временем получения
    // чипа из стартового лога (строки вида "номер время дата"), чтобы найти отклонения от
    // обычного интервала между стартом и отметкой на старте (как правило, около 2-3 минут).
    public static class StartDeviationChecker
    {
        private class Entry
        {
            public string Description;
            public TimeSpan Deviation;
            public bool FromResult;
        }

        private static readonly Regex LineRegex = new(@"^\s*(\d+)\D+(\d{1,2}:\d{2}:\d{2})", RegexOptions.Compiled);

        public static string Check(JToken jBase, string logText)
        {
            StringBuilder log = new();
            if (jBase == null)
            {
                log.AppendLine("База не загружена.");
                return log.ToString();
            }

            JToken race = PBCurrentRaceFromBase(jBase);
            JArray persons = race != null ? PBPersons(race) : null;
            JArray results = race != null ? PBResults(race) : null;
            if (persons == null)
            {
                log.AppendLine("Не найдены участники текущего дня.");
                return log.ToString();
            }

            Dictionary<string, JToken> resultByPersonId = results != null
                ? DictRIdPerson(results, out _)
                : new Dictionary<string, JToken>();

            List<Entry> entries = new();
            string[] lines = (logText ?? "").Replace("\r", "").Split('\n');

            foreach (string rawLine in lines)
            {
                string line = rawLine.Trim();
                if (line.Length == 0)
                    continue;

                Match match = LineRegex.Match(line);
                if (!match.Success)
                {
                    log.AppendLine($"Не удалось разобрать строку лога \"{line}\"");
                    continue;
                }

                int bib = int.Parse(match.Groups[1].Value);
                if (!TimeSpan.TryParse(match.Groups[2].Value, out TimeSpan logTime))
                {
                    log.AppendLine($"Не удалось разобрать время из строки \"{line}\"");
                    continue;
                }

                JToken person = FPByBib(bib, persons);
                if (person == null)
                {
                    log.AppendLine($"Номер {bib} из лога не найден среди участников");
                    continue;
                }

                bool fromResult = resultByPersonId.TryGetValue(PPId(person), out JToken result) && PRStartTime(result) != 0;
                int startTimeMs = fromResult ? PRStartTime(result) : PPStartTime(person);

                if (startTimeMs == 0)
                {
                    log.AppendLine($"Номер {bib} ({PersonToString.Name(person)}) - нет стартового времени ни в результате, ни у участника");
                    continue;
                }

                TimeSpan startTime = StartTimeToTimeSpan(startTimeMs);
                entries.Add(new Entry
                {
                    Description = PersonToString.BibName(person),
                    Deviation = logTime - startTime,
                    FromResult = fromResult,
                });
            }

            log.AppendLine($"Обработано записей: {entries.Count}");
            foreach (Entry entry in entries.OrderBy(e => e.Deviation))
                log.AppendLine($"{entry.Description} - {FormatDeviation(entry.Deviation)}{(entry.FromResult ? " R" : "")}");

            return log.ToString();
        }

        private static string FormatDeviation(TimeSpan deviation)
        {
            string sign = deviation < TimeSpan.Zero ? "-" : "";
            return sign + deviation.Duration().ToString(@"hh\:mm\:ss");
        }
    }
}
