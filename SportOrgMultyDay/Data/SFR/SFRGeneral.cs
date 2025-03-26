using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportOrgMultyDay.Data.SFR
{
    public class SFRGeneral
    {
        public string event_name { get; set; }
        public SortedList<int, SFRPerson> Persons { get; set; } = [];
        public SortedList<int, SFRGroup> Groups { get; set; } = [];
        public SortedList<int, SFRTeam> Teams { get; set; } = [];
        public SortedList<int, SFRPart> Parts { get; set; } = [];


        public string Serialize()
        {
            string header = $"SFRx_v2404\t{event_name}\t\t1\t1\t\t\tИндивидуальные\tКак введено\tНомер команды и этап (последняя цифра)\r";
            List<ISFRBase> all = 
            [
                .. Parts.Values,
                .. Groups.Values,
                .. Teams.Values,
                .. Persons.Values
            ];
            
            return string.Join("\n", [header, ..all.Select(a => a.Serialize())] );
        }
    }
}
