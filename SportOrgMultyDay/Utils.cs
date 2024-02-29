﻿using Newtonsoft.Json.Linq;
using SportOrgMultyDay.Processing;
using System.Text;
using static SportOrgMultyDay.Processing.Parsing.ParseBase;
using static SportOrgMultyDay.Processing.Parsing.ParseGroup;
using static SportOrgMultyDay.Processing.Parsing.ParseOrganization;
using static SportOrgMultyDay.Processing.Logger;
using SportOrgMultyDay.Processing.SFRSmartTerminal;
using SportOrgMultyDay.Helpers;
using SportOrgMultyDay.Data;
using SportOrgMultyDay.Processing.Parsing.Things;

namespace SportOrgMultyDay
{
    public partial class Utils : Form
    {
        public Utils(Numbers numbersForm, General generalForm)
        {
            NumbersForm = numbersForm;
            GeneralForm = generalForm;
            InitializeComponent();
        }
        public Numbers NumbersForm;
        public General GeneralForm;
        public JObject JBase;
        //public JBase SportOrgBase;
        int raceCount = 0;
        AutoResize autoResize;
        List<int> SFRStartLog = new();
        OrganizationItemsController organizationItemsController = new();


        Dictionary<string, string> splitterStartLog = new()
        {
            { "space"," " },
            { "\\n","\n" },
            { ";",";" }
        };

