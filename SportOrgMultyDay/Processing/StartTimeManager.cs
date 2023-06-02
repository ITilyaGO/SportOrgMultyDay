using Newtonsoft.Json.Linq;
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
using static SportOrgMultyDay.Processing.Logger;
using static SportOrgMultyDay.Extensions.ListExtensions;
using static SportOrgMultyDay.Processing.Parsing.Things.ParseStartTime;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskBand;

namespace SportOrgMultyDay.Processing
{
    public static class StartTimeManager
    {
        public static string AutoGroupOrder(JToken race)
        {
            JArray groups = PBGroups(race);
            List<string> corridorsStr = new();
            Dictionary<int, List<JToken>> corridorGroups = ToDictCorridorGroups(groups, out string log);
            List<int> allCorridors = corridorGroups.Keys.ToList();
            foreach (int corridorId in allCorridors)
            {
                List<string> groupNames = new();
                List<JToken> corGroups = corridorGroups[corridorId];
                corGroups.ForEach(group => { groupNames.Add(PGName(group)); });
                string outStr = string.Join('/', groupNames);
                corridorsStr.Add(outStr);
            }
            return string.Join('\n', corridorsStr);
        }

        public static string SetStartTimes(JToken race, TimeSpan timeOfStart, TimeSpan startInterval, bool shuffle)
        {
            string msgLog = "Установка стартовых минут...\n";
            msgLog += $"  Начало старта: {StartTimeToString(timeOfStart)}\n";
            try
            {
                JArray groups = PBGroups(race);
                JArray persons = PBPersons(race);

                Dictionary<int, List<JToken>> corridorGroups = ToDictCorridorGroups(groups, out string log);
                msgLog += log;

                List<int> allCorridors = corridorGroups.Keys.ToList();
                msgLog += $"  Установка...\n";
                foreach (int corridorId in allCorridors)
                {
                    msgLog += $"    Коридор {corridorId}...\n";
                    List<JToken> corGroups = corridorGroups[corridorId];
                    int groupCount = corGroups.Count;
                    //TimeSpan startTime = timeOfStart;


                    for (int i = 0; i < groupCount; i++)
                    {
                        JToken group = corGroups[i];
                        msgLog += $"      Группа: {PGName(group)}...\n";

                        int relayLegs = PGRelayLegs(group);

                        List<JToken> personsInGroup = FPAllByGroup(PGId(group), persons);
                        if (personsInGroup == null) return msgLog;

                        if (shuffle)
                            personsInGroup.Shuffle();
                        for (int j = 0; j < personsInGroup.Count; j++)
                        {
                            JToken person = personsInGroup[j];
                            person["start_time"] = timeOfStart.Add(TimeSpan.FromMinutes(j * groupCount + i) * startInterval.Minutes).TotalMilliseconds;
                            msgLog += $"        - Старт: {StartTimeToString(PPStartTime(person))} Участник {j + 1}: {PPToString(person)}\n";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                msgLog += "\nERROR SetStartTimes() вызвало ошибку\n";
                LogError("7sdf34hbsad", ex);
            }

            return msgLog;
        }

        public static string OrderedSetStartTimes(JToken race, TimeSpan timeOfStart, TimeSpan startInterval, string rawOrder, bool shuffle)
        {
            string msgLog = "Установка стартовых минут по заданному порядку...\n";
            JArray groups = PBGroups(race);
            JArray persons = PBPersons(race);
            List<Corridor> corridors = ParseRawOrder(persons, groups, rawOrder, ref msgLog, shuffle);
            
            foreach (Corridor corridor in corridors)
            {
                int corridorColumnCount = corridor.CorridorColumns.Count;
                for (int i = 0; i < corridor.CorridorColumns.Count; i++)
                {
                    CorridorColumn corridorColumn = corridor.CorridorColumns[i];
                    for (int j = 0; j < corridorColumn.Persons.Count; j++)
                    {
                        JToken person = corridorColumn.Persons[j];
                        person["start_time"] = timeOfStart.Add(TimeSpan.FromMinutes(j * corridorColumnCount + i) * startInterval.Minutes).TotalMilliseconds;
                        msgLog += $"        - Старт: {StartTimeToString(PPStartTime(person))} Участник {j + 1}: {PPToString(person)}\n";
                    }
                }
            }
            return msgLog;
        }

        internal static List<Corridor> ParseRawOrder(JArray persons, JArray groups, string rawOrder, ref string log, bool shuffle)
        {
            log += "  Парсинг порядка групп...\n";
            Dictionary<string, JToken> groupsByName = DictGNameGroup(groups);

            List<Corridor> corridors = new();
            string[] rowsOfRawOrder = rawOrder.Split('\n', StringSplitOptions.RemoveEmptyEntries);
            foreach (string row in rowsOfRawOrder)
            {
                Corridor corridor = new();
                string[] columns = row.Split(new char[] { '/', ';' });
                foreach (string column in columns)
                {
                    CorridorColumn corridorColumn = new();
                    string[] groupNames = column.Split(new char[] { ',', ' ' });
                    foreach (string groupName in groupNames)
                    {
                        if (groupsByName.TryGetValue(groupName, out JToken group))
                        {
                            List<JToken> personsInGroup = FPAllByGroup(PGId(group), persons);
                            if (shuffle)
                                personsInGroup.Shuffle();
                            corridorColumn.AddRange(personsInGroup);
                        }
                        else
                            log += $"    Группа не найдена.  Строка [{row}] Колонка [{column}] Группа [{groupName}]\n";
                    }
                    corridor.Add(corridorColumn);
                }
                corridors.Add(corridor);
            }
            log += "    Парсинг порядка групп завершен\n";

            return corridors;
        }

        internal static Dictionary<int, List<JToken>> ToDictCorridorGroups(JArray groups, out string log)
        {
            log = "  Поиск коридоров групп...\n";
            Dictionary<int, List<JToken>> CorridorGroups = new();
            foreach (JToken group in groups)
            {
                int startCorridor = PGStartCorridor(group);
                if (!CorridorGroups.ContainsKey(startCorridor))
                    CorridorGroups[startCorridor] = new List<JToken>();
                CorridorGroups[startCorridor].Add(group);
                log += $"    Группа: {PGName(group)} Коридор: {startCorridor}\n";
            }
            log += $"    Все группы найдены\n";
            return CorridorGroups;
        }

        //public static void Shuffle<T>(List<T> list)
        //{
        //    Random random = new Random();

        //    int n = list.Count;
        //    while (n > 1)
        //    {
        //        n--;
        //        int k = random.Next(n + 1);
        //        T value = list[k];
        //        list[k] = list[n];
        //        list[n] = value;
        //    }
        //}
    }

    class Corridor
    { 
        public List<CorridorColumn> CorridorColumns { get; private set; }
        public Corridor()
        {
            CorridorColumns = new List<CorridorColumn>();
        }

        public void Add(CorridorColumn column)
        {
            CorridorColumns.Add(column);
        }
    }

    class CorridorColumn 
    { 
        public List<JToken> Persons { get; private set; }
        public CorridorColumn()
        {
            Persons = new List<JToken>();
        }

        public void Add(JToken person)
        {
            Persons.Add(person);
        }

        public void AddRange(List<JToken> persons)
        {
            Persons.AddRange(persons);
        }
    }
}
