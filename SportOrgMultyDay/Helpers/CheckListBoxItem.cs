using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.CheckedListBox;

namespace SportOrgMultyDay.Helpers
{
    public class CheckListBoxItem
    {
        public CheckListBoxItem(string text, string tag, bool chacked = true)
        {
            Text = text;
            Tag = tag;
            Chacked = chacked;
        }
        public string Tag;
        public string Text;
        public bool Chacked;
        public override string ToString() { return Text; }
        public static string[] ToStringMS(CheckedItemCollection clbi)
        {
            List<string> fields = new();
            foreach (CheckListBoxItem item in clbi)
            {
                fields.Add(item.Tag);
            }
            return fields.ToArray();
        }
    }
}
