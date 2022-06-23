namespace SportOrgMultyDay
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.buttonImportHtml = new System.Windows.Forms.Button();
            this.openFileDialogHtml = new System.Windows.Forms.OpenFileDialog();
            this.richTextBoxOut = new System.Windows.Forms.RichTextBox();
            this.labelRaceIndex = new System.Windows.Forms.Label();
            this.buttonCombine = new System.Windows.Forms.Button();
            this.buttonImportHtml2 = new System.Windows.Forms.Button();
            this.textBoxStart1 = new System.Windows.Forms.TextBox();
            this.textBoxStart2 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.buttonPutToBib = new System.Windows.Forms.Button();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.labelDay1Info = new System.Windows.Forms.Label();
            this.labelDay2Info = new System.Windows.Forms.Label();
            this.buttonImportRaceJson = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.labelBibFinded = new System.Windows.Forms.Label();
            this.buttonImportBib = new System.Windows.Forms.Button();
            this.buttonProcessing = new System.Windows.Forms.Button();
            this.labelRaceFindedDays = new System.Windows.Forms.Label();
            this.buttonExportBibs = new System.Windows.Forms.Button();
            this.openFileDialogJson = new System.Windows.Forms.OpenFileDialog();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonImportHtml
            // 
            this.buttonImportHtml.Location = new System.Drawing.Point(161, 8);
            this.buttonImportHtml.Name = "buttonImportHtml";
            this.buttonImportHtml.Size = new System.Drawing.Size(97, 23);
            this.buttonImportHtml.TabIndex = 0;
            this.buttonImportHtml.Text = "Импорт HTML";
            this.buttonImportHtml.UseVisualStyleBackColor = true;
            this.buttonImportHtml.Click += new System.EventHandler(this.buttonImportHtml_Click);
            // 
            // openFileDialogHtml
            // 
            this.openFileDialogHtml.FileName = "openFileDialog1";
            this.openFileDialogHtml.Filter = "Html|*.html|All files|*.*";
            // 
            // richTextBoxOut
            // 
            this.richTextBoxOut.Location = new System.Drawing.Point(474, 12);
            this.richTextBoxOut.Name = "richTextBoxOut";
            this.richTextBoxOut.Size = new System.Drawing.Size(314, 292);
            this.richTextBoxOut.TabIndex = 1;
            this.richTextBoxOut.Text = "";
            // 
            // labelRaceIndex
            // 
            this.labelRaceIndex.AutoSize = true;
            this.labelRaceIndex.Location = new System.Drawing.Point(474, 308);
            this.labelRaceIndex.Name = "labelRaceIndex";
            this.labelRaceIndex.Size = new System.Drawing.Size(38, 15);
            this.labelRaceIndex.TabIndex = 2;
            this.labelRaceIndex.Text = "label1";
            // 
            // buttonCombine
            // 
            this.buttonCombine.Location = new System.Drawing.Point(161, 66);
            this.buttonCombine.Name = "buttonCombine";
            this.buttonCombine.Size = new System.Drawing.Size(137, 23);
            this.buttonCombine.TabIndex = 4;
            this.buttonCombine.Text = "Комбинировать JSON";
            this.buttonCombine.UseVisualStyleBackColor = true;
            this.buttonCombine.Click += new System.EventHandler(this.buttonCombine_Click);
            // 
            // buttonImportHtml2
            // 
            this.buttonImportHtml2.Location = new System.Drawing.Point(161, 37);
            this.buttonImportHtml2.Name = "buttonImportHtml2";
            this.buttonImportHtml2.Size = new System.Drawing.Size(97, 23);
            this.buttonImportHtml2.TabIndex = 5;
            this.buttonImportHtml2.Text = "Импорт HTML";
            this.buttonImportHtml2.UseVisualStyleBackColor = true;
            this.buttonImportHtml2.Click += new System.EventHandler(this.buttonImportHtml2_Click);
            // 
            // textBoxStart1
            // 
            this.textBoxStart1.Location = new System.Drawing.Point(55, 8);
            this.textBoxStart1.Name = "textBoxStart1";
            this.textBoxStart1.PlaceholderText = "Старт 01.01";
            this.textBoxStart1.Size = new System.Drawing.Size(100, 23);
            this.textBoxStart1.TabIndex = 6;
            // 
            // textBoxStart2
            // 
            this.textBoxStart2.Location = new System.Drawing.Point(55, 38);
            this.textBoxStart2.Name = "textBoxStart2";
            this.textBoxStart2.PlaceholderText = "Старт 02.01";
            this.textBoxStart2.Size = new System.Drawing.Size(100, 23);
            this.textBoxStart2.TabIndex = 7;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(43, 15);
            this.label1.TabIndex = 8;
            this.label1.Text = "День 1";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 41);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(43, 15);
            this.label2.TabIndex = 8;
            this.label2.Text = "День 2";
            // 
            // buttonPutToBib
            // 
            this.buttonPutToBib.Location = new System.Drawing.Point(161, 95);
            this.buttonPutToBib.Name = "buttonPutToBib";
            this.buttonPutToBib.Size = new System.Drawing.Size(115, 23);
            this.buttonPutToBib.TabIndex = 9;
            this.buttonPutToBib.Text = "Экспорт номеров";
            this.buttonPutToBib.UseVisualStyleBackColor = true;
            this.buttonPutToBib.Click += new System.EventHandler(this.buttonPutToBib_Click);
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.Filter = "Html|*.html|All files|*.*";
            // 
            // labelDay1Info
            // 
            this.labelDay1Info.AutoSize = true;
            this.labelDay1Info.Location = new System.Drawing.Point(260, 12);
            this.labelDay1Info.Name = "labelDay1Info";
            this.labelDay1Info.Size = new System.Drawing.Size(0, 15);
            this.labelDay1Info.TabIndex = 10;
            // 
            // labelDay2Info
            // 
            this.labelDay2Info.AutoSize = true;
            this.labelDay2Info.Location = new System.Drawing.Point(260, 41);
            this.labelDay2Info.Name = "labelDay2Info";
            this.labelDay2Info.Size = new System.Drawing.Size(0, 15);
            this.labelDay2Info.TabIndex = 11;
            // 
            // buttonImportRaceJson
            // 
            this.buttonImportRaceJson.Location = new System.Drawing.Point(6, 6);
            this.buttonImportRaceJson.Name = "buttonImportRaceJson";
            this.buttonImportRaceJson.Size = new System.Drawing.Size(179, 23);
            this.buttonImportRaceJson.TabIndex = 12;
            this.buttonImportRaceJson.Text = "Выбрать файл соревнований";
            this.buttonImportRaceJson.UseVisualStyleBackColor = true;
            this.buttonImportRaceJson.Click += new System.EventHandler(this.buttonImportRaceJson_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(456, 292);
            this.tabControl1.TabIndex = 13;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.buttonImportHtml);
            this.tabPage1.Controls.Add(this.labelDay2Info);
            this.tabPage1.Controls.Add(this.buttonCombine);
            this.tabPage1.Controls.Add(this.labelDay1Info);
            this.tabPage1.Controls.Add(this.buttonImportHtml2);
            this.tabPage1.Controls.Add(this.buttonPutToBib);
            this.tabPage1.Controls.Add(this.textBoxStart1);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.textBoxStart2);
            this.tabPage1.Location = new System.Drawing.Point(4, 24);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(448, 264);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Комбинирование";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.labelBibFinded);
            this.tabPage2.Controls.Add(this.buttonImportBib);
            this.tabPage2.Controls.Add(this.buttonProcessing);
            this.tabPage2.Controls.Add(this.labelRaceFindedDays);
            this.tabPage2.Controls.Add(this.buttonExportBibs);
            this.tabPage2.Controls.Add(this.buttonImportRaceJson);
            this.tabPage2.Location = new System.Drawing.Point(4, 24);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(448, 244);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Файла соревнований";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // labelBibFinded
            // 
            this.labelBibFinded.AutoSize = true;
            this.labelBibFinded.Location = new System.Drawing.Point(191, 39);
            this.labelBibFinded.Name = "labelBibFinded";
            this.labelBibFinded.Size = new System.Drawing.Size(36, 15);
            this.labelBibFinded.TabIndex = 20;
            this.labelBibFinded.Text = "None";
            // 
            // buttonImportBib
            // 
            this.buttonImportBib.Location = new System.Drawing.Point(6, 35);
            this.buttonImportBib.Name = "buttonImportBib";
            this.buttonImportBib.Size = new System.Drawing.Size(179, 23);
            this.buttonImportBib.TabIndex = 19;
            this.buttonImportBib.Text = "Выбрать файл номеров";
            this.buttonImportBib.UseVisualStyleBackColor = true;
            this.buttonImportBib.Click += new System.EventHandler(this.buttonImportBib_Click);
            // 
            // buttonProcessing
            // 
            this.buttonProcessing.Location = new System.Drawing.Point(6, 78);
            this.buttonProcessing.Name = "buttonProcessing";
            this.buttonProcessing.Size = new System.Drawing.Size(179, 23);
            this.buttonProcessing.TabIndex = 17;
            this.buttonProcessing.Text = "Обработать";
            this.buttonProcessing.UseVisualStyleBackColor = true;
            this.buttonProcessing.Click += new System.EventHandler(this.buttonProcessing_Click);
            // 
            // labelRaceFindedDays
            // 
            this.labelRaceFindedDays.AutoSize = true;
            this.labelRaceFindedDays.Location = new System.Drawing.Point(191, 10);
            this.labelRaceFindedDays.Name = "labelRaceFindedDays";
            this.labelRaceFindedDays.Size = new System.Drawing.Size(36, 15);
            this.labelRaceFindedDays.TabIndex = 16;
            this.labelRaceFindedDays.Text = "None";
            // 
            // buttonExportBibs
            // 
            this.buttonExportBibs.Location = new System.Drawing.Point(6, 123);
            this.buttonExportBibs.Name = "buttonExportBibs";
            this.buttonExportBibs.Size = new System.Drawing.Size(179, 23);
            this.buttonExportBibs.TabIndex = 15;
            this.buttonExportBibs.Text = "Экспорт номеров";
            this.buttonExportBibs.UseVisualStyleBackColor = true;
            this.buttonExportBibs.Click += new System.EventHandler(this.buttonExportBibs_Click);
            // 
            // openFileDialogJson
            // 
            this.openFileDialogJson.FileName = "openFileDialog1";
            this.openFileDialogJson.Filter = "Json|*.json|All files|*.*";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 328);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.labelRaceIndex);
            this.Controls.Add(this.richTextBoxOut);
            this.Name = "Form1";
            this.Text = "Form1";
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Button buttonImportHtml;
        private OpenFileDialog openFileDialogHtml;
        private RichTextBox richTextBoxOut;
        private Label labelRaceIndex;
        private Button buttonCombine;
        private Button buttonImportHtml2;
        private TextBox textBoxStart1;
        private TextBox textBoxStart2;
        private Label label1;
        private Label label2;
        private Button buttonPutToBib;
        private SaveFileDialog saveFileDialog1;
        private Label labelDay1Info;
        private Label labelDay2Info;
        private Button buttonImportRaceJson;
        private TabControl tabControl1;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private Label labelRaceFindedDays;
        private Button buttonExportBibs;
        private OpenFileDialog openFileDialogJson;
        private Button buttonProcessing;
        private Button buttonImportBib;
        private Label labelBibFinded;
    }
}