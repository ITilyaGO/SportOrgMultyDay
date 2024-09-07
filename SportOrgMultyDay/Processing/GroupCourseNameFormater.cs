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
using static SportOrgMultyDay.Processing.Logger;
using static SportOrgMultyDay.Extensions.ListExtensions;
using static SportOrgMultyDay.Processing.Parsing.Things.ParseStartTime;
using System.Text.RegularExpressions;

namespace SportOrgMultyDay.Processing
{
    public static class GroupCourseNameFormater
    {
        public static string FormatAll(JToken jBase, bool combineCourses)
        {
            string log = "Форматирование назщваний групп и дистанций... \n";
            JToken race = PBCurrentRaceFromBase(jBase);
            FormatGroupNames(race, ref log);
            FormatCourseNames(race, ref log);
            if (combineCourses)
                CompactCourses(race, ref log);
            return log;
        }

        public static void FormatGroupNames(JToken race, ref string log)
        {
            log += $"  Форматирование имен групп...\n";
            JArray groups = PBGroups(race);

            for (int i = 0; i < groups.Count; i++)
            {
                JToken group = groups[i];
                string groupName = PGName(group);
                string formatedName = FormatName(groupName);
                group["name"] = formatedName;
                log += $"    [{groupName}] > [{formatedName}]\n";
            }
            log += "  Форматирование имен групп завершено\n\n";
        }
        public static void FormatCourseNames(JToken race, ref string log)
        {
            log += $"  Форматирование имен дистанций...\n";
            JArray courses = PBCourses(race);
            for (int i = 0; i < courses.Count; i++)
            {
                JToken course = courses[i];
                string courseName = PCName(course);
                string formatedName = FormatName(courseName);
                course["name"] = formatedName;
                log += $"    [{courseName}] > [{formatedName}]\n";
            }
            log += "  Форматирование имен дистанций завершено\n\n";
        }
        public static void CompactCourses(JToken race, ref string log)
        {
            log += $"  Объеденение дистанций...\n";
            JArray courses = PBCourses(race);
            List<JToken> toRemove = new List<JToken>();
            for (int i = 0; i < courses.Count - 1; i++)
            {
                JToken course = courses[i];
                string courseName = PCName(course);
                JArray controls = PCControls(course);
                List<string> findedNames = new();
                if (toRemove.Contains(course))
                    continue;
                log += $"    Ищем дубликаты дистанции {courseName}. Найдено - ";
                for (int j = i + 1; j < courses.Count; j++)
                {
                    JToken courseJ = courses[j];
                    string courseJName = PCName(courseJ);
                    JArray controlsJ = PCControls(courseJ);
                    if (!toRemove.Contains(courseJ) && JArray.DeepEquals(controls, controlsJ))
                    {
                        findedNames.Add(courseJName);
                        toRemove.Add(courseJ);
                        log += $"[{courseJName}], ";
                    }
                }
                string resultCourseName = courseName;
                foreach (string name in findedNames)
                    resultCourseName += $" {name}";
                course["name"] = resultCourseName;
                log += $". Переиминование [{courseName}] > [{resultCourseName}]\n";
            }
            log += $"    Удаление - ";
            foreach (JToken course in toRemove)
            {
                log += $"[{PCName(course)}], ";
                courses.Remove(course);
            }
            log += "Успешно!\n";
        }

        private static string FormatName(string name)
        {
            return Regex.Replace(name, @"[^\p{L}\p{N}]", "");
        }


    }
}
