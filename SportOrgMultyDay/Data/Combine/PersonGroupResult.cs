using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SportOrgMultyDay.Processing.Parsing.ParseBase;
using static SportOrgMultyDay.Processing.Parsing.ParseResult;
using static SportOrgMultyDay.Processing.Parsing.ParseGroup;
using static SportOrgMultyDay.Processing.Parsing.ParsePerson;
namespace SportOrgMultyDay.Data.Combine
{
    public class PersonGroupResult
    {
        public JToken Person { get; private set; }
        public JToken Group { get; private set; }
        public JToken Result { get; private set; }
        public string GroupID => PGId(Group);
        public string PersonID => PPId(Person);
        public string ResultID => PRId(Result);
        public int ResultPlace => PRPlace(Result);
        public PersonGroupResult(JToken person, JToken group, JToken result)
        {
            Person = person;
            Group = group;
            Result = result;
        }
    }
}
