using Newtonsoft.Json.Linq;
using SportOrgMultyDay.Processing;
using System.Text;
using static SportOrgMultyDay.Data.QualificationNames;
using static SportOrgMultyDay.Processing.Parsing.ParseBase;
using static SportOrgMultyDay.Processing.Parsing.ParseGroup;
using static SportOrgMultyDay.Processing.Parsing.ParseOrganization;
using static SportOrgMultyDay.Processing.Parsing.ParsePerson;
using static SportOrgMultyDay.Processing.Parsing.ParseData;
using static SportOrgMultyDay.Processing.Parsing.ParseResult;
using static SportOrgMultyDay.Processing.Parsing.Things.ParseStartTime;
using static SportOrgMultyDay.Processing.Logger;
using SportOrgMultyDay.Processing.SFRSmartTerminal;
using SportOrgMultyDay.Helpers;
using SportOrgMultyDay.Data;
using SportOrgMultyDay.Processing.Parsing.Things;
using System.Reflection.Metadata;
using SportOrgMultyDay.Data.SportOrg;
using SportOrgMultyDay.Data.Combine;
using System.Diagnostics;
using System.Windows.Forms;
using System;
using System.Collections.Generic;
using SportOrgMultyDay.Processing.SFR;
using SportOrgMultyDay.Processing.FTP;

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
            buttonImportKodRegionsFromCsv.Enabled = active;
            buttonMapCountCalculateAll.Enabled = active;
            buttonReplaceAllPersonsForOtherDays.Enabled = active;
            buttonBibsAutoCreateListNumbering.Enabled = active;
            buttonCalculatePersonStartPrice.Enabled = active;
            buttonExportSFRx.Enabled = active;
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
            autoResize.Add(dataGridViewPersonMinutes, true, true);

            dateTimePickerExportStartLog.Value.AddHours(22);
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
            string instruction = ("Функция в разработке. \r\n Пишем через пробел - \"[Название группы] [Номера] r:[Резервы]\"\r\nКонечные номера и резервы писать не обязательно не обязательно.\r\n Если написать просто \"М21 100\" то будут присвоены номера с 100 но если добавить \"М18 110\" и в группе М21 будет больше 10 участников, номера переназначатся. Не допускайте пересечения номеров или исопльзуйте ограничение такого плана 100-199\r\n\r\n Пример обычных минут - \r\nМЭ 5000-5199\r\nЖЭ 100-5199\r\n\r\nПример обычных минут с резервами - \r\nМЭ 5000-5199 r:10\r\nЖЭ 100-5199 r:5\r\n\r\nПример эстафеты - \r\nМ21 101-119\r\nМ16-20 201-215\r\nМ45 301-309\r\nЖ21 401-412\r\nЖ16-20 501-504\r\nМ60 601-605\r\nЖ60 701-703\r\nМ12-14 801-816\r\nЖ12-14 901-907");
            MessageBox.Show(instruction);
            SendLog(instruction);
        }

        private void buttonGroupSetNumbersByGroups_Click(object sender, EventArgs e)
        {
            SendLog(BibsNumbering.SetNumbers(PBCurrentRaceFromBase(JBase), richTextBoxBibsNumbering.Text, checkBoxSetNumbersByGroupsDebug.Checked, checkBoxSetNumbersRelay.Checked, checkBoxSetNumbersCreateReserv.Checked));
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

        private void buttonMapCountCalculate_Click(object sender, EventArgs e)
        {

        }

        private void buttonGroupCurseNamesFormat_Click(object sender, EventArgs e)
        {
            SendLog(GroupCourseNameFormater.FormatAll(JBase, checkBoxCombineCourse.Checked));

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
            ReloadStartMinutes(group);
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

        private void buttonReplaceAllPersonsForOtherDays_Click(object sender, EventArgs e)
        {
            SendLog(PersonListReplacer.ReplacePersonsListInOtherDays(JBase));
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

        private void labelHowToWorkStartMinutesSwap_Click(object sender, EventArgs e)
        {
            MessageBox.Show("ПКМ по одному участнику, затем по другому, с которым нужно поменять стартовые минуты местами");
        }

        private void labelHTWStartBibs_Click(object sender, EventArgs e)
        {
            string instruction = "Расширенный генератор минут участников.\r\nКаждая новая строка это \"Карридор\" или колонка группы в строке - своеборазный порядок в корридоре.\r\n\r\nДля правильной авто генерации порядка старта установите коридоры группам в спорт орг. После не забудте проверишть шахматку на предмет стартующих на одной минуте из одного корридора.\r\n\r\nИспользуются два основных символа разделения - \r\n\" \" - пробел\r\n\"/\" - слеш\r\nВ начале стсроки можно написать \"+\" тогда все стартовые минуты в этом корридоре сдвинкться на 1 минуту за каждый +\r\n\r\nЕсли написать группы через пробел то минуты будут присваиваться сначала участникам одной группе зачем второй и т.д.\r\nЕсли использовать слеш то участини будут браться последовательно из разделенных таким способом групп.\r\nСлеши имеют приоритет, то есть сначала строка разделяется на части которым будут применяться минуты по очереди, но внути этих частей разделенных слешем можно использовать последовательные группы.\r\nСжимать конец старта в колонках - если выключено то при использовании слешей в итоговых минутах в каждой группе будет поддерживаться постоянный интервал старта. Включено - когда в одной из групп разделенных слешем кончатся участники, то оставшимся будут выдаваться ближайшие возможные минуты, но не меньше чем минимальный заданный интервал. \r\nПримеры \r\nЕсть группы\r\nМ21 - 3 участника\r\nЖ21 - 3 участника\r\nМ18 - 1 участник\r\nМ35 - 2 участника\r\nСтрока - \"М21 Ж21 М18 М35\" результат - М21 10:01, М21 10:02, М21 10:03, Ж21 10:04, Ж21 10:05, Ж21 10:06, М18 10:07, М35 10:08, М35 10:09\r\nСтрока - \"М21/Ж21/М18/М35\" результат - М21 10:01, Ж21 10:02, М18 10:03, М35 10:04, М21 10:05, Ж21 10:06, М35 10:07, М21 10:08, Ж21 10:09\r\nСтрока - \"М21 Ж21/М18 М35\" результат - М21 10:01, М18 10:02, М21 10:03, М35 10:04, М21 10:05, М35 10:06, Ж21 10:07, Ж21 10:08, Ж21 10:09\r\nСтрока - \"М21/Ж21/М18 М35\" результат - М21 10:01, Ж21 10:02, М18 10:03, М21 10:04, Ж21 10:05, М35 10:06, М21 10:07, Ж21 10:08, М35 10:09 \r\n\r\nЕсли выключено - Сжимать конец старта в колонках \r\nСтрока - \"Ж21 М35/М18/М21\" результат - Ж21 10:00, М18 10:01, М21 10:02, Ж21 10:03, М21 10:05, Ж21 10:06, М21 10:08, М35 10:09, М35 10:12";
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

        private async void buttonPhoneFtpSendBase_Click(object sender, EventArgs e)
        {
            PhoneFTPManager phoneFTPManager = new PhoneFTPManager("ips.txt", SendSubLog);
            SendLog("Запуск задачи");
            await Task.Run(() => phoneFTPManager.SendBaseToAllFromRace(PBCurrentRaceFromBase(JBase)));
        }
    }
}
