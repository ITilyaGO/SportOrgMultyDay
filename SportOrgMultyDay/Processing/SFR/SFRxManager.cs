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
        public static string RaceToSFRx(out string _log, JToken race)
        {
            SFRGeneral sFRGeneral = SportOrgToSFR.RaceToSFR(race, out string log);
            string result = sFRGeneral.Serialize(out string log2);
            _log = log;
            _log += log2;
            return result;
        }
    }
}
