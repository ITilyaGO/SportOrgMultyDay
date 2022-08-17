using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

using static SportOrgMultyDay.Processing.Parsing.ParseBase;
using static SportOrgMultyDay.Processing.Parsing.ParsePerson;
using static SportOrgMultyDay.Processing.Parsing.ParseGroup;
using static SportOrgMultyDay.Processing.Parsing.ParseOrganization;
using static SportOrgMultyDay.Processing.Parsing.ParseData;
using static SportOrgMultyDay.Processing.Parsing.ParseResult;
using static SportOrgMultyDay.Processing.Logger;

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
            List<int> allBibs = GetPersonsBibs(jBase, ref Log);
            List<StartCell> startCells = StartLogParse(StartLog, ref Log);
            List<int> startedBibsNoDupl = StartCellsToInt(startCells).Distinct().ToList();
            List<int> dnsIncludeFinished = GetDNSList(allBibs, startedBibsNoDupl);
            List<int> finished = GetFinished(jBase, ref Log);
            List<int> checklessFinished = dnsIncludeFinished.Intersect(finished).ToList();
            List<int> dns = dnsIncludeFinished.Except(finished).ToList();
            StartedPersons = startedBibsNoDupl.Count;
            ChecklessFinished = String.Join ("\n", checklessFinished.OrderBy(x => x));
            Duplicates = String.Join("\n",SearchDuplicates(startCells, ref Log));
            DNS = String.Join("\n", dns.OrderBy(x => x));

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

        private static List<int> GetPersonsBibs(JToken jBase,ref string Log)
        {
            try
            {
                Log += $" Получение всех номеров из дня {CurrentRaceID(jBase)}\n";
                List<int> allPersonsBibs = new List<int>();
                JArray persons = PersonsFromBase(jBase);
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
                string dupls = CheckDuplicates(allPersonsBibs);
                if (dupls != "") Log += $"  Повторяется номера: {dupls}\n";
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

        private static List<int> GetFinished(JToken jBase, ref string Log) 
        {
            try
            {
                Log += $" Получение всех номеров финишировавших из результатов\n";
                JArray results = ResultsFromBase(jBase);
                JArray persons = PersonsFromBase(jBase);
                List<int> bibs = new();
                foreach (JToken result in results)
                {
                    try
                    {
                        bibs.Add(PPBib(FPById(PRPersonId(result), persons)));
                    }
                    catch (Exception ex){ Log += $"  Ошибка обработки рузультата: {PRId(result)}\n"; }
                }
                string dupls = CheckDuplicates(bibs);
                if (dupls != "") Log += $"  Повторяется номера: {dupls}\n";
                return bibs.Distinct().ToList();
            }
            catch (Exception ex)
            {
                Log += "\nERROR AllFinished() вызвало ошибку\n";
                LogError("632ls9fh3", ex);
            }
            return new List<int>();
        }
        private static string CheckDuplicates(List<int> bibs)
        {
            IEnumerable<int> duplicates = bibs.GroupBy(x => x).Where(g => g.Count() > 1).Select(x => x.Key);
            if (duplicates.Any())
                return String.Join(",", duplicates);
            return "";
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