        private JObject ParseJson(string rawJsonBase)
        {
            try
            {
                JObject jobj = JObject.Parse(rawJsonBase);
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
            File.WriteAllText(saveFileDialogJson.FileName, saveJ);

        }

        private void ImportBase()
        {
            try
            {
                if (openFileDialogJson.ShowDialog() != DialogResult.OK)
                {
                    SendLog("Импорт отменен");
                    return;
                }
                string json = File.ReadAllText(openFileDialogJson.FileName);
                ImportBase(json);
            }
            catch (Exception ex)
            {
                SendLog($"⚠Ошибка импорта базы: {ex.Message}");
                LogError("sdhd9ds9d", ex);
            }
        }

        private void ImportBase(string rawBaseJson)
        {
            try
            {
                JObject rawJBase = ParseJson(rawBaseJson);

                if (rawJBase == null)
                {
                    labelBaseImport.Text = $"Ошибка";
                    SendLog("⚠Ошибка импорта базы: в данном файле база не найдена");
                    BaseEditButtons(false);
                    return;
                }
                if (rawJBase.ContainsKey("version"))
                {
                    string ver = rawJBase["version"].ToString();
                    SendLog($"Версия базы {ver}");
                }
                else
                {
                    SendLog($"Версия базы не распознана");
                    string msgLog = "Проверка структуры...\n";
                    try
                    {
                        msgLog += rawJBase.ContainsKey("data") ? $"  Название: {rawJBase["data"]["title"]} \n  Дата: {rawJBase["data"]["start_datetime"]}\n" : throw new BaseParseException("data");
                        msgLog += rawJBase.ContainsKey("groups") ? $"  Группы: {rawJBase["groups"].Count()}\n" : throw new BaseParseException("groups");
                        msgLog += rawJBase.ContainsKey("persons") ? $"  Участники: {rawJBase["persons"].Count()}\n" : throw new BaseParseException("persons");
                        msgLog += rawJBase.ContainsKey("results") ? $"  Результаты: {rawJBase["results"].Count()}\n" : throw new BaseParseException("results");
                    }
                    catch (BaseParseException e)
                    {
                        SendLog($"Неизвестная структура базы. Не найден ключ [{e.Message}]. Импорт прерван.");
                        return;
                    }
                    catch (Exception e)
                    {
                        LogError("qweiu6gf23d", e);
                        SendLog($"ERROR - {e.Message}");
                        return;
                    }
                    SendLog(msgLog);

                    JObject newJbase = new();
                    JArray jArrayRaces = new();
                    newJbase.Add("current_race", 0);
                    newJbase.Add("version", "1.6.0.0");

                    jArrayRaces.Add(rawJBase);
                    newJbase.Add("races", jArrayRaces);

                    JBase = newJbase;
                }

                raceCount = JBase["races"].Count();
                labelBaseImport.Text = $"День:";
                comboBoxDays.Items.Clear();
                for (int i = 0; i < raceCount; i++)
                    comboBoxDays.Items.Add(i + 1);
                comboBoxDays.SelectedIndex = CurrentRaceID(JBase);

                BaseDayChange();

                SendLog("Импорт выполнен");

                LoadOrganizationItems();

                BaseEditButtons(true);
            }
            catch (Exception ex)
            {
                SendLog($"⚠Ошибка импорта базы: {ex.Message}");
                LogError("d13fewa", ex);
            }
        }

        private void BaseDayChange()
        {
            JToken currRace = PBCurrentRaceFromBase(JBase);

            ReloadOrganizationNameListInCombobox(currRace);

            comboBoxSourceRankGroupName.Items.Clear();
            foreach (JToken group in PBGroups(currRace))
                comboBoxSourceRankGroupName.Items.Add(new ComboBoxItemId(PGId(group), PGName(group)));
        }

        private void ReloadOrganizationNameListInCombobox(JToken currRace = null)
        {
            currRace ??= PBCurrentRaceFromBase(JBase);

            List<string> orgNames = new();
            foreach (JToken org in PBOrganizations(currRace))
            {
                string orgName = POName(org);
                if (!organizationItemsController.OrganizationItems.ContainsKey(orgName))
                    orgNames.Add(orgName);
            }
            orgNames.Sort();

            comboBoxOrganizationName.Items.Clear();
            orgNames.ForEach(n => comboBoxOrganizationName.Items.Add(n));
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
            buttonImportStartLogFile.Enabled = active;
            buttonStartFeeCalculate.Enabled = active;
            buttonRemvoeWorstResult.Enabled = active;
            buttonImportStartLogClipboard.Enabled = active;
            buttonOpenNumbersForm.Enabled = active;
            buttonCalculateRanks.Enabled = active;
            buttonSetAutoOrderStartTimes.Enabled = active;
            buttonSetStartMinutes.Enabled = active;
            buttonImportFromYarfso.Enabled = active;
            buttonGroupSetNumbersByGroups.Enabled = active;
            buttonSyncOrganizations.Enabled = active;
            buttonOrganizationTweaksLoad.Enabled = active;
            buttonOrganizationTweaksSave.Enabled = active;
            buttonAddOrganizationTweakItem.Enabled = active;
            buttonOrganizationRename.Enabled = active;
        }

        private void ReloadOrganizationRenameList()
        {
            listBoxCityOrganizationTweaks.Items.Clear();

            List<OrganizationItem> orgItems = organizationItemsController.OrganizationItems.Values.ToList();
            orgItems.Sort((x, y) => String.Compare(x.Name, y.Name));
            foreach (OrganizationItem orgIntem in orgItems)
                listBoxCityOrganizationTweaks.Items.Add(orgIntem);

            HashSet<string> citys = new();
            orgItems.ForEach(x => citys.Add(x.City));
            comboBoxOrganizationCity.Items.Clear();
            List<string> strtedUniqCitys = citys.ToList();
            strtedUniqCitys.Sort();
            strtedUniqCitys.ForEach(x => comboBoxOrganizationCity.Items.Add(x));


            ReloadOrganizationNameListInCombobox();
        }

        private void buttonBaseImport_Click(object sender, EventArgs e)
        {
            ImportBase();
        }
        private void buttonBaseExport_Click(object sender, EventArgs e)
        {
            ExportJson(JBase);
        }
        private void buttonRemoveMissingPersons_Click(object sender, EventArgs e)
        {
            SendLog(RemoveExtraPersons.Remove(JBase));

        }

        private void buttonSynchronizeReorders_Click(object sender, EventArgs e)
        {
            string[] syncFields = CheckListBoxItem.ToStringMS(checkedListBoxWithSync.CheckedItems);
            SendLog(SynchronizeRaces.SynchronizeReservWithCurrentRace(JBase, textBoxReservName.Text, syncFields, checkBoxCopyChangedOtherDays.Checked));
        }

        private void SendLog(string message)
        {
            richTextBoxLog.Text += $"[{DateTime.Now:HH:mm:ss}] >> {message}\n";
            richTextBoxLog.ScrollToCaret();
        }

        private void Utils_Load(object sender, EventArgs e)
        {
            BaseEditButtons(false);

            for (int i = 1; i <= 4; i++)
                comboBoxLogType.Items.Add((EStartLogType)i);
            comboBoxLogType.SelectedIndex = 0;
            foreach (string s in splitterStartLog.Keys)
                comboBoxStartLogOutFieldsSplitter.Items.Add(s);
            comboBoxStartLogOutFieldsSplitter.SelectedIndex = 0;

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
                checkedListBoxWithSync.Items.Add(item, item.Chacked);

            autoResize = new(this);
            autoResize.Add(richTextBoxLog);
            autoResize.Add(tabControl1);
            autoResize.Add(tabControlFunc, false, true);
            autoResize.Add(checkedListBoxWithSync, false, true);

        }

        private void LoadOrganizationItems()
        {
            organizationItemsController = OrganizationItemsController.Load(out string log);
            ReloadOrganizationRenameList();
            SendLog(log);
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
            SendLog(SynchronizeRaces.CreateNewPersons(JBase));
        }

        private void buttonCardNumAsNum_Click(object sender, EventArgs e)
        {
            SendLog(CardNumberAsBib.Process(JBase));
        }

        private void buttonCopyPersonByNumber_Click(object sender, EventArgs e)
        {
            string bibList = textBoxPersonsFromCopy.Text;
            string[] bibs = bibList.Split(",", StringSplitOptions.RemoveEmptyEntries);

            List<int> ints = new();
            foreach (string bib in bibs)
            {
                ints.Add(Convert.ToInt32(bib));
            }
            string[] syncFields = CheckListBoxItem.ToStringMS(checkedListBoxWithSync.CheckedItems);
            SendLog(SynchronizeRaces.CopyPersonsByNumberList(JBase, ints.ToArray(), syncFields));
        }

        private void buttonCombineAllBase_Click(object sender, EventArgs e)
        {

        }

        private void buttonFindAddWithComment_Click(object sender, EventArgs e)
        {
            textBoxPersonsFromCopy.Text = SynchronizeRaces.FindAddWithComment(JBase, textBoxStringFindComment.Text);
        }

        private void buttonCopyGroupSettings_Click(object sender, EventArgs e)
        {
            SendLog(SyncGroups.SyncByFields(JBase));
        }

        private void buttonExportStartTimes_Click(object sender, EventArgs e)
        {
            if (saveFileDialogSfrst.ShowDialog() != DialogResult.OK) return;
            string sftStartTxt = ExportStartTimes.ToSFRSmartTerminal(JBase);
            File.WriteAllText(saveFileDialogSfrst.FileName, sftStartTxt, new UTF8Encoding(true));
            SendLog($"Экспорт стартового файла SFT Smart Terminal...\nСохранено в файл: {saveFileDialogSfrst.FileName}\n==========================\n{sftStartTxt}\n==========================");
        }

        private void comboBoxDays_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!int.TryParse(comboBoxDays.Text.ToString(), out int day)) return;
            day--;

