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
            if (combineCourses)
            {
                FormatCourseNames(race, ref log);
                CompactCourses(race, ref log);
            }
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
                string formatedName = FormatName(groupName).Trim();

                // Предобработка начала строки (убрать пробелы/дефисы между буквой и цифрой)
                string normalizedStart = Regex.Replace(formatedName, @"^([МЖO])\s*[-]?\s*(\d+)", "$1$2");

                if (Regex.IsMatch(normalizedStart, @"^(М|Ж)\d{1,2}$") || Regex.IsMatch(normalizedStart, @"^O\d*$"))
                {
                    // Просто обрезаем по строчной или спецсимволу
                    int cutIndex = -1;
                    for (int j = 0; j < formatedName.Length; j++)
                    {
                        char ch = formatedName[j];
                        if (char.IsLower(ch) || ch == ' ' || ch == '-' || ch == '(')
                        {
                            cutIndex = j;
                            break;
                        }
                    }

                    if (cutIndex > 0)
                        formatedName = formatedName.Substring(0, cutIndex).Trim();
                    else
                        formatedName = normalizedStart;
                }
                else if (formatedName.StartsWith("Мужчины"))
                {
                    formatedName = "М21";
                }
                else if (formatedName.StartsWith("Женщины"))
                {
                    formatedName = "Ж21";
                }
                else if (formatedName.StartsWith("OPEN"))
                {
                    formatedName = Regex.Replace(formatedName, @"^OPEN(\d*)", "O$1");

                    int cutIndex = -1;
                    for (int j = 0; j < formatedName.Length; j++)
                    {
                        char ch = formatedName[j];
                        if (char.IsLower(ch) || ch == ' ' || ch == '-' || ch == '(')
                        {
                            cutIndex = j;
                            break;
                        }
                    }

                    if (cutIndex > 0)
                        formatedName = formatedName.Substring(0, cutIndex).Trim();
                }
                else
                {
                    int cutIndex = -1;
                    for (int j = 0; j < formatedName.Length; j++)
                    {
                        char ch = formatedName[j];
                        if (char.IsLower(ch) || ch == ' ' || ch == '-' || ch == '(')
                        {
                            cutIndex = j;
                            break;
                        }
                    }

                    if (cutIndex > 0)
                        formatedName = formatedName.Substring(0, cutIndex).Trim();
                }

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
                formatedName = formatedName.Replace("О", "O");
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
            return string.Join(" ", name
                .Split(',')
                .Select(part =>
                {
                    var beforeParen = part.Split('(')[0];
                    return Regex.Replace(beforeParen, @"[^\p{L}\p{N}]", "");
                }));
        }



    }
}
