using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportOrgMultyDay.Data.SFR
{
    public class SFRTeam : ISFRBase
    {
        public int Id { get; set; }
        public string Header { get; } = "t";
        public int MinHeaderDigits { get; } = 5;

        public string Name { get; set; } // 2 Название
        // 3 Входит В Def: -1
        // 4 comment Def: empty
        // 5 Контакт Def: empty
        // 6 Даза заявки Def: empty
        // 7 Дата оплаты Def: empty
        // 8 ?? Def: 0
        // 9 Пусто потому что нужен таб

        public SFRTeam(int id)
        {
            Id = id;
        }

        public string[] FieldsArray()
        {
            return
            [
                Name,         // 2 Название
                "-1",         // 3 Входит В Def: -1
                string.Empty, // 4 comment Def: empty
                string.Empty, // 5 Контакт Def: empty
                string.Empty, // 6 Даза заявки Def: empty
                string.Empty, // 7 Дата оплаты Def: empty
                "0",          // 8 ?? Def: 0
                string.Empty  // 9 Пусто потому что нужен таб
            ];
        }
    }
}
