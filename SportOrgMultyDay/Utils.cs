using Newtonsoft.Json.Linq;
using SportOrgMultyDay.Processing;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.CheckedListBox;
using static SportOrgMultyDay.Processing.Parsing.ParseBase;
using SportOrgMultyDay.Processing.SFRSmartTerminal;
using SportOrgMultyDay.Helpers;

namespace SportOrgMultyDay
{
    public partial class Utils : Form
    {
        public Utils(Numbers numbersForm)
        {
            NumbersForm = numbersForm;
            InitializeComponent();
        }
        public Numbers NumbersForm;
        public JObject Base;
        int raceCount = 0;
        AutoResize autoResize;
        List<int> SFRStartLog = new();
        private JObject ImportJson()
        {
            if (openFileDialogJson.ShowDialog() != DialogResult.OK)
            {
                SendLog("Импорт отменен");
                return null;
            }
            try
            {
                string json = File.ReadAllText(openFileDialogJson.FileName);
                JObject jobj = JObject.Parse(json);
                return jobj;
            }
            catch (Exception ex)
            {
                SendLog($"⚠Ошибка импорта: {ex.Message}");
                return null;
            }
        }

        private void ExportJson(JObject savingBase)
        {
            if (saveFileDialogJson.ShowDialog() != DialogResult.OK) return;


            string ojson = savingBase.ToString();
            string saveJ = ResaveToJsonUnicode.Convert(ojson);
            File.WriteAllText(saveFileDialogJson.FileName,saveJ);
           
        }

        private void ImportBase()
        {
            Base = ImportJson();
            if (Base == null)
            {
                labelBaseImport.Text = $"Ошибка";
                SendLog("⚠Ошибка импорта базы: в данном файле база не найдена");
                BaseEditButtons(false);
                return;
            }
            raceCount = Base["races"].Count();
            labelBaseImport.Text = $"День:";
            comboBoxDays.Items.Clear();
            for (int i = 0; i < Base["races"].Count(); i++)
            {
                comboBoxDays.Items.Add(i+1);
            }
            comboBoxDays.SelectedIndex = CurrentRaceID(Base);
            SendLog("Импорт выполнен");
            BaseEditButtons(true);
        }

        private void BaseEditButtons(bool active)
        {
            buttonBaseExport.Enabled = active;
            buttonRemoveMissingPersons.Enabled = active;
            buttonSynchronizeReorders.Enabled = active;
            buttonCreateNewAdded.Enabled = active;
            buttonCardNumAsNum.Enabled = active;
            buttonFindAddWithComment.Enabled = active;
            buttonCopyPersonByNumber.Enabled = active;
            buttonExportStartTimes.Enabled = active;
            buttonCopyGroupSettings.Enabled = active;
            comboBoxDays.Enabled = active;
            buttonImportSFRStartLog.Enabled = active;
            buttonStartFeeCalculate.Enabled = active;
            buttonRemvoeWorstResult.Enabled = active;
        }

        private void buttonBaseImport_Click(object sender, EventArgs e)
        {
            ImportBase();

        }
        private void buttonBaseExport_Click(object sender, EventArgs e)
        {
            ExportJson(Base);
        }
        private void buttonRemoveMissingPersons_Click(object sender, EventArgs e)
        {
            SendLog(RemoveExtraPersons.Remove(Base));

        }

        private void buttonSynchronizeReorders_Click(object sender, EventArgs e)
        {
            string[] syncFields = CheckListBoxItem.ToStringMS(checkedListBoxWithSync.CheckedItems);
            SendLog( SynchronizeRaces.SynchronizeReservWithCurrentRace(Base,textBoxReservName.Text, syncFields,checkBoxCopyChangedOtherDays.Checked));
        }

        private void SendLog(string message)
        {
            richTextBoxLog.Text += $"[{DateTime.Now:HH:mm:ss}] >> {message}\n";
            richTextBoxLog.ScrollToCaret();
        }

        private void Utils_Load(object sender, EventArgs e)
        {
            BaseEditButtons(false);


            CheckListBoxItem[] checkListBoxItems = {
                new("Чип","card_number"),
                new("Комментарий","comment"),
                new("Группа","group_id"),
                new("Старт","start_time",false),
                new("Имя","name"),
                new("Фамилия","surname"),
                new("Год рождения","year"),
                new("Команда","organization_id"),
                new("Квалификация","qual"),
                new("в/к","is_out_of_competition"),
                new("Оплачено","is_paid"),
                new("Аренда чипа","is_rented_card"),
                new("Лично","is_personal"),
                new("Дата рождения","birth_date"),
                new("Старт группы","start_group",false),
                new("Номер","bib",false),
                new("id","id"),
                new("sex","sex"),
                new("object","object",false),
                new("world_code","world_code"),
                new("national_code","national_code"),

            };
            foreach (CheckListBoxItem item in checkListBoxItems)
                checkedListBoxWithSync.Items.Add(item,item.Chacked);

            autoResize = new(this);
            autoResize.Add(richTextBoxLog);
            autoResize.Add(tabControl1);
            autoResize.Add(tabControlFunc,false,true);
            autoResize.Add(checkedListBoxWithSync,false,true);

        }

