using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportOrgMultyDay.Data.SFR
{
    public class SFRPerson : ISFRBase
    {
        public int Id { get; set; }
        public string Header { get; } = "c";
        public int MinHeaderDigits { get; } = 5;

        public int Bib { get; set; } // 2 Номер
        public int GroupId { get; set; } // 3 Группа
        public string Surname { get; set; } // 4 Фамилия
        public string Name { get; set; } // 5 Имя
        public int OrganizationId { get; set; } // 6 Команда
        public int Year { get; set; } // 7 Год рождения
        public int Qualification { get; set; } // 8 Разряд
        public string Comment { get; set; } // 9 Коммент
        // ???? 10 Def: 0
        public int EntryFee { get; set; } // 11 Стартовый взнос
        // ? 12 Уч. Def: 0
        // ? 13 Этап Def: 1
        public string StartTime { get; set; } // 14 Время старта
        // ? 15 Def: empty
        // ? 16 Def: empty
        // ? 17 Def: empty
        // ? 18 Def: 0
        // ? 19 Def: 0
        // ? 20 Def: 0
        // ? 21 Пусто потому что нужен таб

        public SFRPerson(int id)
        {
            Id = id;
        }

        public string[] FieldsArray()
        {
            return
            [
                Bib.ToString(),           // 2
                GroupId.ToString(),       // 3
                Surname,                  // 4
                Name,                     // 5
                OrganizationId.ToString(),// 6
                Year.ToString(),          // 7
                Qualification.ToString(), // 8
                Comment,                  // 9
                "0",                      // 10 Def: 0
                EntryFee.ToString(),      // 11
                "0",                      // 12 Уч. Def: 0
                "1",                      // 13 Этап Def: 1
                StartTime,                // 14
                "",                       // 15 Def: empty
                "",                       // 16 Def: empty
                "",                       // 17 Def: empty
                "0",                      // 18 Def: 0
                "0",                      // 19 Def: 0
                "0",                      // 20 Def: 0
                ""                        // 21 Пусто потому что нужен таб
            ];
        }
    }

}
