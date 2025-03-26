using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportOrgMultyDay.Data.SFR
{
    public class SFRPart : ISFRBase
    {
        public int Id { get; set; }
        public string Header { get; } = "p";
        public int MinHeaderDigits { get; } = 1;

        public string Name { get; set; } // 2 Название
        public string Date { get; set; } // 3 Дата

        public SFRPart(int id)
        {
            Id = id;
        }

        public string[] FieldsArray()
        {
            return
            [
                Name, // 2 Название
                Date,  // 3 Дата
                string.Empty  // 3 Дата
            ];
        }
    }
}