        private void richTextBoxLog_TextChanged(object sender, EventArgs e)
        {
            if (ScrollLogToolStripMenuItem.Checked)
            {
                richTextBoxLog.SelectionStart = richTextBoxLog.Text.Length;
                richTextBoxLog.ScrollToCaret();
            }
        }

        private void buttonCreateNewAdded_Click(object sender, EventArgs e)
        {
            SendLog(SynchronizeRaces.CreateNewPersons(Base));
        }

        private void buttonCardNumAsNum_Click(object sender, EventArgs e)
        {
            SendLog(CardNumberAsBib.Process(Base));
        }

        private void buttonCopyPersonByNumber_Click(object sender, EventArgs e)
        {
            string bibList = textBoxPersonsFromCopy.Text;
            string[] bibs = bibList.Split(",",StringSplitOptions.RemoveEmptyEntries);

            List<int> ints = new();
            foreach (string bib in bibs)
            {
                ints.Add(Convert.ToInt32(bib));
            }
            string[] syncFields = CheckListBoxItem.ToStringMS(checkedListBoxWithSync.CheckedItems);
            SendLog( SynchronizeRaces.CopyPersonsByNumberList(Base, ints.ToArray(), syncFields));
        }

        private void buttonCombineAllBase_Click(object sender, EventArgs e)
        {

        }

        private void buttonFindAddWithComment_Click(object sender, EventArgs e)
        {
            textBoxPersonsFromCopy.Text = SynchronizeRaces.FindAddWithComment(Base, textBoxStringFindComment.Text);
        }

        private void buttonCopyGroupSettings_Click(object sender, EventArgs e)
        {
            SendLog(SyncGroups.SyncByFields(Base));
        }

        private void buttonExportStartTimes_Click(object sender, EventArgs e)
        {
            if (saveFileDialogSfrst.ShowDialog() != DialogResult.OK) return;
            string sftStartTxt = ExportStartTimes.ToSFRSmartTerminal(Base);
            File.WriteAllText(saveFileDialogSfrst.FileName, sftStartTxt, new UTF8Encoding(true));
            SendLog($"Экспорт стартового файла SFT Smart Terminal...\nСохранено в файл: {saveFileDialogSfrst.FileName}\n==========================\n{sftStartTxt}\n==========================");
        }

        private void comboBoxDays_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!int.TryParse(comboBoxDays.Text.ToString(), out int day)) return;
            day--;
            if (day < raceCount && day >= 0)
                Base["current_race"] = day;
            else
                comboBoxDays.Text = "Err";
        }
       
        private void Utils_SizeChanged(object sender, EventArgs e)
        {
            if (autoResize is null) return;
            autoResize.Update();
        }

        private void ScrollLogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ScrollLogToolStripMenuItem.Checked = !ScrollLogToolStripMenuItem.Checked;
        }

        private void ClearLogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBoxLog.Clear();
        }

        private void buttonImportSFRStartLog_Click(object sender, EventArgs e)
        {
            if (openFileDialogStartLog.ShowDialog() != DialogResult.OK) return;
            string startLog = File.ReadAllText(openFileDialogStartLog.FileName);
            StartLogProcessing slp = new(Base, startLog);
            richTextBoxSFRStartLogDupl.Text = slp.Duplicates;
            richTextBoxSFRStartLogDNS.Text = slp.DNS;
            richTextBoxChecklessFinished.Text = slp.ChecklessFinished;
            labelSFRStartLogCount.Text = $"Стартовало: {slp.StartedPersons}";
            SendLog(slp.GetLog());
        }

        private void buttonSFRStartLogDNSCopy_Click(object sender, EventArgs e)
        {
            if (richTextBoxSFRStartLogDNS.Text == "") return;
            Clipboard.SetText(richTextBoxSFRStartLogDNS.Text);
        }

        private void buttonStartFeeCalculate_Click(object sender, EventArgs e)
        {
            SendLog(StartFeeCalculate.GetStatistic(Base, textBoxStartFeeWithCardSymbol.Text));
        }

        private void buttonRemvoeWorstResult_Click(object sender, EventArgs e)
        {
            SendLog(RemoveWorstResults.Remove(Base));
        }
    }

 
}
