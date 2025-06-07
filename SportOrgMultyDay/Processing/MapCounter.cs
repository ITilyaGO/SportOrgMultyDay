using Newtonsoft.Json;
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
using static SportOrgMultyDay.Processing.Parsing.ParseCourse;
using static SportOrgMultyDay.Processing.Parsing.ParseData;
using static SportOrgMultyDay.Processing.Logger;
using static SportOrgMultyDay.Extensions.ListExtensions;
using static SportOrgMultyDay.Processing.Parsing.Things.ParseStartTime;
using System.Diagnostics;
using SportOrgMultyDay.Data.SportOrg;

namespace SportOrgMultyDay.Processing
{
    public static class MapCounter
    {
        public static string CalculateCurrentRaceCount(JToken jBase, bool onlyCurrentRace, bool calcReserv)
        {
            string log = "Подсчет карт текущего дня...\n";
            try
            {
                JToken race = PBCurrentRaceFromBase(jBase);
                int correntRaceId = CurrentRaceID(jBase);
                log += CalculateCount(race, correntRaceId, onlyCurrentRace, calcReserv);
            }
            catch (Exception ex) { log += $"  Ошибка\n{ex.Message}"; LogError("22gf32ef3efd", ex); }
            
            return log;
        }
        public static string CalculateAllRaceCount(JToken jBase, bool onlyCurrentRace, bool calcReserv)
        {
            string log = "Подсчет карт всех дней...\n";
            try
            {
                JArray races = PBRaces(jBase);
                for (int r = 0; r < races.Count; r++)  //Перечисляем Races для копирования настроек групп
                {
                    JToken race = races[r];
                    log += CalculateCount(race, r, onlyCurrentRace, calcReserv);
                }
            }
            catch (Exception ex) { log += $"  Ошибка\n{ex.Message}"; LogError("adstf324g3", ex); }
            return log;
        }
        public static string CalculateCount(JToken race, int raceId, bool onlyCurrentRace, bool calcReserv)
        {
            
            JToken data = PBData(race);
            string log = $"День {PDStartDatetime(data)}...\n";

            Dictionary<string, int> groupPersonCount = new Dictionary<string, int>();
            Dictionary<string, int> groupPersonCountReserv = new Dictionary<string, int>();
            Dictionary<string, int> coursePersonCount = new Dictionary<string, int>();
            Dictionary<string, int> coursePersonCountReserv = new Dictionary<string, int>();
            Dictionary<string, string> groupCourses = new Dictionary<string, string>();

            JArray persons = PBPersons(race);
            JArray courses = PBCourses(race);
            JArray groups = PBGroups(race);
            foreach (JToken person in persons)
            {
                bool isReserv = PPSurname(person) == "_Резерв";
                if (onlyCurrentRace && !RemoveExtraPersons.RunPersonInDay(person, raceId))
                    continue;
                if (isReserv)
                {
                    string personGroupIdr = PPGroupId(person);
                    if (groupPersonCountReserv.ContainsKey(personGroupIdr))
                        groupPersonCountReserv[personGroupIdr] += 1;
                    else groupPersonCountReserv[personGroupIdr] = 1;
                    if (!calcReserv)
                        continue;
                }


                string personGroupId = PPGroupId(person);
                if (groupPersonCount.ContainsKey(personGroupId))
                    groupPersonCount[personGroupId] += 1;
                else groupPersonCount[personGroupId] = 1;
            }

            foreach (JToken group in groups)
            {
                string groupId = PGId(group);
                string groupCouseId = PGCourseId(group);
                groupCourses[groupId] = groupCouseId;
            }

            foreach (var gp in groupPersonCount)
            {
                if (groupCourses.TryGetValue(gp.Key, out string courceId) && courceId != null)
                    if (coursePersonCount.ContainsKey(courceId))
                        coursePersonCount[courceId] += gp.Value;
                    else coursePersonCount[courceId] = gp.Value;
                else
                    Debug.WriteLine(gp.Key);
            }

            foreach (var gp in groupPersonCountReserv)
            {
                if (groupCourses.TryGetValue(gp.Key, out string courceId) && courceId != null)
                    if (coursePersonCountReserv.ContainsKey(courceId))
                        coursePersonCountReserv[courceId] += gp.Value;
                    else coursePersonCountReserv[courceId] = gp.Value;
                else
                    Debug.WriteLine(gp.Key);
            }

            log += $"  Группы:\n";
            int summG = 0;
            foreach (var gp in groupPersonCount)
            {
                JToken group = FGById(gp.Key, groups);
                if (group == null)
                {
                    log += $"    Группа не найдена [{gp.Key}]\n";
                    continue;
                }
                string groupName = PGName(group);
                groupPersonCountReserv.TryGetValue(gp.Key, out int val);
                log += $"      {groupName} - {gp.Value} r:{val} \n";
                summG += gp.Value;
            }
            log += $"  Всего карт: {summG}\n";

            log += $"  Дистанции:\n";
            int summ = 0;
            foreach (var cp in coursePersonCount)
            {
                JToken cource = FCById(cp.Key, courses);
                if (cource == null)
                {
                    log += $"    Дистанция не найдена [{cp.Key}]\n";
                    continue;
                }
                string courceName = PCName(cource);
                
                coursePersonCountReserv.TryGetValue(cp.Key, out int val);
                // TODO: ВЫнести переенные и UI 

               // int spare = (cp.Value / 10) + val;
                int spare = (cp.Value / 10);
                if (spare > 10)
                    spare = 10;
                if (spare < 5)
                    spare = 5;
                log += $"      {courceName} - {cp.Value + spare} r:{(calcReserv ? "" : "+")}{val} s:{spare} \n";
                summ += cp.Value + spare;
            }
            log += $"  Всего карт: {summ}\n";
            return log;
        }
    }
}
