namespace GTRHRIS.Admin.FormEntry
{
    partial class frmDownloader
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
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            this.pnlLeft = new Infragistics.Win.Misc.UltraPanel();
            this.pnlBottomTop = new Infragistics.Win.Misc.UltraPanel();
            this.btnClose = new Infragistics.Win.Misc.UltraButton();
            this.pnlTop = new Infragistics.Win.Misc.UltraPanel();
            this.pnlBottom = new Infragistics.Win.Misc.UltraPanel();
            this.label2 = new System.Windows.Forms.Label();
            this.ultraPanel1 = new Infragistics.Win.Misc.UltraPanel();
            this.ultraGroupBox2 = new Infragistics.Win.Misc.UltraGroupBox();
            this.chkClear = new System.Windows.Forms.CheckBox();
            this.btnAdd = new System.Windows.Forms.Button();
            this.gridDetails = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.ultraPanel7 = new Infragistics.Win.Misc.UltraPanel();
            this.ultraGroupBox1 = new Infragistics.Win.Misc.UltraGroupBox();
            this.gridInfo = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.btnLog = new System.Windows.Forms.Button();
            this.btnDownload = new System.Windows.Forms.Button();
            this.ultraPanel3 = new Infragistics.Win.Misc.UltraPanel();
            this.ultraGroupBox4 = new Infragistics.Win.Misc.UltraGroupBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnSetTime = new System.Windows.Forms.Button();
            this.btnGetTime = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.pnlLeft.SuspendLayout();
            this.pnlBottomTop.SuspendLayout();
            this.pnlTop.SuspendLayout();
            this.pnlBottom.ClientArea.SuspendLayout();
            this.pnlBottom.SuspendLayout();
            this.ultraPanel1.ClientArea.SuspendLayout();
            this.ultraPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox2)).BeginInit();
            this.ultraGroupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridDetails)).BeginInit();
            this.ultraPanel7.ClientArea.SuspendLayout();
            this.ultraPanel7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox1)).BeginInit();
            this.ultraGroupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridInfo)).BeginInit();
            this.ultraPanel3.ClientArea.SuspendLayout();
            this.ultraPanel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox4)).BeginInit();
            this.ultraGroupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlLeft
            // 
            this.pnlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlLeft.Location = new System.Drawing.Point(0, 12);
            this.pnlLeft.Name = "pnlLeft";
            this.pnlLeft.Size = new System.Drawing.Size(10, 493);
            this.pnlLeft.TabIndex = 112;
            // 
            // pnlBottomTop
            // 
            this.pnlBottomTop.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBottomTop.Location = new System.Drawing.Point(0, 505);
            this.pnlBottomTop.Name = "pnlBottomTop";
            this.pnlBottomTop.Size = new System.Drawing.Size(1309, 10);
            this.pnlBottomTop.TabIndex = 111;
            // 
            // btnClose
            // 
            this.btnClose.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.Location = new System.Drawing.Point(37, 10);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(131, 40);
            this.btnClose.TabIndex = 3;
            this.btnClose.Text = "C&lose";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // pnlTop
            // 
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTop.Location = new System.Drawing.Point(0, 0);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Size = new System.Drawing.Size(1309, 12);
            this.pnlTop.TabIndex = 113;
            // 
            // pnlBottom
            // 
            appearance1.BackColor = System.Drawing.Color.SlateGray;
            this.pnlBottom.Appearance = appearance1;
            // 
            // pnlBottom.ClientArea
            // 
            this.pnlBottom.ClientArea.Controls.Add(this.btnClose);
            this.pnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBottom.Location = new System.Drawing.Point(0, 515);
            this.pnlBottom.Name = "pnlBottom";
            this.pnlBottom.Size = new System.Drawing.Size(1309, 60);
            this.pnlBottom.TabIndex = 17;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.White;
            this.label2.Font = new System.Drawing.Font("Verdana", 14F, System.Drawing.FontStyle.Bold);
            this.label2.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label2.Location = new System.Drawing.Point(3, 3);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(399, 41);
            this.label2.TabIndex = 114;
            this.label2.Text = "Data Downloader......";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ultraPanel1
            // 
            // 
            // ultraPanel1.ClientArea
            // 
            this.ultraPanel1.ClientArea.Controls.Add(this.ultraGroupBox2);
            this.ultraPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.ultraPanel1.Location = new System.Drawing.Point(10, 12);
            this.ultraPanel1.Name = "ultraPanel1";
            this.ultraPanel1.Size = new System.Drawing.Size(1299, 47);
            this.ultraPanel1.TabIndex = 115;
            // 
            // ultraGroupBox2
            // 
            appearance2.FontData.BoldAsString = "True";
            appearance2.FontData.Name = "Verdana";
            this.ultraGroupBox2.Appearance = appearance2;
            this.ultraGroupBox2.CaptionAlignment = Infragistics.Win.Misc.GroupBoxCaptionAlignment.Near;
            this.ultraGroupBox2.Controls.Add(this.label2);
            this.ultraGroupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraGroupBox2.Location = new System.Drawing.Point(0, 0);
            this.ultraGroupBox2.Name = "ultraGroupBox2";
            this.ultraGroupBox2.Size = new System.Drawing.Size(1299, 47);
            this.ultraGroupBox2.TabIndex = 443;
            this.ultraGroupBox2.Click += new System.EventHandler(this.ultraGroupBox2_Click);
            // 
            // chkClear
            // 
            this.chkClear.AutoSize = true;
            this.chkClear.Location = new System.Drawing.Point(470, 158);
            this.chkClear.Name = "chkClear";
            this.chkClear.Size = new System.Drawing.Size(97, 17);
            this.chkClear.TabIndex = 431;
            this.chkClear.Text = "Data Clear Yes";
            this.chkClear.UseVisualStyleBackColor = true;
            this.chkClear.CheckedChanged += new System.EventHandler(this.chkClear_CheckedChanged);
            // 
            // btnAdd
            // 
            this.btnAdd.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAdd.Location = new System.Drawing.Point(16, 65);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(66, 23);
            this.btnAdd.TabIndex = 430;
            this.btnAdd.Text = "Add";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // gridDetails
            // 
            this.gridDetails.DisplayLayout.MaxColScrollRegions = 1;
            this.gridDetails.DisplayLayout.MaxRowScrollRegions = 1;
            this.gridDetails.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.gridDetails.DisplayLayout.Override.CellPadding = 0;
            this.gridDetails.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.gridDetails.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.gridDetails.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.gridDetails.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.gridDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridDetails.Font = new System.Drawing.Font("Verdana", 7.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gridDetails.Location = new System.Drawing.Point(3, 3);
            this.gridDetails.Name = "gridDetails";
            this.gridDetails.Size = new System.Drawing.Size(400, 388);
            this.gridDetails.TabIndex = 429;
            this.gridDetails.TabStop = false;
            this.gridDetails.Text = "Machine Info";
            this.gridDetails.AfterCellUpdate += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.gridDetails_AfterCellUpdate);
            this.gridDetails.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.gridDetails_InitializeLayout);
            // 
            // ultraPanel7
            // 
            // 
            // ultraPanel7.ClientArea
            // 
            this.ultraPanel7.ClientArea.Controls.Add(this.ultraGroupBox1);
            this.ultraPanel7.Location = new System.Drawing.Point(618, 94);
            this.ultraPanel7.Name = "ultraPanel7";
            this.ultraPanel7.Size = new System.Drawing.Size(650, 400);
            this.ultraPanel7.TabIndex = 434;
            // 
            // ultraGroupBox1
            // 
            appearance3.FontData.BoldAsString = "True";
            appearance3.FontData.Name = "Verdana";
            this.ultraGroupBox1.Appearance = appearance3;
            this.ultraGroupBox1.CaptionAlignment = Infragistics.Win.Misc.GroupBoxCaptionAlignment.Near;
            this.ultraGroupBox1.Controls.Add(this.gridInfo);
            this.ultraGroupBox1.Location = new System.Drawing.Point(3, 3);
            this.ultraGroupBox1.Name = "ultraGroupBox1";
            this.ultraGroupBox1.Size = new System.Drawing.Size(644, 394);
            this.ultraGroupBox1.TabIndex = 444;
            // 
            // gridInfo
            // 
            this.gridInfo.DisplayLayout.MaxColScrollRegions = 1;
            this.gridInfo.DisplayLayout.MaxRowScrollRegions = 1;
            this.gridInfo.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.gridInfo.DisplayLayout.Override.CellPadding = 0;
            this.gridInfo.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.gridInfo.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.gridInfo.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.gridInfo.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.gridInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridInfo.Font = new System.Drawing.Font("Verdana", 7.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gridInfo.Location = new System.Drawing.Point(3, 3);
            this.gridInfo.Name = "gridInfo";
            this.gridInfo.Size = new System.Drawing.Size(638, 388);
            this.gridInfo.TabIndex = 424;
            this.gridInfo.TabStop = false;
            this.gridInfo.Text = "Information";
            this.gridInfo.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.gridInfo_InitializeLayout);
            // 
            // btnLog
            // 
            this.btnLog.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold);
            this.btnLog.Location = new System.Drawing.Point(445, 257);
            this.btnLog.Name = "btnLog";
            this.btnLog.Size = new System.Drawing.Size(157, 39);
            this.btnLog.TabIndex = 439;
            this.btnLog.Text = "Data Log";
            this.btnLog.UseVisualStyleBackColor = true;
            this.btnLog.Click += new System.EventHandler(this.btnLog_Click);
            // 
            // btnDownload
            // 
            this.btnDownload.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold);
            this.btnDownload.Location = new System.Drawing.Point(445, 203);
            this.btnDownload.Name = "btnDownload";
            this.btnDownload.Size = new System.Drawing.Size(157, 39);
            this.btnDownload.TabIndex = 438;
            this.btnDownload.Text = "Download";
            this.btnDownload.UseVisualStyleBackColor = true;
            this.btnDownload.Click += new System.EventHandler(this.btnDownload_Click);
            // 
            // ultraPanel3
            // 
            // 
            // ultraPanel3.ClientArea
            // 
            this.ultraPanel3.ClientArea.Controls.Add(this.ultraGroupBox4);
            this.ultraPanel3.Location = new System.Drawing.Point(10, 94);
            this.ultraPanel3.Name = "ultraPanel3";
            this.ultraPanel3.Size = new System.Drawing.Size(411, 400);
            this.ultraPanel3.TabIndex = 435;
            // 
            // ultraGroupBox4
            // 
            appearance4.FontData.BoldAsString = "True";
            appearance4.FontData.Name = "Verdana";
            this.ultraGroupBox4.Appearance = appearance4;
            this.ultraGroupBox4.CaptionAlignment = Infragistics.Win.Misc.GroupBoxCaptionAlignment.Near;
            this.ultraGroupBox4.Controls.Add(this.gridDetails);
            this.ultraGroupBox4.Location = new System.Drawing.Point(2, 3);
            this.ultraGroupBox4.Name = "ultraGroupBox4";
            this.ultraGroupBox4.Size = new System.Drawing.Size(406, 394);
            this.ultraGroupBox4.TabIndex = 443;
            // 
            // btnSave
            // 
            this.btnSave.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold);
            this.btnSave.Location = new System.Drawing.Point(96, 65);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(72, 23);
            this.btnSave.TabIndex = 440;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnSetTime
            // 
            this.btnSetTime.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSetTime.Location = new System.Drawing.Point(735, 65);
            this.btnSetTime.Name = "btnSetTime";
            this.btnSetTime.Size = new System.Drawing.Size(94, 23);
            this.btnSetTime.TabIndex = 442;
            this.btnSetTime.Text = "&Set Time";
            this.btnSetTime.UseVisualStyleBackColor = true;
            this.btnSetTime.Click += new System.EventHandler(this.btnSetTime_Click);
            // 
            // btnGetTime
            // 
            this.btnGetTime.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGetTime.Location = new System.Drawing.Point(622, 65);
            this.btnGetTime.Name = "btnGetTime";
            this.btnGetTime.Size = new System.Drawing.Size(94, 23);
            this.btnGetTime.TabIndex = 441;
            this.btnGetTime.Text = "Get Time";
            this.btnGetTime.UseVisualStyleBackColor = true;
            this.btnGetTime.Click += new System.EventHandler(this.btnGetTime_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold);
            this.btnDelete.Location = new System.Drawing.Point(190, 65);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(72, 23);
            this.btnDelete.TabIndex = 443;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Visible = false;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // frmDownloader
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1309, 575);
            this.ControlBox = false;
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnSetTime);
            this.Controls.Add(this.btnGetTime);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.ultraPanel3);
            this.Controls.Add(this.btnLog);
            this.Controls.Add(this.btnDownload);
            this.Controls.Add(this.ultraPanel7);
            this.Controls.Add(this.chkClear);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.ultraPanel1);
            this.Controls.Add(this.pnlLeft);
            this.Controls.Add(this.pnlBottomTop);
            this.Controls.Add(this.pnlTop);
            this.Controls.Add(this.pnlBottom);
            this.Name = "frmDownloader";
            this.Text = "Data Downloader...";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmDownloader_FormClosing);
            this.Load += new System.EventHandler(this.frmDownloader_Load);
            this.pnlLeft.ResumeLayout(false);
            this.pnlBottomTop.ResumeLayout(false);
            this.pnlTop.ResumeLayout(false);
            this.pnlBottom.ClientArea.ResumeLayout(false);
            this.pnlBottom.ResumeLayout(false);
            this.ultraPanel1.ClientArea.ResumeLayout(false);
            this.ultraPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox2)).EndInit();
            this.ultraGroupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridDetails)).EndInit();
            this.ultraPanel7.ClientArea.ResumeLayout(false);
            this.ultraPanel7.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox1)).EndInit();
            this.ultraGroupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridInfo)).EndInit();
            this.ultraPanel3.ClientArea.ResumeLayout(false);
            this.ultraPanel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox4)).EndInit();
            this.ultraGroupBox4.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Infragistics.Win.Misc.UltraPanel pnlLeft;
        private Infragistics.Win.Misc.UltraPanel pnlBottomTop;
        private Infragistics.Win.Misc.UltraButton btnClose;
        private Infragistics.Win.Misc.UltraPanel pnlTop;
        private Infragistics.Win.Misc.UltraPanel pnlBottom;
        private System.Windows.Forms.Label label2;
        private Infragistics.Win.Misc.UltraPanel ultraPanel1;
        private System.Windows.Forms.CheckBox chkClear;
        private System.Windows.Forms.Button btnAdd;
        private Infragistics.Win.UltraWinGrid.UltraGrid gridDetails;
        private Infragistics.Win.Misc.UltraPanel ultraPanel7;
        private System.Windows.Forms.Button btnLog;
        private System.Windows.Forms.Button btnDownload;
        private Infragistics.Win.Misc.UltraPanel ultraPanel3;
        private Infragistics.Win.UltraWinGrid.UltraGrid gridInfo;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnSetTime;
        private System.Windows.Forms.Button btnGetTime;
        private Infragistics.Win.Misc.UltraGroupBox ultraGroupBox4;
        private Infragistics.Win.Misc.UltraGroupBox ultraGroupBox1;
        private Infragistics.Win.Misc.UltraGroupBox ultraGroupBox2;
        private System.Windows.Forms.Button btnDelete;
    }
}