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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.buttonBaseImport = new System.Windows.Forms.Button();
            this.richTextBoxOut = new System.Windows.Forms.RichTextBox();
            this.richTextBoxLog = new System.Windows.Forms.RichTextBox();
            this.labelBaseImport = new System.Windows.Forms.Label();
            this.buttonRemoveMissingPersons = new System.Windows.Forms.Button();
            this.buttonBaseExport = new System.Windows.Forms.Button();
            this.buttonSynchronizeReorders = new System.Windows.Forms.Button();
            this.saveFileDialogJson = new System.Windows.Forms.SaveFileDialog();
            this.openFileDialogJson = new System.Windows.Forms.OpenFileDialog();
            this.tabControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(12, 202);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(400, 138);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Location = new System.Drawing.Point(4, 24);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(392, 110);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "tabPage1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Location = new System.Drawing.Point(4, 24);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(392, 110);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
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
            // richTextBoxOut
            // 
            this.richTextBoxOut.Location = new System.Drawing.Point(12, 342);
            this.richTextBoxOut.Name = "richTextBoxOut";
            this.richTextBoxOut.Size = new System.Drawing.Size(151, 96);
            this.richTextBoxOut.TabIndex = 1;
            this.richTextBoxOut.Text = "";
            // 
            // richTextBoxLog
            // 
            this.richTextBoxLog.Location = new System.Drawing.Point(418, 12);
            this.richTextBoxLog.Name = "richTextBoxLog";
            this.richTextBoxLog.ReadOnly = true;
            this.richTextBoxLog.Size = new System.Drawing.Size(620, 426);
            this.richTextBoxLog.TabIndex = 2;
            this.richTextBoxLog.Text = "";
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
            this.buttonRemoveMissingPersons.Size = new System.Drawing.Size(209, 23);
            this.buttonRemoveMissingPersons.TabIndex = 4;
            this.buttonRemoveMissingPersons.Text = "Удалить отсутствующих по дням";
            this.buttonRemoveMissingPersons.UseVisualStyleBackColor = true;
            this.buttonRemoveMissingPersons.Click += new System.EventHandler(this.buttonRemoveMissingPersons_Click);
            // 
            // buttonBaseExport
            // 
            this.buttonBaseExport.Location = new System.Drawing.Point(307, 12);
            this.buttonBaseExport.Name = "buttonBaseExport";
            this.buttonBaseExport.Size = new System.Drawing.Size(105, 23);
            this.buttonBaseExport.TabIndex = 5;
            this.buttonBaseExport.Text = "Сохранить базу";
            this.buttonBaseExport.UseVisualStyleBackColor = true;
            this.buttonBaseExport.Click += new System.EventHandler(this.buttonBaseExport_Click);
            // 
            // buttonSynchronizeReorders
            // 
            this.buttonSynchronizeReorders.Location = new System.Drawing.Point(12, 70);
            this.buttonSynchronizeReorders.Name = "buttonSynchronizeReorders";
            this.buttonSynchronizeReorders.Size = new System.Drawing.Size(209, 23);
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
            // Utils
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1050, 450);
            this.Controls.Add(this.buttonSynchronizeReorders);
            this.Controls.Add(this.buttonBaseExport);
            this.Controls.Add(this.buttonRemoveMissingPersons);
            this.Controls.Add(this.labelBaseImport);
            this.Controls.Add(this.richTextBoxLog);
            this.Controls.Add(this.richTextBoxOut);
            this.Controls.Add(this.buttonBaseImport);
            this.Controls.Add(this.tabControl1);
            this.Name = "Utils";
            this.Text = "Utils";
            this.Load += new System.EventHandler(this.Utils_Load);
            this.tabControl1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private TabControl tabControl1;
        private TabPage tabPage1;
        private Button buttonBaseImport;
        private TabPage tabPage2;
        private RichTextBox richTextBoxOut;
        private RichTextBox richTextBoxLog;
        private Label labelBaseImport;
        private Button buttonRemoveMissingPersons;
        private Button buttonBaseExport;
        private Button buttonSynchronizeReorders;
        private SaveFileDialog saveFileDialogJson;
        private OpenFileDialog openFileDialogJson;
    }
}