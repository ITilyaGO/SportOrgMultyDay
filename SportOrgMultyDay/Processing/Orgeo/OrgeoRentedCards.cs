using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SportOrgMultyDay.Processing.Parsing.ParseBase;
using static SportOrgMultyDay.Processing.Parsing.ParseOrganization;
using static SportOrgMultyDay.Processing.Parsing.ParsePerson;
namespace SportOrgMultyDay.Processing.Orgeo
{
    public class OrgeoRentedCards
    {
        static string FindingPhrase = "Аренда чипа: Да";
        static string FindingPhraseNo = "Аренда чипа: Нет";
        public static string FromComment(JToken race)
        {
            string log = "Обработка арендованных карт из комментариев...\n";
            JArray persons = PBPersons(race);
            for (int i = 0; i < persons.Count; i++)
            {
                JToken person = persons[i];
                string comment = PPComment(person);
                if (String.IsNullOrEmpty(comment)) continue;
                if (comment.Contains(FindingPhrase))
                {
                    person["is_rented_card"] = true;
                    log += $"  Участник {PPToString(person)} - Аренда чипа.\n";
                    int fpIndex = comment.IndexOf(FindingPhrase);
                    if (fpIndex >= 2 && comment.Substring(fpIndex - 2, 2) == ", ")
                    {
                        comment = comment.Remove(fpIndex - 2, 2);
                    }
                    person["comment"] = comment.Replace(FindingPhrase, "").Trim();
                }
                else if (comment.Contains(FindingPhraseNo))
                {
                    int fpIndex = comment.IndexOf(FindingPhraseNo);
                    if (fpIndex >= 2 && comment.Substring(fpIndex - 2, 2) == ", ")
                    {
                        comment = comment.Remove(fpIndex - 2, 2);
                    }
                    person["comment"] = comment.Replace(FindingPhraseNo, "").Trim();
                }
            }
            log += "Завершено";

            return log;
        }
    }
}
