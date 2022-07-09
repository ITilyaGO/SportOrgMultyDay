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
            this.groupBox1.SuspendLayout();
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
            this.richTextBoxLog.Location = new System.Drawing.Point(426, 12);
            this.richTextBoxLog.Name = "richTextBoxLog";
            this.richTextBoxLog.Size = new System.Drawing.Size(959, 500);
            this.richTextBoxLog.TabIndex = 2;
            this.richTextBoxLog.Text = "";
            this.richTextBoxLog.TextChanged += new System.EventHandler(this.richTextBoxLog_TextChanged);
            // 
            // labelBaseImport
            // 
            this.labelBaseImport.AutoSize = true;
            this.labelBaseImport.Location = new System.Drawing.Point(127, 16);
            this.labelBaseImport.Name = "labelBaseImport";
            this.labelBaseImport.Size = new System.Drawing.Size(36, 15);
            this.labelBaseImport.TabIndex = 3;
            this.labelBaseImport.Text = "None";
            // 
            // buttonRemoveMissingPersons
            // 
            this.buttonRemoveMissingPersons.Location = new System.Drawing.Point(12, 41);
            this.buttonRemoveMissingPersons.Name = "buttonRemoveMissingPersons";
            this.buttonRemoveMissingPersons.Size = new System.Drawing.Size(231, 23);
            this.buttonRemoveMissingPersons.TabIndex = 4;
            this.buttonRemoveMissingPersons.Text = "Удалить отсутствующих по дням";
            this.buttonRemoveMissingPersons.UseVisualStyleBackColor = true;
            this.buttonRemoveMissingPersons.Click += new System.EventHandler(this.buttonRemoveMissingPersons_Click);
            // 
            // buttonBaseExport
            // 
            this.buttonBaseExport.Location = new System.Drawing.Point(315, 12);
            this.buttonBaseExport.Name = "buttonBaseExport";
            this.buttonBaseExport.Size = new System.Drawing.Size(105, 23);
            this.buttonBaseExport.TabIndex = 5;
            this.buttonBaseExport.Text = "Сохранить базу";
            this.buttonBaseExport.UseVisualStyleBackColor = true;
            this.buttonBaseExport.Click += new System.EventHandler(this.buttonBaseExport_Click);
            // 
            // buttonSynchronizeReorders
            // 
            this.buttonSynchronizeReorders.Location = new System.Drawing.Point(12, 128);
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
            this.textBoxReservName.Location = new System.Drawing.Point(249, 129);
            this.textBoxReservName.Name = "textBoxReservName";
            this.textBoxReservName.Size = new System.Drawing.Size(59, 23);
            this.textBoxReservName.TabIndex = 7;
            this.textBoxReservName.Text = "_Резерв";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(308, 132);
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
            this.checkedListBoxWithSync.Location = new System.Drawing.Point(12, 157);
            this.checkedListBoxWithSync.Name = "checkedListBoxWithSync";
            this.checkedListBoxWithSync.Size = new System.Drawing.Size(231, 346);
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
            this.groupBox1.Location = new System.Drawing.Point(322, 434);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(98, 78);
            this.groupBox1.TabIndex = 12;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Логи";
            // 
            // checkBoxCopyChangedOtherDays
            // 
            this.checkBoxCopyChangedOtherDays.Location = new System.Drawing.Point(249, 158);
            this.checkBoxCopyChangedOtherDays.Name = "checkBoxCopyChangedOtherDays";
            this.checkBoxCopyChangedOtherDays.Size = new System.Drawing.Size(171, 37);
            this.checkBoxCopyChangedOtherDays.TabIndex = 13;
            this.checkBoxCopyChangedOtherDays.Text = "Копирование при изменении в других днях";
            this.checkBoxCopyChangedOtherDays.UseVisualStyleBackColor = true;
            // 
            // buttonCreateNewAdded
            // 
            this.buttonCreateNewAdded.Location = new System.Drawing.Point(12, 70);
            this.buttonCreateNewAdded.Name = "buttonCreateNewAdded";
            this.buttonCreateNewAdded.Size = new System.Drawing.Size(231, 23);
            this.buttonCreateNewAdded.TabIndex = 15;
            this.buttonCreateNewAdded.Text = "Создать дозаявленых в остальных днях";
            this.buttonCreateNewAdded.UseVisualStyleBackColor = true;
            this.buttonCreateNewAdded.Click += new System.EventHandler(this.buttonCreateNewAdded_Click);
            // 
            // buttonCardNumAsNum
            // 
            this.buttonCardNumAsNum.Location = new System.Drawing.Point(12, 99);
            this.buttonCardNumAsNum.Name = "buttonCardNumAsNum";
            this.buttonCardNumAsNum.Size = new System.Drawing.Size(231, 23);
            this.buttonCardNumAsNum.TabIndex = 16;
            this.buttonCardNumAsNum.Text = "Установить номер чипа = номеру";
            this.buttonCardNumAsNum.UseVisualStyleBackColor = true;
            this.buttonCardNumAsNum.Click += new System.EventHandler(this.buttonCardNumAsNum_Click);
            // 
            // Utils
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1397, 518);
            this.Controls.Add(this.buttonCardNumAsNum);
            this.Controls.Add(this.buttonCreateNewAdded);
            this.Controls.Add(this.checkBoxCopyChangedOtherDays);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.checkedListBoxWithSync);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxReservName);
            this.Controls.Add(this.buttonSynchronizeReorders);
            this.Controls.Add(this.buttonBaseExport);
            this.Controls.Add(this.buttonRemoveMissingPersons);
            this.Controls.Add(this.labelBaseImport);
            this.Controls.Add(this.richTextBoxLog);
            this.Controls.Add(this.buttonBaseImport);
            this.Name = "Utils";
            this.Text = "Утилиты";
            this.Load += new System.EventHandler(this.Utils_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
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
    }
}