            if (day < raceCount && day >= 0)
            {
                JBase["current_race"] = day;
                BaseDayChange();
                SendLog($"Текущий день: {JBase["current_race"]}");
            }
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

        private void buttonImportStartLogFile_Click(object sender, EventArgs e)
        {
            if (openFileDialogStartLog.ShowDialog() != DialogResult.OK) return;
            StartLogProcess(File.ReadAllText(openFileDialogStartLog.FileName));
        }

        private void buttonImportStartLogClipboard_Click(object sender, EventArgs e)
        {
            if (!Clipboard.ContainsText())
            {
                SendLog("Буфер объмена не содержит текст");
                return;
            }
            StartLogProcess(Clipboard.GetText());
        }

        private void StartLogProcess(string startLog)
        {
            StartLogProcessing slp = new(JBase, startLog, dateTimePickerExportStartLog.Value.TimeOfDay, (EStartLogType)comboBoxLogType.SelectedItem,
                splitterStartLog.TryGetValue(comboBoxStartLogOutFieldsSplitter.Text, out string val) ? val : comboBoxStartLogOutFieldsSplitter.Text);
            richTextBoxStartLogDupl.Text = slp.Duplicates;
            richTextBoxStartLogDNS.Text = slp.DNS;
            richTextBoxChecklessFinished.Text = slp.ChecklessFinished;
            labelSFRStartLogCount.Text = $"Стартовало: {slp.StartedPersons}";
            SendLog(slp.GetLog());
        }

