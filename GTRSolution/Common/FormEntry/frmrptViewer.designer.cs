namespace GTRHRIS.Common.FormEntry
{
    partial class frmrptViewer
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
            this.rptViewer = new Microsoft.Reporting.WinForms.ReportViewer();
            this.SuspendLayout();
            // 
            // rptViewer
            // 
            this.rptViewer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rptViewer.Location = new System.Drawing.Point(0, 0);
            this.rptViewer.Name = "rptViewer";
            this.rptViewer.Size = new System.Drawing.Size(725, 453);
            this.rptViewer.TabIndex = 0;
            this.rptViewer.ZoomMode = Microsoft.Reporting.WinForms.ZoomMode.PageWidth;
            this.rptViewer.Load += new System.EventHandler(this.rptViewer_Load);
            // 
            // frmrptViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(725, 453);
            this.Controls.Add(this.rptViewer);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "frmrptViewer";
            this.Text = "Report Viewer ...";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmrptViewer_FormClosing);
            this.Load += new System.EventHandler(this.frmrptViewer_Load);
            this.Resize += new System.EventHandler(this.frmrptViewer_Resize);
            this.ResumeLayout(false);

        }

        #endregion

        private Microsoft.Reporting.WinForms.ReportViewer rptViewer;
    }
}