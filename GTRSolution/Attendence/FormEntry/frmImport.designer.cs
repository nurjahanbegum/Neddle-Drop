namespace GTRHRIS.Attendence.FormEntry
{
    partial class frmImport
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
            Infragistics.Win.Appearance appearance14 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance10 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance13 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance11 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance12 = new Infragistics.Win.Appearance();
            this.ultraPanel4 = new Infragistics.Win.Misc.UltraPanel();
            this.txtFileName = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.btnBrowse = new Infragistics.Win.Misc.UltraButton();
            this.ultraLabel1 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraPanel3 = new Infragistics.Win.Misc.UltraPanel();
            this.btnDelete = new Infragistics.Win.Misc.UltraButton();
            this.btnShow = new Infragistics.Win.Misc.UltraButton();
            this.txtProcessNo = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.btnProcess = new Infragistics.Win.Misc.UltraButton();
            this.btnSave = new Infragistics.Win.Misc.UltraButton();
            this.btnClose = new Infragistics.Win.Misc.UltraButton();
            this.ultraPanel1 = new Infragistics.Win.Misc.UltraPanel();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.gridList = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.ultraPanel4.ClientArea.SuspendLayout();
            this.ultraPanel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtFileName)).BeginInit();
            this.ultraPanel3.ClientArea.SuspendLayout();
            this.ultraPanel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtProcessNo)).BeginInit();
            this.ultraPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridList)).BeginInit();
            this.SuspendLayout();
            // 
            // ultraPanel4
            // 
            // 
            // ultraPanel4.ClientArea
            // 
            this.ultraPanel4.ClientArea.Controls.Add(this.txtFileName);
            this.ultraPanel4.ClientArea.Controls.Add(this.btnBrowse);
            this.ultraPanel4.ClientArea.Controls.Add(this.ultraLabel1);
            this.ultraPanel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.ultraPanel4.Location = new System.Drawing.Point(0, 0);
            this.ultraPanel4.Name = "ultraPanel4";
            this.ultraPanel4.Size = new System.Drawing.Size(892, 72);
            this.ultraPanel4.TabIndex = 4;
            // 
            // txtFileName
            // 
            this.txtFileName.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtFileName.Location = new System.Drawing.Point(12, 24);
            this.txtFileName.Multiline = true;
            this.txtFileName.Name = "txtFileName";
            this.txtFileName.ReadOnly = true;
            this.txtFileName.Size = new System.Drawing.Size(724, 45);
            this.txtFileName.TabIndex = 9;
            // 
            // btnBrowse
            // 
            this.btnBrowse.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBrowse.Location = new System.Drawing.Point(742, 24);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(79, 45);
            this.btnBrowse.TabIndex = 7;
            this.btnBrowse.Tag = "0";
            this.btnBrowse.Text = "&1 Browse";
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // ultraLabel1
            // 
            this.ultraLabel1.Location = new System.Drawing.Point(12, 9);
            this.ultraLabel1.Name = "ultraLabel1";
            this.ultraLabel1.Size = new System.Drawing.Size(77, 18);
            this.ultraLabel1.TabIndex = 8;
            this.ultraLabel1.Text = "File Name :";
            // 
            // ultraPanel3
            // 
            appearance1.BackColor = System.Drawing.Color.SlateGray;
            this.ultraPanel3.Appearance = appearance1;
            // 
            // ultraPanel3.ClientArea
            // 
            this.ultraPanel3.ClientArea.Controls.Add(this.btnDelete);
            this.ultraPanel3.ClientArea.Controls.Add(this.btnShow);
            this.ultraPanel3.ClientArea.Controls.Add(this.txtProcessNo);
            this.ultraPanel3.ClientArea.Controls.Add(this.btnProcess);
            this.ultraPanel3.ClientArea.Controls.Add(this.btnSave);
            this.ultraPanel3.ClientArea.Controls.Add(this.btnClose);
            this.ultraPanel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.ultraPanel3.Location = new System.Drawing.Point(0, 358);
            this.ultraPanel3.Name = "ultraPanel3";
            this.ultraPanel3.Size = new System.Drawing.Size(892, 60);
            this.ultraPanel3.TabIndex = 5;
            // 
            // btnDelete
            // 
            this.btnDelete.Enabled = false;
            this.btnDelete.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDelete.Location = new System.Drawing.Point(427, 10);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(131, 40);
            this.btnDelete.TabIndex = 12;
            this.btnDelete.Text = "Delete";
            this.btnDelete.Visible = false;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnShow
            // 
            this.btnShow.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnShow.Location = new System.Drawing.Point(337, 10);
            this.btnShow.Name = "btnShow";
            this.btnShow.Size = new System.Drawing.Size(89, 40);
            this.btnShow.TabIndex = 11;
            this.btnShow.Text = "&Show";
            this.btnShow.Click += new System.EventHandler(this.btnShow_Click);
            // 
            // txtProcessNo
            // 
            this.txtProcessNo.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtProcessNo.Location = new System.Drawing.Point(166, 17);
            this.txtProcessNo.Multiline = true;
            this.txtProcessNo.Name = "txtProcessNo";
            this.txtProcessNo.ReadOnly = true;
            this.txtProcessNo.Size = new System.Drawing.Size(171, 25);
            this.txtProcessNo.TabIndex = 10;
            this.txtProcessNo.Visible = false;
            // 
            // btnProcess
            // 
            this.btnProcess.Enabled = false;
            this.btnProcess.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnProcess.Location = new System.Drawing.Point(690, 10);
            this.btnProcess.Name = "btnProcess";
            this.btnProcess.Size = new System.Drawing.Size(131, 40);
            this.btnProcess.TabIndex = 8;
            this.btnProcess.Tag = "0";
            this.btnProcess.Text = "&3 Process";
            this.btnProcess.Click += new System.EventHandler(this.btnProcess_Click);
            // 
            // btnSave
            // 
            this.btnSave.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.Location = new System.Drawing.Point(558, 10);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(131, 40);
            this.btnSave.TabIndex = 4;
            this.btnSave.Tag = "0";
            this.btnSave.Text = "&2 Upload";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnClose
            // 
            this.btnClose.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.Location = new System.Drawing.Point(12, 10);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(131, 40);
            this.btnClose.TabIndex = 7;
            this.btnClose.Text = "&4 Close";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // ultraPanel1
            // 
            this.ultraPanel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.ultraPanel1.Location = new System.Drawing.Point(0, 72);
            this.ultraPanel1.Name = "ultraPanel1";
            this.ultraPanel1.Size = new System.Drawing.Size(9, 286);
            this.ultraPanel1.TabIndex = 6;
            // 
            // timer1
            // 
            this.timer1.Interval = 10;
            // 
            // gridList
            // 
            appearance14.BackColor = System.Drawing.SystemColors.Window;
            appearance14.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.gridList.DisplayLayout.Appearance = appearance14;
            this.gridList.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.gridList.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance3.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance3.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance3.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance3.BorderColor = System.Drawing.SystemColors.Window;
            this.gridList.DisplayLayout.GroupByBox.Appearance = appearance3;
            appearance5.ForeColor = System.Drawing.SystemColors.GrayText;
            this.gridList.DisplayLayout.GroupByBox.BandLabelAppearance = appearance5;
            this.gridList.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance4.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance4.BackColor2 = System.Drawing.SystemColors.Control;
            appearance4.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance4.ForeColor = System.Drawing.SystemColors.GrayText;
            this.gridList.DisplayLayout.GroupByBox.PromptAppearance = appearance4;
            this.gridList.DisplayLayout.MaxColScrollRegions = 1;
            this.gridList.DisplayLayout.MaxRowScrollRegions = 1;
            appearance10.BackColor = System.Drawing.SystemColors.Window;
            appearance10.ForeColor = System.Drawing.SystemColors.ControlText;
            this.gridList.DisplayLayout.Override.ActiveCellAppearance = appearance10;
            appearance6.BackColor = System.Drawing.SystemColors.Highlight;
            appearance6.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.gridList.DisplayLayout.Override.ActiveRowAppearance = appearance6;
            this.gridList.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.gridList.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance13.BackColor = System.Drawing.SystemColors.Window;
            this.gridList.DisplayLayout.Override.CardAreaAppearance = appearance13;
            appearance9.BorderColor = System.Drawing.Color.Silver;
            appearance9.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.gridList.DisplayLayout.Override.CellAppearance = appearance9;
            this.gridList.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.gridList.DisplayLayout.Override.CellPadding = 0;
            appearance7.BackColor = System.Drawing.SystemColors.Control;
            appearance7.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance7.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance7.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance7.BorderColor = System.Drawing.SystemColors.Window;
            this.gridList.DisplayLayout.Override.GroupByRowAppearance = appearance7;
            appearance8.TextHAlignAsString = "Left";
            this.gridList.DisplayLayout.Override.HeaderAppearance = appearance8;
            this.gridList.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.gridList.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance11.BackColor = System.Drawing.SystemColors.Window;
            appearance11.BorderColor = System.Drawing.Color.Silver;
            this.gridList.DisplayLayout.Override.RowAppearance = appearance11;
            this.gridList.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance12.BackColor = System.Drawing.SystemColors.ControlLight;
            this.gridList.DisplayLayout.Override.TemplateAddRowAppearance = appearance12;
            this.gridList.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.gridList.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.gridList.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.gridList.Dock = System.Windows.Forms.DockStyle.Left;
            this.gridList.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gridList.Location = new System.Drawing.Point(9, 72);
            this.gridList.Name = "gridList";
            this.gridList.Size = new System.Drawing.Size(812, 286);
            this.gridList.TabIndex = 388;
            this.gridList.TabStop = false;
            this.gridList.Text = "Country";
            this.gridList.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.gridList_InitializeLayout);
            this.gridList.DoubleClick += new System.EventHandler(this.gridList_DoubleClick);
            // 
            // frmImport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(892, 418);
            this.ControlBox = false;
            this.Controls.Add(this.gridList);
            this.Controls.Add(this.ultraPanel1);
            this.Controls.Add(this.ultraPanel3);
            this.Controls.Add(this.ultraPanel4);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmImport";
            this.Text = "Upload File";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmImport_FormClosing);
            this.Load += new System.EventHandler(this.frmImport_Load);
            this.ultraPanel4.ClientArea.ResumeLayout(false);
            this.ultraPanel4.ClientArea.PerformLayout();
            this.ultraPanel4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtFileName)).EndInit();
            this.ultraPanel3.ClientArea.ResumeLayout(false);
            this.ultraPanel3.ClientArea.PerformLayout();
            this.ultraPanel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtProcessNo)).EndInit();
            this.ultraPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridList)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.Misc.UltraPanel ultraPanel4;
        private Infragistics.Win.Misc.UltraPanel ultraPanel3;
        private Infragistics.Win.Misc.UltraButton btnSave;
        private Infragistics.Win.Misc.UltraButton btnClose;
        private Infragistics.Win.Misc.UltraPanel ultraPanel1;
        private System.Windows.Forms.Timer timer1;
        private Infragistics.Win.Misc.UltraButton btnBrowse;
        private Infragistics.Win.Misc.UltraLabel ultraLabel1;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtFileName;
        private Infragistics.Win.Misc.UltraButton btnProcess;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtProcessNo;
        private Infragistics.Win.UltraWinGrid.UltraGrid gridList;
        private Infragistics.Win.Misc.UltraButton btnDelete;
        private Infragistics.Win.Misc.UltraButton btnShow;
    }
}