namespace GTRHRIS.HK.FormEntry
{
    partial class frmGrade
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
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance14 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance15 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance16 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance17 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance18 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance19 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance20 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance21 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance22 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance23 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance24 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance25 = new Infragistics.Win.Appearance();
            this.pnlLeft = new Infragistics.Win.Misc.UltraPanel();
            this.pnlTop = new Infragistics.Win.Misc.UltraPanel();
            this.lblCaption = new Infragistics.Win.Misc.UltraLabel();
            this.btnAddNew = new Infragistics.Win.Misc.UltraButton();
            this.btnSave = new Infragistics.Win.Misc.UltraButton();
            this.btnClose = new Infragistics.Win.Misc.UltraButton();
            this.btnDelete = new Infragistics.Win.Misc.UltraButton();
            this.pnlBottom = new Infragistics.Win.Misc.UltraPanel();
            this.btnCancel = new Infragistics.Win.Misc.UltraButton();
            this.pnlBottomTop = new Infragistics.Win.Misc.UltraPanel();
            this.gridList = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.pnlLeft.SuspendLayout();
            this.pnlTop.ClientArea.SuspendLayout();
            this.pnlTop.SuspendLayout();
            this.pnlBottom.ClientArea.SuspendLayout();
            this.pnlBottom.SuspendLayout();
            this.pnlBottomTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridList)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlLeft
            // 
            this.pnlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlLeft.Location = new System.Drawing.Point(0, 39);
            this.pnlLeft.Name = "pnlLeft";
            this.pnlLeft.Size = new System.Drawing.Size(10, 363);
            this.pnlLeft.TabIndex = 86;
            // 
            // pnlTop
            // 
            // 
            // pnlTop.ClientArea
            // 
            this.pnlTop.ClientArea.Controls.Add(this.lblCaption);
            this.pnlTop.ClientArea.Controls.Add(this.btnAddNew);
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTop.Location = new System.Drawing.Point(0, 0);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Size = new System.Drawing.Size(957, 39);
            this.pnlTop.TabIndex = 87;
            //this.pnlTop.PaintClient += new System.Windows.Forms.PaintEventHandler(this.pnlTop_PaintClient);
            // 
            // lblCaption
            // 
            appearance2.FontData.BoldAsString = "True";
            appearance2.TextVAlignAsString = "Middle";
            this.lblCaption.Appearance = appearance2;
            this.lblCaption.Font = new System.Drawing.Font("Verdana", 12F);
            this.lblCaption.Location = new System.Drawing.Point(12, 4);
            this.lblCaption.Name = "lblCaption";
            this.lblCaption.Size = new System.Drawing.Size(615, 32);
            this.lblCaption.TabIndex = 5;
            this.lblCaption.Text = "Caption";
            // 
            // btnAddNew
            // 
            this.btnAddNew.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAddNew.Location = new System.Drawing.Point(629, 3);
            this.btnAddNew.Name = "btnAddNew";
            this.btnAddNew.Size = new System.Drawing.Size(131, 34);
            this.btnAddNew.TabIndex = 4;
            this.btnAddNew.Tag = "0";
            this.btnAddNew.Text = "Add New";
            this.btnAddNew.Click += new System.EventHandler(this.btnAddNew_Click);
            // 
            // btnSave
            // 
            this.btnSave.Enabled = false;
            this.btnSave.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.Location = new System.Drawing.Point(362, 10);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(131, 40);
            this.btnSave.TabIndex = 0;
            this.btnSave.Tag = "0";
            this.btnSave.Text = "&Save";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
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
            // btnDelete
            // 
            this.btnDelete.Enabled = false;
            this.btnDelete.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDelete.Location = new System.Drawing.Point(496, 10);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(131, 40);
            this.btnDelete.TabIndex = 1;
            this.btnDelete.Tag = "2";
            this.btnDelete.Text = "&Delete";
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // pnlBottom
            // 
            appearance1.BackColor = System.Drawing.Color.SlateGray;
            this.pnlBottom.Appearance = appearance1;
            // 
            // pnlBottom.ClientArea
            // 
            this.pnlBottom.ClientArea.Controls.Add(this.btnSave);
            this.pnlBottom.ClientArea.Controls.Add(this.btnClose);
            this.pnlBottom.ClientArea.Controls.Add(this.btnDelete);
            this.pnlBottom.ClientArea.Controls.Add(this.btnCancel);
            this.pnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBottom.Location = new System.Drawing.Point(0, 412);
            this.pnlBottom.Name = "pnlBottom";
            this.pnlBottom.Size = new System.Drawing.Size(957, 60);
            this.pnlBottom.TabIndex = 84;
            // 
            // btnCancel
            // 
            this.btnCancel.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.Location = new System.Drawing.Point(629, 10);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(131, 40);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // pnlBottomTop
            // 
            this.pnlBottomTop.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBottomTop.Location = new System.Drawing.Point(0, 402);
            this.pnlBottomTop.Name = "pnlBottomTop";
            this.pnlBottomTop.Size = new System.Drawing.Size(957, 10);
            this.pnlBottomTop.TabIndex = 93;
            // 
            // gridList
            // 
            appearance14.BackColor = System.Drawing.SystemColors.Window;
            appearance14.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.gridList.DisplayLayout.Appearance = appearance14;
            this.gridList.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.gridList.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance15.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance15.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance15.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance15.BorderColor = System.Drawing.SystemColors.Window;
            this.gridList.DisplayLayout.GroupByBox.Appearance = appearance15;
            appearance16.ForeColor = System.Drawing.SystemColors.GrayText;
            this.gridList.DisplayLayout.GroupByBox.BandLabelAppearance = appearance16;
            this.gridList.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance17.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance17.BackColor2 = System.Drawing.SystemColors.Control;
            appearance17.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance17.ForeColor = System.Drawing.SystemColors.GrayText;
            this.gridList.DisplayLayout.GroupByBox.PromptAppearance = appearance17;
            this.gridList.DisplayLayout.MaxColScrollRegions = 1;
            this.gridList.DisplayLayout.MaxRowScrollRegions = 1;
            appearance18.BackColor = System.Drawing.SystemColors.Window;
            appearance18.ForeColor = System.Drawing.SystemColors.ControlText;
            this.gridList.DisplayLayout.Override.ActiveCellAppearance = appearance18;
            appearance19.BackColor = System.Drawing.SystemColors.Highlight;
            appearance19.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.gridList.DisplayLayout.Override.ActiveRowAppearance = appearance19;
            this.gridList.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.gridList.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance20.BackColor = System.Drawing.SystemColors.Window;
            this.gridList.DisplayLayout.Override.CardAreaAppearance = appearance20;
            appearance21.BorderColor = System.Drawing.Color.Silver;
            appearance21.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.gridList.DisplayLayout.Override.CellAppearance = appearance21;
            this.gridList.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.gridList.DisplayLayout.Override.CellPadding = 0;
            appearance22.BackColor = System.Drawing.SystemColors.Control;
            appearance22.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance22.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance22.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance22.BorderColor = System.Drawing.SystemColors.Window;
            this.gridList.DisplayLayout.Override.GroupByRowAppearance = appearance22;
            appearance23.TextHAlignAsString = "Left";
            this.gridList.DisplayLayout.Override.HeaderAppearance = appearance23;
            this.gridList.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.gridList.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance24.BackColor = System.Drawing.SystemColors.Window;
            appearance24.BorderColor = System.Drawing.Color.Silver;
            this.gridList.DisplayLayout.Override.RowAppearance = appearance24;
            this.gridList.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance25.BackColor = System.Drawing.SystemColors.ControlLight;
            this.gridList.DisplayLayout.Override.TemplateAddRowAppearance = appearance25;
            this.gridList.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.gridList.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.gridList.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.gridList.Dock = System.Windows.Forms.DockStyle.Left;
            this.gridList.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gridList.Location = new System.Drawing.Point(10, 39);
            this.gridList.Name = "gridList";
            this.gridList.Size = new System.Drawing.Size(750, 363);
            this.gridList.TabIndex = 94;
            this.gridList.TabStop = false;
            this.gridList.Text = "Country";
            this.gridList.AfterCellActivate += new System.EventHandler(this.gridList_AfterCellActivate);
            this.gridList.AfterCellUpdate += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.gridList_AfterCellUpdate);
            this.gridList.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.gridList_InitializeLayout);
            // 
            // frmGrade
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(957, 472);
            this.ControlBox = false;
            this.Controls.Add(this.gridList);
            this.Controls.Add(this.pnlLeft);
            this.Controls.Add(this.pnlBottomTop);
            this.Controls.Add(this.pnlTop);
            this.Controls.Add(this.pnlBottom);
            this.Name = "frmGrade";
            this.Tag = "Grade";
            this.Text = "Grade Entry .....";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmGrade_FormClosing);
            this.Load += new System.EventHandler(this.frmGrade_Load);
            this.pnlLeft.ResumeLayout(false);
            this.pnlTop.ClientArea.ResumeLayout(false);
            this.pnlTop.ResumeLayout(false);
            this.pnlBottom.ClientArea.ResumeLayout(false);
            this.pnlBottom.ResumeLayout(false);
            this.pnlBottomTop.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridList)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.Misc.UltraPanel pnlLeft;
        private Infragistics.Win.Misc.UltraPanel pnlTop;
        private Infragistics.Win.Misc.UltraButton btnSave;
        private Infragistics.Win.Misc.UltraButton btnClose;
        private Infragistics.Win.Misc.UltraButton btnDelete;
        private Infragistics.Win.Misc.UltraPanel pnlBottom;
        private Infragistics.Win.Misc.UltraButton btnCancel;
        private Infragistics.Win.Misc.UltraPanel pnlBottomTop;
        private Infragistics.Win.UltraWinGrid.UltraGrid gridList;
        private Infragistics.Win.Misc.UltraButton btnAddNew;
        private Infragistics.Win.Misc.UltraLabel lblCaption;
    }
}