        private void buttonSFRStartLogDNSCopy_Click(object sender, EventArgs e)
        {
            if (richTextBoxStartLogDNS.Text == "") return;
            Clipboard.SetText(richTextBoxStartLogDNS.Text);
        }

        private void buttonStartFeeCalculate_Click(object sender, EventArgs e)
        {
            SendLog(StartFeeCalculate.GetStatistic(JBase, textBoxStartFeeWithCardSymbol.Text));
        }

        private void buttonRemvoeWorstResult_Click(object sender, EventArgs e)
        {
            SendLog(RemoveWorstResults.Remove(JBase));
        }


        private void buttonOpenNumbersForm_Click(object sender, EventArgs e)
        {
            GeneralForm.ShowNumbers();
        }

        private void Utils_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            GeneralForm.ShowIfAllClosed(hideUtils: true);
            this.Hide();
        }

        private void buttonCalculateRanks_Click(object sender, EventArgs e)
        {
            int rankComplete = (int)numericUpDownGroupResultsCountToCompleteRank.Value;

            SendLog(CalculateGroupsRank.ProcessCurrentRace(JBase, ((ComboBoxItemId)comboBoxSourceRankGroupName.SelectedItem).Id));
        }

        private void buttonImportFromYarfso_Click(object sender, EventArgs e)
        {
            if (openFileDialogYarfso.ShowDialog() != DialogResult.OK) return;
            try
            {
                string file = File.ReadAllText(openFileDialogYarfso.FileName);
                SendLog(YarfsoParser.SetQualFromYarfso(JBase, file, checkBoxPayAmountToComment.Checked));
            }
            catch (Exception ex)
            {
                LogError("d9283hdvak", ex);
                SendLog("ERROR buttonImportFromYarfso_Click() вызвало ошибку");
            }
        }

        private void buttonSetStartMinutes_Click(object sender, EventArgs e)
        {
            buttonSetStartMinutes.Text = dateTimePickerStartTime.Value.TimeOfDay.ToString();
            if (richTextBoxGroupStartOrder.Text.Length == 0)
                SendLog(StartTimeManager.SetStartTimes(PBCurrentRaceFromBase(JBase),
                    dateTimePickerStartTime.Value.TimeOfDay,
                    dateTimePickerStartInterval.Value.TimeOfDay,
                    checkBoxStartTimesPersonShuffle.Checked));
            else
            {
                if (checkBoxUseShortStartTimeAlg.Checked)
                    SendLog(StartTimeManager.ShortOrderedSetStartTimes(PBCurrentRaceFromBase(JBase),
                        dateTimePickerStartTime.Value.TimeOfDay,
                        dateTimePickerStartInterval.Value.TimeOfDay,
                        dateTimePickerMinColumnStartInterval.Value.TimeOfDay,
                        richTextBoxGroupStartOrder.Text,
                        checkBoxStartTimesPersonShuffle.Checked));
                else
                    SendLog(StartTimeManager.OrderedSetStartTimes(PBCurrentRaceFromBase(JBase),
                        dateTimePickerStartTime.Value.TimeOfDay,
                        dateTimePickerStartInterval.Value.TimeOfDay,
                        richTextBoxGroupStartOrder.Text,
                        checkBoxStartTimesPersonShuffle.Checked));

            }

        }

        private void buttonSetAutoOrderStartTimes_Click(object sender, EventArgs e)
        {
            richTextBoxGroupStartOrder.Text = StartTimeManager.AutoGroupOrder(PBCurrentRaceFromBase(JBase));
        }

