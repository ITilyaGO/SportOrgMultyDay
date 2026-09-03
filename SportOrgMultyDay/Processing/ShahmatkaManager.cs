using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using static SportOrgMultyDay.Processing.Parsing.ParseBase;
using static SportOrgMultyDay.Processing.Parsing.ParseGroup;
using static SportOrgMultyDay.Processing.Parsing.ParsePerson;
using static SportOrgMultyDay.Processing.Parsing.Things.ParseStartTime;

namespace SportOrgMultyDay.Processing
{
    public class ShahmatkaGrid
    {
        public DataTable Table { get; }
        public List<int> Corridors { get; }
        public List<TimeSpan> RowTimes { get; }
        public List<List<JToken>[]> RowsPersons { get; }

        public ShahmatkaGrid(DataTable table, List<int> corridors, List<TimeSpan> rowTimes, List<List<JToken>[]> rowsPersons)
        {
            Table = table;
            Corridors = corridors;
            RowTimes = rowTimes;
            RowsPersons = rowsPersons;
        }

        public List<JToken> PersonsAt(int rowIndex, int corridorColumnIndex)
        {
            if (rowIndex < 0 || rowIndex >= RowsPersons.Count)
                return null;
            List<JToken>[] row = RowsPersons[rowIndex];
            if (corridorColumnIndex < 0 || corridorColumnIndex >= row.Length)
                return null;
            return row[corridorColumnIndex];
        }
    }

    public static class ShahmatkaManager
    {
        public const string TimeColumnName = "Время";

        public static ShahmatkaGrid BuildGrid(JToken race, bool showBib, bool showGroup, bool showSurname)
        {
            if (!showBib && !showGroup && !showSurname)
                showGroup = true;

            DataTable table = new();
            table.Columns.Add(TimeColumnName, typeof(string));

            JArray groups = PBGroups(race);
            JArray persons = PBPersons(race);
            List<int> corridors = new();
            List<TimeSpan> rowTimes = new();
            List<List<JToken>[]> rowsPersons = new();

            if (groups == null || persons == null)
                return new ShahmatkaGrid(table, corridors, rowTimes, rowsPersons);

            Dictionary<string, JToken> groupById = groups.ToDictionary(PGId, group => group);

            corridors = groups
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
                TimeSpan rowTime = StartTimeToTimeSpan(timeRow.Key);
                DataRow row = table.NewRow();
                row[TimeColumnName] = StartTimeToString(rowTime);

                List<JToken>[] rowPersons = new List<JToken>[corridors.Count];
                for (int i = 0; i < corridors.Count; i++)
                {
                    int corridor = corridors[i];
                    if (timeRow.Value.TryGetValue(corridor, out List<JToken> personsInCell))
                    {
                        row[corridor.ToString()] = CellText(personsInCell, groupById, showBib, showGroup, showSurname);
                        rowPersons[i] = personsInCell;
                    }
                }
                table.Rows.Add(row);
                rowTimes.Add(rowTime);
                rowsPersons.Add(rowPersons);
            }

            return new ShahmatkaGrid(table, corridors, rowTimes, rowsPersons);
        }

        private static string CellText(List<JToken> personsInCell, Dictionary<string, JToken> groupById, bool showBib, bool showGroup, bool showSurname)
        {
            IEnumerable<string> labels = personsInCell
                .Select(person =>
                {
                    List<string> parts = new();
                    if (showBib)
                        parts.Add(PPBib(person).ToString());
                    if (showGroup)
                        parts.Add(groupById.TryGetValue(PPGroupId(person), out JToken group) ? PGName(group) : "?");
                    if (showSurname)
                        parts.Add(PPSurname(person));
                    return string.Join(" ", parts);
                })
                .Distinct();

            return string.Join(" / ", labels);
        }
    }
}
