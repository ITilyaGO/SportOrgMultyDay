using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static SportOrgMultyDay.Processing.Parsing.ParseBase;
using static SportOrgMultyDay.Processing.Parsing.ParseCourse;
using static SportOrgMultyDay.Processing.Parsing.ParseGroup;


namespace SportOrgMultyDay.Processing
{
    public static class GroupCourseFinder
    {
        public static string Process(JToken race)
        {
            JArray jcourses = PBCourses(race);
            JArray groups = PBGroups(race);

            List<JToken> courses = jcourses.ToList();
            Dictionary<string, string> namesCoursesId = [];

            foreach (var course in courses)
            {
                string courseName = PCName(course);
                string courseId = PCId(course);
                string[] names = courseName.Split(' ');
                foreach (string name in names)
                    namesCoursesId[name] = courseId;
            }

            string log = ""; // Ensure 'log' is initialized
            foreach (JToken group in groups)
            {
                string groupId = PGId(group);
                string groupName = PGName(group);

                if (namesCoursesId.TryGetValue(groupName, out string courseId))
                {
                    group["course_id"] = courseId;
                    log += $"    OK - [{groupName}] -> [{PCName(courses.First(c => PCId(c) == courseId))}]\n";
                }
                else
                    log += $"    Не найдено для [{groupName}]\n";
            }

            return log; // Ensure 'log' is returned
        }
    }
}
