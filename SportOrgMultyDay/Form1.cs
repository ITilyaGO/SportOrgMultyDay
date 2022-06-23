using Newtonsoft.Json.Linq;

namespace SportOrgMultyDay
{
    public partial class Form1 : Form
    {
        const string StartStrJson = "var race = ";
       // string Html;
        string jsonSource1;
        string jsonSource2;
        JObject jsonRace;
        string htmlPattern = "null";
        string htmlBib = "null";
        JObject jsonBib;

        public Form1()
        {
            InitializeComponent();
        }

        private void buttonImportHtml_Click(object sender, EventArgs e)
        {
            string inHtml = ImportHtml();
            if (inHtml == "null") return;
            jsonSource1 = FindJson(inHtml);
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

        private void ExportNumbers(string json)
        {
            if (saveFileDialog1.ShowDialog() != DialogResult.OK) return;
            string shablon = htmlPattern;
            if (shablon == "null")
            {

                shablon = htmlBib;
                shablon = shablon.Replace(FindJson(htmlBib), json);
            }
            else
                shablon = shablon.Replace("<|JsonRace|>", json);


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

        private string ProcessJson(JObject jbib, JObject joRace)
        {
            var races = joRace["races"];
            var bibPersons = jbib["persons"];
            var jOut = jbib.DeepClone();
            JArray jOutPersons = (JArray)jOut["persons"];

            int racesCount = races.Count();
            for (int i = 0; i < jOutPersons.Count(); i++)
            {
                jOutPersons[i]["start_times"] = CreateStartTimes(racesCount);
            }

            for (int rc = 0; rc < races.Count(); rc++)
            {
                var curRace = races[rc];
                var curPersons = curRace["persons"];

                for (int cp = 0; cp < curPersons.Count(); cp++)
                {
                    bool searched = false;
                    var curPers = curPersons[cp];
                    int curBib = (int)curPers["bib"];
                    JToken joPers = null;
                    for (int jop = 0; jop < jOutPersons.Count(); jop++)
                    {
                        int bpBib = (int)jOutPersons[jop]["bib"];
                        if (bpBib == curBib)
                        {
                            joPers = jOutPersons[jop];
                            searched = true;
                            break;
                        }
                    }
                    if (searched)
                    {
                        JArray joStartTimes = (JArray)joPers["start_times"];
                        joStartTimes[rc] = curPers["start_time"];
                    }
                    else
                    {
                        JToken persToInsert = curPers.DeepClone();
                        persToInsert["start_times"] = CreateStartTimes(racesCount);
                        persToInsert["start_times"][rc] = curPers["start_time"];
                        jOutPersons.Add(persToInsert);
                    }
                }
            }


            string newjson = jOut.ToString(Newtonsoft.Json.Formatting.None);
            return newjson;
        }
        private JArray CreateStartTimes(int count)
        {
            JArray st = new JArray();
            for (int i = 0; i < count; i++)
            {
                st.Add(0);
            }
            return st;
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
            if (openFileDialogHtml.ShowDialog() != DialogResult.OK) return "null";
            string Html = File.ReadAllText(openFileDialogHtml.FileName);
            return Html;
           // jsonSource2 = FindJson(Html);
        }

        private JObject ImportJson()
        {
            openFileDialogJson.ShowDialog();
            string json = File.ReadAllText(openFileDialogJson.FileName);
            JObject jobj = JObject.Parse(json);
            return jobj;
        }

        private int GetRaceDayCount(JObject jobo)
        {
            try
            {
                var race = jobo;
                int days = race["races"].Count();
                return days;

            }
            catch (Exception ex)
            {
                MessageBox.Show("Парсинг файла не удался.","Возможно файл соревнований однодневный, попробуйте добавить еще день.\n\n" + ex.Message);
            }
            return 0;
        }
        private bool GetPatternDayCount(string pattrn)
        {
            return pattrn.Contains("<|JsonRace|>");
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

        private void buttonImportRaceJson_Click(object sender, EventArgs e)
        {
            jsonRace = ImportJson();
            labelRaceFindedDays.Text = "Найдено дней: " + GetRaceDayCount(jsonRace);
        }

        private void buttonProcessing_Click(object sender, EventArgs e)
        {
            richTextBoxOut.Text = ProcessJson(jsonBib, jsonRace);
        }

        private void buttonImportBib_Click(object sender, EventArgs e)
        {
            try
            {
                htmlBib = ImportHtml();
                string jsonStringBib = FindJson(htmlBib);
                richTextBoxOut.Text = jsonStringBib;
                jsonBib = JObject.Parse(jsonStringBib);
                labelBibFinded.Text = "Найдено участников: " + jsonBib["persons"].Count();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void buttonExportBibs_Click(object sender, EventArgs e)
        {
            ExportNumbers(richTextBoxOut.Text);
        }
    }
}