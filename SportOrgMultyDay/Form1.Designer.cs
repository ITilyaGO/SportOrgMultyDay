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
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
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
            this.SuspendLayout();
            // 
            // buttonImportHtml
            // 
            this.buttonImportHtml.Location = new System.Drawing.Point(167, 5);
            this.buttonImportHtml.Name = "buttonImportHtml";
            this.buttonImportHtml.Size = new System.Drawing.Size(97, 23);
            this.buttonImportHtml.TabIndex = 0;
            this.buttonImportHtml.Text = "Импорт HTML";
            this.buttonImportHtml.UseVisualStyleBackColor = true;
            this.buttonImportHtml.Click += new System.EventHandler(this.buttonImportHtml_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            this.openFileDialog1.Filter = "Html|*.html|All files|*.*";
            // 
            // richTextBoxOut
            // 
            this.richTextBoxOut.Location = new System.Drawing.Point(474, 13);
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
            this.buttonCombine.Location = new System.Drawing.Point(167, 63);
            this.buttonCombine.Name = "buttonCombine";
            this.buttonCombine.Size = new System.Drawing.Size(137, 23);
            this.buttonCombine.TabIndex = 4;
            this.buttonCombine.Text = "Комбинировать JSON";
            this.buttonCombine.UseVisualStyleBackColor = true;
            this.buttonCombine.Click += new System.EventHandler(this.buttonCombine_Click);
            // 
            // buttonImportHtml2
            // 
            this.buttonImportHtml2.Location = new System.Drawing.Point(167, 34);
            this.buttonImportHtml2.Name = "buttonImportHtml2";
            this.buttonImportHtml2.Size = new System.Drawing.Size(97, 23);
            this.buttonImportHtml2.TabIndex = 5;
            this.buttonImportHtml2.Text = "Импорт HTML";
            this.buttonImportHtml2.UseVisualStyleBackColor = true;
            this.buttonImportHtml2.Click += new System.EventHandler(this.buttonImportHtml2_Click);
            // 
            // textBoxStart1
            // 
            this.textBoxStart1.Location = new System.Drawing.Point(61, 5);
            this.textBoxStart1.Name = "textBoxStart1";
            this.textBoxStart1.PlaceholderText = "Старт 01.01";
            this.textBoxStart1.Size = new System.Drawing.Size(100, 23);
            this.textBoxStart1.TabIndex = 6;
            // 
            // textBoxStart2
            // 
            this.textBoxStart2.Location = new System.Drawing.Point(61, 35);
            this.textBoxStart2.Name = "textBoxStart2";
            this.textBoxStart2.PlaceholderText = "Старт 02.01";
            this.textBoxStart2.Size = new System.Drawing.Size(100, 23);
            this.textBoxStart2.TabIndex = 7;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(43, 15);
            this.label1.TabIndex = 8;
            this.label1.Text = "День 1";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 38);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(43, 15);
            this.label2.TabIndex = 8;
            this.label2.Text = "День 2";
            // 
            // buttonPutToBib
            // 
            this.buttonPutToBib.Location = new System.Drawing.Point(167, 92);
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
            this.labelDay1Info.Location = new System.Drawing.Point(266, 9);
            this.labelDay1Info.Name = "labelDay1Info";
            this.labelDay1Info.Size = new System.Drawing.Size(0, 15);
            this.labelDay1Info.TabIndex = 10;
            // 
            // labelDay2Info
            // 
            this.labelDay2Info.AutoSize = true;
            this.labelDay2Info.Location = new System.Drawing.Point(266, 38);
            this.labelDay2Info.Name = "labelDay2Info";
            this.labelDay2Info.Size = new System.Drawing.Size(0, 15);
            this.labelDay2Info.TabIndex = 11;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.labelDay2Info);
            this.Controls.Add(this.labelDay1Info);
            this.Controls.Add(this.buttonPutToBib);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxStart2);
            this.Controls.Add(this.textBoxStart1);
            this.Controls.Add(this.buttonImportHtml2);
            this.Controls.Add(this.buttonCombine);
            this.Controls.Add(this.labelRaceIndex);
            this.Controls.Add(this.richTextBoxOut);
            this.Controls.Add(this.buttonImportHtml);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Button buttonImportHtml;
        private OpenFileDialog openFileDialog1;
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
    }
}