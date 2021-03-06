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
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
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
            this.richTextBoxLog.Location = new System.Drawing.Point(510, 12);
            this.richTextBoxLog.Name = "richTextBoxLog";
            this.richTextBoxLog.Size = new System.Drawing.Size(875, 500);
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
            this.labelBaseImport.Size = new System.Drawing.Size(36, 15);
            this.labelBaseImport.TabIndex = 3;
            this.labelBaseImport.Text = "None";
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
            this.buttonBaseExport.Location = new System.Drawing.Point(399, 16);
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
            this.checkedListBoxWithSync.Location = new System.Drawing.Point(18, 175);
            this.checkedListBoxWithSync.Name = "checkedListBoxWithSync";
            this.checkedListBoxWithSync.Size = new System.Drawing.Size(231, 328);
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
            this.groupBox1.Location = new System.Drawing.Point(406, 434);
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
            this.buttonCopyPersonByNumber.Size = new System.Drawing.Size(231, 23);
            this.buttonCopyPersonByNumber.TabIndex = 17;
            this.buttonCopyPersonByNumber.Text = "Копирвоать участников";
            this.buttonCopyPersonByNumber.UseVisualStyleBackColor = true;
            this.buttonCopyPersonByNumber.Click += new System.EventHandler(this.buttonCopyPersonByNumber_Click);
            // 
            // textBoxPersonsFromCopy
            // 
            this.textBoxPersonsFromCopy.Location = new System.Drawing.Point(6, 84);
            this.textBoxPersonsFromCopy.Name = "textBoxPersonsFromCopy";
            this.textBoxPersonsFromCopy.Size = new System.Drawing.Size(230, 23);
            this.textBoxPersonsFromCopy.TabIndex = 18;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(58, 66);
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
            this.textBoxStringFindComment.Size = new System.Drawing.Size(92, 23);
            this.textBoxStringFindComment.TabIndex = 22;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.buttonRemoveMissingPersons);
            this.groupBox2.Controls.Add(this.buttonCreateNewAdded);
            this.groupBox2.Controls.Add(this.buttonCardNumAsNum);
            this.groupBox2.Location = new System.Drawing.Point(12, 41);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(244, 110);
            this.groupBox2.TabIndex = 23;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Остальное";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.buttonSynchronizeReorders);
            this.groupBox3.Controls.Add(this.textBoxReservName);
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.Controls.Add(this.checkBoxCopyChangedOtherDays);
            this.groupBox3.Location = new System.Drawing.Point(262, 41);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(242, 127);
            this.groupBox3.TabIndex = 24;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Синхронизация";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.buttonFindAddWithComment);
            this.groupBox4.Controls.Add(this.buttonCopyPersonByNumber);
            this.groupBox4.Controls.Add(this.textBoxPersonsFromCopy);
            this.groupBox4.Controls.Add(this.textBoxStringFindComment);
            this.groupBox4.Controls.Add(this.label2);
            this.groupBox4.Location = new System.Drawing.Point(262, 174);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(242, 143);
            this.groupBox4.TabIndex = 25;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Копирование";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(18, 157);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(225, 15);
            this.label3.TabIndex = 26;
            this.label3.Text = "Поля для копирования/синхронизации";
            // 
            // Utils
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1392, 518);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.buttonCombineAllBase);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.checkedListBoxWithSync);
            this.Controls.Add(this.buttonBaseExport);
            this.Controls.Add(this.labelBaseImport);
            this.Controls.Add(this.richTextBoxLog);
            this.Controls.Add(this.buttonBaseImport);
            this.Name = "Utils";
            this.Text = "Утилиты";
            this.Load += new System.EventHandler(this.Utils_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
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
    }
}