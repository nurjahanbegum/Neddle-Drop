namespace GTRHRIS.Attendence.FormEntry
{
    partial class frmEmpCardAssign
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
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance10 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance11 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance12 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance29 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance24 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance30 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance31 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance32 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance33 = new Infragistics.Win.Appearance();
            this.btnClose = new Infragistics.Win.Misc.UltraButton();
            this.gridList = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.ultraPanel4 = new Infragistics.Win.Misc.UltraPanel();
            this.ultraPanel2 = new Infragistics.Win.Misc.UltraPanel();
            this.btnSave = new Infragistics.Win.Misc.UltraButton();
            this.ultraPanel3 = new Infragistics.Win.Misc.UltraPanel();
            this.ultraPanel1 = new Infragistics.Win.Misc.UltraPanel();
            this.btnCancel = new Infragistics.Win.Misc.UltraButton();
            this.ultraLabel3 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel6 = new Infragistics.Win.Misc.UltraLabel();
            this.txtName = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.ultraLabel1 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel4 = new Infragistics.Win.Misc.UltraLabel();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.txtEmpCode = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.txtCard = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.dtJoinDate = new Infragistics.Win.UltraWinEditors.UltraDateTimeEditor();
            this.dtCardAssign = new Infragistics.Win.UltraWinEditors.UltraDateTimeEditor();
            this.txtId = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.ultraLabel5 = new Infragistics.Win.Misc.UltraLabel();
            this.dtFrom = new Infragistics.Win.UltraWinEditors.UltraDateTimeEditor();
            ((System.ComponentModel.ISupportInitialize)(this.gridList)).BeginInit();
            this.ultraPanel4.SuspendLayout();
            this.ultraPanel2.SuspendLayout();
            this.ultraPanel3.SuspendLayout();
            this.ultraPanel1.ClientArea.SuspendLayout();
            this.ultraPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtEmpCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCard)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtJoinDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtCardAssign)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtId)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFrom)).BeginInit();
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
            // gridList
            // 
            appearance1.BackColor = System.Drawing.SystemColors.Window;
            appearance1.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.gridList.DisplayLayout.Appearance = appearance1;
            this.gridList.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.gridList.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance2.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance2.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance2.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance2.BorderColor = System.Drawing.SystemColors.Window;
            this.gridList.DisplayLayout.GroupByBox.Appearance = appearance2;
            appearance3.ForeColor = System.Drawing.SystemColors.GrayText;
            this.gridList.DisplayLayout.GroupByBox.BandLabelAppearance = appearance3;
            this.gridList.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance4.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance4.BackColor2 = System.Drawing.SystemColors.Control;
            appearance4.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance4.ForeColor = System.Drawing.SystemColors.GrayText;
            this.gridList.DisplayLayout.GroupByBox.PromptAppearance = appearance4;
            this.gridList.DisplayLayout.MaxColScrollRegions = 1;
            this.gridList.DisplayLayout.MaxRowScrollRegions = 1;
            appearance5.BackColor = System.Drawing.SystemColors.Window;
            appearance5.ForeColor = System.Drawing.SystemColors.ControlText;
            this.gridList.DisplayLayout.Override.ActiveCellAppearance = appearance5;
            appearance6.BackColor = System.Drawing.SystemColors.Highlight;
            appearance6.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.gridList.DisplayLayout.Override.ActiveRowAppearance = appearance6;
            this.gridList.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.gridList.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance7.BackColor = System.Drawing.SystemColors.Window;
            this.gridList.DisplayLayout.Override.CardAreaAppearance = appearance7;
            appearance8.BorderColor = System.Drawing.Color.Silver;
            appearance8.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.gridList.DisplayLayout.Override.CellAppearance = appearance8;
            this.gridList.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.gridList.DisplayLayout.Override.CellPadding = 0;
            appearance9.BackColor = System.Drawing.SystemColors.Control;
            appearance9.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance9.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance9.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance9.BorderColor = System.Drawing.SystemColors.Window;
            this.gridList.DisplayLayout.Override.GroupByRowAppearance = appearance9;
            appearance10.TextHAlignAsString = "Left";
            this.gridList.DisplayLayout.Override.HeaderAppearance = appearance10;
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
            this.gridList.Location = new System.Drawing.Point(14, 12);
            this.gridList.Name = "gridList";
            this.gridList.Size = new System.Drawing.Size(633, 427);
            this.gridList.TabIndex = 39;
            this.gridList.TabStop = false;
            this.gridList.Text = "Country";
            this.gridList.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.gridList_InitializeLayout);
            this.gridList.DoubleClick += new System.EventHandler(this.gridList_DoubleClick);
            // 
            // ultraPanel4
            // 
            this.ultraPanel4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.ultraPanel4.Location = new System.Drawing.Point(14, 439);
            this.ultraPanel4.Name = "ultraPanel4";
            this.ultraPanel4.Size = new System.Drawing.Size(1196, 10);
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
            this.btnSave.Location = new System.Drawing.Point(731, 7);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(131, 40);
            this.btnSave.TabIndex = 6;
            this.btnSave.Tag = "0";
            this.btnSave.Text = "&Save";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // ultraPanel3
            // 
            this.ultraPanel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.ultraPanel3.Location = new System.Drawing.Point(0, 0);
            this.ultraPanel3.Name = "ultraPanel3";
            this.ultraPanel3.Size = new System.Drawing.Size(1210, 12);
            this.ultraPanel3.TabIndex = 45;
            // 
            // ultraPanel1
            // 
            appearance29.BackColor = System.Drawing.Color.SlateGray;
            this.ultraPanel1.Appearance = appearance29;
            // 
            // ultraPanel1.ClientArea
            // 
            this.ultraPanel1.ClientArea.Controls.Add(this.btnSave);
            this.ultraPanel1.ClientArea.Controls.Add(this.btnClose);
            this.ultraPanel1.ClientArea.Controls.Add(this.btnCancel);
            this.ultraPanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.ultraPanel1.Location = new System.Drawing.Point(0, 449);
            this.ultraPanel1.Name = "ultraPanel1";
            this.ultraPanel1.Size = new System.Drawing.Size(1210, 60);
            this.ultraPanel1.TabIndex = 5;
            this.ultraPanel1.TabStop = false;
            // 
            // btnCancel
            // 
            this.btnCancel.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.Location = new System.Drawing.Point(889, 7);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(131, 40);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // ultraLabel3
            // 
            appearance24.TextHAlignAsString = "Right";
            appearance24.TextVAlignAsString = "Middle";
            this.ultraLabel3.Appearance = appearance24;
            this.ultraLabel3.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabel3.Location = new System.Drawing.Point(656, 168);
            this.ultraLabel3.Name = "ultraLabel3";
            this.ultraLabel3.Size = new System.Drawing.Size(117, 21);
            this.ultraLabel3.TabIndex = 48;
            this.ultraLabel3.Text = "Assign Date :";
            // 
            // ultraLabel6
            // 
            appearance30.TextHAlignAsString = "Right";
            appearance30.TextVAlignAsString = "Middle";
            this.ultraLabel6.Appearance = appearance30;
            this.ultraLabel6.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabel6.Location = new System.Drawing.Point(670, 88);
            this.ultraLabel6.Name = "ultraLabel6";
            this.ultraLabel6.Size = new System.Drawing.Size(103, 21);
            this.ultraLabel6.TabIndex = 47;
            this.ultraLabel6.Text = "Employee  Id : ";
            // 
            // txtName
            // 
            this.txtName.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtName.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtName.Location = new System.Drawing.Point(779, 115);
            this.txtName.MaxLength = 75;
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(257, 22);
            this.txtName.TabIndex = 1;
            this.toolTip1.SetToolTip(this.txtName, "Example : Chittagong");
            this.txtName.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtName_KeyDown);
            this.txtName.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtName_KeyPress);
            // 
            // ultraLabel1
            // 
            appearance31.TextHAlignAsString = "Right";
            appearance31.TextVAlignAsString = "Middle";
            this.ultraLabel1.Appearance = appearance31;
            this.ultraLabel1.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabel1.Location = new System.Drawing.Point(648, 115);
            this.ultraLabel1.Name = "ultraLabel1";
            this.ultraLabel1.Size = new System.Drawing.Size(125, 21);
            this.ultraLabel1.TabIndex = 41;
            this.ultraLabel1.Text = "Employee Name :";
            // 
            // ultraLabel4
            // 
            appearance32.TextHAlignAsString = "Right";
            appearance32.TextVAlignAsString = "Middle";
            this.ultraLabel4.Appearance = appearance32;
            this.ultraLabel4.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabel4.Location = new System.Drawing.Point(670, 143);
            this.ultraLabel4.Name = "ultraLabel4";
            this.ultraLabel4.Size = new System.Drawing.Size(103, 21);
            this.ultraLabel4.TabIndex = 50;
            this.ultraLabel4.Text = "Join Date :";
            // 
            // txtEmpCode
            // 
            this.txtEmpCode.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtEmpCode.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtEmpCode.Location = new System.Drawing.Point(779, 88);
            this.txtEmpCode.MaxLength = 75;
            this.txtEmpCode.Name = "txtEmpCode";
            this.txtEmpCode.Size = new System.Drawing.Size(257, 22);
            this.txtEmpCode.TabIndex = 109;
            this.toolTip1.SetToolTip(this.txtEmpCode, "Example : Chittagong");
            // 
            // txtCard
            // 
            this.txtCard.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtCard.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCard.Location = new System.Drawing.Point(779, 195);
            this.txtCard.MaxLength = 75;
            this.txtCard.Name = "txtCard";
            this.txtCard.Size = new System.Drawing.Size(257, 22);
            this.txtCard.TabIndex = 110;
            this.toolTip1.SetToolTip(this.txtCard, "Example : Chittagong");
            // 
            // dtJoinDate
            // 
            this.dtJoinDate.AutoSize = false;
            this.dtJoinDate.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtJoinDate.FormatString = "dd-MMM-yyyy";
            this.dtJoinDate.Location = new System.Drawing.Point(779, 143);
            this.dtJoinDate.MaskInput = "";
            this.dtJoinDate.Name = "dtJoinDate";
            this.dtJoinDate.Size = new System.Drawing.Size(257, 20);
            this.dtJoinDate.TabIndex = 2;
            this.dtJoinDate.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dtJoinDate_KeyDown);
            // 
            // dtCardAssign
            // 
            this.dtCardAssign.AutoSize = false;
            this.dtCardAssign.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtCardAssign.FormatString = "dd-MMM-yyyy";
            this.dtCardAssign.Location = new System.Drawing.Point(779, 169);
            this.dtCardAssign.MaskInput = "";
            this.dtCardAssign.Name = "dtCardAssign";
            this.dtCardAssign.Size = new System.Drawing.Size(257, 20);
            this.dtCardAssign.TabIndex = 3;
            this.dtCardAssign.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dtReleasedDate_KeyDown);
            // 
            // txtId
            // 
            this.txtId.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.txtId.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtId.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtId.Location = new System.Drawing.Point(918, 18);
            this.txtId.MaxLength = 100;
            this.txtId.Name = "txtId";
            this.txtId.Size = new System.Drawing.Size(118, 20);
            this.txtId.TabIndex = 108;
            // 
            // ultraLabel5
            // 
            appearance33.TextHAlignAsString = "Right";
            appearance33.TextVAlignAsString = "Middle";
            this.ultraLabel5.Appearance = appearance33;
            this.ultraLabel5.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabel5.Location = new System.Drawing.Point(648, 195);
            this.ultraLabel5.Name = "ultraLabel5";
            this.ultraLabel5.Size = new System.Drawing.Size(125, 21);
            this.ultraLabel5.TabIndex = 111;
            this.ultraLabel5.Text = "Card No :";
            // 
            // dtFrom
            // 
            this.dtFrom.AutoSize = false;
            this.dtFrom.Enabled = false;
            this.dtFrom.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtFrom.FormatString = "dd-MMM-yyyy";
            this.dtFrom.Location = new System.Drawing.Point(1071, 18);
            this.dtFrom.MaskInput = "";
            this.dtFrom.Name = "dtFrom";
            this.dtFrom.Size = new System.Drawing.Size(127, 20);
            this.dtFrom.TabIndex = 112;
            this.dtFrom.Visible = false;
            // 
            // frmEmpCardAssign
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.ClientSize = new System.Drawing.Size(1210, 509);
            this.ControlBox = false;
            this.Controls.Add(this.dtFrom);
            this.Controls.Add(this.txtCard);
            this.Controls.Add(this.ultraLabel5);
            this.Controls.Add(this.txtEmpCode);
            this.Controls.Add(this.txtId);
            this.Controls.Add(this.dtCardAssign);
            this.Controls.Add(this.dtJoinDate);
            this.Controls.Add(this.ultraLabel4);
            this.Controls.Add(this.gridList);
            this.Controls.Add(this.ultraPanel4);
            this.Controls.Add(this.ultraPanel2);
            this.Controls.Add(this.ultraPanel3);
            this.Controls.Add(this.ultraPanel1);
            this.Controls.Add(this.ultraLabel3);
            this.Controls.Add(this.ultraLabel6);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.ultraLabel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmEmpCardAssign";
            this.Text = "Card Assign...";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmEmpCardAssign_FormClosing);
            this.Load += new System.EventHandler(this.frmEmpCardAssign_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gridList)).EndInit();
            this.ultraPanel4.ResumeLayout(false);
            this.ultraPanel2.ResumeLayout(false);
            this.ultraPanel3.ResumeLayout(false);
            this.ultraPanel1.ClientArea.ResumeLayout(false);
            this.ultraPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtEmpCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCard)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtJoinDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtCardAssign)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtId)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFrom)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Infragistics.Win.Misc.UltraButton btnClose;
        private Infragistics.Win.UltraWinGrid.UltraGrid gridList;
        private Infragistics.Win.Misc.UltraPanel ultraPanel4;
        private Infragistics.Win.Misc.UltraPanel ultraPanel2;
        private Infragistics.Win.Misc.UltraButton btnSave;
        private Infragistics.Win.Misc.UltraPanel ultraPanel3;
        private Infragistics.Win.Misc.UltraPanel ultraPanel1;
        private Infragistics.Win.Misc.UltraButton btnCancel;
        private Infragistics.Win.Misc.UltraLabel ultraLabel3;
        private Infragistics.Win.Misc.UltraLabel ultraLabel6;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtName;
        private Infragistics.Win.Misc.UltraLabel ultraLabel1;
        private Infragistics.Win.Misc.UltraLabel ultraLabel4;
        private System.Windows.Forms.ToolTip toolTip1;
        private Infragistics.Win.UltraWinEditors.UltraDateTimeEditor dtJoinDate;
        private Infragistics.Win.UltraWinEditors.UltraDateTimeEditor dtCardAssign;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtId;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtEmpCode;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtCard;
        private Infragistics.Win.Misc.UltraLabel ultraLabel5;
        private Infragistics.Win.UltraWinEditors.UltraDateTimeEditor dtFrom;
    }
}