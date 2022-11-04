using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

using SportOrgMultyDay.Data;
using static SportOrgMultyDay.Processing.Parsing.ParseBase;
using static SportOrgMultyDay.Processing.Parsing.ParsePerson;
using static SportOrgMultyDay.Processing.Parsing.ParseGroup;
using static SportOrgMultyDay.Processing.Parsing.ParseOrganization;
using static SportOrgMultyDay.Processing.Parsing.ParseData;
using static SportOrgMultyDay.Processing.Parsing.ParseResult;
using static SportOrgMultyDay.Processing.Logger;

namespace SportOrgMultyDay.Processing
{

    internal enum StartLogType { None, SFR, SportIdent }

    public class StartLogProcessing
    {
        public int StartedPersons { get; private set; }
        public string Duplicates { get; private set; }
        public string ChecklessFinished { get; private set; }
        public string DNS { get; private set; }

        JToken jBase;
        readonly string StartLog;
        string Log;
        public StartLogProcessing(JToken jbase, string startLog)
        {
            StartLog = startLog;
            jBase = jbase;
            Process();
        }

        private void Process()
        {
            List<int> allBibs = GetPersonsBibs(jBase, ref Log);
            List<StartCell> startCells = StartLogParse(jBase,StartLog, ref Log);
            List<int> startedBibsNoDupl = StartCellsToInt(startCells).Distinct().ToList();
            List<int> dnsIncludeFinished = GetDNSList(allBibs, startedBibsNoDupl);
            List<int> finished = GetFinished(jBase, ref Log);
            List<int> checklessFinished = dnsIncludeFinished.Intersect(finished).ToList();
            List<int> dns = dnsIncludeFinished.Except(finished).ToList();
            StartedPersons = startedBibsNoDupl.Count;
            ChecklessFinished = string.Join("\n", checklessFinished.OrderBy(x => x));
            Duplicates = string.Join("\n", SearchDuplicates(startCells, ref Log));
            DNS = string.Join("\n", dns.OrderBy(x => x));
        }

        private static List<int> GetDNSList(List<int> all, List<int> started)
        {
            return all.Except(started).ToList();
        }

        private static List<StartCell> SearchDuplicates(List<StartCell> startCells, ref string log)
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

        private static List<StartCell> StartLogParse(JToken jbase, string startLog, ref string log)
        {
            try
            {
                log += $" Парсинг стартовых логов\n";
                StartLogType logType = GetStartLogType(startLog);
                string[] logLines = startLog.Split("\n", StringSplitOptions.RemoveEmptyEntries);
                switch (logType)
                {
                    case StartLogType.SportIdent:
                        return StartLogParseSportIdent(jbase, logLines, ref log);
                    case StartLogType.SFR:
                        return StartLogParseSFR(logLines, ref log);
                    case StartLogType.None:
                        log += "  Тип лога не определён, процесс прерван!\n";
                        return new();
                }

            }
            catch (Exception ex)
            {
                LogError("ggfcf52qw", ex);
                log += "\nERROR StartLogParse() вызвало ошибку\n";
            }

            return new List<StartCell>();
        }

        private static List<StartCell> StartLogParseSFR(string[] logLines, ref string log)
        {
            List<StartCell> startCells = new();
            try
            {
                log += $" Тип лога SFR smart terminal\n";
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
                        LogError("9i32bhisadg", ex);
                    }
                }

            }
            catch (Exception ex)
            {
                LogError("38dhd9gasd", ex);
                log += "\nERROR StartLogParseSFR() вызвало ошибку\n";
            }
            return startCells;
        }
        private static List<StartCell> StartLogParseSportIdent(JToken jbase,string[] logLines, ref string log)
        {
            List<StartCell> startCells = new();
            try
            {
                log += $" Тип лога станция SportIdent\n";

                Dictionary<int, int> siidBib = new();
                JArray persons = PBPersonsFromBase(jbase);
                foreach (JToken person in persons)
                {
                    int bib = PPBib(person);
                    int siid = PPCardNumber(person);
                    if (bib == 0 || siid == 0 || bib == -1 || siid == -1) continue;
                    siidBib.Add(siid, bib);
                }

                for (int i = 1; i < logLines.Length; i++)
                {
                    try
                    {
                        string[] values = logLines[i].Split(";");
                        int siid = Convert.ToInt32(values[2]);
                        if (siidBib.TryGetValue(siid, out int bib))
                            startCells.Add(new(bib, values[1]));
                    }
                    catch (Exception ex)
                    {
                        log += $"  Ошибка парсинга строки:[{logLines[i]}]\n";
                        LogError("sg378ehdfa", ex);
                    }
                }

            }
            catch (Exception ex)
            {
                LogError("93jdhg31da4u", ex);
                log += "\nERROR StartLogParseSportIdent() вызвало ошибку\n";
            }
            return startCells;
        }

        private static StartLogType GetStartLogType(string startLog)
        {
            int siStart = startLog.IndexOf("No;Read on;SIID;");
            if (siStart == 0)
                return StartLogType.SportIdent;
            if (siStart == -1)
            {
                string firstStr = startLog[..startLog.IndexOf("\n")];
                string[] values = firstStr.Split("\t");
                if (int.TryParse(values[0], out _))
                    return StartLogType.SFR;
            }
            return StartLogType.None;
        }

        private static List<int> GetPersonsBibs(JToken jBase, ref string Log)
        {
            try
            {
                Log += $" Получение всех номеров из дня {CurrentRaceID(jBase)}\n";
                List<int> allPersonsBibs = new List<int>();
                JArray persons = PBPersonsFromBase(jBase);
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
                JArray results = PBResultsFromBase(jBase);
                JArray persons = PBPersonsFromBase(jBase);
                List<int> bibs = new();
                foreach (JToken result in results)
                {
                    try
                    {
                        bibs.Add(PPBib(FPById(PRPersonId(result), persons)));
                    }
                    catch (Exception ex) { Log += $"  Ошибка обработки рузультата: {PRId(result)}\n"; }
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
                return string.Join(",", duplicates);
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
