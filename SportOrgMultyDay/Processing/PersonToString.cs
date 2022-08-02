using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static SportOrgMultyDay.Processing.Parsing.ParsePerson;

namespace SportOrgMultyDay.Processing
{
    public static class PersonToString
    {
        public static string Name(JToken person)
        {
            return $"{PPSurname(person)} {PPName(person)}";
        }
        public static string Bib(JToken person)
        {
            return $"{PPBib(person)}";
        }
        public static string Comment(JToken person)
        {
            return $"{PPComment(person)}";
        }
        public static string BibName(JToken person)
        {
            return $"#{Bib(person)} {Name(person)}";
        }

        public static string BibNameComment(JToken person)
        {
            return $"#{Bib(person)} {Name(person)} {Comment(person)}";
        }



    }
}
