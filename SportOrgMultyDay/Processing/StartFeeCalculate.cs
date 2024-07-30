using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static SportOrgMultyDay.Processing.Parsing.ParseBase;
using static SportOrgMultyDay.Processing.Parsing.ParsePerson;
using static SportOrgMultyDay.Processing.Parsing.ParseGroup;
using static SportOrgMultyDay.Processing.Parsing.ParseData;
using static SportOrgMultyDay.Processing.Logger;
using Microsoft.VisualBasic;
namespace SportOrgMultyDay.Processing
{
    public class StartFeeCalculate
    {
        public static string GetStatistic(JToken Base, string cardString)
        {
            string msgLog = "Подсчёт стартовых взносов... \n";
            int cardSum = 0, cardCount = 0;
            int cashSum = 0, cashCount = 0;
            JArray persons = PBPersonsFromBase(Base);
            foreach (JToken person in persons)
            {
                string personNC = PersonToString.BibNameComment(person);
                string comment = PPComment(person);
                int numInComment = IntFromString(comment);
                if (numInComment == -1) continue;
                if (comment.Contains(cardString))
                {
                    cardSum += numInComment;
                    cardCount++;
                    msgLog += $"  {personNC} \tНа карту: {numInComment}\n";
                }
                else
                {
                    cashSum += numInComment;
                    cashCount++;
                    msgLog += $"  {personNC} \tНаличными: {numInComment}\n";
                }
            }
            msgLog += $"На карту {cardCount} участников. Сумма: {cardSum}\n";
            msgLog += $"Наличными {cashCount} участников. Сумма: {cashSum}\n";
            msgLog += $"Итог: {cashSum + cardSum}\n";
            return msgLog;
        }
        public static int GetSum(JToken Base)
        {
            int summ = 0;
            JArray persons = PBPersonsFromBase(Base);
            foreach (JToken person in persons)
            {
                string comment = PPComment(person);
                int numInComment = IntFromString(comment);
                if (numInComment != -1)
                    summ += numInComment;
            }
            return summ;
        }
        static int IntFromString(string str)
        {
            try
            {
                string number = "";
                for (int i = 0; i < str.Length; i++)
                {
                    if (!Char.IsDigit(str[i])) continue;
                    while (i < str.Length)
                    {
                        if (!Char.IsDigit(str[i])) break;
                        number += str[i];
                        i++;
                    }
                    break;
                }
                if (number.Length == 0) return -1;
                return int.Parse(number);
            }
            catch (Exception ex) { LogError("a8dbd38vb", ex); }
            return -1;
        }

        public static string CalculatePersonStartPriceAllDays(JToken Base)
        {
            string msgLog = "Подсчет стартовых взносов для участников во всех днях...\n";

            JArray races = PBRaces(Base);
            foreach (JToken race in races)
            {
                JToken raceData = PBData(race);
                string dateTime = PDStartDatetime(raceData);
                msgLog += $"  День {dateTime}...\n";
                msgLog += CalculatePersonStartPrice(race, races.Count);
            }

            msgLog += "  Завершено\n";
            return msgLog;
        }

        public static string CalculatePersonStartPrice(JToken race, int raceCount)
        {
            string msgLog = "    Подсчет стартовых взносов для участников...\n";

            JArray persons = PBPersons(race);
            JArray groups = PBGroups(race);

            Dictionary<string, int> groupIdPrice = DictGIdPrice(groups);
            
            foreach (JToken person in persons)
            {
                if (PPIsPaid(person))
                {
                    continue;
                }
                string personGroupId = PPGroupId(person);
                if (groupIdPrice.TryGetValue(personGroupId, out int groupPrice))
                {
                    string startDays = RemoveExtraPersons.PersonStartDays(person);
                    person["world_code"] = (groupPrice * (startDays?.Length ?? raceCount)).ToString();
                    //msgLog += $"      Подсчитано [{PPToString(person)}] Дни:{startDays?.Length}/{raceCount} Взнос: [{person["world_code"]}]\n";
                }
                else
                {
                    msgLog += $"    Группа не найдена [{personGroupId}] Участник - [{PPToString(person)}]\n";
                }
            }
            msgLog += "    Ок...\n";


            return msgLog;
        }
    }
}
