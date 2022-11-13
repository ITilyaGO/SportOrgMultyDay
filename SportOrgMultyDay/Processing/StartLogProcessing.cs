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

    

    public class StartLogProcessing
    {
        public int StartedPersons { get; private set; }
        public string Duplicates { get; private set; }
        public string ChecklessFinished { get; private set; }
        public string DNS { get; private set; }

        JToken jBase;
        readonly string startLog;
        string log;
        EStartLogType startLogType;
        public StartLogProcessing(JToken jBase, string startLog, EStartLogType type,string outFieldsSplitter)
        {
            this.startLog = startLog;
            this.jBase = jBase;
            startLogType = type;
            ChecklessFinished = string.Empty;
            Duplicates = string.Empty;
            DNS = string.Empty;
            Process(outFieldsSplitter);
        }

        private void Process(string splitter)
        {
            try
            {
                log += "Запущена обработка стартовых логов...\n";
                if (jBase is null)
                {
                    log += "    База не найдена, процесс прерван!";
                    return;
                }
                List<int> allBibs = GetPersonsBibs();
                List<StartCell> startCells = StartLogParse();
                if (startCells is null) return;
                List<int> startedBibsNoDupl = StartCellsToInt(startCells).Distinct().ToList();
                List<int> dnsIncludeFinished = GetDNSList(allBibs, startedBibsNoDupl);
                List<int> finished = GetFinished(jBase, ref log);
                List<int> checklessFinished = dnsIncludeFinished.Intersect(finished).ToList();
                List<int> dns = dnsIncludeFinished.Except(finished).ToList();
                StartedPersons = startedBibsNoDupl.Count;
                ChecklessFinished = string.Join(splitter, checklessFinished.OrderBy(x => x));
                Duplicates = string.Join('\n', SearchDuplicates(startCells, ref log));
                DNS = string.Join(splitter, dns.OrderBy(x => x));
            }
            catch (Exception ex)
            {
                LogError("dk032vhuid", ex);
                log += "\nERROR Process() вызвало ошибку\n";
            }
            
        }

        private static List<int> GetDNSList(List<int> all, List<int> started)
        {
            return all.Except(started).ToList();
        }

        private static List<StartCell> SearchDuplicates(List<StartCell> startCells, ref string log)
        {
            try
            {
                log += $"  Поиск дубликатов в стартовом логе...\n";
                //List<StartCell> withOutDubls = startCells.Distinct().ToList();
                List<int> ints = StartCellsToInt(startCells);
                List<StartCell> duplicates = new();

                IEnumerable<int> duplicatesInt = ints.GroupBy(x => x).Where(g => g.Count() > 1).Select(x => x.Key);

                foreach (StartCell startCell in startCells)
                    if (duplicatesInt.Contains(startCell.Bib))
                    {
                        log += $"    Дубликат {startCell}\n";
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

        private List<StartCell> StartLogParse()
        {
            try
            {
                log += $"  Парсинг стартовых логов...\n";
                EStartLogType logType = startLogType == EStartLogType.Auto ? GetStartLogType(startLog) : startLogType;
                string[] logLines = startLog.Split("\n", StringSplitOptions.RemoveEmptyEntries);
                switch (logType)
                {
                    case EStartLogType.SFR:
                        return StartLogParseSFR(logLines, ref log);
                    case EStartLogType.Sportident:
                        return StartLogParseSportIdent(jBase, logLines, ref log);
                    case EStartLogType.Sportiduino:
                        return StartLogParseSportiduino(jBase,logLines, ref log);
                        
                    default:
                        log += "    Тип лога не определён, процесс прерван!\n";
                        return null;
                }

            }
            catch (Exception ex)
            {
                LogError("ggfcf52qw", ex);
                log += "\nERROR StartLogParse() вызвало ошибку\n";
            }

            return new List<StartCell>();
        }

        private static Dictionary<int, int> GetSiidBibDictionary(JToken jbase)
        {
            Dictionary<int, int> siidBib = new();
            try
            {
                JArray persons = PBPersonsFromBase(jbase);
                foreach (JToken person in persons)
                {
                    int bib = PPBib(person);
                    int siid = PPCardNumber(person);
                    if (bib == 0 || siid == 0 || bib == -1 || siid == -1) continue;
                    siidBib.Add(siid, bib);
                }
                return siidBib;
            }
            catch (Exception ex)
            {
                LogError("9wbvskbjcq", ex);
            }
            return siidBib;
        }

        private static List<StartCell> StartLogParseSFR(string[] logLines, ref string log)
        {
            List<StartCell> startCells = new();
            try
            {
                log += $"  Тип лога SFR smart terminal\n";
                foreach (string line in logLines)
                {
                    try
                    {
                        string[] values = line.Split("\t");
                        if (values.Length > 1 && int.TryParse(values[0], out int bib))
                            startCells.Add(new(bib, values[1]));
                        else
                            log += $"    Неудалось распознать строку:[{line}]\n";
                    }
                    catch (Exception ex)
                    {
                        log += $"    Ошибка при парсинге строки:[{line}]\n";
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
                log += $"  Тип лога станция Sportident\n";

                Dictionary<int, int> siidBib = GetSiidBibDictionary(jbase);

                for (int i = 1; i < logLines.Length; i++)
                {
                    try
                    {
                        string[] values = logLines[i].Split(";");
                        if (values.Length > 2 && int.TryParse(values[2], out int siid))
                            if (siidBib.TryGetValue(siid, out int bib))
                                startCells.Add(new(bib, values[1]));
                            else
                                log += $"    Участник с таким чипом не найден:[{siid}]\n";
                        else
                            log += $"    Неудалось распознать строку:[{logLines[i]}]\n";
                    }
                    catch (Exception ex)
                    {
                        log += $"    Ошибка парсинга строки:[{logLines[i]}]\n";
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


        private static List<StartCell> StartLogParseSportiduino(JToken jbase, string[] logLines, ref string log)
        {
            List<StartCell> startCells = new();
            try
            {
                log += $"  Тип лога станция Sportiduino\n";
                Dictionary<int, int> siidBib = GetSiidBibDictionary(jbase);
                foreach (string line in logLines)
                {
                    try
                    {
                        string[] values = line.Split(" ", StringSplitOptions.RemoveEmptyEntries);
                        if (values.Length > 2 && int.TryParse(values[0], out int siid))
                            if (siidBib.TryGetValue(siid, out int bib))
                                startCells.Add(new(bib, $"{values[1]} {values[2]}"));
                            else
                                log += $"    Участник с таким чипом не найден:[{siid}]\n";
                        else
                            log += $"    Неудалось распознать строку:[{line.Replace("\r", "")}]\n";
                    }
                    catch (Exception ex)
                    {
                        log += $"    Ошибка при парсинге строки:[{line}]\n";
                        LogError("39jksadih3", ex);
                    }
                }
            }
            catch (Exception ex)
            {
                LogError("saj3igsdaq1", ex);
                log += "\nERROR StartLogParseSportiduino() вызвало ошибку\n";
            }
            return startCells;
        }


        private static EStartLogType GetStartLogType(string startLog)
        {
            int siStart = startLog.IndexOf("No;Read on;SIID;");
            if (siStart == 0)
                return EStartLogType.Sportident;
            int ns = startLog.IndexOf('\n');
            if (ns <= -1) return EStartLogType.None;
            string firstStr = startLog[..ns];
            if (firstStr.Contains('\t'))
            {
                string[] values = firstStr.Split('\t');
                if (int.TryParse(values[0], out _))
                    return EStartLogType.SFR;
            }
            else if (firstStr.Contains(' '))
            {
                string[] values = firstStr.Split(' ',StringSplitOptions.RemoveEmptyEntries);
                if (int.TryParse(values[0], out _))
                    return EStartLogType.Sportiduino;
            }
            return EStartLogType.None;
        }

        private List<int> GetPersonsBibs()
        {
            try
            {
                log += $"  Получение всех номеров из дня {CurrentRaceID(jBase)+1}...\n";
                List<int> allPersonsBibs = new List<int>();
                JArray persons = PBPersonsFromBase(jBase);
                foreach (JToken person in persons)
                {
                    int bib = PPBib(person);
                    if (bib == -1)
                    {
                        log += $"   Номер отсутствует: {PersonToString.Name(person)}\n";
                        continue;
                    }
                    allPersonsBibs.Add(bib);
                }
                string dupls = CheckDuplicates(allPersonsBibs);
                if (dupls != "") log += $"    Повторяется номера: {dupls}\n";
                log += $"  Получено {allPersonsBibs.Count} номеров\n";
                return allPersonsBibs;

            }
            catch (Exception ex)
            {
                log += "\nERROR AllPersonsBibs() вызвало ошибку\n";
                LogError("h0d9nbtfs", ex);
            }
            return new List<int>();
        }

        private static List<int> GetFinished(JToken jBase, ref string Log)
        {
            try
            {
                Log += $"  Получение всех номеров финишировавших из результатов...\n";
                JArray results = PBResultsFromBase(jBase);
                JArray persons = PBPersonsFromBase(jBase);
                List<int> bibs = new();
                foreach (JToken result in results)
                {
                    try
                    {
                        bibs.Add(PPBib(FPById(PRPersonId(result), persons)));
                    }
                    catch (Exception ex) { Log += $"    Ошибка обработки рузультата: {PRId(result)}\n"; }
                }
                string dupls = CheckDuplicates(bibs);
                if (dupls != "") Log += $"    Повторяется номера: {dupls}\n";
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
            string log = this.log;
            this.log = "";
            return log;
        }
    }
}
