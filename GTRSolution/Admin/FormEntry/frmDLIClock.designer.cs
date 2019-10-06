namespace GTRHRIS.Admin.FormEntry
{
    partial class frmDLIClock
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
            this.components = new System.ComponentModel.Container();
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance12 = new Infragistics.Win.Appearance();
            this.btnClose = new Infragistics.Win.Misc.UltraButton();
            this.ultraPanel4 = new Infragistics.Win.Misc.UltraPanel();
            this.ultraPanel2 = new Infragistics.Win.Misc.UltraPanel();
            this.btnSave = new Infragistics.Win.Misc.UltraButton();
            this.ultraPanel3 = new Infragistics.Win.Misc.UltraPanel();
            this.ultraPanel1 = new Infragistics.Win.Misc.UltraPanel();
            this.btnCancel = new Infragistics.Win.Misc.UltraButton();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.ultraPanel5 = new Infragistics.Win.Misc.UltraPanel();
            this.ultraLabel1 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraPanel4.SuspendLayout();
            this.ultraPanel2.SuspendLayout();
            this.ultraPanel3.SuspendLayout();
            this.ultraPanel1.ClientArea.SuspendLayout();
            this.ultraPanel1.SuspendLayout();
            this.ultraPanel5.ClientArea.SuspendLayout();
            this.ultraPanel5.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            this.btnClose.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.Location = new System.Drawing.Point(8, 7);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(131, 40);
            this.btnClose.TabIndex = 3;
            this.btnClose.Text = "C&lose";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // ultraPanel4
            // 
            this.ultraPanel4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.ultraPanel4.Location = new System.Drawing.Point(14, 439);
            this.ultraPanel4.Name = "ultraPanel4";
            this.ultraPanel4.Size = new System.Drawing.Size(1040, 10);
            this.ultraPanel4.TabIndex = 46;
            // 
            // ultraPanel2
            // 
            this.ultraPanel2.Dock = System.Windows.Forms.DockStyle.Left;
            this.ultraPanel2.Location = new System.Drawing.Point(0, 12);
            this.ultraPanel2.Name = "ultraPanel2";
            this.ultraPanel2.Size = new System.Drawing.Size(14, 437);
            this.ultraPanel2.TabIndex = 44;
            // 
            // btnSave
            // 
            this.btnSave.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.Location = new System.Drawing.Point(563, 7);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(131, 40);
            this.btnSave.TabIndex = 6;
            this.btnSave.Tag = "0";
            this.btnSave.Text = "&Process";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // ultraPanel3
            // 
            this.ultraPanel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.ultraPanel3.Location = new System.Drawing.Point(0, 0);
            this.ultraPanel3.Name = "ultraPanel3";
            this.ultraPanel3.Size = new System.Drawing.Size(1054, 12);
            this.ultraPanel3.TabIndex = 45;
            // 
            // ultraPanel1
            // 
            appearance1.BackColor = System.Drawing.Color.SlateGray;
            this.ultraPanel1.Appearance = appearance1;
            // 
            // ultraPanel1.ClientArea
            // 
            this.ultraPanel1.ClientArea.Controls.Add(this.btnSave);
            this.ultraPanel1.ClientArea.Controls.Add(this.btnClose);
            this.ultraPanel1.ClientArea.Controls.Add(this.btnCancel);
            this.ultraPanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.ultraPanel1.Location = new System.Drawing.Point(0, 449);
            this.ultraPanel1.Name = "ultraPanel1";
            this.ultraPanel1.Size = new System.Drawing.Size(1054, 60);
            this.ultraPanel1.TabIndex = 5;
            this.ultraPanel1.TabStop = false;
            // 
            // btnCancel
            // 
            this.btnCancel.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.Location = new System.Drawing.Point(700, 7);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(131, 40);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // ultraPanel5
            // 
            // 
            // ultraPanel5.ClientArea
            // 
            this.ultraPanel5.ClientArea.Controls.Add(this.ultraLabel1);
            this.ultraPanel5.Dock = System.Windows.Forms.DockStyle.Top;
            this.ultraPanel5.Location = new System.Drawing.Point(14, 12);
            this.ultraPanel5.Name = "ultraPanel5";
            this.ultraPanel5.Size = new System.Drawing.Size(1040, 55);
            this.ultraPanel5.TabIndex = 416;
            // 
            // ultraLabel1
            // 
            appearance12.FontData.BoldAsString = "True";
            appearance12.FontData.SizeInPoints = 15F;
            appearance12.TextHAlignAsString = "Left";
            appearance12.TextVAlignAsString = "Middle";
            this.ultraLabel1.Appearance = appearance12;
            this.ultraLabel1.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabel1.Location = new System.Drawing.Point(3, 6);
            this.ultraLabel1.Name = "ultraLabel1";
            this.ultraLabel1.Size = new System.Drawing.Size(202, 43);
            this.ultraLabel1.TabIndex = 114;
            this.ultraLabel1.Text = "Downloader";
            // 
            // frmDLIClock
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.ClientSize = new System.Drawing.Size(1054, 509);
            this.ControlBox = false;
            this.Controls.Add(this.ultraPanel5);
            this.Controls.Add(this.ultraPanel4);
            this.Controls.Add(this.ultraPanel2);
            this.Controls.Add(this.ultraPanel3);
            this.Controls.Add(this.ultraPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmDLIClock";
            this.Text = "Downloader [I-Clock]";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmCollectData_FormClosing);
            this.Load += new System.EventHandler(this.frmCollectData_Load);
            this.ultraPanel4.ResumeLayout(false);
            this.ultraPanel2.ResumeLayout(false);
            this.ultraPanel3.ResumeLayout(false);
            this.ultraPanel1.ClientArea.ResumeLayout(false);
            this.ultraPanel1.ResumeLayout(false);
            this.ultraPanel5.ClientArea.ResumeLayout(false);
            this.ultraPanel5.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.Misc.UltraButton btnClose;
        private Infragistics.Win.Misc.UltraPanel ultraPanel4;
        private Infragistics.Win.Misc.UltraPanel ultraPanel2;
        private Infragistics.Win.Misc.UltraButton btnSave;
        private Infragistics.Win.Misc.UltraPanel ultraPanel3;
        private Infragistics.Win.Misc.UltraPanel ultraPanel1;
        private Infragistics.Win.Misc.UltraButton btnCancel;
        private System.Windows.Forms.ToolTip toolTip1;
        private Infragistics.Win.Misc.UltraPanel ultraPanel5;
        private Infragistics.Win.Misc.UltraLabel ultraLabel1;
    }
}