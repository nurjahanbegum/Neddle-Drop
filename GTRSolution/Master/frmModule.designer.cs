namespace GTRHRIS.Master
{
    partial class frmModule
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
            Infragistics.Win.Appearance appearance19 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance17 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance20 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance18 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance21 = new Infragistics.Win.Appearance();
            this.btnClose = new Infragistics.Win.Misc.UltraButton();
            this.btnCancel = new Infragistics.Win.Misc.UltraButton();
            this.btnDelete = new Infragistics.Win.Misc.UltraButton();
            this.btnSave = new Infragistics.Win.Misc.UltraButton();
            this.ultraPanel1 = new Infragistics.Win.Misc.UltraPanel();
            this.gridList = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.ultraLabel1 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel2 = new Infragistics.Win.Misc.UltraLabel();
            this.txtModuleName = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.txtModuleCaption = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.ultraPanel2 = new Infragistics.Win.Misc.UltraPanel();
            this.ultraPanel3 = new Infragistics.Win.Misc.UltraPanel();
            this.ultraPanel4 = new Infragistics.Win.Misc.UltraPanel();
            this.txtModuleId = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.ultraLabel6 = new Infragistics.Win.Misc.UltraLabel();
            this.chkInactive = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            this.ultraPanel1.ClientArea.SuspendLayout();
            this.ultraPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtModuleName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtModuleCaption)).BeginInit();
            this.ultraPanel2.SuspendLayout();
            this.ultraPanel3.SuspendLayout();
            this.ultraPanel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtModuleId)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkInactive)).BeginInit();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            this.btnClose.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.Location = new System.Drawing.Point(12, 3);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(131, 40);
            this.btnClose.TabIndex = 10;
            this.btnClose.Text = "C&lose";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.Location = new System.Drawing.Point(912, 3);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(131, 40);
            this.btnCancel.TabIndex = 9;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Enabled = false;
            this.btnDelete.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDelete.Location = new System.Drawing.Point(781, 3);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(131, 40);
            this.btnDelete.TabIndex = 8;
            this.btnDelete.Tag = "2";
            this.btnDelete.Text = "&Delete";
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnSave
            // 
            this.btnSave.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.Location = new System.Drawing.Point(650, 3);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(131, 40);
            this.btnSave.TabIndex = 7;
            this.btnSave.Tag = "0";
            this.btnSave.Text = "&Save";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // ultraPanel1
            // 
            appearance1.BackColor = System.Drawing.Color.SlateGray;
            this.ultraPanel1.Appearance = appearance1;
            this.ultraPanel1.BorderStyle = Infragistics.Win.UIElementBorderStyle.Raised;
            // 
            // ultraPanel1.ClientArea
            // 
            this.ultraPanel1.ClientArea.Controls.Add(this.btnSave);
            this.ultraPanel1.ClientArea.Controls.Add(this.btnClose);
            this.ultraPanel1.ClientArea.Controls.Add(this.btnDelete);
            this.ultraPanel1.ClientArea.Controls.Add(this.btnCancel);
            this.ultraPanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.ultraPanel1.Location = new System.Drawing.Point(0, 445);
            this.ultraPanel1.Name = "ultraPanel1";
            this.ultraPanel1.Size = new System.Drawing.Size(1094, 60);
            this.ultraPanel1.TabIndex = 6;
            // 
            // gridList
            // 
            appearance2.BackColor = System.Drawing.SystemColors.Window;
            appearance2.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.gridList.DisplayLayout.Appearance = appearance2;
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
            this.gridList.Location = new System.Drawing.Point(14, 10);
            this.gridList.Name = "gridList";
            this.gridList.Size = new System.Drawing.Size(575, 425);
            this.gridList.TabIndex = 0;
            this.gridList.TabStop = false;
            this.gridList.Text = "Country";
            this.gridList.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.gridList_InitializeLayout);
            this.gridList.DoubleClick += new System.EventHandler(this.gridList_DoubleClick);
            // 
            // ultraLabel1
            // 
            appearance19.TextHAlignAsString = "Right";
            appearance19.TextVAlignAsString = "Middle";
            this.ultraLabel1.Appearance = appearance19;
            this.ultraLabel1.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabel1.Location = new System.Drawing.Point(614, 176);
            this.ultraLabel1.Name = "ultraLabel1";
            this.ultraLabel1.Size = new System.Drawing.Size(165, 21);
            this.ultraLabel1.TabIndex = 3;
            this.ultraLabel1.Text = "Module Name :";
            // 
            // ultraLabel2
            // 
            appearance17.TextHAlignAsString = "Right";
            appearance17.TextVAlignAsString = "Middle";
            this.ultraLabel2.Appearance = appearance17;
            this.ultraLabel2.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabel2.Location = new System.Drawing.Point(614, 207);
            this.ultraLabel2.Name = "ultraLabel2";
            this.ultraLabel2.Size = new System.Drawing.Size(165, 21);
            this.ultraLabel2.TabIndex = 4;
            this.ultraLabel2.Text = "Module Caption :";
            // 
            // txtModuleName
            // 
            this.txtModuleName.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.txtModuleName.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtModuleName.Location = new System.Drawing.Point(786, 176);
            this.txtModuleName.MaxLength = 30;
            this.txtModuleName.Name = "txtModuleName";
            this.txtModuleName.Size = new System.Drawing.Size(257, 20);
            this.txtModuleName.TabIndex = 1;
            this.txtModuleName.ValueChanged += new System.EventHandler(this.txtModuleName_ValueChanged);
            this.txtModuleName.Enter += new System.EventHandler(this.txtModuleName_Enter);
            this.txtModuleName.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtModuleName_KeyDown);
            this.txtModuleName.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtModuleName_KeyPress);
            this.txtModuleName.Leave += new System.EventHandler(this.txtModuleName_Leave);
            this.txtModuleName.MouseClick += new System.Windows.Forms.MouseEventHandler(this.txtModuleName_MouseClick);
            // 
            // txtModuleCaption
            // 
            this.txtModuleCaption.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.txtModuleCaption.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtModuleCaption.Location = new System.Drawing.Point(786, 207);
            this.txtModuleCaption.MaxLength = 30;
            this.txtModuleCaption.Name = "txtModuleCaption";
            this.txtModuleCaption.Size = new System.Drawing.Size(257, 20);
            this.txtModuleCaption.TabIndex = 2;
            this.txtModuleCaption.Enter += new System.EventHandler(this.txtModuleCaption_Enter);
            this.txtModuleCaption.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtModuleCaption_KeyDown);
            this.txtModuleCaption.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtModuleCaption_KeyPress);
            this.txtModuleCaption.Leave += new System.EventHandler(this.txtModuleCaption_Leave);
            this.txtModuleCaption.MouseClick += new System.Windows.Forms.MouseEventHandler(this.txtModuleCaption_MouseClick);
            // 
            // ultraPanel2
            // 
            this.ultraPanel2.Dock = System.Windows.Forms.DockStyle.Left;
            this.ultraPanel2.Location = new System.Drawing.Point(0, 10);
            this.ultraPanel2.Name = "ultraPanel2";
            this.ultraPanel2.Size = new System.Drawing.Size(14, 425);
            this.ultraPanel2.TabIndex = 13;
            // 
            // ultraPanel3
            // 
            this.ultraPanel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.ultraPanel3.Location = new System.Drawing.Point(0, 0);
            this.ultraPanel3.Name = "ultraPanel3";
            this.ultraPanel3.Size = new System.Drawing.Size(1094, 10);
            this.ultraPanel3.TabIndex = 14;
            // 
            // ultraPanel4
            // 
            this.ultraPanel4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.ultraPanel4.Location = new System.Drawing.Point(0, 435);
            this.ultraPanel4.Name = "ultraPanel4";
            this.ultraPanel4.Size = new System.Drawing.Size(1094, 10);
            this.ultraPanel4.TabIndex = 15;
            // 
            // txtModuleId
            // 
            appearance20.BackColor = System.Drawing.Color.WhiteSmoke;
            appearance20.TextHAlignAsString = "Center";
            appearance20.TextVAlignAsString = "Middle";
            this.txtModuleId.Appearance = appearance20;
            this.txtModuleId.BackColor = System.Drawing.Color.WhiteSmoke;
            this.txtModuleId.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.txtModuleId.Enabled = false;
            this.txtModuleId.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtModuleId.Location = new System.Drawing.Point(786, 145);
            this.txtModuleId.Name = "txtModuleId";
            this.txtModuleId.Size = new System.Drawing.Size(257, 20);
            this.txtModuleId.TabIndex = 0;
            // 
            // ultraLabel6
            // 
            appearance18.TextHAlignAsString = "Right";
            appearance18.TextVAlignAsString = "Middle";
            this.ultraLabel6.Appearance = appearance18;
            this.ultraLabel6.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabel6.Location = new System.Drawing.Point(614, 145);
            this.ultraLabel6.Name = "ultraLabel6";
            this.ultraLabel6.Size = new System.Drawing.Size(165, 21);
            this.ultraLabel6.TabIndex = 17;
            this.ultraLabel6.Text = "Module Id : ";
            // 
            // chkInactive
            // 
            appearance21.FontData.BoldAsString = "True";
            appearance21.FontData.Name = "Verdana";
            appearance21.FontData.SizeInPoints = 8F;
            this.chkInactive.Appearance = appearance21;
            this.chkInactive.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkInactive.Location = new System.Drawing.Point(716, 240);
            this.chkInactive.Name = "chkInactive";
            this.chkInactive.Size = new System.Drawing.Size(85, 25);
            this.chkInactive.TabIndex = 3;
            this.chkInactive.Tag = "0";
            this.chkInactive.Text = "Inactive :";
            this.chkInactive.CheckedChanged += new System.EventHandler(this.chkInactive_CheckedChanged);
            this.chkInactive.KeyDown += new System.Windows.Forms.KeyEventHandler(this.chkInactive_KeyDown);
            // 
            // frmModule
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1094, 505);
            this.ControlBox = false;
            this.Controls.Add(this.chkInactive);
            this.Controls.Add(this.txtModuleId);
            this.Controls.Add(this.ultraLabel6);
            this.Controls.Add(this.txtModuleCaption);
            this.Controls.Add(this.txtModuleName);
            this.Controls.Add(this.ultraLabel2);
            this.Controls.Add(this.ultraLabel1);
            this.Controls.Add(this.gridList);
            this.Controls.Add(this.ultraPanel2);
            this.Controls.Add(this.ultraPanel3);
            this.Controls.Add(this.ultraPanel4);
            this.Controls.Add(this.ultraPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmModule";
            this.Text = "Module Entry ...";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmModule_FormClosing);
            this.Load += new System.EventHandler(this.frmModule_Load);
            this.ultraPanel1.ClientArea.ResumeLayout(false);
            this.ultraPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtModuleName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtModuleCaption)).EndInit();
            this.ultraPanel2.ResumeLayout(false);
            this.ultraPanel3.ResumeLayout(false);
            this.ultraPanel4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtModuleId)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkInactive)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Infragistics.Win.Misc.UltraButton btnClose;
        private Infragistics.Win.Misc.UltraButton btnSave;
        private Infragistics.Win.Misc.UltraButton btnDelete;
        private Infragistics.Win.Misc.UltraButton btnCancel;
        private Infragistics.Win.Misc.UltraPanel ultraPanel1;
        private Infragistics.Win.UltraWinGrid.UltraGrid gridList;
        private Infragistics.Win.Misc.UltraLabel ultraLabel1;
        private Infragistics.Win.Misc.UltraLabel ultraLabel2;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtModuleName;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtModuleCaption;
        private Infragistics.Win.Misc.UltraPanel ultraPanel2;
        private Infragistics.Win.Misc.UltraPanel ultraPanel3;
        private Infragistics.Win.Misc.UltraPanel ultraPanel4;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtModuleId;
        private Infragistics.Win.Misc.UltraLabel ultraLabel6;
        private Infragistics.Win.UltraWinEditors.UltraCheckEditor chkInactive;
    }
}