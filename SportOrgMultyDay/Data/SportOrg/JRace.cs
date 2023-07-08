using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportOrgMultyDay.Data.SportOrg
{
    public class JRace
    {
        public JRace(JToken race) => jRace = race;
        public JToken jRace { get; set; }
    }
}
