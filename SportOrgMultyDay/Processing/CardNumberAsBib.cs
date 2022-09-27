using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static SportOrgMultyDay.Processing.Parsing.ParseBase;
using static SportOrgMultyDay.Processing.Parsing.ParsePerson;

namespace SportOrgMultyDay.Processing
{
    public static class CardNumberAsBib
    {
        public static string Process(JToken jBase)
        {
            string msgLog = "Копирование номера в номер чипа...\n";
            JArray races = PBRaces(jBase);
            for (int r = 0; r < races.Count; r++)
            {
                msgLog += $"День:{r}\n";
                JArray persons = PBPersons(races[r]);
                for (int i = 0; i < persons.Count; i++)
                {
                    JToken person = persons[i];
                    msgLog += $"{PPBib(person)},";
                    person["card_number"] = PPBib(person);
                }
                msgLog += $"\n";
            }
            return msgLog;
        }
    }
}
