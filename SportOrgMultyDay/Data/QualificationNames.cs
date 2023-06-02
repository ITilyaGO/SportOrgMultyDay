using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportOrgMultyDay.Data
{
    public class QualificationNames
    {
        public static readonly Dictionary<int, string> DictIdToString = new()
        {
            { -1, "б/р" },
            { 0, "б/р" },
            { 1, "Iю" },
            { 2, "IIю" },
            { 3, "IIIю" },
            { 4, "I" },
            { 5, "II" },
            { 6, "III" },
            { 7, "КМС" },
            { 8, "МС" },
            { 9, "МСМК" }
        };

        public static readonly Dictionary<string, int> DictStringToId = new()
        {
            { "б/р", 0 },
            { "Iю", 1 },
            { "IIю", 2 },
            { "IIIю", 3 },
            { "I", 4 },
            { "II", 5 },
            { "III", 6 },
            { "КМС", 7 },
            { "МС", 8 },
            { "МСМК", 9 }
        };

    }
}