        private void comboBoxLogType_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void buttonBibsAutoCreateListNumbering_Click(object sender, EventArgs e)
        {

        }

        private void buttonGroupSetNumbersByGroups_Click(object sender, EventArgs e)
        {
            SendLog(BibsNumbering.SetNumbers(PBCurrentRaceFromBase(JBase), richTextBoxBibsNumbering.Text, checkBoxSetNumbersByGroupsDebug.Checked));
        }

        private void buttonSyncOrganizations_Click(object sender, EventArgs e)
        {
            SendLog(SyncOrganizations.SyncNames(JBase));
        }

        private void buttonOrganizationTweaksSave_Click(object sender, EventArgs e)
        {
            organizationItemsController.Save(out string log);
            SendLog(log);
        }

        private void buttonOrganizationTweaksLoad_Click(object sender, EventArgs e)
        {
            LoadOrganizationItems();
        }

        private void buttonOrganizationTweaksLoadLast_Click(object sender, EventArgs e)
        {

        }

        private void buttonAddOrganizationTweakItem_Click(object sender, EventArgs e)
        {
            if (organizationItemsController == null)
                organizationItemsController = new();
            SendLog(organizationItemsController.Add(comboBoxOrganizationName.Text, textBoxOrganizationNewName.Text.Trim(), comboBoxOrganizationCity.Text.Trim(), checkBoxOrganizationIsRemoving.Checked, checkBoxOrganizationIsShowCity.Checked));
            ReloadOrganizationRenameList();
            organizationItemsController.Save(out string log);
            SendLog(log);
        }

        private void labelOrganizationNameToNewName_Click(object sender, EventArgs e)
        {
            textBoxOrganizationNewName.Text = comboBoxOrganizationName.Text;
        }

        private void buttonOrganizationRename_Click(object sender, EventArgs e)
        {
            SendLog(OrganizationTweaker.RenameOrganizations(organizationItemsController, PBCurrentRaceFromBase(JBase)));
        }

        private void listBoxCityOrganizationTweaks_DoubleClick(object sender, EventArgs e)
        {
            OrganizationItem orgItem = listBoxCityOrganizationTweaks.SelectedItem as OrganizationItem;
            if (orgItem == null)
            {
                SendLog("Не удалось получить элемент переиминования");
                return;
            }
            comboBoxOrganizationName.Text = orgItem.Name;
            textBoxOrganizationNewName.Text = orgItem.NewNameRaw;
            comboBoxOrganizationCity.Text = orgItem.City;
            checkBoxOrganizationIsRemoving.Checked = orgItem.IsRemoveable;
            checkBoxOrganizationIsShowCity.Checked = orgItem.IsShowCity;
        }

        private void labelClearOrganizationNewName_Click(object sender, EventArgs e)
        {
            textBoxOrganizationNewName.Text = string.Empty;
        }

        private void labelClearOrganozationCity_Click(object sender, EventArgs e)
        {

            comboBoxOrganizationCity.Text = string.Empty;
        }

        private void buttonBaseImportFromUrl_Click(object sender, EventArgs e)
        {
            if (!Clipboard.ContainsText() || !Uri.TryCreate(Clipboard.GetText(), UriKind.Absolute, out Uri uriResult) || !(uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps))
            {
                SendLog("Буфер обмена не содержит корректную ссылку");
                return;
            }

            string rawBase = ProtocolParser.GetBaseFromProtocolUrl(uriResult.ToString(), out string log);
            SendLog(log);
            ImportBase(rawBase);
        }

        private void buttonBaseImportFromProtocol_Click(object sender, EventArgs e)
        {
            if (openFileDialogBaseFromProtocol.ShowDialog() != DialogResult.OK)
            {
                SendLog("Импорт отменен");
                return;
            }
            string json = File.ReadAllText(openFileDialogBaseFromProtocol.FileName);
            string msgLog = "Импорт базы из файла протакола...";
            string rawBase = ProtocolParser.ParseRawProtocol(json, ref msgLog);
            SendLog(msgLog);
            ImportBase(rawBase);
        }

        private void tabPageGroups_Click(object sender, EventArgs e)
        {

        }
    }
}
