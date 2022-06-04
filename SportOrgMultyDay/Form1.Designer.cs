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
            this.labelTest = new System.Windows.Forms.Label();
            this.buttonDeserialize = new System.Windows.Forms.Button();
            this.buttonImportHtml2 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // buttonImportHtml
            // 
            this.buttonImportHtml.Location = new System.Drawing.Point(12, 12);
            this.buttonImportHtml.Name = "buttonImportHtml";
            this.buttonImportHtml.Size = new System.Drawing.Size(89, 23);
            this.buttonImportHtml.TabIndex = 0;
            this.buttonImportHtml.Text = "importHtml";
            this.buttonImportHtml.UseVisualStyleBackColor = true;
            this.buttonImportHtml.Click += new System.EventHandler(this.buttonImportHtml_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
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
            this.labelRaceIndex.Location = new System.Drawing.Point(750, 308);
            this.labelRaceIndex.Name = "labelRaceIndex";
            this.labelRaceIndex.Size = new System.Drawing.Size(38, 15);
            this.labelRaceIndex.TabIndex = 2;
            this.labelRaceIndex.Text = "label1";
            // 
            // labelTest
            // 
            this.labelTest.AutoSize = true;
            this.labelTest.Location = new System.Drawing.Point(752, 349);
            this.labelTest.Name = "labelTest";
            this.labelTest.Size = new System.Drawing.Size(38, 15);
            this.labelTest.TabIndex = 3;
            this.labelTest.Text = "label1";
            // 
            // buttonDeserialize
            // 
            this.buttonDeserialize.Location = new System.Drawing.Point(12, 41);
            this.buttonDeserialize.Name = "buttonDeserialize";
            this.buttonDeserialize.Size = new System.Drawing.Size(75, 23);
            this.buttonDeserialize.TabIndex = 4;
            this.buttonDeserialize.Text = "deserialize";
            this.buttonDeserialize.UseVisualStyleBackColor = true;
            this.buttonDeserialize.Click += new System.EventHandler(this.buttonDeserialize_Click);
            // 
            // buttonImportHtml2
            // 
            this.buttonImportHtml2.Location = new System.Drawing.Point(107, 12);
            this.buttonImportHtml2.Name = "buttonImportHtml2";
            this.buttonImportHtml2.Size = new System.Drawing.Size(89, 23);
            this.buttonImportHtml2.TabIndex = 5;
            this.buttonImportHtml2.Text = "importHtml";
            this.buttonImportHtml2.UseVisualStyleBackColor = true;
            this.buttonImportHtml2.Click += new System.EventHandler(this.buttonImportHtml2_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.buttonImportHtml2);
            this.Controls.Add(this.buttonDeserialize);
            this.Controls.Add(this.labelTest);
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
        private Label labelTest;
        private Button buttonDeserialize;
        private Button buttonImportHtml2;
    }
}