using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SportOrgMultyDay.Processing.Parsing.ParseBase;
using static SportOrgMultyDay.Processing.Parsing.ParsePerson;
using static SportOrgMultyDay.Processing.Parsing.ParseGroup;
using static SportOrgMultyDay.Processing.Parsing.ParseOrganization;
namespace SportOrgMultyDay.Processing
{
    public static class RemovePersonDuplicates
    {
        public static string RemoveDuplicates(JToken race)
        {
            string log = "Удаление дубликатов участников...\n";

            JArray persons = PBPersons(race);
            if (persons == null) return log + "  Ошибка: Не удалось получить список участников\n";

            // Group by surname, name, and year
            IEnumerable<IGrouping<string, JToken>> groupedPersons = persons.GroupBy(p => $"{PPSurname(p)} {PPName(p)} {PPYear(p)} {PPGroupId(p)} {PPOrganizationId(p)} {PPQual(p)} {PPComment(p)}");

            // Remove duplicates
            int duplicatesCount = 0;
            foreach (var group in groupedPersons)
            {
                if (group.Count() > 1)
                {
                    foreach (var p in group)
                    {
                        log += $"    - {PPBib(p)} {PPSurname(p)} {PPName(p)} {PPYear(p)} {PPGroupId(p)} {PPOrganizationId(p)} {PPQual(p)} {PPComment(p)}\n";
                    }
                    List<JToken> duplicates = group.Skip(1).ToList();
                    foreach (JToken dupl in duplicates)
                    {
                        duplicatesCount++;
                        log += $"      - Удален дубликат: {PPBib(dupl)} {PPSurname(dupl)} {PPName(dupl)} {PPYear(dupl)} {PPGroupId(dupl)} {PPOrganizationId(dupl)} {PPQual(dupl)} {PPComment(dupl)}\n";
                        persons.Remove(dupl);
                    }
                }
            }
            log += $"  Удалено {duplicatesCount} дубликатов\n";
            return log;
        }
    }
}
