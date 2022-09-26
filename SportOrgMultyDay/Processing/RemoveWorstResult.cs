using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static SportOrgMultyDay.Processing.Parsing.ParseBase;
using static SportOrgMultyDay.Processing.Parsing.ParsePerson;
using static SportOrgMultyDay.Processing.Parsing.ParseResult;

namespace SportOrgMultyDay.Processing
{
    public class RemoveWorstResults
    {
        public static string Remove(JToken Base)
        {
            string msgLog = "Удаление худших повторяющихся результатов... \n";

            JArray results = ResultsFromBase(Base);
            List<JToken> toRemove = new();
            for (int i = 0; i < results.Count; i++)
            {
                for (int j = i + 1; j < results.Count; j++)
                {
                    if (PRPersonId(results[i]) != PRPersonId(results[j])) continue;
                    JToken rToRem = (PRResultMsec(results[i]) > PRResultMsec(results[j])) ? results[i] : results[j];
                    if (!toRemove.Contains(rToRem)) toRemove.Add(rToRem);
                }
            }
            foreach (JToken rToRem in toRemove)
            {
                msgLog += $"  Удаление - Номер: {PRBib(rToRem)} Id: {PRPersonId(rToRem)}\n";
                results.Remove(rToRem);
            }
            msgLog += $"  Удалено {toRemove.Count} результатов\n";

            return msgLog;
        }
    }
}
