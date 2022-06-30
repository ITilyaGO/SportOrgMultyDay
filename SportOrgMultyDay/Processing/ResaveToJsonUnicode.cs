using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace SportOrgMultyDay.Processing
{
    public class ResaveToJsonUnicode
    {
        public static string Convert(string inputJson)
        {
            var jdes = JsonSerializer.Deserialize<object>(inputJson);
            string ser = JsonSerializer.Serialize(jdes);
            return ser;
        }
    }
}
