using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportOrgMultyDay.Data
{
    public class StartCell
    {
        public int Bib { get; set; }
        public string Time { get; set; }
        public StartCell(int bib, string time)
        {
            Bib = bib;
            Time = time;
        }
        public override string ToString()
        {
            return $"{Bib} {Time}";
        }
        public override bool Equals(object obj)
        {
            if (obj is StartCell person) return Bib == person.Bib;
            return false;
        }
        public override int GetHashCode() => Bib.GetHashCode();
    }
}
