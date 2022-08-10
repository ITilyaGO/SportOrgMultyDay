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
            this.buttonBaseImport = new System.Windows.Forms.Button();
            this.richTextBoxLog = new System.Windows.Forms.RichTextBox();
            this.labelBaseImport = new System.Windows.Forms.Label();
            this.buttonRemoveMissingPersons = new System.Windows.Forms.Button();
            this.buttonBaseExport = new System.Windows.Forms.Button();
            this.buttonSynchronizeReorders = new System.Windows.Forms.Button();
            this.saveFileDialogJson = new System.Windows.Forms.SaveFileDialog();
            this.openFileDialogJson = new System.Windows.Forms.OpenFileDialog();
            this.textBoxReservName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.checkBoxLogAutoScroll = new System.Windows.Forms.CheckBox();
            this.checkedListBoxWithSync = new System.Windows.Forms.CheckedListBox();
            this.buttonClearLog = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.checkBoxCopyChangedOtherDays = new System.Windows.Forms.CheckBox();
            this.buttonCreateNewAdded = new System.Windows.Forms.Button();
            this.buttonCardNumAsNum = new System.Windows.Forms.Button();
            this.buttonCopyPersonByNumber = new System.Windows.Forms.Button();
            this.textBoxPersonsFromCopy = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.buttonCombineAllBase = new System.Windows.Forms.Button();
            this.buttonFindAddWithComment = new System.Windows.Forms.Button();
            this.textBoxStringFindComment = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.buttonCopyGroupSettings = new System.Windows.Forms.Button();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.buttonExportStartTimes = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.saveFileDialogSfrst = new System.Windows.Forms.SaveFileDialog();
            this.buttonImportSFRStartLog = new System.Windows.Forms.Button();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.comboBoxDays = new System.Windows.Forms.ComboBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabControl2.SuspendLayout();
            this.tabPage4.SuspendLayout();
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
            // richTextBoxLog
            // 
            this.richTextBoxLog.Location = new System.Drawing.Point(6, 6);
            this.richTextBoxLog.Name = "richTextBoxLog";
            this.richTextBoxLog.Size = new System.Drawing.Size(857, 474);
            this.richTextBoxLog.TabIndex = 2;
            this.richTextBoxLog.Text = "";
            this.richTextBoxLog.WordWrap = false;
            this.richTextBoxLog.TextChanged += new System.EventHandler(this.richTextBoxLog_TextChanged);
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
            // buttonBaseExport
            // 
            this.buttonBaseExport.Location = new System.Drawing.Point(443, 8);
            this.buttonBaseExport.Name = "buttonBaseExport";
            this.buttonBaseExport.Size = new System.Drawing.Size(105, 23);
            this.buttonBaseExport.TabIndex = 5;
            this.buttonBaseExport.Text = "Сохранить базу";
            this.buttonBaseExport.UseVisualStyleBackColor = true;
            this.buttonBaseExport.Click += new System.EventHandler(this.buttonBaseExport_Click);
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
            // saveFileDialogJson
            // 
            this.saveFileDialogJson.Filter = "Json|*.json|All files|*.*";
            // 
            // openFileDialogJson
            // 
            this.openFileDialogJson.FileName = "openFileDialog1";
            this.openFileDialogJson.Filter = "Json|*.json|All files|*.*";
            // 
            // textBoxReservName
            // 
            this.textBoxReservName.Location = new System.Drawing.Point(6, 22);
            this.textBoxReservName.Name = "textBoxReservName";
            this.textBoxReservName.Size = new System.Drawing.Size(59, 23);
            this.textBoxReservName.TabIndex = 7;
            this.textBoxReservName.Text = "_Резерв";
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
            // checkBoxLogAutoScroll
            // 
            this.checkBoxLogAutoScroll.AutoSize = true;
            this.checkBoxLogAutoScroll.Checked = true;
            this.checkBoxLogAutoScroll.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxLogAutoScroll.Location = new System.Drawing.Point(7, 22);
            this.checkBoxLogAutoScroll.Name = "checkBoxLogAutoScroll";
            this.checkBoxLogAutoScroll.Size = new System.Drawing.Size(85, 19);
            this.checkBoxLogAutoScroll.TabIndex = 9;
            this.checkBoxLogAutoScroll.Text = "Прокрутка";
            this.checkBoxLogAutoScroll.UseVisualStyleBackColor = true;
            // 
            // checkedListBoxWithSync
            // 
            this.checkedListBoxWithSync.FormattingEnabled = true;
            this.checkedListBoxWithSync.Location = new System.Drawing.Point(15, 158);
            this.checkedListBoxWithSync.Name = "checkedListBoxWithSync";
            this.checkedListBoxWithSync.Size = new System.Drawing.Size(231, 310);
            this.checkedListBoxWithSync.TabIndex = 10;
            // 
            // buttonClearLog
            // 
            this.buttonClearLog.Location = new System.Drawing.Point(6, 47);
            this.buttonClearLog.Name = "buttonClearLog";
            this.buttonClearLog.Size = new System.Drawing.Size(85, 23);
            this.buttonClearLog.TabIndex = 11;
            this.buttonClearLog.Text = "Очистить";
            this.buttonClearLog.UseVisualStyleBackColor = true;
            this.buttonClearLog.Click += new System.EventHandler(this.buttonClearLog_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.checkBoxLogAutoScroll);
            this.groupBox1.Controls.Add(this.buttonClearLog);
            this.groupBox1.Location = new System.Drawing.Point(428, 448);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(98, 78);
            this.groupBox1.TabIndex = 12;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Логи";
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
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(68, 66);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(117, 15);
            this.label2.TabIndex = 19;
            this.label2.Text = "Номера участников";
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
            // textBoxStringFindComment
            // 
            this.textBoxStringFindComment.Location = new System.Drawing.Point(144, 29);
            this.textBoxStringFindComment.Name = "textBoxStringFindComment";
            this.textBoxStringFindComment.Size = new System.Drawing.Size(110, 23);
            this.textBoxStringFindComment.TabIndex = 22;
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
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(15, 137);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(225, 15);
            this.label3.TabIndex = 26;
            this.label3.Text = "Поля для копирования/синхронизации";
            // 
            // buttonExportStartTimes
            // 
            this.buttonExportStartTimes.Location = new System.Drawing.Point(6, 6);
            this.buttonExportStartTimes.Name = "buttonExportStartTimes";
            this.buttonExportStartTimes.Size = new System.Drawing.Size(197, 44);
            this.buttonExportStartTimes.TabIndex = 27;
            this.buttonExportStartTimes.Text = "Экспоритровать стартовые минуты для SFT Smart Terminal";
            this.buttonExportStartTimes.UseVisualStyleBackColor = true;
            this.buttonExportStartTimes.Click += new System.EventHandler(this.buttonExportStartTimes_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Location = new System.Drawing.Point(557, 41);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(877, 514);
            this.tabControl1.TabIndex = 28;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.richTextBoxLog);
            this.tabPage1.Location = new System.Drawing.Point(4, 24);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(869, 486);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Логи";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.buttonExportStartTimes);
            this.tabPage2.Location = new System.Drawing.Point(4, 24);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(869, 486);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Стартовые минуты ";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // saveFileDialogSfrst
            // 
            this.saveFileDialogSfrst.Filter = "Sfrst|*.sfrst|All files|*.*";
            // 
            // buttonImportSFRStartLog
            // 
            this.buttonImportSFRStartLog.Location = new System.Drawing.Point(3, 5);
            this.buttonImportSFRStartLog.Name = "buttonImportSFRStartLog";
            this.buttonImportSFRStartLog.Size = new System.Drawing.Size(197, 23);
            this.buttonImportSFRStartLog.TabIndex = 28;
            this.buttonImportSFRStartLog.Text = "Импортировать стартовый лог";
            this.buttonImportSFRStartLog.UseVisualStyleBackColor = true;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.buttonImportSFRStartLog);
            this.tabPage3.Location = new System.Drawing.Point(4, 24);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(869, 486);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "SFR Smart terminal";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // tabControl2
            // 
            this.tabControl2.Controls.Add(this.tabPage4);
            this.tabControl2.Controls.Add(this.tabPage5);
            this.tabControl2.Location = new System.Drawing.Point(12, 41);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(543, 481);
            this.tabControl2.TabIndex = 30;
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
            this.tabPage4.Size = new System.Drawing.Size(535, 453);
            this.tabPage4.TabIndex = 0;
            this.tabPage4.Text = "База";
            // 
            // tabPage5
            // 
            this.tabPage5.BackColor = System.Drawing.Color.WhiteSmoke;
            this.tabPage5.Location = new System.Drawing.Point(4, 24);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage5.Size = new System.Drawing.Size(192, 72);
            this.tabPage5.TabIndex = 1;
            this.tabPage5.Text = "SFR Smart Terminal";
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
            // Utils
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1410, 533);
            this.Controls.Add(this.tabControl2);
            this.Controls.Add(this.comboBoxDays);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.buttonCombineAllBase);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.buttonBaseExport);
            this.Controls.Add(this.labelBaseImport);
            this.Controls.Add(this.buttonBaseImport);
            this.MinimumSize = new System.Drawing.Size(600, 400);
            this.Name = "Utils";
            this.Text = "Утилиты";
            this.Load += new System.EventHandler(this.Utils_Load);
            this.SizeChanged += new System.EventHandler(this.Utils_SizeChanged);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tabControl2.ResumeLayout(false);
            this.tabPage4.ResumeLayout(false);
            this.tabPage4.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private Button buttonBaseImport;
        private RichTextBox richTextBoxLog;
        private Label labelBaseImport;
        private Button buttonRemoveMissingPersons;
        private Button buttonBaseExport;
        private Button buttonSynchronizeReorders;
        private SaveFileDialog saveFileDialogJson;
        private OpenFileDialog openFileDialogJson;
        private TextBox textBoxReservName;
        private Label label1;
        private CheckBox checkBoxLogAutoScroll;
        private CheckedListBox checkedListBoxWithSync;
        private Button buttonClearLog;
        private GroupBox groupBox1;
        private CheckBox checkBoxCopyChangedOtherDays;
        private Button buttonCreateNewAdded;
        private Button buttonCardNumAsNum;
        private Button buttonCopyPersonByNumber;
        private TextBox textBoxPersonsFromCopy;
        private Label label2;
        private Button buttonCombineAllBase;
        private Button buttonFindAddWithComment;
        private TextBox textBoxStringFindComment;
        private GroupBox groupBox2;
        private GroupBox groupBox3;
        private GroupBox groupBox4;
        private Label label3;
        private Button buttonCopyGroupSettings;
        private GroupBox groupBox5;
        private Button buttonExportStartTimes;
        private TabControl tabControl1;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private SaveFileDialog saveFileDialogSfrst;
        private TabPage tabPage3;
        private Button buttonImportSFRStartLog;
        private TabControl tabControl2;
        private TabPage tabPage4;
        private TabPage tabPage5;
        private ComboBox comboBoxDays;
    }
}