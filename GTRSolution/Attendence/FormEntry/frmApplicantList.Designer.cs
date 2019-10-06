namespace GTRHRIS.Attendence.FormEntry
{
    partial class frmApplicantList
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
            Infragistics.Win.Appearance appearance13 = new Infragistics.Win.Appearance();
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
            Infragistics.Win.Appearance appearance26 = new Infragistics.Win.Appearance();
            this.pnlLeft = new Infragistics.Win.Misc.UltraPanel();
            this.pnlTop = new Infragistics.Win.Misc.UltraPanel();
            this.pnlRight = new Infragistics.Win.Misc.UltraPanel();
            this.pnlBottomTop = new Infragistics.Win.Misc.UltraPanel();
            this.pnlBottom = new Infragistics.Win.Misc.UltraPanel();
            this.btnPrevEng = new Infragistics.Win.Misc.UltraButton();
            this.btnPreview = new Infragistics.Win.Misc.UltraButton();
            this.btnUpdate = new Infragistics.Win.Misc.UltraButton();
            this.ultraButton1 = new Infragistics.Win.Misc.UltraButton();
            this.gridList = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.pnlLeft.SuspendLayout();
            this.pnlTop.SuspendLayout();
            this.pnlRight.SuspendLayout();
            this.pnlBottomTop.SuspendLayout();
            this.pnlBottom.ClientArea.SuspendLayout();
            this.pnlBottom.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridList)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlLeft
            // 
            this.pnlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlLeft.Location = new System.Drawing.Point(0, 10);
            this.pnlLeft.Name = "pnlLeft";
            this.pnlLeft.Size = new System.Drawing.Size(10, 382);
            this.pnlLeft.TabIndex = 0;
            // 
            // pnlTop
            // 
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTop.Location = new System.Drawing.Point(0, 0);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Size = new System.Drawing.Size(1275, 10);
            this.pnlTop.TabIndex = 0;
            // 
            // pnlRight
            // 
            this.pnlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlRight.Location = new System.Drawing.Point(1263, 10);
            this.pnlRight.Name = "pnlRight";
            this.pnlRight.Size = new System.Drawing.Size(12, 382);
            this.pnlRight.TabIndex = 0;
            // 
            // pnlBottomTop
            // 
            this.pnlBottomTop.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBottomTop.Location = new System.Drawing.Point(10, 382);
            this.pnlBottomTop.Name = "pnlBottomTop";
            this.pnlBottomTop.Size = new System.Drawing.Size(1253, 10);
            this.pnlBottomTop.TabIndex = 0;
            // 
            // pnlBottom
            // 
            appearance13.BackColor = System.Drawing.Color.SlateGray;
            this.pnlBottom.Appearance = appearance13;
            // 
            // pnlBottom.ClientArea
            // 
            this.pnlBottom.ClientArea.Controls.Add(this.btnPrevEng);
            this.pnlBottom.ClientArea.Controls.Add(this.btnPreview);
            this.pnlBottom.ClientArea.Controls.Add(this.btnUpdate);
            this.pnlBottom.ClientArea.Controls.Add(this.ultraButton1);
            this.pnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBottom.Location = new System.Drawing.Point(0, 392);
            this.pnlBottom.Name = "pnlBottom";
            this.pnlBottom.Size = new System.Drawing.Size(1275, 60);
            this.pnlBottom.TabIndex = 0;
            // 
            // btnPrevEng
            // 
            this.btnPrevEng.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPrevEng.Location = new System.Drawing.Point(580, 6);
            this.btnPrevEng.Name = "btnPrevEng";
            this.btnPrevEng.Size = new System.Drawing.Size(131, 40);
            this.btnPrevEng.TabIndex = 3;
            this.btnPrevEng.Text = "&Preview[ENG]";
            this.btnPrevEng.Click += new System.EventHandler(this.btnPrevEng_Click);
            // 
            // btnPreview
            // 
            this.btnPreview.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPreview.Location = new System.Drawing.Point(443, 6);
            this.btnPreview.Name = "btnPreview";
            this.btnPreview.Size = new System.Drawing.Size(131, 40);
            this.btnPreview.TabIndex = 2;
            this.btnPreview.Text = "&Preview[BNG]";
            this.btnPreview.Click += new System.EventHandler(this.btnPreview_Click);
            // 
            // btnUpdate
            // 
            this.btnUpdate.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnUpdate.Location = new System.Drawing.Point(851, 6);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(131, 40);
            this.btnUpdate.TabIndex = 1;
            this.btnUpdate.Text = "&Update";
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // ultraButton1
            // 
            this.ultraButton1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraButton1.Location = new System.Drawing.Point(12, 6);
            this.ultraButton1.Name = "ultraButton1";
            this.ultraButton1.Size = new System.Drawing.Size(131, 40);
            this.ultraButton1.TabIndex = 0;
            this.ultraButton1.Text = "&Close";
            this.ultraButton1.Click += new System.EventHandler(this.ultraButton1_Click);
            // 
            // gridList
            // 
            appearance15.BackColor = System.Drawing.SystemColors.Window;
            appearance15.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.gridList.DisplayLayout.Appearance = appearance15;
            this.gridList.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.gridList.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance16.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance16.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance16.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance16.BorderColor = System.Drawing.SystemColors.Window;
            this.gridList.DisplayLayout.GroupByBox.Appearance = appearance16;
            appearance17.ForeColor = System.Drawing.SystemColors.GrayText;
            this.gridList.DisplayLayout.GroupByBox.BandLabelAppearance = appearance17;
            this.gridList.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance18.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance18.BackColor2 = System.Drawing.SystemColors.Control;
            appearance18.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance18.ForeColor = System.Drawing.SystemColors.GrayText;
            this.gridList.DisplayLayout.GroupByBox.PromptAppearance = appearance18;
            this.gridList.DisplayLayout.MaxColScrollRegions = 1;
            this.gridList.DisplayLayout.MaxRowScrollRegions = 1;
            appearance19.BackColor = System.Drawing.SystemColors.Window;
            appearance19.ForeColor = System.Drawing.SystemColors.ControlText;
            this.gridList.DisplayLayout.Override.ActiveCellAppearance = appearance19;
            appearance20.BackColor = System.Drawing.SystemColors.Highlight;
            appearance20.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.gridList.DisplayLayout.Override.ActiveRowAppearance = appearance20;
            this.gridList.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.gridList.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance21.BackColor = System.Drawing.SystemColors.Window;
            this.gridList.DisplayLayout.Override.CardAreaAppearance = appearance21;
            appearance22.BorderColor = System.Drawing.Color.Silver;
            appearance22.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.gridList.DisplayLayout.Override.CellAppearance = appearance22;
            this.gridList.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.gridList.DisplayLayout.Override.CellPadding = 0;
            appearance23.BackColor = System.Drawing.SystemColors.Control;
            appearance23.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance23.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance23.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance23.BorderColor = System.Drawing.SystemColors.Window;
            this.gridList.DisplayLayout.Override.GroupByRowAppearance = appearance23;
            appearance24.TextHAlignAsString = "Left";
            this.gridList.DisplayLayout.Override.HeaderAppearance = appearance24;
            this.gridList.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.gridList.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance25.BackColor = System.Drawing.SystemColors.Window;
            appearance25.BorderColor = System.Drawing.Color.Silver;
            this.gridList.DisplayLayout.Override.RowAppearance = appearance25;
            this.gridList.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance26.BackColor = System.Drawing.SystemColors.ControlLight;
            this.gridList.DisplayLayout.Override.TemplateAddRowAppearance = appearance26;
            this.gridList.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.gridList.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.gridList.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.gridList.Dock = System.Windows.Forms.DockStyle.Left;
            this.gridList.Location = new System.Drawing.Point(10, 10);
            this.gridList.Name = "gridList";
            this.gridList.Size = new System.Drawing.Size(1247, 372);
            this.gridList.TabIndex = 1;
            this.gridList.Text = "ultraGrid1";
            this.gridList.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.gridList_InitializeLayout);
            this.gridList.CellChange += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.gridList_CellChange);
            this.gridList.ClickCell += new Infragistics.Win.UltraWinGrid.ClickCellEventHandler(this.gridList_ClickCell);
            // 
            // frmApplicantList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1275, 452);
            this.ControlBox = false;
            this.Controls.Add(this.gridList);
            this.Controls.Add(this.pnlBottomTop);
            this.Controls.Add(this.pnlLeft);
            this.Controls.Add(this.pnlRight);
            this.Controls.Add(this.pnlBottom);
            this.Controls.Add(this.pnlTop);
            this.Name = "frmApplicantList";
            this.Text = "frmApplicantList";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmApplicantList_FormClosing);
            this.Load += new System.EventHandler(this.frmApplicantList_Load);
            this.pnlLeft.ResumeLayout(false);
            this.pnlTop.ResumeLayout(false);
            this.pnlRight.ResumeLayout(false);
            this.pnlBottomTop.ResumeLayout(false);
            this.pnlBottom.ClientArea.ResumeLayout(false);
            this.pnlBottom.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridList)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.Misc.UltraPanel pnlLeft;
        private Infragistics.Win.Misc.UltraPanel pnlTop;
        private Infragistics.Win.Misc.UltraPanel pnlRight;
        private Infragistics.Win.Misc.UltraPanel pnlBottomTop;
        private Infragistics.Win.Misc.UltraPanel pnlBottom;
        private Infragistics.Win.UltraWinGrid.UltraGrid gridList;
        private Infragistics.Win.Misc.UltraButton btnUpdate;
        private Infragistics.Win.Misc.UltraButton ultraButton1;
        private Infragistics.Win.Misc.UltraButton btnPreview;
        private Infragistics.Win.Misc.UltraButton btnPrevEng;
    }
}