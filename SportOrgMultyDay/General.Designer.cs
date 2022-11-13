namespace SportOrgMultyDay
{
    partial class General
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(General));
            this.buttonOpenNumbers = new System.Windows.Forms.Button();
            this.buttonOpenUtils = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // buttonOpenNumbers
            // 
            this.buttonOpenNumbers.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.buttonOpenNumbers.Location = new System.Drawing.Point(12, 12);
            this.buttonOpenNumbers.Name = "buttonOpenNumbers";
            this.buttonOpenNumbers.Size = new System.Drawing.Size(100, 100);
            this.buttonOpenNumbers.TabIndex = 0;
            this.buttonOpenNumbers.Text = "Номера";
            this.buttonOpenNumbers.UseVisualStyleBackColor = true;
            this.buttonOpenNumbers.Click += new System.EventHandler(this.buttonOpenNumbers_Click);
            // 
            // buttonOpenUtils
            // 
            this.buttonOpenUtils.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.buttonOpenUtils.Location = new System.Drawing.Point(116, 12);
            this.buttonOpenUtils.Name = "buttonOpenUtils";
            this.buttonOpenUtils.Size = new System.Drawing.Size(100, 100);
            this.buttonOpenUtils.TabIndex = 1;
            this.buttonOpenUtils.Text = "Утилиты";
            this.buttonOpenUtils.UseVisualStyleBackColor = true;
            this.buttonOpenUtils.Click += new System.EventHandler(this.buttonOpenUtils_Click);
            // 
            // General
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(230, 122);
            this.Controls.Add(this.buttonOpenUtils);
            this.Controls.Add(this.buttonOpenNumbers);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "General";
            this.Text = "SportOrg Helper";
            this.Load += new System.EventHandler(this.General_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private Button buttonOpenNumbers;
        private Button buttonOpenUtils;
    }
}