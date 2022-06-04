using Newtonsoft.Json.Linq;

namespace SportOrgMultyDay
{
    public partial class Form1 : Form
    {
        const string StartStrJson = "var race = ";
       // string Html;
        string jsonSource1;
        string jsonSource2;

        public Form1()
        {
            InitializeComponent();
        }

        private void buttonImportHtml_Click(object sender, EventArgs e)
        {
            jsonSource1 = FindJson(ImportHtml());
            var data1 = JObject.Parse(jsonSource1);
            //var data1pers = data1["persons"];
            labelDay1Info.Text = "Участники:" + data1["persons"].Count();
        }


        private void PutToNumbers(string json)
        {
            if (saveFileDialog1.ShowDialog() != DialogResult.OK) return;
            string shablon = Properties.Resources.HtmlNumbers;
           // string shablon = ;
            shablon = shablon.Replace("<|JsonRace|>", json);
            shablon = shablon.Replace("<|StartTime|>", textBoxStart1.Text);
            shablon = shablon.Replace("<|StartTime2|>", textBoxStart2.Text);
            File.WriteAllText(saveFileDialog1.FileName, shablon);
        }

        private string CombineJson(string start1,string start2)
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
            return newjson;
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

        private string ImportHtml()
        {
            openFileDialog1.ShowDialog();
            string Html = File.ReadAllText(openFileDialog1.FileName);
            return Html;
           // jsonSource2 = FindJson(Html);
        }

        private void buttonImportHtml2_Click(object sender, EventArgs e)
        {
            jsonSource2 = FindJson(ImportHtml());
            var data2 = JObject.Parse(jsonSource1);
            labelDay2Info.Text = "Участники:" + data2["persons"].Count();

            // richTextBoxOut.Text = jsonSource2;
        }

        private void buttonCombine_Click(object sender, EventArgs e)
        {
            richTextBoxOut.Text = CombineJson(jsonSource1, jsonSource2);
        }

        private void buttonPutToBib_Click(object sender, EventArgs e)
        {
            //  openFileDialog1.ShowDialog();
            //  string sh = File.ReadAllText(openFileDialog1.FileName);
            PutToNumbers(richTextBoxOut.Text);
        }
    }
}