namespace SportOrgMultyDay
{
    partial class Numbers
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
            buttonImportHtml = new Button();
            openFileDialogHtml = new OpenFileDialog();
            richTextBoxOut = new RichTextBox();
            labelRaceIndex = new Label();
            buttonCombine = new Button();
            buttonImportHtml2 = new Button();
            textBoxStart1 = new TextBox();
            textBoxStart2 = new TextBox();
            label1 = new Label();
            label2 = new Label();
            buttonPutToBib = new Button();
            saveFileDialog1 = new SaveFileDialog();
            labelDay1Info = new Label();
            labelDay2Info = new Label();
            buttonImportRaceJson = new Button();
            tabControl1 = new TabControl();
            tabPage2 = new TabPage();
            buttonBaseFromUtilits = new Button();
            labelBibFinded = new Label();
            buttonImportBib = new Button();
            buttonProcessing = new Button();
            labelRaceFindedDays = new Label();
            buttonExportBibs = new Button();
            tabPage1 = new TabPage();
            openFileDialogJson = new OpenFileDialog();
            tabControl1.SuspendLayout();
            tabPage2.SuspendLayout();
            tabPage1.SuspendLayout();
            SuspendLayout();
            // 
            // buttonImportHtml
            // 
            buttonImportHtml.Location = new Point(161, 8);
            buttonImportHtml.Name = "buttonImportHtml";
            buttonImportHtml.Size = new Size(97, 23);
            buttonImportHtml.TabIndex = 0;
            buttonImportHtml.Text = "Импорт HTML";
            buttonImportHtml.UseVisualStyleBackColor = true;
            buttonImportHtml.Click += buttonImportHtml_Click;
            // 
            // openFileDialogHtml
            // 
            openFileDialogHtml.FileName = "openFileDialog1";
            openFileDialogHtml.Filter = "Html|*.html|All files|*.*";
            // 
            // richTextBoxOut
            // 
            richTextBoxOut.Location = new Point(474, 12);
            richTextBoxOut.Name = "richTextBoxOut";
            richTextBoxOut.Size = new Size(314, 292);
            richTextBoxOut.TabIndex = 1;
            richTextBoxOut.Text = "";
            richTextBoxOut.TextChanged += richTextBoxOut_TextChanged;
            // 
            // labelRaceIndex
            // 
            labelRaceIndex.AutoSize = true;
            labelRaceIndex.Location = new Point(474, 308);
            labelRaceIndex.Name = "labelRaceIndex";
            labelRaceIndex.Size = new Size(38, 15);
            labelRaceIndex.TabIndex = 2;
            labelRaceIndex.Text = "label1";
            // 
            // buttonCombine
            // 
            buttonCombine.Location = new Point(161, 66);
            buttonCombine.Name = "buttonCombine";
            buttonCombine.Size = new Size(137, 23);
            buttonCombine.TabIndex = 4;
            buttonCombine.Text = "Комбинировать JSON";
            buttonCombine.UseVisualStyleBackColor = true;
            buttonCombine.Click += buttonCombine_Click;
            // 
            // buttonImportHtml2
            // 
            buttonImportHtml2.Location = new Point(161, 37);
            buttonImportHtml2.Name = "buttonImportHtml2";
            buttonImportHtml2.Size = new Size(97, 23);
            buttonImportHtml2.TabIndex = 5;
            buttonImportHtml2.Text = "Импорт HTML";
            buttonImportHtml2.UseVisualStyleBackColor = true;
            buttonImportHtml2.Click += buttonImportHtml2_Click;
            // 
            // textBoxStart1
            // 
            textBoxStart1.Location = new Point(55, 8);
            textBoxStart1.Name = "textBoxStart1";
            textBoxStart1.PlaceholderText = "Старт 01.01";
            textBoxStart1.Size = new Size(100, 23);
            textBoxStart1.TabIndex = 6;
            // 
            // textBoxStart2
            // 
            textBoxStart2.Location = new Point(55, 38);
            textBoxStart2.Name = "textBoxStart2";
            textBoxStart2.PlaceholderText = "Старт 02.01";
            textBoxStart2.Size = new Size(100, 23);
            textBoxStart2.TabIndex = 7;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(6, 12);
            label1.Name = "label1";
            label1.Size = new Size(43, 15);
            label1.TabIndex = 8;
            label1.Text = "День 1";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(6, 41);
            label2.Name = "label2";
            label2.Size = new Size(43, 15);
            label2.TabIndex = 8;
            label2.Text = "День 2";
            // 
            // buttonPutToBib
            // 
            buttonPutToBib.Location = new Point(161, 95);
            buttonPutToBib.Name = "buttonPutToBib";
            buttonPutToBib.Size = new Size(115, 23);
            buttonPutToBib.TabIndex = 9;
            buttonPutToBib.Text = "Экспорт номеров";
            buttonPutToBib.UseVisualStyleBackColor = true;
            buttonPutToBib.Click += buttonPutToBib_Click;
            // 
            // saveFileDialog1
            // 
            saveFileDialog1.Filter = "Html|*.html|All files|*.*";
            // 
            // labelDay1Info
            // 
            labelDay1Info.AutoSize = true;
            labelDay1Info.Location = new Point(260, 12);
            labelDay1Info.Name = "labelDay1Info";
            labelDay1Info.Size = new Size(0, 15);
            labelDay1Info.TabIndex = 10;
            // 
            // labelDay2Info
            // 
            labelDay2Info.AutoSize = true;
            labelDay2Info.Location = new Point(260, 41);
            labelDay2Info.Name = "labelDay2Info";
            labelDay2Info.Size = new Size(0, 15);
            labelDay2Info.TabIndex = 11;
            // 
            // buttonImportRaceJson
            // 
            buttonImportRaceJson.Location = new Point(6, 6);
            buttonImportRaceJson.Name = "buttonImportRaceJson";
            buttonImportRaceJson.Size = new Size(179, 23);
            buttonImportRaceJson.TabIndex = 12;
            buttonImportRaceJson.Text = "Выбрать файл соревнований";
            buttonImportRaceJson.UseVisualStyleBackColor = true;
            buttonImportRaceJson.Click += buttonImportRaceJson_Click;
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tabPage2);
            tabControl1.Controls.Add(tabPage1);
            tabControl1.Location = new Point(12, 12);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(456, 292);
            tabControl1.TabIndex = 13;
            // 
            // tabPage2
            // 
            tabPage2.Controls.Add(buttonBaseFromUtilits);
            tabPage2.Controls.Add(labelBibFinded);
            tabPage2.Controls.Add(buttonImportBib);
            tabPage2.Controls.Add(buttonProcessing);
            tabPage2.Controls.Add(labelRaceFindedDays);
            tabPage2.Controls.Add(buttonExportBibs);
            tabPage2.Controls.Add(buttonImportRaceJson);
            tabPage2.Location = new Point(4, 24);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new Padding(3);
            tabPage2.Size = new Size(448, 264);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "Файла соревнований";
            tabPage2.UseVisualStyleBackColor = true;
            // 
            // buttonBaseFromUtilits
            // 
            buttonBaseFromUtilits.Location = new Point(186, 6);
            buttonBaseFromUtilits.Name = "buttonBaseFromUtilits";
            buttonBaseFromUtilits.Size = new Size(129, 23);
            buttonBaseFromUtilits.TabIndex = 21;
            buttonBaseFromUtilits.Text = "Взять базу из Утилит";
            buttonBaseFromUtilits.UseVisualStyleBackColor = true;
            buttonBaseFromUtilits.Click += buttonBaseFromUtilits_Click;
            // 
            // labelBibFinded
            // 
            labelBibFinded.AutoSize = true;
            labelBibFinded.Location = new Point(191, 39);
            labelBibFinded.Name = "labelBibFinded";
            labelBibFinded.Size = new Size(36, 15);
            labelBibFinded.TabIndex = 20;
            labelBibFinded.Text = "None";
            // 
            // buttonImportBib
            // 
            buttonImportBib.Location = new Point(6, 35);
            buttonImportBib.Name = "buttonImportBib";
            buttonImportBib.Size = new Size(179, 23);
            buttonImportBib.TabIndex = 19;
            buttonImportBib.Text = "Выбрать файл номеров";
            buttonImportBib.UseVisualStyleBackColor = true;
            buttonImportBib.Click += buttonImportBib_Click;
            // 
            // buttonProcessing
            // 
            buttonProcessing.Location = new Point(6, 78);
            buttonProcessing.Name = "buttonProcessing";
            buttonProcessing.Size = new Size(179, 23);
            buttonProcessing.TabIndex = 17;
            buttonProcessing.Text = "Обработать";
            buttonProcessing.UseVisualStyleBackColor = true;
            buttonProcessing.Click += buttonProcessing_Click;
            // 
            // labelRaceFindedDays
            // 
            labelRaceFindedDays.AutoSize = true;
            labelRaceFindedDays.Location = new Point(321, 10);
            labelRaceFindedDays.Name = "labelRaceFindedDays";
            labelRaceFindedDays.Size = new Size(36, 15);
            labelRaceFindedDays.TabIndex = 16;
            labelRaceFindedDays.Text = "None";
            // 
            // buttonExportBibs
            // 
            buttonExportBibs.Location = new Point(6, 123);
            buttonExportBibs.Name = "buttonExportBibs";
            buttonExportBibs.Size = new Size(179, 23);
            buttonExportBibs.TabIndex = 15;
            buttonExportBibs.Text = "Экспорт номеров";
            buttonExportBibs.UseVisualStyleBackColor = true;
            buttonExportBibs.Click += buttonExportBibs_Click;
            // 
            // tabPage1
            // 
            tabPage1.Controls.Add(label1);
            tabPage1.Controls.Add(buttonImportHtml);
            tabPage1.Controls.Add(labelDay2Info);
            tabPage1.Controls.Add(buttonCombine);
            tabPage1.Controls.Add(labelDay1Info);
            tabPage1.Controls.Add(buttonImportHtml2);
            tabPage1.Controls.Add(buttonPutToBib);
            tabPage1.Controls.Add(textBoxStart1);
            tabPage1.Controls.Add(label2);
            tabPage1.Controls.Add(textBoxStart2);
            tabPage1.Location = new Point(4, 24);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(3);
            tabPage1.Size = new Size(448, 264);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "Комбинирование";
            tabPage1.UseVisualStyleBackColor = true;
            // 
            // openFileDialogJson
            // 
            openFileDialogJson.FileName = "openFileDialog1";
            openFileDialogJson.Filter = "Json|*.json|All files|*.*";
            // 
            // Numbers
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 328);
            Controls.Add(tabControl1);
            Controls.Add(labelRaceIndex);
            Controls.Add(richTextBoxOut);
            Name = "Numbers";
            Text = "Номера (Устарело!)";
            FormClosing += Numbers_FormClosing;
            Load += Numbers_Load;
            tabControl1.ResumeLayout(false);
            tabPage2.ResumeLayout(false);
            tabPage2.PerformLayout();
            tabPage1.ResumeLayout(false);
            tabPage1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
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
        private Button buttonBaseFromUtilits;
    }
}