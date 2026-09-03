using Newtonsoft.Json.Linq;
using SportOrgMultyDay.Data;
using SportOrgMultyDay.Data.Combine;
using SportOrgMultyDay.Data.SportOrg;
using SportOrgMultyDay.Helpers;
using SportOrgMultyDay.Processing;
using SportOrgMultyDay.Processing.FTP;
using SportOrgMultyDay.Processing.Orgeo;
using SportOrgMultyDay.Processing.Parsing.Things;
using SportOrgMultyDay.Processing.SFR;
using SportOrgMultyDay.Processing.SFRSmartTerminal;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO.Compression;
using System.Reflection.Metadata;
using System.Text;
using System.Windows.Forms;
using static SportOrgMultyDay.Data.QualificationNames;
using static SportOrgMultyDay.Processing.Logger;
using static SportOrgMultyDay.Processing.Parsing.ParseBase;
using static SportOrgMultyDay.Processing.Parsing.ParseData;
using static SportOrgMultyDay.Processing.Parsing.ParseGroup;
using static SportOrgMultyDay.Processing.Parsing.ParseOrganization;
using static SportOrgMultyDay.Processing.Parsing.ParsePerson;
using static SportOrgMultyDay.Processing.Parsing.ParseResult;
using static SportOrgMultyDay.Processing.Parsing.Things.ParseStartTime;

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
        OrganizationItemsController organizationItemsController = new();

        PersonStartMinute PersonStartMinuteSelected;
        List<PersonStartMinute> PersonStartMinutes = new List<PersonStartMinute>();

        Panel panelStartMinutesMultiDay;
        bool[] StartMinutesMultiDayEnabled;
        List<int> StartMinutesMultiDaySplitterDistances;

        JToken ChessPersonSelected;
        ShahmatkaGrid ChessGrid;

        Dictionary<string, string> splitterStartLog = new()
        {
            { "space"," " },
            { "\\n","\n" },
            { ";",";" }
        };

        string ipsPath = "ips.txt";

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
            if (saveFileDialogJson.ShowDialog() != DialogResult.OK)
                return;

            try
            {
                string ojson = savingBase.ToString();
                string saveJ = ResaveToJsonUnicode.Convert(ojson);
                string filePath = saveFileDialogJson.FileName;

                if (checkBoxIsSaveToGzip.Checked)
                {
                    using (var fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                    using (var gzip = new GZipStream(fs, CompressionMode.Compress))
                    using (var writer = new StreamWriter(gzip))
                    {
                        writer.Write(saveJ);
                    }

                    SendLog("💾 Файл сохранён в GZIP-сжатом виде (.json)");
                }
                else
                {
                    File.WriteAllText(filePath, saveJ);
                    SendLog("💾 Файл сохранён как обычный JSON");
                }
            }
            catch (Exception ex)
            {
                SendLog($"⚠ Ошибка при экспорте JSON: {ex.Message}");
                LogError("ExportJson", ex);
            }
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

                string filePath = openFileDialogJson.FileName;
                string json;

                // Определяем, GZip это или обычный текст
                using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    bool isGzip = fs.Length > 2 &&
                                  fs.ReadByte() == 0x1F && // GZIP header bytes
                                  fs.ReadByte() == 0x8B;

                    fs.Position = 0; // возвращаемся в начало

                    if (isGzip)
                    {
                        using (GZipStream gzip = new(fs, CompressionMode.Decompress))
                        using (StreamReader reader = new(gzip))
                        {
                            json = reader.ReadToEnd();
                        }

                        SendLog("📦 Обнаружен GZIP-файл, выполнена распаковка");
                    }
                    else
                    {
                        using StreamReader reader = new StreamReader(fs);
                        json = reader.ReadToEnd();
                    }
                }

                ImportBase(json);
            }
            catch (Exception ex)
            {
                SendLog($"⚠ Ошибка импорта базы: {ex.Message}");
                LogError("ImportBase", ex);
            }
        }

        private void ImportBase(string rawBaseJson)
        {
            try
            {
                JObject rawJBase = ParseJson(rawBaseJson);
                JObject newJbase = new();

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
                    newJbase = rawJBase;
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

                    JArray jArrayRaces = new();
                    newJbase.Add("current_race", 0);
                    newJbase.Add("version", "1.6.0.0");

                    jArrayRaces.Add(rawJBase);
                    newJbase.Add("races", jArrayRaces);

                }

                JBase = newJbase;

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
            ReloadOrganizationsNameListInComboboxStartMinuters(currRace);
            comboBoxSourceRankGroupName.Items.Clear();
            foreach (JToken group in PBGroups(currRace))
            {
                comboBoxSourceRankGroupName.Items.Add(new ComboBoxItemId(PGId(group), PGName(group)));
            }
            PersonStartMinuteSelected = null;
            dataGridViewPersonMinutes.DataSource = null;

            ChessPersonSelected = null;
            ChessGrid = null;
            if (tabControl1.SelectedTab == tabPageChess)
                ReloadShahmatka();
            else
            {
                dataGridViewChess.DataSource = null;
                ReloadChessSelectedPerson();
            }
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab == tabPageChess && ChessGrid == null && JBase != null)
                ReloadShahmatka();
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

        private void ReloadOrganizationsNameListInComboboxStartMinuters(JToken currRace = null)
        {
            currRace ??= PBCurrentRaceFromBase(JBase);
            JArray persons = PBPersons(currRace);
            JArray groups = PBGroups(currRace);
            Dictionary<string, int> groupIdCount = DictGIdPersonsCount(groups, persons);



            List<ComboBoxItemId> orgNames = new();
            foreach (JToken group in groups)
            {
                string groupId = PGId(group);
                string countIsValid = groupIdCount.TryGetValue(groupId, out int count) ? "" : "?";
                orgNames.Add(new(groupId, $"{PGName(group)} - {count}{countIsValid}"));
            }

            orgNames.Sort((a, b) => a.Name.CompareTo(b.Name));

            comboBoxStartMinutesGroupSelect.Items.Clear();
            comboBoxStartMinutesGroupSelect.Items.AddRange(orgNames.ToArray());
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
            buttonShiftStartMinutesBelow.Enabled = active;
            buttonShiftAllStartMinutesBack.Enabled = active;
            buttonShiftAllStartMinutesForward.Enabled = active;
            buttonApplyGroupStartInterval.Enabled = active;
            buttonImportFromYarfso.Enabled = active;
            buttonGroupSetNumbersByGroups.Enabled = active;
            buttonSyncOrganizations.Enabled = active;
            buttonOrganizationTweaksLoad.Enabled = active;
            buttonOrganizationTweaksSave.Enabled = active;
            buttonAddOrganizationTweakItem.Enabled = active;
            buttonOrganizationRename.Enabled = active;
            buttonMapCountCalculateCurrent.Enabled = active;
            buttonGroupCourseNamesFormat.Enabled = active;
            comboBoxStartMinutesGroupSelect.Enabled = active;
            comboBoxShiftStartMinutesScope.Enabled = active;
            numericUpDownShiftStartMinutes.Enabled = active;
            dateTimePickerShiftAllStartMinutes.Enabled = active;
            dateTimePickerGroupStartInterval.Enabled = active;
            buttonImportKodRegionsFromCsv.Enabled = active;
            buttonMapCountCalculateAll.Enabled = active;
            buttonReplaceAllPersonsForOtherDays.Enabled = active;
            buttonBibsAutoCreateListNumbering.Enabled = active;
            buttonCalculatePersonStartPrice.Enabled = active;
            buttonExportSFRx.Enabled = active;
            buttonInportResultsFromAnothrBase.Enabled = active;
            buttonFindCoursesForGroups.Enabled = active;
            buttonRemovePersonDuplicates.Enabled = active;
            buttonSetAllDaysToComment.Enabled = active;
            buttonGroupRemoveGetList.Enabled = active;
            buttonPhoneFtpGetLogs.Enabled = active;
            buttonPhoneFtpSaveIps.Enabled = active;
            buttonPhoneFtpSendBase.Enabled = active;
            buttonGroupRemoveIfNotInList.Enabled = active;
            buttonOrganizationCreateReserveOrg.Enabled = active;
            buttonChipRentFromComment.Enabled = active;
            buttonSetMinutesMasstart.Enabled = active;
            buttonRemovePresonsWithOutResultsInAnyDay.Enabled = active;
            buttonGroupRemoveByPrice.Enabled = active;
            buttonPayidToView.Enabled = active;
            buttonImportEstafetRequestsInWorldCodeFromAnotherBase.Enabled = active;
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
        private void buttonRemovePresonsWithOutResultsInAnyDay_Click(object sender, EventArgs e)
        {
            SendLog(RemovePersonsWithoutResultsInAnyDay.Remove(JBase));
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
        private void SendSubLog(string message, bool isNextLine = true)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => SendSubLog(message, isNextLine)));
            }
            else
            {
                message += isNextLine ? "\n" : "";
                richTextBoxLog.Text += message;
                richTextBoxLog.ScrollToCaret();
            }
        }

        private void Utils_Load(object sender, EventArgs e)
        {
            BaseEditButtons(false);

            PhoneFtpLoadIps();

            for (int i = 1; i <= 4; i++)
                comboBoxLogType.Items.Add((EStartLogType)i);
            comboBoxLogType.SelectedIndex = 0;
            foreach (string s in splitterStartLog.Keys)
                comboBoxStartLogOutFieldsSplitter.Items.Add(s);
            comboBoxStartLogOutFieldsSplitter.SelectedIndex = 0;
            comboBoxShiftStartMinutesScope.Items.AddRange(new string[] { "Группа", "Коридор" });
            comboBoxShiftStartMinutesScope.SelectedIndex = 1;

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
            autoResize.Add(dataGridViewPersonMinutes, true, true);
            autoResize.Add(dataGridViewChess, true, true);

            textBoxShahmatkaDateFilter.Text = DateTime.Now.ToString("dd.MM.yyyy");
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

        private void buttonExportSFRx_Click(object sender, EventArgs e)
        {
            JToken race = PBCurrentRaceFromBase(JBase);
            JToken data = PBData(race);
            string race_date = PDStartDate(data);
            saveFileDialogSFRx.FileName = race_date;
            if (saveFileDialogSFRx.ShowDialog() != DialogResult.OK) return;
            string sftxTxt = SFRxManager.RaceToSFRx(out string log, race);
            SendLog(log);
            File.WriteAllText(saveFileDialogSFRx.FileName, sftxTxt, Encoding.UTF8); // без BOM
            if (checkBoxShahmatkaExtendedLogs.Checked)
                SendLog($"Экспорт SFRx...\nСохранено в файл: {saveFileDialogSFRx.FileName}\n==========================\n{sftxTxt}\n==========================");
        }

        private void comboBoxDays_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!int.TryParse(comboBoxDays.Text.ToString(), out int day)) return;
            day--;

            if (day < raceCount && day >= 0)
            {
                JBase["current_race"] = day;
                BaseDayChange();
                SendLog($"Текущий день: {(int)JBase["current_race"] + 1}");
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
                splitterStartLog.TryGetValue(comboBoxStartLogOutFieldsSplitter.Text, out string val) ? val : comboBoxStartLogOutFieldsSplitter.Text, textBoxShahmatkaDateFilter.Text);
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
                SendLog(YarfsoParser.ImportFromYarfso(JBase, file,
                    checkBoxPayAmountToComment.Checked,
                    checkBoxYarfsoParserPayAmountToWorldCode.Checked,
                    checkBoxYarfsoParserReplaceQual.Checked,
                    checkBoxYarfsoParserWriteOldQual.Checked
                    ));
            }
            catch (Exception ex)
            {
                LogError("d9283hdvak", ex);
                SendLog("ERROR buttonImportFromYarfso_Click() вызвало ошибку");
            }
        }

        private void buttonSetStartMinutes_Click(object sender, EventArgs e)
        {
            int currentRaceId = checkBoxSetStartTimeOnlyCurrentDayPersons.Checked ? CurrentRaceID(JBase) : -1;

            JToken currentRace = PBCurrentRaceFromBase(JBase);

            if (richTextBoxGroupStartOrder.Text.Length == 0)
                SendLog(StartTimeManager.SetStartTimes(currentRace,
                    currentRaceId,
                    dateTimePickerStartTime.Value.TimeOfDay,
                    dateTimePickerStartInterval.Value.TimeOfDay,
                    checkBoxStartTimesPersonShuffle.Checked,
                    checkBoxSetStartTimeSuffleWithOrgs.Checked,
                    (int)numericUpDownSetStartTimeMinGap.Value));
            else
            {
                if (checkBoxUseShortStartTimeAlg.Checked)
                    SendLog(StartTimeManager.ShortOrderedSetStartTimes(currentRace,
                        currentRaceId,
                        dateTimePickerStartTime.Value.TimeOfDay,
                        dateTimePickerStartInterval.Value.TimeOfDay,
                        dateTimePickerMinColumnStartInterval.Value.TimeOfDay,
                        richTextBoxGroupStartOrder.Text,
                        checkBoxStartTimesPersonShuffle.Checked,
                        checkBoxSetStartTimeSuffleWithOrgs.Checked,
                        (int)numericUpDownSetStartTimeMinGap.Value));
                else
                    SendLog(StartTimeManager.OrderedSetStartTimes(currentRace,
                        currentRaceId,
                        dateTimePickerStartTime.Value.TimeOfDay,
                        dateTimePickerStartInterval.Value.TimeOfDay,
                        richTextBoxGroupStartOrder.Text,
                        checkBoxStartTimesPersonShuffle.Checked,
                        checkBoxSetStartTimeSuffleWithOrgs.Checked,
                        (int)numericUpDownSetStartTimeMinGap.Value));

            }
            JToken lastPerson = StartTimeManager.LastStartPerson(currentRace);
            SendLog("Последний стартующий участник - " + StartTimeManager.StartPersonToString(lastPerson));
            ReloadStartMinutes();
            ReloadShahmatka();
            buttonSetStartMinutes.Text = $"{dateTimePickerStartTime.Value.TimeOfDay} - {PPStartTimeTS(lastPerson).Value}";
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
            JToken race = PBCurrentRaceFromBase(JBase);
            JArray groups = PBGroups(race);
            JArray persons = PBPersons(race);
            Dictionary<string, int> groupIdCount = DictGIdPersonsCount(groups, persons);

            StringBuilder sb = new();
            string log = "Автоматическое формирование списка номеров...\n";
            foreach (JToken group in groups)
            {
                string groupId = PGId(group);
                string groupName = PGName(group);
                if (string.IsNullOrWhiteSpace(groupName))
                    continue;

                int count = groupIdCount.TryGetValue(groupId, out int c) ? c : 0;
                if (count == 0)
                {
                    log += $"  Группа {groupName} пропущена, нет участников\n";
                    continue;
                }

                int reserv = (int)Math.Ceiling(count / 10.0);
                sb.AppendLine($"{groupName} * r:{reserv}");
                log += $"  {groupName}: участников {count}, резерв {reserv} (номер будет присвоен автоматически по порядку)\n";
            }

            richTextBoxBibsNumbering.Text = sb.ToString();
            log += "Список сформирован без распределения пулов номеров: * означает, что номера для группы будут взяты по порядку (продолжая после предыдущей группы) в момент присвоения номеров кнопкой присвоения. Проверьте список и нажмите кнопку присвоения номеров.\n";
            SendLog(log);
        }

        private void buttonGroupSetNumbersByGroups_Click(object sender, EventArgs e)
        {
            int day = ((int)numericUpDownSetNumbersInActiveDayFirst.Value - 1);
            SendLog(BibsNumbering.SetNumbers(PBCurrentRaceFromBase(JBase), richTextBoxBibsNumbering.Text, checkBoxSetNumbersByGroupsDebug.Checked, checkBoxSetNumbersRelay.Checked, checkBoxSetNumbersCreateReserv.Checked, day));
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

        private void buttonImportKodRegionsFromCsv_Click(object sender, EventArgs e)
        {
            if (openFileDialogCsvUTF8.ShowDialog() != DialogResult.OK)
            {
                SendLog("Импорт отменен");
                return;
            }
            string csv = File.ReadAllText(openFileDialogCsvUTF8.FileName);
            SendLog(OrgeoCsvParser.AddRegionsKodToOrgs(PBCurrentRaceFromBase(JBase), csv, checkBoxRenameOrgsImportKodRegionsFromCsv.Checked));
        }

        private void buttonImportCommentsFromCSV_Click(object sender, EventArgs e)
        {
            if (openFileDialogCsvWO.ShowDialog() != DialogResult.OK)
            {
                SendLog("Импорт отменен");
                return;
            }
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            string csv = File.ReadAllText(openFileDialogCsvWO.FileName, Encoding.GetEncoding(1251));
            OrgeoCsvCommentImporter orgeoCsvCommentImporter = new(csv);
            orgeoCsvCommentImporter.InsertToComments(PBCurrentRaceFromBase(JBase), csv);
            SendLog(orgeoCsvCommentImporter.GetLog());
        }

        private void buttonMapCountCalculate_Click(object sender, EventArgs e)
        {

        }

        private void buttonGroupCurseNamesFormat_Click(object sender, EventArgs e)
        {
            SendLog(GroupCourseNameFormater.FormatAll(JBase, checkBoxCombineCourse.Checked, checkBoxRenameCourse.Checked, checkBoxRenameGroups.Checked));

        }

        private void buttonMapCountCalculateCurrent_Click(object sender, EventArgs e)
        {
            SendLog(MapCounter.CalculateCurrentRaceCount(JBase, checkBoxMapCountCalculateOnlyInDay.Checked, checkBoxMapCountCalculateReserv.Checked));
        }

        private void buttonMapCountCalculateAll_Click(object sender, EventArgs e)
        {
            SendLog(MapCounter.CalculateAllRaceCount(JBase, checkBoxMapCountCalculateOnlyInDay.Checked, checkBoxMapCountCalculateReserv.Checked));
        }

        private void buttonStartMinutesLoad_Click(object sender, EventArgs e)
        {
        }

        private void groupBoxStartTime_Enter(object sender, EventArgs e)
        {

        }

        private void comboBoxStartMinutesGroupSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            ReloadStartMinutes();
        }
        private void ReloadStartMinutes()
        {
            if (comboBoxStartMinutesGroupSelect.SelectedItem == null)
                return;
            string selectedGroupId = ((ComboBoxItemId)comboBoxStartMinutesGroupSelect.SelectedItem).Id;
            JToken race = PBCurrentRaceFromBase(JBase);
            JArray groups = PBGroups(race);
            JToken group = FGById(selectedGroupId, groups);

            if (checkBoxStartMinutesMultiDay.Checked)
                ReloadStartMinutesMultiDay(PGName(group));
            else
                ReloadStartMinutes(group);
        }

        private void checkBoxStartMinutesMultiDay_CheckedChanged(object sender, EventArgs e)
        {
            bool multiDay = checkBoxStartMinutesMultiDay.Checked;
            EnsureMultiDayPanel();
            dataGridViewPersonMinutes.Visible = !multiDay;
            panelStartMinutesMultiDay.Visible = multiDay;
            ReloadStartMinutes();
        }

        private void EnsureMultiDayPanel()
        {
            if (panelStartMinutesMultiDay != null)
                return;

            panelStartMinutesMultiDay = new Panel
            {
                Location = dataGridViewPersonMinutes.Location,
                Size = dataGridViewPersonMinutes.Size,
                Visible = false,
            };
            tabPage2.Controls.Add(panelStartMinutesMultiDay);
            autoResize.Add(panelStartMinutesMultiDay, true, true);
        }

        private static Control BuildDayColumns(List<Panel> dayPanels, int index)
        {
            Panel dayPanel = dayPanels[index];
            dayPanel.Dock = DockStyle.Fill;

            if (index == dayPanels.Count - 1)
                return dayPanel;

            SplitContainer split = new()
            {
                Dock = DockStyle.Fill,
                Orientation = Orientation.Vertical,
                SplitterWidth = 6,
                Panel1MinSize = 80,
                Panel2MinSize = 80,
            };
            split.Panel1.Controls.Add(dayPanel);
            split.Panel2.Controls.Add(BuildDayColumns(dayPanels, index + 1));
            return split;
        }

        private static List<int> CaptureSplitterDistances(Control root)
        {
            List<int> distances = new();
            while (root is SplitContainer split)
            {
                distances.Add(split.SplitterDistance);
                root = split.Panel2.Controls.Count > 0 ? split.Panel2.Controls[0] : null;
            }
            return distances;
        }

        // SplitContainers only report real widths once parented and laid out, so
        // SplitterDistance has to be applied top-down after attaching to the form -
        // setting it right after construction (default ~150px wide) silently fails
        // and resets to a WinForms default, which looked like widths "resetting".
        private static void ApplySplitterDistances(Control root, List<int> splitterDistances)
        {
            if (splitterDistances == null)
                return;

            int index = 0;
            while (root is SplitContainer split && index < splitterDistances.Count)
            {
                try { split.SplitterDistance = splitterDistances[index]; } catch { }
                root = split.Panel2.Controls.Count > 0 ? split.Panel2.Controls[0] : null;
                index++;
            }
        }

        private void WireStartMinutesSwap(DataGridView grid, List<PersonStartMinute> list, Label statusLabel)
        {
            PersonStartMinute selected = null;

            void UpdateStatusLabel()
            {
                if (selected != null)
                {
                    statusLabel.ForeColor = Color.Black;
                    statusLabel.Text = $"Выбран: {selected.FullName} {selected.StartMinute}";
                }
                else
                {
                    statusLabel.ForeColor = Color.Gray;
                    statusLabel.Text = "Выберите участника (ПКМ)";
                }
            }

            UpdateStatusLabel();

            grid.CellMouseUp += (s, e) =>
            {
                if (e.Button == MouseButtons.Right && e.RowIndex >= 0 && e.ColumnIndex >= 0)
                {
                    PersonStartMinute psm = (PersonStartMinute)grid.Rows[e.RowIndex].DataBoundItem;
                    if (psm == null)
                    {
                        SendLog("Не удалось получить участника из этой строки");
                        return;
                    }

                    if (selected == null)
                    {
                        grid.ClearSelection();
                        grid.Rows[e.RowIndex].Cells[e.ColumnIndex].Selected = true;
                        selected = psm;
                    }
                    else
                    {
                        grid.ClearSelection();
                        TimeSpan selectedStartTime = selected.StartMinute;
                        TimeSpan clickedStartTime = psm.StartMinute;
                        psm.StartMinute = selectedStartTime;
                        selected.StartMinute = clickedStartTime;
                        SendLog($"Поменяли местами стартовые минуты [{psm.FullName}] {psm.StartMinute} и [{selected.FullName}] {selected.StartMinute}");
                        selected = null;

                        list.Sort((a, b) => a.StartMinute.CompareTo(b.StartMinute));
                        grid.Refresh();
                    }
                    UpdateStatusLabel();
                }
                else if (e.Button == MouseButtons.Left)
                {
                    selected = null;
                    UpdateStatusLabel();
                }
            };
        }

        private void ReloadStartMinutesMultiDay(string groupName)
        {
            EnsureMultiDayPanel();
            panelStartMinutesMultiDay.SuspendLayout();

            if (panelStartMinutesMultiDay.Controls.Count > 0)
                StartMinutesMultiDaySplitterDistances = CaptureSplitterDistances(panelStartMinutesMultiDay.Controls[0]);

            panelStartMinutesMultiDay.Controls.Clear();

            JArray races = PBRaces(JBase);
            if (StartMinutesMultiDayEnabled == null || StartMinutesMultiDayEnabled.Length != races.Count)
                StartMinutesMultiDayEnabled = Enumerable.Repeat(true, races.Count).ToArray();

            Dictionary<int, string> qualsDict = QualificationNames.DictIdToString;
            List<Panel> dayPanels = new();

            for (int dayIndex = 0; dayIndex < races.Count; dayIndex++)
            {
                JToken race = races[dayIndex];
                JArray groups = PBGroups(race);
                JToken group = groups.FirstOrDefault(g => PGName(g) == groupName);

                List<PersonStartMinute> dayList = new();
                if (group != null)
                {
                    JArray persons = PBPersons(race);
                    JArray orgs = PBOrganizations(race);
                    List<JToken> groupPersons = FPAllByGroup(PGId(group), persons);
                    foreach (JToken groupPerson in groupPersons)
                    {
                        if (PPStartTime(groupPerson) == 0)
                            continue;
                        string orgId = PPOrganizationId(groupPerson);
                        JToken org = FOById(orgId, orgs);
                        string orgName = POName(org);
                        string qual = qualsDict[PPQual(groupPerson)];
                        dayList.Add(new(groupPerson, orgName, qual));
                    }
                    dayList.Sort((a, b) => a.StartMinute.CompareTo(b.StartMinute));
                }

                Panel dayPanel = new();

                int capturedIndex = dayIndex;
                bool dayEnabled = StartMinutesMultiDayEnabled[dayIndex];

                DataGridView dayGrid = new()
                {
                    Dock = DockStyle.Fill,
                    ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize,
                    DataSource = dayList,
                    Visible = dayEnabled,
                };

                Label dayStatusLabel = new()
                {
                    Dock = DockStyle.Top,
                    AutoSize = false,
                    Height = 25,
                    TextAlign = ContentAlignment.MiddleLeft,
                };
                WireStartMinutesSwap(dayGrid, dayList, dayStatusLabel);

                CheckBox dayCheckBox = new()
                {
                    Dock = DockStyle.Top,
                    AutoSize = true,
                    Checked = dayEnabled,
                    Text = group == null ? $"День {dayIndex + 1} (нет группы)" : $"День {dayIndex + 1}",
                };
                dayCheckBox.CheckedChanged += (s, e) =>
                {
                    StartMinutesMultiDayEnabled[capturedIndex] = dayCheckBox.Checked;
                    dayGrid.Visible = dayCheckBox.Checked;
                };

                dayPanel.Controls.Add(dayGrid);
                dayPanel.Controls.Add(dayStatusLabel);
                dayPanel.Controls.Add(dayCheckBox);

                dayPanels.Add(dayPanel);
            }

            if (dayPanels.Count > 0)
            {
                Control root = BuildDayColumns(dayPanels, 0);
                root.Dock = DockStyle.Fill;
                panelStartMinutesMultiDay.Controls.Add(root);

                // Resume (forcing the deferred layout) before applying distances -
                // SplitContainers only get their real width once actually laid out
                // inside the parent, and setting SplitterDistance any earlier
                // silently fails against their default unparented size.
                panelStartMinutesMultiDay.ResumeLayout(true);

                List<int> distances = StartMinutesMultiDaySplitterDistances
                    ?? Enumerable.Repeat(300, dayPanels.Count - 1).ToList();
                ApplySplitterDistances(root, distances);
            }
            else
            {
                panelStartMinutesMultiDay.ResumeLayout();
            }
        }
        private void ReloadStartMinutes(JToken group)
        {
            PersonStartMinutes = new List<PersonStartMinute>();
            JToken race = PBCurrentRaceFromBase(JBase);
            JArray persons = PBPersons(race);
            JArray orgs = PBOrganizations(race);
            List<JToken> groupPersons = FPAllByGroup(PGId(group), persons);
            Dictionary<int, string> qualsDict = QualificationNames.DictIdToString;
            foreach (JToken groupPerson in groupPersons)
            {
                if (PPStartTime(groupPerson) == 0)
                    continue;
                string orgId = PPOrganizationId(groupPerson);
                JToken org = FOById(orgId, orgs);
                string orgName = POName(org);
                string qual = qualsDict[PPQual(groupPerson)];
                PersonStartMinutes.Add(new(groupPerson, orgName, qual));
            }
            PersonStartMinutes.Sort((a, b) => a.StartMinute.CompareTo(b.StartMinute));
            dataGridViewPersonMinutes.DataSource = PersonStartMinutes;
        }

        private void ReloadSelectedStartMinute()
        {
            if (PersonStartMinuteSelected != null)
            {

                labelStartMinutesSelectedPerson.ForeColor = Color.Black;
                labelStartMinutesSelectedPerson.Text = $"Выбран: {PersonStartMinuteSelected.FullName} {PersonStartMinuteSelected.StartMinute}";
            }
            else
            {
                labelStartMinutesSelectedPerson.ForeColor = Color.Gray;
                labelStartMinutesSelectedPerson.Text = "Выберите первого участника (ПКМ)";
            }
        }

        private void ReloadShahmatka()
        {
            Dictionary<string, int> columnWidths = dataGridViewChess.Columns
                .Cast<DataGridViewColumn>()
                .ToDictionary(c => c.Name, c => c.Width);

            JToken race = PBCurrentRaceFromBase(JBase);
            ChessGrid = ShahmatkaManager.BuildGrid(race, checkBoxChessBib.Checked, checkBoxChessGroup.Checked, checkBoxChessSurname.Checked, checkBoxChessQual.Checked);
            dataGridViewChess.DataSource = ChessGrid.Table;

            foreach (DataGridViewColumn column in dataGridViewChess.Columns)
            {
                if (columnWidths.TryGetValue(column.Name, out int width))
                    column.Width = width;
            }

            for (int rowIndex = 0; rowIndex < ChessGrid.RowsPersons.Count; rowIndex++)
            {
                List<JToken>[] rowPersons = ChessGrid.RowsPersons[rowIndex];
                for (int corridorIndex = 0; corridorIndex < rowPersons.Length; corridorIndex++)
                {
                    if (rowPersons[corridorIndex]?.Count > 1)
                        dataGridViewChess.Rows[rowIndex].Cells[corridorIndex + 1].Style.BackColor = Color.Yellow;
                }
            }

            ChessPersonSelected = null;
            ReloadChessSelectedPerson();
        }

        private void buttonChessRefresh_Click(object sender, EventArgs e)
        {
            ReloadShahmatka();
        }

        private void checkBoxChessMode_CheckedChanged(object sender, EventArgs e)
        {
            ReloadShahmatka();
        }

        private void ReloadChessSelectedPerson()
        {
            if (ChessPersonSelected != null)
            {
                labelChessSelectedPerson.ForeColor = Color.Black;
                labelChessSelectedPerson.Text = $"Выбран: {PPSurnameName(ChessPersonSelected)}";
            }
            else
            {
                labelChessSelectedPerson.ForeColor = Color.Gray;
                labelChessSelectedPerson.Text = "Выберите первого участника (ПКМ)";
            }
        }

        private void dataGridViewChess_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ChessPersonSelected = null;
                ReloadChessSelectedPerson();
                return;
            }

            if (e.Button != MouseButtons.Right || e.RowIndex < 0 || e.ColumnIndex <= 0 || ChessGrid == null)
                return;

            List<JToken> personsInCell = ChessGrid.PersonsAt(e.RowIndex, e.ColumnIndex - 1);
            if (personsInCell == null || personsInCell.Count == 0)
            {
                if (ChessPersonSelected == null)
                {
                    SendLog("В этой ячейке нет участников");
                    return;
                }

                int clickedCorridor = ChessGrid.Corridors[e.ColumnIndex - 1];
                int selectedCorridor = ChessPersonCorridor(ChessPersonSelected);
                if (clickedCorridor != selectedCorridor)
                {
                    SendLog("Нельзя перенести участника в другой коридор (другую группу)");
                    return;
                }

                TimeSpan targetTime = ChessGrid.RowTimes[e.RowIndex];
                SendLog($"Участник [{PPSurnameName(ChessPersonSelected)}] перенесён на {targetTime}");
                ChessPersonSelected["start_time"] = targetTime.TotalMilliseconds;

                ChessPersonSelected = null;
                ReloadShahmatka();
                return;
            }

            if (personsInCell.Count == 1)
            {
                ChessPickPerson(personsInCell[0]);
                return;
            }

            ContextMenuStrip menu = new();
            foreach (JToken person in personsInCell)
            {
                JToken pickedPerson = person;
                menu.Items.Add($"{PPBib(person)} {PPSurnameName(person)}", null, (s, args) => ChessPickPerson(pickedPerson));
            }
            menu.Show(Cursor.Position);
        }

        private int ChessPersonCorridor(JToken person)
        {
            JToken race = PBCurrentRaceFromBase(JBase);
            JArray groups = PBGroups(race);
            JToken group = FGById(PPGroupId(person), groups);
            return group == null ? -1 : PGStartCorridor(group);
        }

        private void ChessPickPerson(JToken person)
        {
            if (ChessPersonSelected == null)
            {
                ChessPersonSelected = person;
                ReloadChessSelectedPerson();
                return;
            }

            if (ChessPersonSelected == person)
            {
                ChessPersonSelected = null;
                ReloadChessSelectedPerson();
                return;
            }

            TimeSpan selectedStartTime = PPStartTimeTS(ChessPersonSelected) ?? TimeSpan.Zero;
            TimeSpan clickedStartTime = PPStartTimeTS(person) ?? TimeSpan.Zero;
            SendLog($"Поменяли местами стартовые минуты [{PPSurnameName(person)}] {clickedStartTime} и [{PPSurnameName(ChessPersonSelected)}] {selectedStartTime}");
            person["start_time"] = selectedStartTime.TotalMilliseconds;
            ChessPersonSelected["start_time"] = clickedStartTime.TotalMilliseconds;

            ChessPersonSelected = null;
            ReloadShahmatka();
        }

        private void dataGridViewPersonMinutes_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {

                PersonStartMinute psm = (PersonStartMinute)dataGridViewPersonMinutes.Rows[e.RowIndex].DataBoundItem;
                if (psm == null)
                {
                    SendLog("Не удалось получить участника из этой строки");
                    return;
                }
                if (PersonStartMinuteSelected == null)
                {
                    dataGridViewPersonMinutes.ClearSelection();
                    dataGridViewPersonMinutes.Rows[e.RowIndex].Cells[e.ColumnIndex].Selected = true;
                    PersonStartMinuteSelected = psm;
                }
                else
                {
                    dataGridViewPersonMinutes.ClearSelection();
                    TimeSpan selectedStartTime = PersonStartMinuteSelected.StartMinute;
                    TimeSpan clickedStartTime = psm.StartMinute;
                    psm.StartMinute = selectedStartTime;
                    PersonStartMinuteSelected.StartMinute = clickedStartTime;
                    SendLog($"Поменяли местами стартовые минуты [{psm.FullName}] {psm.StartMinute} и [{PersonStartMinuteSelected.FullName}] {PersonStartMinuteSelected.StartMinute}");

                    PersonStartMinuteSelected = null;



                    PersonStartMinutes.Sort((a, b) => a.StartMinute.CompareTo(b.StartMinute));
                    dataGridViewPersonMinutes.Refresh();
                }
                ReloadSelectedStartMinute();
            }
            else if (e.Button == MouseButtons.Left)
            {
                PersonStartMinuteSelected = null;
                ReloadSelectedStartMinute();
            }

        }

        private void buttonShiftStartMinutesBelow_Click(object sender, EventArgs e)
        {
            if (PersonStartMinuteSelected == null)
            {
                SendLog("Для сдвига стартовых минут сначала выберите участника правой кнопкой мыши");
                ReloadSelectedStartMinute();
                return;
            }

            JToken race = PBCurrentRaceFromBase(JBase);
            JArray persons = PBPersons(race);
            JArray groups = PBGroups(race);
            string selectedGroupId = PPGroupId(PersonStartMinuteSelected.Person);
            JToken selectedGroup = FGById(selectedGroupId, groups);
            int selectedCorridor = PGStartCorridor(selectedGroup);
            TimeSpan fromStartTime = PersonStartMinuteSelected.StartMinute;
            TimeSpan shift = TimeSpan.FromMinutes((double)numericUpDownShiftStartMinutes.Value);
            bool shiftOnlyGroup = comboBoxShiftStartMinutesScope.SelectedIndex == 0;

            int shiftedCount = 0;
            foreach (JToken person in persons)
            {
                if (shiftOnlyGroup)
                {
                    if (PPGroupId(person) != selectedGroupId)
                        continue;
                }
                else
                {
                    JToken personGroup = FGById(PPGroupId(person), groups);
                    if (PGStartCorridor(personGroup) != selectedCorridor)
                        continue;
                }

                TimeSpan? personStartTime = PPStartTimeTS(person);
                if (personStartTime == null || personStartTime < fromStartTime)
                    continue;

                person["start_time"] = personStartTime.Value.Add(shift).TotalMilliseconds;
                shiftedCount++;
            }

            string scopeName = shiftOnlyGroup ? $"группе {PGName(selectedGroup)}" : $"коридоре {selectedCorridor}";
            SendLog($"Сдвинули стартовые минуты в {scopeName} на {shift.TotalMinutes} мин. Начиная с {fromStartTime}: {shiftedCount} участников");
            PersonStartMinuteSelected = null;
            ReloadStartMinutes();
            ReloadSelectedStartMinute();
        }

        private void buttonShiftAllStartMinutesBack_Click(object sender, EventArgs e)
        {
            ShiftAllStartMinutes(-dateTimePickerShiftAllStartMinutes.Value.TimeOfDay);
        }

        private void buttonShiftAllStartMinutesForward_Click(object sender, EventArgs e)
        {
            ShiftAllStartMinutes(dateTimePickerShiftAllStartMinutes.Value.TimeOfDay);
        }

        private void ShiftAllStartMinutes(TimeSpan shift)
        {
            if (shift == TimeSpan.Zero)
            {
                SendLog("Сдвиг не выполнен: выбран нулевой интервал");
                return;
            }

            if (comboBoxStartMinutesGroupSelect.SelectedItem == null)
            {
                SendLog("Сдвиг не выполнен: выберите группу");
                return;
            }

            JToken race = PBCurrentRaceFromBase(JBase);
            JArray persons = PBPersons(race);
            JArray groups = PBGroups(race);
            string selectedGroupId = ((ComboBoxItemId)comboBoxStartMinutesGroupSelect.SelectedItem).Id;
            JToken selectedGroup = FGById(selectedGroupId, groups);
            int selectedCorridor = PGStartCorridor(selectedGroup);
            bool shiftOnlyGroup = comboBoxShiftStartMinutesScope.SelectedIndex == 0;

            int shiftedCount = 0;
            foreach (JToken person in persons)
            {
                if (shiftOnlyGroup)
                {
                    if (PPGroupId(person) != selectedGroupId)
                        continue;
                }
                else
                {
                    JToken personGroup = FGById(PPGroupId(person), groups);
                    if (PGStartCorridor(personGroup) != selectedCorridor)
                        continue;
                }

                if (PPStartTime(person) == 0)
                    continue;

                TimeSpan? personStartTime = PPStartTimeTS(person);
                if (personStartTime == null)
                    continue;

                person["start_time"] = personStartTime.Value.Add(shift).TotalMilliseconds;
                shiftedCount++;
            }

            string scopeName = shiftOnlyGroup ? $"группы {PGName(selectedGroup)}" : $"коридора {selectedCorridor}";
            SendLog($"Сдвинули стартовые минуты {scopeName} на {shift}: {shiftedCount} участников");
            ReloadStartMinutes();
            ReloadSelectedStartMinute();
        }

        private void buttonApplyGroupStartInterval_Click(object sender, EventArgs e)
        {
            if (comboBoxStartMinutesGroupSelect.SelectedItem == null)
            {
                SendLog("Интервал группы не применен: выберите группу");
                return;
            }

            TimeSpan interval = dateTimePickerGroupStartInterval.Value.TimeOfDay;
            if (interval == TimeSpan.Zero)
            {
                SendLog("Интервал группы не применен: выбран нулевой интервал");
                return;
            }

            JToken race = PBCurrentRaceFromBase(JBase);
            JArray persons = PBPersons(race);
            JArray groups = PBGroups(race);
            string selectedGroupId = ((ComboBoxItemId)comboBoxStartMinutesGroupSelect.SelectedItem).Id;
            JToken selectedGroup = FGById(selectedGroupId, groups);
            List<JToken> groupPersons = FPAllByGroup(selectedGroupId, persons)
                .Where(person => PPStartTime(person) != 0 && PPStartTimeTS(person) != null)
                .OrderBy(person => PPStartTimeTS(person))
                .ToList();

            if (groupPersons.Count == 0)
            {
                SendLog($"Интервал группы {PGName(selectedGroup)} не применен: нет участников со стартовой минутой");
                return;
            }

            TimeSpan firstStart = PPStartTimeTS(groupPersons[0]).GetValueOrDefault();
            for (int i = 0; i < groupPersons.Count; i++)
                groupPersons[i]["start_time"] = firstStart.Add(interval * i).TotalMilliseconds;

            SendLog($"Переустановили интервал группы {PGName(selectedGroup)}: первый старт {firstStart}, интервал {interval}, участников {groupPersons.Count}");
            ReloadStartMinutes();
            ReloadSelectedStartMinute();
        }

        private void buttonReplaceAllPersonsForOtherDays_Click(object sender, EventArgs e)
        {
            SendLog(PersonListReplacer.ReplacePersonsListInOtherDays(
                JBase,
                checkBoxDeepClonePersons.Checked,
                checkBoxDeepCloneOrganizations.Checked,
                checkBoxDeepCloneGroups.Checked));
        }

        private void buttonCalculatePersonStartPrice_Click(object sender, EventArgs e)
        {
            SendLog(StartFeeCalculate.CalculatePersonStartPriceAllDays(JBase));
        }

        private void buttonVichestStart_Click(object sender, EventArgs e)
        {
            int milis = (int)TimeSpan.Parse("05:40:41").TotalMilliseconds;
            JToken race = PBCurrentRaceFromBase(JBase);
            JArray results = PBResults(race);
            for (int i = 0; i < results.Count; i++)
            {
                JToken result = results[i];

                if (int.TryParse(result["start_time"]?.ToString(), out int currentMilis) && currentMilis > 47000000)
                {
                    result["start_time"] = currentMilis - milis;
                    Debug.WriteLine(result["start_time"]);
                }
            }
        }

        private void buttonQualFromOtherBase_Click(object sender, EventArgs e)
        {
            string msgLog = "Копирование квалификаций\n";
            JToken race = PBCurrentRaceFromBase(JBase);
            JArray persons = PBPersons(race);
            OpenFileDialog openFileDialog = new();

            if (openFileDialog.ShowDialog() != DialogResult.OK)
            {
                msgLog += "Отмена.";
                SendLog(msgLog);
                return;
            }
            string fileContent = File.ReadAllText(openFileDialog.FileName);
            JObject rawJBase = ParseJson(fileContent);
            JToken fromJbase = PBCurrentRaceFromBase(rawJBase);
            JArray fromPersons = PBPersons(fromJbase);
            foreach (JToken person in persons)
            {
                JToken fromPerson = FPByPerson(person, fromPersons);
                if (fromPerson != null)
                {
                    string oldQual = person["qual"]?.ToString();
                    int newQual = PPQual(fromPerson);
                    person["qual"] = newQual;
                    msgLog += $" Разряд изменен {PPToString(person)} [{oldQual} > {newQual}]\n";
                }
                else
                {
                    msgLog += $" Участник не найден во вторичной базе {PPToString(person)}\n";
                }
            }
            SendLog(msgLog);
        }

        private void linkLabelGitHub_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            linkLabelGitHub.LinkVisited = true;
            var url = "http://github.com/ITilyaGO/SportOrgMultyDay";
            Process.Start(new ProcessStartInfo
            {
                FileName = url,
                UseShellExecute = true
            });
        }

        private void PhoneFtpLoadIps()
        {
            if (File.Exists(ipsPath))
                richTextBoxPhoneFtpIps.Text = File.ReadAllText(ipsPath);
        }

        private void PhoneFtpSaveIps()
        {
            File.WriteAllText(ipsPath, richTextBoxPhoneFtpIps.Text);
        }
        private void labelHowToWorkStartMinutesSwap_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Обмен минутами: ПКМ по одному участнику, затем ПКМ по другому.\r\n\r\nСдвиг ниже: ПКМ по участнику, выберите \"Группа\" или \"Коридор\", задайте количество минут и нажмите \"Сдвинуть ниже\". Можно указать отрицательное значение.\r\n\r\nСдвиг выбранной области: выберите группу в списке, выберите \"Группа\" или \"Коридор\", задайте время и нажмите \"Раньше\" или \"Позже\".\r\n\r\nИнтервал группы: выберите группу, задайте интервал и нажмите \"Применить интервал\". Первый участник остается на своей минуте, остальные идут в том же порядке с новым интервалом.");
        }

        private void labelHTWStartBibs_Click(object sender, EventArgs e)
        {
            string instruction = "Расширенный генератор минут участников.\r\nКаждая новая строка это \"Карридор\" или колонка группы в строке - своеборазный порядок в корридоре.\r\n\r\nКнопка \"Установить минуты на масстарт\" назначает всем участникам указанной группы одно общее стартовое время. В поле порядка групп задайте по одной группе в строке в формате \"Название группы ЧЧ:ММ:СС\", например: \"М21 10:00:00\". Если включен флажок текущего дня, время устанавливается только участникам выбранного дня, иначе — всем участникам.\r\n\r\nДля правильной авто генерации порядка старта установите коридоры группам в спорт орг. После не забудте проверишть шахматку на предмет стартующих на одной минуте из одного корридора.\r\n\r\nИспользуются два основных символа разделения - \r\n\" \" - пробел\r\n\"/\" - слеш\r\nВ начале стсроки можно написать \"+\" тогда все стартовые минуты в этом корридоре сдвинкться на 1 минуту за каждый +\r\n\r\nЕсли написать группы через пробел то минуты будут присваиваться сначала участникам одной группе зачем второй и т.д.\r\nЕсли использовать слеш то участини будут браться последовательно из разделенных таким способом групп.\r\nСлеши имеют приоритет, то есть сначала строка разделяется на части которым будут применяться минуты по очереди, но внути этих частей разделенных слешем можно использовать последовательные группы.\r\nСжимать конец старта в колонках - если выключено то при использовании слешей в итоговых минутах в каждой группе будет поддерживаться постоянный интервал старта. Включено - когда в одной из групп разделенных слешем кончатся участники, то оставшимся будут выдаваться ближайшие возможные минуты, но не меньше чем минимальный заданный интервал. \r\nПримеры \r\nЕсть группы\r\nМ21 - 3 участника\r\nЖ21 - 3 участника\r\nМ18 - 1 участник\r\nМ35 - 2 участника\r\nСтрока - \"М21 Ж21 М18 М35\" результат - М21 10:01, М21 10:02, М21 10:03, Ж21 10:04, Ж21 10:05, Ж21 10:06, М18 10:07, М35 10:08, М35 10:09\r\nСтрока - \"М21/Ж21/М18/М35\" результат - М21 10:01, Ж21 10:02, М18 10:03, М35 10:04, М21 10:05, Ж21 10:06, М35 10:07, М21 10:08, Ж21 10:09\r\nСтрока - \"М21 Ж21/М18 М35\" результат - М21 10:01, М18 10:02, М21 10:03, М35 10:04, М21 10:05, М35 10:06, Ж21 10:07, Ж21 10:08, Ж21 10:09\r\nСтрока - \"М21/Ж21/М18 М35\" результат - М21 10:01, Ж21 10:02, М18 10:03, М21 10:04, Ж21 10:05, М35 10:06, М21 10:07, Ж21 10:08, М35 10:09 \r\n\r\nЕсли выключено - Сжимать конец старта в колонках \r\nСтрока - \"Ж21 М35/М18/М21\" результат - Ж21 10:00, М18 10:01, М21 10:02, Ж21 10:03, М21 10:05, Ж21 10:06, М21 10:08, М35 10:09, М35 10:12";
            SendLog(instruction);
            MessageBox.Show(instruction);
        }

        private void buttonRemovePersonDuplicates_Click(object sender, EventArgs e)
        {
            SendLog(RemovePersonDuplicates.RemoveDuplicates(PBCurrentRaceFromBase(JBase)));
        }

        private void buttonGroupRemoveIfNotInList_Click(object sender, EventArgs e)
        {
            SendLog(RemoveGroups.RemoveGroupsIfNotInList(PBCurrentRaceFromBase(JBase), richTextBoxGroupNotRemoveList.Text));
        }

        private void buttonGroupRemoveGetList_Click(object sender, EventArgs e)
        {
            richTextBoxGroupNotRemoveList.Text = RemoveGroups.GetGroups(PBCurrentRaceFromBase(JBase));
        }

        private void buttonGroupRemoveByPrice_Click(object sender, EventArgs e)
        {
            SendLog(RemoveGroups.RemoveGroupsIfPriceNotEqual(PBCurrentRaceFromBase(JBase), 1));
        }

        private async void buttonPhoneFtpSendBase_Click(object sender, EventArgs e)
        {
            PhoneFTPManager phoneFTPManager = new PhoneFTPManager(richTextBoxPhoneFtpIps.Text, SendSubLog);
            SendLog("Запуск задачи");
            await Task.Run(() => phoneFTPManager.SendBaseToAllFromRace(PBCurrentRaceFromBase(JBase)));
        }

        private void buttonChipRentFromComment_Click(object sender, EventArgs e)
        {
            SendLog(OrgeoRentedCards.FromComment(PBCurrentRaceFromBase(JBase)));
        }

        private async void buttonPhoneFtpGetLogs_Click(object sender, EventArgs e)
        {
            PhoneFTPManager phoneFTPManager = new PhoneFTPManager(richTextBoxPhoneFtpIps.Text, SendSubLog);
            SendLog("Запуск задачи");

            string summary = await phoneFTPManager.DownloadAndArchiveLogsAsync();
            if (summary != "")
                StartLogProcess(summary);

        }

        private void buttonInportResultsFromAnothrBase_Click(object sender, EventArgs e)
        {
            Dictionary<int, string> bibIds = [];

            openFileDialogJson.ShowDialog();
            string json = File.ReadAllText(openFileDialogJson.FileName);
            JObject rawJBase = ParseJson(json);
            var curRaceNew = PBCurrentRaceFromBase(rawJBase);
            var resultsNew = PBResults(curRaceNew);


            var race = PBCurrentRaceFromBase(JBase);
            var persons = PBPersons(race);
            var results = PBResults(race);
            foreach (var person in persons)
            {
                string pid = PPId(person);
                int pbib = PPBib(person);
                bibIds.Add(pbib, pid);
            }

            foreach (var newResult in resultsNew)
            {
                int cardNumber = PRcardNumber(newResult);
                bibIds.TryGetValue(cardNumber, out string newpid);
                newResult["person_id"] = newpid;
                results.Add(newResult);
            }
        }

        private void buttonSetAllDaysToComment_Click(object sender, EventArgs e)
        {
            SendLog(CommentEditor.SetAllDaysToComment(PBCurrentRaceFromBase(JBase), richTextBoxGroupNotRemoveList.Text, "C:1234"));
        }

        private void buttonFindCoursesForGroups_Click(object sender, EventArgs e)
        {
            SendLog(GroupCourseFinder.Process(PBCurrentRaceFromBase(JBase)));
        }

        private void buttonPhoneFtpSaveIps_Click(object sender, EventArgs e)
        {
            PhoneFtpSaveIps();
            SendLog("Список IP сохранен");
        }

        private void buttonStartingFeeSerGroupPrices_Click(object sender, EventArgs e)
        {

        }

        private void buttonOrganizationCreateReserveOrg_Click(object sender, EventArgs e)
        {


            string log = "Создание организаций для резервов...";
            try
            {
                JArray races = PBRaces(JBase);
                JToken reservOrg = null;
                foreach (JToken race in races)
                {
                    JArray orgs = PBOrganizations(race);
                    JToken cr_reservOrg = orgs.FirstOrDefault(o => POName(o) == "_");
                    if (reservOrg != null && POId(reservOrg) != POId(cr_reservOrg))
                    {
                        log += "  !!!Найдены разные ID организваций резервов в разных днях. Удалите все команды с названием _ и попробуйте снова\n";
                    }
                }
                if (reservOrg == null)
                {
                    reservOrg = SportOrgTemplates.Organization;
                    reservOrg["name"] = "_";
                }
                string reservOrgId = POId(reservOrg);

                for (int i = 0; i < races.Count; i++)
                {
                    log += $"  День {i + 1}\n";
                    JToken race = races[i];
                    JArray persons = PBPersons(race);
                    JArray orgs = PBOrganizations(race);
                    orgs.Add(reservOrg.DeepClone());
                    foreach (JToken person in persons)
                    {
                        if (PPSurname(person) == textBoxReservName.Text)
                        {
                            log += $"    Найден резерв - {PPToString(person)}. Команда заменена\n";
                            person["organization_id"] = reservOrgId;
                        }
                    }
                }
            }
            catch (Exception ex) { log += $"Ошибка - {ex.Message}\n"; LogError("aqsd2fasdfsad", ex); }
            SendLog(log);
        }

        private void buttonPayidToView_Click(object sender, EventArgs e)
        {
            string log = "Установка статуса вне конкурса для неоплативших стартовый взнос...\n";
            var race = PBCurrentRaceFromBase(JBase);
            var persons = PBPersons(race);
            foreach (var person in persons)
            {
                person["is_out_of_competition"] = !PPIsPaid(person);
                log += $"  {PPToString(person)} - {(PPIsPaid(person) ? "Оплатил" : "Не оплатил")} - {(PPIsPaid(person) ? "Снимаем вне конкурса" : "Ставим вне конкурса")}\n";
            }
            SendLog(log);
        }

        private void buttonCompressAndClearHtml_Click(object sender, EventArgs e)
        {
            if (openFileDialogHtml.ShowDialog() != DialogResult.OK)
                return;

            string html = File.ReadAllText(openFileDialogHtml.FileName, Encoding.UTF8);

            ProtocolHtmlCompressor compressor = new();

            string resultHtml = compressor.CompressProtocolHtmlWithLocalPakoFile(
                html,
                "pako.min.js",
                new ProtocolHtmlCompressorSettings
                {
                    ClearPersonsSensitiveData = true,
                    ClearOrganizationsSensitiveData = true
                });

            File.WriteAllText(openFileDialogHtml.FileName + "compressed.html", resultHtml, new UTF8Encoding(false));
            SendLog("Уменьшеный протокол - " + openFileDialogHtml.FileName + "compressed.html");
        }

        private void buttonSetMinutesMasstart_Click(object sender, EventArgs e)
        {
            int currentRaceId = checkBoxSetStartTimeOnlyCurrentDayPersons.Checked ? CurrentRaceID(JBase) : -1;
            JToken race = PBCurrentRaceFromBase(JBase);
            SendLog(StartTimeManager.SetGroupStartTimes(race, currentRaceId, richTextBoxGroupStartOrder.Text));
            ReloadShahmatka();

        }

        private void buttonImportEstafetRequestsInWorldCodeFromAnotherBase_Click(object sender, EventArgs e)
        {
            Dictionary<int, string> bibIds = [];

            if (openFileDialogJson.ShowDialog() != DialogResult.OK)
            {
                SendLog("Импорт world_code отменен");
                return;
            }

            string log = $"Импорт world_code из другой базы - {openFileDialogJson.FileName}\n";
            try
            {
                string json = File.ReadAllText(openFileDialogJson.FileName);
                JObject rawJBase = ParseJson(json);
                var curRaceNew = PBCurrentRaceFromBase(rawJBase);
                var race = PBCurrentRaceFromBase(JBase);


                var personsNew = PBPersons(curRaceNew);

                var groupsNew = PBGroups(curRaceNew);
                var dictNewGroups = DictGIdGroupName(groupsNew);

                var orgsNew = PBOrganizations(curRaceNew);
                var dictNewOrgs = DictOIdOrgName(orgsNew);

                var groups = PBGroups(race);
                var dictGroups = DictGGroupNameId(groups);

                var orgs = PBOrganizations(race);
                var dictOrgs = DictOOrgNameId(orgs);

                JArray persons = PBPersons(race);
                //var results = PBResults(race);
                //foreach (var person in persons)
                //{
                //    string pid = PPId(person);
                //    int pbib = PPBib(person);
                //    bibIds.Add(pbib, pid);
                //}
                List<JToken> newPersonsToAddInOurBase = new List<JToken>();
                int updatedCount = 0;

                foreach (var newPerson in personsNew)
                {
                    bool notFound = true;

                    string NPsname = PPSurname(newPerson);
                    string NPname = PPName(newPerson);
                    string NPmname = PPMiddleName(newPerson);

                    foreach (var person in persons)
                    {
                        string sname = PPSurname(person);
                        string name = PPName(person);
                        string mname = PPMiddleName(person);
                        if (sname == NPsname && name == NPname && mname == NPmname)
                        {
                            notFound = false;
                            person["world_code"] = PPWorldCode(newPerson);
                            updatedCount++;
                            log += $"  Найден - {PPToString(person)}. world_code установлен - {PPWorldCode(newPerson)}\n";
                            break;
                        }
                    }
                    if (notFound)
                    {
                        var newGroupID = PPGroupId(newPerson);
                        var newGname = dictNewGroups[newGroupID];
                        var targetGroupId = dictGroups[newGname];
                        newPerson["group_id"] = targetGroupId;

                        //var newOrgID = PPOrganizationId(newPerson);
                        //var newOname = dictNewOrgs[newOrgID];
                        //var targetOrgId = dictOrgs[newOname];
                        //newPerson["organization_id"] = targetOrgId;


                        newPersonsToAddInOurBase.Add(newPerson);
                        log += $"  Не найден - {NPsname} {NPname} {NPmname}. Будет добавлен новым участником в группу {newGname}\n";
                    }
                }
                foreach (var person in newPersonsToAddInOurBase)
                {
                    persons.Add(person);
                }

                log += $"Готово. Обновлено world_code - {updatedCount}, добавлено новых участников - {newPersonsToAddInOurBase.Count}";
            }
            catch (Exception ex)
            {
                log += $"Ошибка - {ex.Message}";
                LogError("importEstafetRequestsWorldCode", ex);
            }
            SendLog(log);
        }
    }
}
