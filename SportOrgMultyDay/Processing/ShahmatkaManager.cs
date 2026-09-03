using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using static SportOrgMultyDay.Processing.Parsing.ParseBase;
using static SportOrgMultyDay.Processing.Parsing.ParseGroup;
using static SportOrgMultyDay.Processing.Parsing.ParsePerson;
using static SportOrgMultyDay.Processing.Parsing.Things.ParseStartTime;

namespace SportOrgMultyDay.Processing
{
    public enum ShahmatkaDisplayMode
    {
        Group,
        Bib,
        Surname
    }

    public static class ShahmatkaManager
    {
        public const string TimeColumnName = "Время";

        public static DataTable BuildGrid(JToken race, ShahmatkaDisplayMode displayMode)
        {
            DataTable table = new();
            table.Columns.Add(TimeColumnName, typeof(string));

            JArray groups = PBGroups(race);
            JArray persons = PBPersons(race);
            if (groups == null || persons == null)
                return table;

            Dictionary<string, JToken> groupById = groups.ToDictionary(PGId, group => group);

            List<int> corridors = groups
                .Select(PGStartCorridor)
                .Where(corridor => corridor > 0)
                .Distinct()
                .OrderBy(corridor => corridor)
                .ToList();

            foreach (int corridor in corridors)
                table.Columns.Add(corridor.ToString());

            SortedDictionary<int, Dictionary<int, List<JToken>>> byTimeThenCorridor = new();

            foreach (JToken person in persons)
            {
                int startTime = PPStartTime(person);
                if (startTime <= 0)
                    continue;

                if (!groupById.TryGetValue(PPGroupId(person), out JToken group))
                    continue;

                int corridor = PGStartCorridor(group);
                if (corridor <= 0)
                    continue;

                if (!byTimeThenCorridor.TryGetValue(startTime, out Dictionary<int, List<JToken>> byCorridor))
                {
                    byCorridor = new Dictionary<int, List<JToken>>();
                    byTimeThenCorridor[startTime] = byCorridor;
                }

                if (!byCorridor.TryGetValue(corridor, out List<JToken> personsInCell))
                {
                    personsInCell = new List<JToken>();
                    byCorridor[corridor] = personsInCell;
                }

                personsInCell.Add(person);
            }

            foreach (KeyValuePair<int, Dictionary<int, List<JToken>>> timeRow in byTimeThenCorridor)
            {
                DataRow row = table.NewRow();
                row[TimeColumnName] = StartTimeToString(StartTimeToTimeSpan(timeRow.Key));
                foreach (int corridor in corridors)
                {
                    if (timeRow.Value.TryGetValue(corridor, out List<JToken> personsInCell))
                        row[corridor.ToString()] = CellText(personsInCell, groupById, displayMode);
                }
                table.Rows.Add(row);
            }

            return table;
        }

        private static string CellText(List<JToken> personsInCell, Dictionary<string, JToken> groupById, ShahmatkaDisplayMode displayMode)
        {
            switch (displayMode)
            {
                case ShahmatkaDisplayMode.Bib:
                    return string.Join(", ", personsInCell.Select(PPBib));
                case ShahmatkaDisplayMode.Surname:
                    return string.Join(", ", personsInCell.Select(PPSurname));
                default:
                    IEnumerable<string> groupNames = personsInCell
                        .Select(person => groupById.TryGetValue(PPGroupId(person), out JToken group) ? PGName(group) : "?")
                        .Distinct();
                    return string.Join(" / ", groupNames);
            }
        }
    }
}
