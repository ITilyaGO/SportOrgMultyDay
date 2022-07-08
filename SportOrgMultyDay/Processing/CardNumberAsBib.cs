using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportOrgMultyDay.Processing
{
    public class CardNumberAsBib
    {
        public static string Process(JToken jBase)
        {
            string msgLog = "Копирование номера в номер чипа...\n";
            JArray races = (JArray)jBase["races"];
            for (int r = 0; r < races.Count; r++)
            {
                msgLog += $"День:{r}\n";
                JToken race = races[r];
                JArray persons = (JArray)race["persons"];
                for (int i = 0; i < persons.Count; i++)
                {
                    JToken person = persons[i];
                    msgLog += $"{person["bib"]},";
                    person["card_number"] = person["bib"];
                }
                msgLog += $"\n";
            }
            return msgLog;
        }
    }
}
