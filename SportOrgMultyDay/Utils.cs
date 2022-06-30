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

namespace SportOrgMultyDay
{
    public partial class Utils : Form
    {
        public Utils()
        {
            InitializeComponent();
        }

        JObject Base;
       
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
                SendLog($"Ошибка импорта: {ex.Message}");
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
                SendLog("Ошибка импорта базы: в данном файле база не найдена");
                BaseEditButtons(false);
            }
            labelBaseImport.Text = $"Найдено дней: {Base["races"].Count()}";
            SendLog("Импорт выполнен");
            BaseEditButtons(true);
        }

        private void BaseEditButtons(bool active)
        {
            buttonBaseExport.Enabled = active;
            buttonRemoveMissingPersons.Enabled = active;
            buttonSynchronizeReorders.Enabled = active;
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

        }

        private void SendLog(string message)
        {
            richTextBoxLog.Text += $"[{DateTime.Now:HH:mm:ss}] >> {message}\n";
            richTextBoxLog.ScrollToCaret();
        }

        private void Utils_Load(object sender, EventArgs e)
        {
            BaseEditButtons(false);
        }
    }
}
