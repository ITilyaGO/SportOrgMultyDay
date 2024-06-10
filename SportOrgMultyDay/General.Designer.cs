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
            buttonOpenNumbers = new Button();
            buttonOpenUtils = new Button();
            SuspendLayout();
            // 
            // buttonOpenNumbers
            // 
            buttonOpenNumbers.Font = new Font("Segoe UI", 15F, FontStyle.Regular, GraphicsUnit.Point);
            buttonOpenNumbers.Location = new Point(12, 12);
            buttonOpenNumbers.Name = "buttonOpenNumbers";
            buttonOpenNumbers.Size = new Size(100, 100);
            buttonOpenNumbers.TabIndex = 0;
            buttonOpenNumbers.Text = "Номера";
            buttonOpenNumbers.UseVisualStyleBackColor = true;
            buttonOpenNumbers.Click += buttonOpenNumbers_Click;
            // 
            // buttonOpenUtils
            // 
            buttonOpenUtils.Font = new Font("Segoe UI", 15F, FontStyle.Regular, GraphicsUnit.Point);
            buttonOpenUtils.Location = new Point(118, 12);
            buttonOpenUtils.Name = "buttonOpenUtils";
            buttonOpenUtils.Size = new Size(100, 100);
            buttonOpenUtils.TabIndex = 1;
            buttonOpenUtils.Text = "Утилиты";
            buttonOpenUtils.UseVisualStyleBackColor = true;
            buttonOpenUtils.Click += buttonOpenUtils_Click;
            // 
            // General
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(228, 120);
            Controls.Add(buttonOpenUtils);
            Controls.Add(buttonOpenNumbers);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "General";
            Text = "SportOrg Helper";
            Load += General_Load;
            Shown += General_Shown;
            ResumeLayout(false);
        }

        #endregion

        private Button buttonOpenNumbers;
        private Button buttonOpenUtils;
    }
}