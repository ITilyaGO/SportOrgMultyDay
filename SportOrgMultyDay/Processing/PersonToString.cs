using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportOrgMultyDay.Processing
{
    public class PersonToString
    {
        public static string Name(JToken person)
        {
            return $"{person["surname"]} {person["name"]}";
        }
        public static string Bib(JToken person)
        {
            return $"{person["bib"]}";
        }
        public static string Comment(JToken person)
        {
            return $"{person["comment"]}";
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
