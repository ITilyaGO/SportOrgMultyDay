using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportOrgMultyDay.Data.SFR
{
    public class SFRGroup : ISFRBase
    {
        public int Id { get; set; }
        public string Header { get; } = "g";
        public int MinHeaderDigits { get; } = 5;

        public string Name { get; set; } // 2 Название
        // 3 Входит В Def: -1
        // 4 Def: 0
        // 5 Def: 0
        // 6 Назхвание соревнований Def: empty
        // 7 Взнос Def: 0 
        // 8 Трасса(дистанция) Def: -1
        // 9 Разряд до Def: 0
        // 10 МС до Def: 0
        // 11 Кмс до Def: 0
        // 12 КВ в миллисек Def: 0
        // 13 очкм Def: 0
        // 14 Пусто потому что нужен таб

        public SFRGroup(int id)
        {
            Id = id;
        }

        public string[] FieldsArray()
        {
            return
            [
                Name,               // 2 Название
                "-1",               // 3 Входит В Def: -1
                "0",                // 4 Def: 0
                "0",                // 5 Def: 0
                string.Empty,       // 6 Назхвание соревнований Def: empty
                "0",                // 7 Взнос Def: 0 
                "-1",               // 8 Трасса(дистанция) Def: -1
                "0",                // 9 Разряд до Def: 0
                "0",                // 10 МС до Def: 0
                "0",                // 11 Кмс до Def: 0
                "0",                // 12 КВ в миллисек Def: 0
                "0",                // 13 очкм Def: 0
                string.Empty        // 14 Пусто потому что нужен таб
            ];
        }
    }
}
