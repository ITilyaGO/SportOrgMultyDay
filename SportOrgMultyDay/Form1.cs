using Newtonsoft.Json.Linq;

namespace SportOrgMultyDay
{
    public partial class Form1 : Form
    {
        const string StartStrJson = "var race = ";
        string Html;
        string jsonSource1;
        string jsonSource2;

        public Form1()
        {
            InitializeComponent();
        }

        private void buttonImportHtml_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            Html = File.ReadAllText(openFileDialog1.FileName);
            jsonSource1= FindJson(Html);
            richTextBoxOut.Text = jsonSource1;
        }

        private void DeserializeJson(string start1,string start2)
        {
            var data1 = JObject.Parse(start1);
            var data2 = JObject.Parse(start2);
            var data1pers = data1["persons"];
            var data2pers = data2["persons"];
      
            for (int i = 0; i < data1pers.Count(); i++)
            {
                bool searched = false;
                int bib = (int)data1pers[i]["bib"];
                for (int ii = 0; ii < data2pers.Count(); ii++)
                {
                    int bib2 = (int)data2pers[ii]["bib"];
                    if (bib == bib2)
                    {
                        data1pers[i]["start_time2"] = data2pers[ii]["start_time"];
                        searched = true;
                        break;
                    }
                }
                if (!searched)
                    data1pers[i]["start_time2"] = 0;

            }

            string newjson = data1.ToString(Newtonsoft.Json.Formatting.None);
            richTextBoxOut.Text = newjson;
        }

        private string FindJson(string html)
        {
            int jsonStart = html.IndexOf(StartStrJson) + StartStrJson.Length;
            int jsonEnd = html.IndexOf("\n", jsonStart);
            labelRaceIndex.Text = "S:"+jsonStart.ToString() + "E:" +jsonEnd.ToString();
            return html.Substring(jsonStart, jsonEnd - jsonStart -1);

           // labelTest.Text = html[jsonEnd-1].ToString();
            //jsonStart + StartStrJson.Length
        }

        private void buttonDeserialize_Click(object sender, EventArgs e)
        {
            DeserializeJson(jsonSource1,jsonSource2);
        }

        private void buttonImportHtml2_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            Html = File.ReadAllText(openFileDialog1.FileName);
            jsonSource2 = FindJson(Html);
           // richTextBoxOut.Text = jsonSource2;
        }
    }
}