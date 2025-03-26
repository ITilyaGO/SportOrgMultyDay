using Newtonsoft.Json.Linq;
using SportOrgMultyDay.Data.SFR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportOrgMultyDay.Processing.SFR
{
    public static class SFRxManager
    {
        public static string RaceToSFRx(out string log, JToken race)
        {
            log = "Перевод в SFRx...\n";
            SFRGeneral sFRGeneral = SportOrgToSFR.RaceToSFR(race);
                log += "  Генерация SFRx...\n";
            return sFRGeneral.Serialize();
        }
    }
}
