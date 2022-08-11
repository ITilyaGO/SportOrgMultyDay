using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static SportOrgMultyDay.Processing.Parsing.ParseBase;
using static SportOrgMultyDay.Processing.Parsing.ParsePerson;
using static SportOrgMultyDay.Processing.Parsing.ParseGroup;
using static SportOrgMultyDay.Processing.Parsing.ParseOrganization;
using static SportOrgMultyDay.Processing.Parsing.ParseData;
using static SportOrgMultyDay.Processing.Logger;
using Newtonsoft.Json.Linq;
using System.Xml.Linq;
using Microsoft.VisualBasic.Logging;

namespace SportOrgMultyDay.Processing.SFRSmartTerminal
{
    class StartCell
    {
        public int Bib { get; set; }
        public string Time { get; set; }
        public StartCell(int bib,string time)
        {
            Bib = bib;
            Time = time;
        }
        public override string ToString()
        {
            return $"{Bib} {Time}";
        }
        public override bool Equals(object? obj)
        {
            if (obj is StartCell person) return Bib == person.Bib;
            return false;
        }
        public override int GetHashCode() => Bib.GetHashCode();
    }

    public class StartLogProcessing
    {
        public int StartedPersons { get; private set; }
        public string Duplicates { get; private set; }
        public string ChecklessFinished { get; private set; }
        public string DNS { get; private set; }

        JToken jBase;
        string StartLog;
        string Log;
        public StartLogProcessing(JToken jbase,string startLog)
        { 
            StartLog = startLog;
            jBase = jbase;
            Process();
        }

        private void Process()
        {
            List<int> allBibs = AllPersonsBibs(jBase, ref Log);
            List<StartCell> startCells = StartLogParse(StartLog, ref Log);
            List<int> StartedBibs = StartCellsToInt(startCells).Distinct().ToList();
            Duplicates = String.Join("\n",SearchDuplicates(startCells, ref Log));
            List<int> dns = GetDNSList(allBibs, StartedBibs);
            DNS = String.Join("\n", dns);

        }

        private static List<int> GetDNSList(List<int> all, List<int> started )
        {
            return all.Except(started).ToList();
        }

        private static List<StartCell> SearchDuplicates(List<StartCell> startCells,ref string log)
        {
            try
            {
                log += $" Поиск дубликатов в стартовом логе\n";
                //List<StartCell> withOutDubls = startCells.Distinct().ToList();
                List<int> ints = StartCellsToInt(startCells);
                List<StartCell> duplicates = new();

                IEnumerable<int> duplicatesInt = ints.GroupBy(x => x).Where(g => g.Count() > 1).Select(x => x.Key);

                foreach (StartCell startCell in startCells)
                    if (duplicatesInt.Contains(startCell.Bib))
                    {
                        log += $"   Дубликат {startCell}\n";
                        duplicates.Add(startCell);
                    }

                return duplicates.OrderBy(x => x.Bib).ToList();
            }
            catch (Exception ex)
            {
                LogError("fy39dj74gf", ex);
                log += "\nERROR SearchDuplicates() вызвало ошибку\n";
            }
            return new List<StartCell>();
        }

        private static List<StartCell> StartLogParse(string startLog,ref string log)
        {
            try
            {
                log += $" Парсинг стартовых логов\n";
                List<StartCell> startCells = new();
                string[] logLines = startLog.Split("\n", StringSplitOptions.RemoveEmptyEntries);
                foreach (string line in logLines)
                {
                    try
                    {
                        string[] values = line.Split("\t");
                        int bib = Convert.ToInt32(values[0]);
                        string time = values[1];
                        startCells.Add(new(bib, time));
                    }
                    catch (Exception ex)
                    {
                        log += $"  Ошибка парсинга строки:[{line}]\n";
                        LogError("fueg6o7osd", ex);
                    }
                }
                return startCells;

            }
            catch (Exception ex)
            {
                LogError("ggfcf52qw", ex);
                log += "\nERROR StartLogParse() вызвало ошибку\n";
            }
           
            return new List<StartCell>();
        }

        private static List<int> AllPersonsBibs(JToken jBase,ref string Log)
        {
            try
            {
                Log += $" Получение всех номеров из дня {CurrentRaceID(jBase)}\n";
                List<int> allPersonsBibs = new List<int>();
                JArray persons = PersonsCurRace(jBase);
                foreach (JToken person in persons)
                {
                    int bib = PPBib(person);
                    if (bib == -1)
                    {
                        Log += $"   Номер отсутствует: {PersonToString.Name(person)}\n";
                        continue;
                    }
                    allPersonsBibs.Add(bib);
                }
                IEnumerable<int> duplicates = allPersonsBibs.GroupBy(x => x).Where(g => g.Count() > 1).Select(x => x.Key);
                if (duplicates.Any())
                    foreach (int i in duplicates)
                        Log += $"  Повторяется номер {i}\n";

                Log += $" Получено {allPersonsBibs.Count} номеров\n";

                return allPersonsBibs;

            }
            catch (Exception ex)
            {
                Log += "\nERROR AllPersonsBibs() вызвало ошибку\n";
                LogError("h0d9nbtfs", ex);
            }
            return new List<int>();
        }

        private static List<int> StartCellsToInt(List<StartCell> startCells)
        {
            List<int> ints = new List<int>();
            foreach (StartCell startCell in startCells)
                ints.Add(startCell.Bib);
            return ints;
        }

        public string GetLog()
        {
            string log = Log;
            Log = "";
            return log;
        }
        



    }
}
