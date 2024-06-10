using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SportOrgMultyDay.Processing.Parsing.ParsePerson;

namespace SportOrgMultyDay.Data.Combine
{
    public class PersonStartMinute
    {
        private TimeSpan startMinute;
        [DisplayName("Имя")]
        public string FullName { get; set; }

        [DisplayName("Коллектив")]
        public string OrganizationName { get; set; }
        [DisplayName("Разряд")]
        public string RangeName { get; set; }

        [DisplayName("Номер")]
        public int Bib { get; set; }
        [DisplayName("Старт")]
        public TimeSpan StartMinute
        {
            get
            {
                return startMinute;
            }
            set
            {
                startMinute = value;
                Person["start_time"] = value.TotalMilliseconds;
            }
        }


        [Browsable(false)]
        public JToken Person { get; set; }

        public PersonStartMinute(JToken person, string organizatonName, string rangeName = "")
        {
            FullName = PPSurnameName(person);
            Bib = PPBib(person);
            startMinute = PPStartTimeTS(person) ?? TimeSpan.Zero;
            Person = person;
            OrganizationName = organizatonName;
            RangeName = rangeName;
        }
    }
}
