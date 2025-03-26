using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportOrgMultyDay.Data.SFR
{
    public interface ISFRBase
    {
        public int Id { get; set; }
        public string Header { get; }
        public int MinHeaderDigits { get; }

        public string[] FieldsArray();

        public string Serialize()
        {
            string[] data = FieldsArray();
            StringBuilder sb = new();
            sb.Append(Header);
            sb.Append(IdNormalizer(Id));
            sb.Append("\t");
            sb.Append(string.Join("\t", data));
            string result = sb.ToString();
            return result;
        }

        private string IdNormalizer(int id)
        {
            string s = "";
            string sId = id.ToString();
            int zeroToAdd = MinHeaderDigits - sId.Length;
            for (int i = 0; i < zeroToAdd; i++)
                s += "0";
            s += sId;
            return s;
        }
    }
}
