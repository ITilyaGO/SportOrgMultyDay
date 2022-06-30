using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportOrgMultyDay.Processing
{
    public class RemoveExtraPersons
    {
        public static string Remove(JObject jbase)
        {
            string log = "Удление отсутствующих по дням... \n";
            var races = jbase["races"];
            for (int i = 0; i < races.Count(); i++)
            {
                log += $"День:{i + 1}\n{TryRemovePersonsFromDay(races[i],i)}\n";
            }
            return log;
        }

        private static string TryRemovePersonsFromDay(JToken race, int raceInd)
        {
            string logMsg = "";
            var persons = race["persons"];
            for (int i = persons.Count()-1; i >= 0; i--)
            {
                if (!RunPersonInDay(persons[i], raceInd))
                {
                    logMsg += $"  Удален - {PersonToString.BibNameComment(persons[i])}\n";
                    persons[i].Remove();
                }
            }

            return logMsg; 
        }

        private static bool RunPersonInDay(JToken person,int raceInd)
        {
            string stringOfEntry = "C:";
            string comment = (string)person["comment"];
            int cIndex = comment.IndexOf(stringOfEntry);
            
            if (cIndex == -1)
                return true;
            int startCline = cIndex + stringOfEntry.Length;
            int endCline = startCline;
            for (int i = startCline; i < comment.Length; i++)
            {
                if (char.IsDigit(comment[i]))
                    endCline = i;
                else
                    break;
            }

            string numberBlock = comment.Substring(startCline, (endCline - startCline)+1);

            return numberBlock.Contains((raceInd+1).ToString());

        }
    }
}
