namespace SportOrgMultyDay
{
    partial class Utils
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.buttonBaseImport = new System.Windows.Forms.Button();
            this.contextMenuStripLog = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ScrollLogToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ClearLogToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.labelBaseImport = new System.Windows.Forms.Label();
            this.buttonBaseExport = new System.Windows.Forms.Button();
            this.saveFileDialogJson = new System.Windows.Forms.SaveFileDialog();
            this.openFileDialogJson = new System.Windows.Forms.OpenFileDialog();
            this.buttonCombineAllBase = new System.Windows.Forms.Button();
            this.saveFileDialogSfrst = new System.Windows.Forms.SaveFileDialog();
            this.comboBoxDays = new System.Windows.Forms.ComboBox();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.groupBoxStartLogProcessing = new System.Windows.Forms.GroupBox();
            this.comboBoxStartLogOutFieldsSplitter = new System.Windows.Forms.ComboBox();
            this.buttonImportStartLogClipboard = new System.Windows.Forms.Button();
            this.comboBoxLogType = new System.Windows.Forms.ComboBox();
            this.buttonImportStartLogFile = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.richTextBoxChecklessFinished = new System.Windows.Forms.RichTextBox();
            this.buttonSFRStartLogDNSCopy = new System.Windows.Forms.Button();
            this.labelSFRStartLogDNS = new System.Windows.Forms.Label();
            this.richTextBoxStartLogDNS = new System.Windows.Forms.RichTextBox();
            this.labelSFRStartLogDupl = new System.Windows.Forms.Label();
            this.richTextBoxStartLogDupl = new System.Windows.Forms.RichTextBox();
            this.labelSFRStartLogCount = new System.Windows.Forms.Label();
            this.buttonExportStartTimes = new System.Windows.Forms.Button();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.buttonRemoveMissingPersons = new System.Windows.Forms.Button();
            this.buttonCreateNewAdded = new System.Windows.Forms.Button();
            this.buttonCardNumAsNum = new System.Windows.Forms.Button();
            this.checkedListBoxWithSync = new System.Windows.Forms.CheckedListBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.buttonCopyGroupSettings = new System.Windows.Forms.Button();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.textBoxReservName = new System.Windows.Forms.TextBox();
            this.buttonSynchronizeReorders = new System.Windows.Forms.Button();
            this.checkBoxCopyChangedOtherDays = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.buttonFindAddWithComment = new System.Windows.Forms.Button();
            this.buttonCopyPersonByNumber = new System.Windows.Forms.Button();
            this.textBoxPersonsFromCopy = new System.Windows.Forms.TextBox();
            this.textBoxStringFindComment = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tabControlFunc = new System.Windows.Forms.TabControl();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.groupBoxStartFee = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.textBoxStartFeeWithCardSymbol = new System.Windows.Forms.TextBox();
            this.buttonStartFeeCalculate = new System.Windows.Forms.Button();
            this.tabPage6 = new System.Windows.Forms.TabPage();
            this.buttonRemvoeWorstResult = new System.Windows.Forms.Button();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.richTextBoxLog = new System.Windows.Forms.RichTextBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.openFileDialogStartLog = new System.Windows.Forms.OpenFileDialog();
            this.toolTipGeneral = new System.Windows.Forms.ToolTip(this.components);
            this.contextMenuStripLog.SuspendLayout();
            this.tabPage5.SuspendLayout();
            this.groupBoxStartLogProcessing.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.tabControlFunc.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.groupBoxStartFee.SuspendLayout();
            this.tabPage6.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonBaseImport
            // 
            this.buttonBaseImport.Location = new System.Drawing.Point(12, 12);
            this.buttonBaseImport.Name = "buttonBaseImport";
            this.buttonBaseImport.Size = new System.Drawing.Size(109, 23);
            this.buttonBaseImport.TabIndex = 0;
            this.buttonBaseImport.Text = "Импорт базы";
            this.buttonBaseImport.UseVisualStyleBackColor = true;
            this.buttonBaseImport.Click += new System.EventHandler(this.buttonBaseImport_Click);
            // 
            // contextMenuStripLog
            // 
            this.contextMenuStripLog.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ScrollLogToolStripMenuItem,
            this.ClearLogToolStripMenuItem});
            this.contextMenuStripLog.Name = "contextMenuStrip1";
            this.contextMenuStripLog.Size = new System.Drawing.Size(134, 48);
            // 
            // ScrollLogToolStripMenuItem
            // 
            this.ScrollLogToolStripMenuItem.Checked = true;
            this.ScrollLogToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ScrollLogToolStripMenuItem.Name = "ScrollLogToolStripMenuItem";
            this.ScrollLogToolStripMenuItem.Size = new System.Drawing.Size(133, 22);
            this.ScrollLogToolStripMenuItem.Text = "Прокрутка";
            this.ScrollLogToolStripMenuItem.Click += new System.EventHandler(this.ScrollLogToolStripMenuItem_Click);
            // 
            // ClearLogToolStripMenuItem
            // 
            this.ClearLogToolStripMenuItem.Name = "ClearLogToolStripMenuItem";
            this.ClearLogToolStripMenuItem.Size = new System.Drawing.Size(133, 22);
            this.ClearLogToolStripMenuItem.Text = "Очистить";
            this.ClearLogToolStripMenuItem.Click += new System.EventHandler(this.ClearLogToolStripMenuItem_Click);
            // 
            // labelBaseImport
            // 
            this.labelBaseImport.AutoSize = true;
            this.labelBaseImport.Location = new System.Drawing.Point(127, 16);
            this.labelBaseImport.Name = "labelBaseImport";
            this.labelBaseImport.Size = new System.Drawing.Size(37, 15);
            this.labelBaseImport.TabIndex = 3;
            this.labelBaseImport.Text = "День:";
            // 
            // buttonBaseExport
            // 
            this.buttonBaseExport.Location = new System.Drawing.Point(446, 13);
            this.buttonBaseExport.Name = "buttonBaseExport";
            this.buttonBaseExport.Size = new System.Drawing.Size(105, 23);
            this.buttonBaseExport.TabIndex = 5;
            this.buttonBaseExport.Text = "Сохранить базу";
            this.buttonBaseExport.UseVisualStyleBackColor = true;
            this.buttonBaseExport.Click += new System.EventHandler(this.buttonBaseExport_Click);
            // 
            // saveFileDialogJson
            // 
            this.saveFileDialogJson.Filter = "Json|*.json|All files|*.*";
            // 
            // openFileDialogJson
            // 
            this.openFileDialogJson.FileName = "openFileDialog1";
            this.openFileDialogJson.Filter = "Json|*.json|All files|*.*";
            // 
            // buttonCombineAllBase
            // 
            this.buttonCombineAllBase.Enabled = false;
            this.buttonCombineAllBase.Location = new System.Drawing.Point(1391, 16);
            this.buttonCombineAllBase.Name = "buttonCombineAllBase";
            this.buttonCombineAllBase.Size = new System.Drawing.Size(115, 23);
            this.buttonCombineAllBase.TabIndex = 20;
            this.buttonCombineAllBase.Text = "Всех в одну базу";
            this.buttonCombineAllBase.UseVisualStyleBackColor = true;
            this.buttonCombineAllBase.Visible = false;
            this.buttonCombineAllBase.Click += new System.EventHandler(this.buttonCombineAllBase_Click);
            // 
            // saveFileDialogSfrst
            // 
            this.saveFileDialogSfrst.Filter = "Sfrst|*.sfrst|All files|*.*";
            // 
            // comboBoxDays
            // 
            this.comboBoxDays.FormattingEnabled = true;
            this.comboBoxDays.Location = new System.Drawing.Point(170, 13);
            this.comboBoxDays.Name = "comboBoxDays";
            this.comboBoxDays.Size = new System.Drawing.Size(79, 23);
            this.comboBoxDays.TabIndex = 29;
            this.comboBoxDays.SelectedIndexChanged += new System.EventHandler(this.comboBoxDays_SelectedIndexChanged);
            // 
            // tabPage5
            // 
            this.tabPage5.BackColor = System.Drawing.Color.WhiteSmoke;
            this.tabPage5.Controls.Add(this.groupBoxStartLogProcessing);
            this.tabPage5.Controls.Add(this.buttonExportStartTimes);
            this.tabPage5.Location = new System.Drawing.Point(4, 24);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage5.Size = new System.Drawing.Size(535, 485);
            this.tabPage5.TabIndex = 1;
            this.tabPage5.Text = "Шахматка";
            // 
            // groupBoxStartLogProcessing
            // 
            this.groupBoxStartLogProcessing.Controls.Add(this.comboBoxStartLogOutFieldsSplitter);
            this.groupBoxStartLogProcessing.Controls.Add(this.buttonImportStartLogClipboard);
            this.groupBoxStartLogProcessing.Controls.Add(this.comboBoxLogType);
            this.groupBoxStartLogProcessing.Controls.Add(this.buttonImportStartLogFile);
            this.groupBoxStartLogProcessing.Controls.Add(this.label4);
            this.groupBoxStartLogProcessing.Controls.Add(this.richTextBoxChecklessFinished);
            this.groupBoxStartLogProcessing.Controls.Add(this.buttonSFRStartLogDNSCopy);
            this.groupBoxStartLogProcessing.Controls.Add(this.labelSFRStartLogDNS);
            this.groupBoxStartLogProcessing.Controls.Add(this.richTextBoxStartLogDNS);
            this.groupBoxStartLogProcessing.Controls.Add(this.labelSFRStartLogDupl);
            this.groupBoxStartLogProcessing.Controls.Add(this.richTextBoxStartLogDupl);
            this.groupBoxStartLogProcessing.Controls.Add(this.labelSFRStartLogCount);
            this.groupBoxStartLogProcessing.Location = new System.Drawing.Point(6, 6);
            this.groupBoxStartLogProcessing.Name = "groupBoxStartLogProcessing";
            this.groupBoxStartLogProcessing.Size = new System.Drawing.Size(290, 417);
            this.groupBoxStartLogProcessing.TabIndex = 29;
            this.groupBoxStartLogProcessing.TabStop = false;
            this.groupBoxStartLogProcessing.Text = "Обработка стартовых логов";
            // 
            // comboBoxStartLogOutFieldsSplitter
            // 
            this.comboBoxStartLogOutFieldsSplitter.FormattingEnabled = true;
            this.comboBoxStartLogOutFieldsSplitter.Location = new System.Drawing.Point(114, 23);
            this.comboBoxStartLogOutFieldsSplitter.Name = "comboBoxStartLogOutFieldsSplitter";
            this.comboBoxStartLogOutFieldsSplitter.Size = new System.Drawing.Size(59, 23);
            this.comboBoxStartLogOutFieldsSplitter.TabIndex = 41;
            this.toolTipGeneral.SetToolTip(this.comboBoxStartLogOutFieldsSplitter, "Символ разделения номеров при выводе");
            // 
            // buttonImportStartLogClipboard
            // 
            this.buttonImportStartLogClipboard.Image = global::SportOrgMultyDay.Properties.Resources.paste_48;
            this.buttonImportStartLogClipboard.Location = new System.Drawing.Point(233, 23);
            this.buttonImportStartLogClipboard.Name = "buttonImportStartLogClipboard";
            this.buttonImportStartLogClipboard.Size = new System.Drawing.Size(48, 48);
            this.buttonImportStartLogClipboard.TabIndex = 40;
            this.toolTipGeneral.SetToolTip(this.buttonImportStartLogClipboard, "Импори стартового лога из буфера объмена");
            this.buttonImportStartLogClipboard.UseVisualStyleBackColor = true;
            this.buttonImportStartLogClipboard.Click += new System.EventHandler(this.buttonImportStartLogClipboard_Click);
            // 
            // comboBoxLogType
            // 
            this.comboBoxLogType.FormattingEnabled = true;
            this.comboBoxLogType.Location = new System.Drawing.Point(6, 23);
            this.comboBoxLogType.Name = "comboBoxLogType";
            this.comboBoxLogType.Size = new System.Drawing.Size(102, 23);
            this.comboBoxLogType.TabIndex = 39;
            // 
            // buttonImportStartLogFile
            // 
            this.buttonImportStartLogFile.BackColor = System.Drawing.Color.Gainsboro;
            this.buttonImportStartLogFile.Image = global::SportOrgMultyDay.Properties.Resources.file_import_48;
            this.buttonImportStartLogFile.Location = new System.Drawing.Point(179, 23);
            this.buttonImportStartLogFile.Name = "buttonImportStartLogFile";
            this.buttonImportStartLogFile.Size = new System.Drawing.Size(48, 48);
            this.buttonImportStartLogFile.TabIndex = 28;
            this.toolTipGeneral.SetToolTip(this.buttonImportStartLogFile, "Импорт стартового лога из файла");
            this.buttonImportStartLogFile.UseVisualStyleBackColor = false;
            this.buttonImportStartLogFile.Click += new System.EventHandler(this.buttonImportStartLogFile_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 175);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(261, 15);
            this.label4.TabIndex = 38;
            this.label4.Text = "Не отмеченные финишировавшие участники";
            // 
            // richTextBoxChecklessFinished
            // 
            this.richTextBoxChecklessFinished.Location = new System.Drawing.Point(6, 193);
            this.richTextBoxChecklessFinished.Name = "richTextBoxChecklessFinished";
            this.richTextBoxChecklessFinished.Size = new System.Drawing.Size(275, 92);
            this.richTextBoxChecklessFinished.TabIndex = 37;
            this.richTextBoxChecklessFinished.Text = "";
            // 
            // buttonSFRStartLogDNSCopy
            // 
            this.buttonSFRStartLogDNSCopy.Location = new System.Drawing.Point(42, 291);
            this.buttonSFRStartLogDNSCopy.Name = "buttonSFRStartLogDNSCopy";
            this.buttonSFRStartLogDNSCopy.Size = new System.Drawing.Size(136, 23);
            this.buttonSFRStartLogDNSCopy.TabIndex = 34;
            this.buttonSFRStartLogDNSCopy.Text = "Копировать";
            this.buttonSFRStartLogDNSCopy.UseVisualStyleBackColor = true;
            this.buttonSFRStartLogDNSCopy.Click += new System.EventHandler(this.buttonSFRStartLogDNSCopy_Click);
            // 
            // labelSFRStartLogDNS
            // 
            this.labelSFRStartLogDNS.AutoSize = true;
            this.labelSFRStartLogDNS.Location = new System.Drawing.Point(6, 299);
            this.labelSFRStartLogDNS.Name = "labelSFRStartLogDNS";
            this.labelSFRStartLogDNS.Size = new System.Drawing.Size(30, 15);
            this.labelSFRStartLogDNS.TabIndex = 33;
            this.labelSFRStartLogDNS.Text = "DNS";
            // 
            // richTextBoxStartLogDNS
            // 
            this.richTextBoxStartLogDNS.Location = new System.Drawing.Point(6, 317);
            this.richTextBoxStartLogDNS.Name = "richTextBoxStartLogDNS";
            this.richTextBoxStartLogDNS.Size = new System.Drawing.Size(275, 92);
            this.richTextBoxStartLogDNS.TabIndex = 32;
            this.richTextBoxStartLogDNS.Text = "";
            // 
            // labelSFRStartLogDupl
            // 
            this.labelSFRStartLogDupl.AutoSize = true;
            this.labelSFRStartLogDupl.Location = new System.Drawing.Point(6, 62);
            this.labelSFRStartLogDupl.Name = "labelSFRStartLogDupl";
            this.labelSFRStartLogDupl.Size = new System.Drawing.Size(68, 15);
            this.labelSFRStartLogDupl.TabIndex = 31;
            this.labelSFRStartLogDupl.Text = "Дубликаты";
            // 
            // richTextBoxStartLogDupl
            // 
            this.richTextBoxStartLogDupl.Location = new System.Drawing.Point(6, 80);
            this.richTextBoxStartLogDupl.Name = "richTextBoxStartLogDupl";
            this.richTextBoxStartLogDupl.Size = new System.Drawing.Size(275, 92);
            this.richTextBoxStartLogDupl.TabIndex = 30;
            this.richTextBoxStartLogDupl.Text = "";
            // 
            // labelSFRStartLogCount
            // 
            this.labelSFRStartLogCount.AutoSize = true;
            this.labelSFRStartLogCount.Location = new System.Drawing.Point(6, 47);
            this.labelSFRStartLogCount.Name = "labelSFRStartLogCount";
            this.labelSFRStartLogCount.Size = new System.Drawing.Size(74, 15);
            this.labelSFRStartLogCount.TabIndex = 30;
            this.labelSFRStartLogCount.Text = "Стартовало:";
            // 
            // buttonExportStartTimes
            // 
            this.buttonExportStartTimes.Location = new System.Drawing.Point(408, 6);
            this.buttonExportStartTimes.Name = "buttonExportStartTimes";
            this.buttonExportStartTimes.Size = new System.Drawing.Size(121, 74);
            this.buttonExportStartTimes.TabIndex = 27;
            this.buttonExportStartTimes.Text = "Экспоритровать стартовые минуты для SFT Smart Terminal";
            this.buttonExportStartTimes.UseVisualStyleBackColor = true;
            this.buttonExportStartTimes.Click += new System.EventHandler(this.buttonExportStartTimes_Click);
            // 
            // tabPage4
            // 
            this.tabPage4.BackColor = System.Drawing.Color.WhiteSmoke;
            this.tabPage4.Controls.Add(this.groupBox2);
            this.tabPage4.Controls.Add(this.checkedListBoxWithSync);
            this.tabPage4.Controls.Add(this.groupBox3);
            this.tabPage4.Controls.Add(this.label3);
            this.tabPage4.Controls.Add(this.groupBox4);
            this.tabPage4.Location = new System.Drawing.Point(4, 24);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(535, 485);
            this.tabPage4.TabIndex = 0;
            this.tabPage4.Text = "База";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.buttonRemoveMissingPersons);
            this.groupBox2.Controls.Add(this.buttonCreateNewAdded);
            this.groupBox2.Controls.Add(this.buttonCardNumAsNum);
            this.groupBox2.Location = new System.Drawing.Point(9, 6);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(244, 128);
            this.groupBox2.TabIndex = 23;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Остальное";
            // 
            // buttonRemoveMissingPersons
            // 
            this.buttonRemoveMissingPersons.Location = new System.Drawing.Point(6, 22);
            this.buttonRemoveMissingPersons.Name = "buttonRemoveMissingPersons";
            this.buttonRemoveMissingPersons.Size = new System.Drawing.Size(231, 23);
            this.buttonRemoveMissingPersons.TabIndex = 4;
            this.buttonRemoveMissingPersons.Text = "Удалить отсутствующих по дням";
            this.buttonRemoveMissingPersons.UseVisualStyleBackColor = true;
            this.buttonRemoveMissingPersons.Click += new System.EventHandler(this.buttonRemoveMissingPersons_Click);
            // 
            // buttonCreateNewAdded
            // 
            this.buttonCreateNewAdded.Location = new System.Drawing.Point(6, 51);
            this.buttonCreateNewAdded.Name = "buttonCreateNewAdded";
            this.buttonCreateNewAdded.Size = new System.Drawing.Size(231, 23);
            this.buttonCreateNewAdded.TabIndex = 15;
            this.buttonCreateNewAdded.Text = "Создать дозаявленых в остальных днях";
            this.buttonCreateNewAdded.UseVisualStyleBackColor = true;
            this.buttonCreateNewAdded.Click += new System.EventHandler(this.buttonCreateNewAdded_Click);
            // 
            // buttonCardNumAsNum
            // 
            this.buttonCardNumAsNum.Location = new System.Drawing.Point(6, 80);
            this.buttonCardNumAsNum.Name = "buttonCardNumAsNum";
            this.buttonCardNumAsNum.Size = new System.Drawing.Size(231, 23);
            this.buttonCardNumAsNum.TabIndex = 16;
            this.buttonCardNumAsNum.Text = "Установить номер чипа = номеру";
            this.buttonCardNumAsNum.UseVisualStyleBackColor = true;
            this.buttonCardNumAsNum.Click += new System.EventHandler(this.buttonCardNumAsNum_Click);
            // 
            // checkedListBoxWithSync
            // 
            this.checkedListBoxWithSync.FormattingEnabled = true;
            this.checkedListBoxWithSync.Location = new System.Drawing.Point(15, 158);
            this.checkedListBoxWithSync.Name = "checkedListBoxWithSync";
            this.checkedListBoxWithSync.Size = new System.Drawing.Size(231, 310);
            this.checkedListBoxWithSync.TabIndex = 10;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.buttonCopyGroupSettings);
            this.groupBox3.Controls.Add(this.groupBox5);
            this.groupBox3.Location = new System.Drawing.Point(259, 6);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(260, 181);
            this.groupBox3.TabIndex = 24;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Синхронизация";
            // 
            // buttonCopyGroupSettings
            // 
            this.buttonCopyGroupSettings.Location = new System.Drawing.Point(9, 152);
            this.buttonCopyGroupSettings.Name = "buttonCopyGroupSettings";
            this.buttonCopyGroupSettings.Size = new System.Drawing.Size(243, 23);
            this.buttonCopyGroupSettings.TabIndex = 27;
            this.buttonCopyGroupSettings.Text = "Синхронизировать ранги в группах";
            this.buttonCopyGroupSettings.UseVisualStyleBackColor = true;
            this.buttonCopyGroupSettings.Click += new System.EventHandler(this.buttonCopyGroupSettings_Click);
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.textBoxReservName);
            this.groupBox5.Controls.Add(this.buttonSynchronizeReorders);
            this.groupBox5.Controls.Add(this.checkBoxCopyChangedOtherDays);
            this.groupBox5.Controls.Add(this.label1);
            this.groupBox5.Location = new System.Drawing.Point(9, 22);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(243, 124);
            this.groupBox5.TabIndex = 14;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Дозаявки";
            // 
            // textBoxReservName
            // 
            this.textBoxReservName.Location = new System.Drawing.Point(6, 22);
            this.textBoxReservName.Name = "textBoxReservName";
            this.textBoxReservName.Size = new System.Drawing.Size(59, 23);
            this.textBoxReservName.TabIndex = 7;
            this.textBoxReservName.Text = "_Резерв";
            // 
            // buttonSynchronizeReorders
            // 
            this.buttonSynchronizeReorders.Location = new System.Drawing.Point(6, 94);
            this.buttonSynchronizeReorders.Name = "buttonSynchronizeReorders";
            this.buttonSynchronizeReorders.Size = new System.Drawing.Size(231, 23);
            this.buttonSynchronizeReorders.TabIndex = 6;
            this.buttonSynchronizeReorders.Text = "Синхронизировать дозаявки";
            this.buttonSynchronizeReorders.UseVisualStyleBackColor = true;
            this.buttonSynchronizeReorders.Click += new System.EventHandler(this.buttonSynchronizeReorders_Click);
            // 
            // checkBoxCopyChangedOtherDays
            // 
            this.checkBoxCopyChangedOtherDays.Location = new System.Drawing.Point(6, 51);
            this.checkBoxCopyChangedOtherDays.Name = "checkBoxCopyChangedOtherDays";
            this.checkBoxCopyChangedOtherDays.Size = new System.Drawing.Size(171, 37);
            this.checkBoxCopyChangedOtherDays.TabIndex = 13;
            this.checkBoxCopyChangedOtherDays.Text = "Копирование при изменении в других днях";
            this.checkBoxCopyChangedOtherDays.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(71, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(104, 15);
            this.label1.TabIndex = 8;
            this.label1.Text = "Фамилия резерва";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(15, 137);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(225, 15);
            this.label3.TabIndex = 26;
            this.label3.Text = "Поля для копирования/синхронизации";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.buttonFindAddWithComment);
            this.groupBox4.Controls.Add(this.buttonCopyPersonByNumber);
            this.groupBox4.Controls.Add(this.textBoxPersonsFromCopy);
            this.groupBox4.Controls.Add(this.textBoxStringFindComment);
            this.groupBox4.Controls.Add(this.label2);
            this.groupBox4.Location = new System.Drawing.Point(259, 193);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(260, 143);
            this.groupBox4.TabIndex = 25;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Копирование";
            // 
            // buttonFindAddWithComment
            // 
            this.buttonFindAddWithComment.Location = new System.Drawing.Point(6, 22);
            this.buttonFindAddWithComment.Name = "buttonFindAddWithComment";
            this.buttonFindAddWithComment.Size = new System.Drawing.Size(132, 38);
            this.buttonFindAddWithComment.TabIndex = 21;
            this.buttonFindAddWithComment.Text = "Найти всех с текстм в комментарии";
            this.buttonFindAddWithComment.UseVisualStyleBackColor = true;
            this.buttonFindAddWithComment.Click += new System.EventHandler(this.buttonFindAddWithComment_Click);
            // 
            // buttonCopyPersonByNumber
            // 
            this.buttonCopyPersonByNumber.Location = new System.Drawing.Point(6, 113);
            this.buttonCopyPersonByNumber.Name = "buttonCopyPersonByNumber";
            this.buttonCopyPersonByNumber.Size = new System.Drawing.Size(248, 23);
            this.buttonCopyPersonByNumber.TabIndex = 17;
            this.buttonCopyPersonByNumber.Text = "Копирвоать участников";
            this.buttonCopyPersonByNumber.UseVisualStyleBackColor = true;
            this.buttonCopyPersonByNumber.Click += new System.EventHandler(this.buttonCopyPersonByNumber_Click);
            // 
            // textBoxPersonsFromCopy
            // 
            this.textBoxPersonsFromCopy.Location = new System.Drawing.Point(6, 84);
            this.textBoxPersonsFromCopy.Name = "textBoxPersonsFromCopy";
            this.textBoxPersonsFromCopy.Size = new System.Drawing.Size(248, 23);
            this.textBoxPersonsFromCopy.TabIndex = 18;
            // 
            // textBoxStringFindComment
            // 
            this.textBoxStringFindComment.Location = new System.Drawing.Point(144, 29);
            this.textBoxStringFindComment.Name = "textBoxStringFindComment";
            this.textBoxStringFindComment.Size = new System.Drawing.Size(110, 23);
            this.textBoxStringFindComment.TabIndex = 22;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(68, 66);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(117, 15);
            this.label2.TabIndex = 19;
            this.label2.Text = "Номера участников";
            // 
            // tabControlFunc
            // 
            this.tabControlFunc.Controls.Add(this.tabPage4);
            this.tabControlFunc.Controls.Add(this.tabPage3);
            this.tabControlFunc.Controls.Add(this.tabPage5);
            this.tabControlFunc.Controls.Add(this.tabPage6);
            this.tabControlFunc.Location = new System.Drawing.Point(12, 42);
            this.tabControlFunc.Name = "tabControlFunc";
            this.tabControlFunc.SelectedIndex = 0;
            this.tabControlFunc.Size = new System.Drawing.Size(543, 513);
            this.tabControlFunc.TabIndex = 30;
            // 
            // tabPage3
            // 
            this.tabPage3.BackColor = System.Drawing.Color.WhiteSmoke;
            this.tabPage3.Controls.Add(this.groupBoxStartFee);
            this.tabPage3.Location = new System.Drawing.Point(4, 24);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(535, 485);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Подсчет";
            // 
            // groupBoxStartFee
            // 
            this.groupBoxStartFee.Controls.Add(this.label5);
            this.groupBoxStartFee.Controls.Add(this.textBoxStartFeeWithCardSymbol);
            this.groupBoxStartFee.Controls.Add(this.buttonStartFeeCalculate);
            this.groupBoxStartFee.Location = new System.Drawing.Point(6, 6);
            this.groupBoxStartFee.Name = "groupBoxStartFee";
            this.groupBoxStartFee.Size = new System.Drawing.Size(422, 256);
            this.groupBoxStartFee.TabIndex = 2;
            this.groupBoxStartFee.TabStop = false;
            this.groupBoxStartFee.Text = "Стартовый взнос ";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(54, 25);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(145, 15);
            this.label5.TabIndex = 3;
            this.label5.Text = "Символ оплаты по карте";
            // 
            // textBoxStartFeeWithCardSymbol
            // 
            this.textBoxStartFeeWithCardSymbol.Location = new System.Drawing.Point(6, 22);
            this.textBoxStartFeeWithCardSymbol.Name = "textBoxStartFeeWithCardSymbol";
            this.textBoxStartFeeWithCardSymbol.Size = new System.Drawing.Size(42, 23);
            this.textBoxStartFeeWithCardSymbol.TabIndex = 1;
            this.textBoxStartFeeWithCardSymbol.Text = "*";
            // 
            // buttonStartFeeCalculate
            // 
            this.buttonStartFeeCalculate.Location = new System.Drawing.Point(6, 51);
            this.buttonStartFeeCalculate.Name = "buttonStartFeeCalculate";
            this.buttonStartFeeCalculate.Size = new System.Drawing.Size(93, 23);
            this.buttonStartFeeCalculate.TabIndex = 0;
            this.buttonStartFeeCalculate.Text = "Подсчитать";
            this.buttonStartFeeCalculate.UseVisualStyleBackColor = true;
            this.buttonStartFeeCalculate.Click += new System.EventHandler(this.buttonStartFeeCalculate_Click);
            // 
            // tabPage6
            // 
            this.tabPage6.BackColor = System.Drawing.Color.WhiteSmoke;
            this.tabPage6.Controls.Add(this.buttonRemvoeWorstResult);
            this.tabPage6.Location = new System.Drawing.Point(4, 24);
            this.tabPage6.Name = "tabPage6";
            this.tabPage6.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage6.Size = new System.Drawing.Size(535, 485);
            this.tabPage6.TabIndex = 3;
            this.tabPage6.Text = "Остальное";
            // 
            // buttonRemvoeWorstResult
            // 
            this.buttonRemvoeWorstResult.Location = new System.Drawing.Point(6, 6);
            this.buttonRemvoeWorstResult.Name = "buttonRemvoeWorstResult";
            this.buttonRemvoeWorstResult.Size = new System.Drawing.Size(201, 41);
            this.buttonRemvoeWorstResult.TabIndex = 0;
            this.buttonRemvoeWorstResult.Text = "Удалить худшие повторяющиеся результаты одного участника";
            this.buttonRemvoeWorstResult.UseVisualStyleBackColor = true;
            this.buttonRemvoeWorstResult.Click += new System.EventHandler(this.buttonRemvoeWorstResult_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.Location = new System.Drawing.Point(4, 24);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(833, 514);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Стартовые минуты ";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.richTextBoxLog);
            this.tabPage1.Location = new System.Drawing.Point(4, 24);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(833, 514);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Логи";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // richTextBoxLog
            // 
            this.richTextBoxLog.ContextMenuStrip = this.contextMenuStripLog;
            this.richTextBoxLog.Location = new System.Drawing.Point(6, 6);
            this.richTextBoxLog.Name = "richTextBoxLog";
            this.richTextBoxLog.Size = new System.Drawing.Size(821, 502);
            this.richTextBoxLog.TabIndex = 2;
            this.richTextBoxLog.Text = "";
            this.richTextBoxLog.WordWrap = false;
            this.richTextBoxLog.TextChanged += new System.EventHandler(this.richTextBoxLog_TextChanged);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(557, 13);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(841, 542);
            this.tabControl1.TabIndex = 28;
            // 
            // openFileDialogStartLog
            // 
            this.openFileDialogStartLog.FileName = "StartLog";
            this.openFileDialogStartLog.Filter = "StartLog|StartLog.txt|Sportident|*.csv|Txt|*.txt|All files|*.*";
            // 
            // Utils
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1408, 565);
            this.Controls.Add(this.tabControlFunc);
            this.Controls.Add(this.comboBoxDays);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.buttonCombineAllBase);
            this.Controls.Add(this.buttonBaseExport);
            this.Controls.Add(this.labelBaseImport);
            this.Controls.Add(this.buttonBaseImport);
            this.MinimumSize = new System.Drawing.Size(900, 470);
            this.Name = "Utils";
            this.Text = "Утилиты";
            this.Load += new System.EventHandler(this.Utils_Load);
            this.SizeChanged += new System.EventHandler(this.Utils_SizeChanged);
            this.contextMenuStripLog.ResumeLayout(false);
            this.tabPage5.ResumeLayout(false);
            this.groupBoxStartLogProcessing.ResumeLayout(false);
            this.groupBoxStartLogProcessing.PerformLayout();
            this.tabPage4.ResumeLayout(false);
            this.tabPage4.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.tabControlFunc.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.groupBoxStartFee.ResumeLayout(false);
            this.groupBoxStartFee.PerformLayout();
            this.tabPage6.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private Button buttonBaseImport;
        private Label labelBaseImport;
        private Button buttonBaseExport;
        private SaveFileDialog saveFileDialogJson;
        private OpenFileDialog openFileDialogJson;
        private Button buttonCombineAllBase;
        private SaveFileDialog saveFileDialogSfrst;
        private ComboBox comboBoxDays;
        private ContextMenuStrip contextMenuStripLog;
        private ToolStripMenuItem ScrollLogToolStripMenuItem;
        private ToolStripMenuItem ClearLogToolStripMenuItem;
        private TabPage tabPage5;
        private TabPage tabPage4;
        private GroupBox groupBox2;
        private Button buttonRemoveMissingPersons;
        private Button buttonCreateNewAdded;
        private Button buttonCardNumAsNum;
        private CheckedListBox checkedListBoxWithSync;
        private GroupBox groupBox3;
        private Button buttonCopyGroupSettings;
        private GroupBox groupBox5;
        private TextBox textBoxReservName;
        private Button buttonSynchronizeReorders;
        private CheckBox checkBoxCopyChangedOtherDays;
        private Label label1;
        private Label label3;
        private GroupBox groupBox4;
        private Button buttonFindAddWithComment;
        private Button buttonCopyPersonByNumber;
        private TextBox textBoxPersonsFromCopy;
        private TextBox textBoxStringFindComment;
        private Label label2;
        private TabControl tabControlFunc;
        private Button buttonImportStartLogFile;
        private TabPage tabPage2;
        private Button buttonExportStartTimes;
        private TabPage tabPage1;
        private RichTextBox richTextBoxLog;
        private TabControl tabControl1;
        private GroupBox groupBoxStartLogProcessing;
        private Label labelSFRStartLogCount;
        private RichTextBox richTextBoxStartLogDupl;
        private Label labelSFRStartLogDupl;
        private Label labelSFRStartLogDNS;
        private RichTextBox richTextBoxStartLogDNS;
        private Label label4;
        private RichTextBox richTextBoxChecklessFinished;
        private Button buttonSFRStartLogDNSCopy;
        private OpenFileDialog openFileDialogStartLog;
        private TabPage tabPage3;
        private GroupBox groupBoxStartFee;
        private Label label5;
        private TextBox textBoxStartFeeWithCardSymbol;
        private Button buttonStartFeeCalculate;
        private TabPage tabPage6;
        private Button buttonRemvoeWorstResult;
        private ComboBox comboBoxLogType;
        private Button buttonImportStartLogClipboard;
        private ToolTip toolTipGeneral;
        private ComboBox comboBoxStartLogOutFieldsSplitter;
    }
}