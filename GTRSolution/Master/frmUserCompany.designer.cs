namespace GTRHRIS.Master
{
    partial class frmUserCompany
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
            this.ultraPanel3 = new Infragistics.Win.Misc.UltraPanel();
            this.ultraPanel2 = new Infragistics.Win.Misc.UltraPanel();
            this.ultraPanel4 = new Infragistics.Win.Misc.UltraPanel();
            this.ultraPanel1 = new Infragistics.Win.Misc.UltraPanel();
            this.btnSave = new Infragistics.Win.Misc.UltraButton();
            this.btnClose = new Infragistics.Win.Misc.UltraButton();
            this.btnDelete = new Infragistics.Win.Misc.UltraButton();
            this.btnCancel = new Infragistics.Win.Misc.UltraButton();
            this.gridTran = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.ultraPanel5 = new Infragistics.Win.Misc.UltraPanel();
            this.gridList = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.txtUserId = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.ultraPanel3.SuspendLayout();
            this.ultraPanel2.SuspendLayout();
            this.ultraPanel4.SuspendLayout();
            this.ultraPanel1.ClientArea.SuspendLayout();
            this.ultraPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridTran)).BeginInit();
            this.ultraPanel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtUserId)).BeginInit();
            this.SuspendLayout();
            // 
            // ultraPanel3
            // 
            this.ultraPanel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.ultraPanel3.Location = new System.Drawing.Point(0, 0);
            this.ultraPanel3.Name = "ultraPanel3";
            this.ultraPanel3.Size = new System.Drawing.Size(1084, 10);
            this.ultraPanel3.TabIndex = 15;
            // 
            // ultraPanel2
            // 
            this.ultraPanel2.Dock = System.Windows.Forms.DockStyle.Left;
            this.ultraPanel2.Location = new System.Drawing.Point(0, 10);
            this.ultraPanel2.Name = "ultraPanel2";
            this.ultraPanel2.Size = new System.Drawing.Size(14, 396);
            this.ultraPanel2.TabIndex = 16;
            // 
            // ultraPanel4
            // 
            this.ultraPanel4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.ultraPanel4.Location = new System.Drawing.Point(0, 406);
            this.ultraPanel4.Name = "ultraPanel4";
            this.ultraPanel4.Size = new System.Drawing.Size(1084, 10);
            this.ultraPanel4.TabIndex = 17;
            // 
            // ultraPanel1
            // 
            appearance1.BackColor = System.Drawing.Color.SlateGray;
            this.ultraPanel1.Appearance = appearance1;
            this.ultraPanel1.BorderStyle = Infragistics.Win.UIElementBorderStyle.Etched;
            // 
            // ultraPanel1.ClientArea
            // 
            this.ultraPanel1.ClientArea.Controls.Add(this.btnSave);
            this.ultraPanel1.ClientArea.Controls.Add(this.btnClose);
            this.ultraPanel1.ClientArea.Controls.Add(this.btnDelete);
            this.ultraPanel1.ClientArea.Controls.Add(this.btnCancel);
            this.ultraPanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.ultraPanel1.Location = new System.Drawing.Point(0, 416);
            this.ultraPanel1.Name = "ultraPanel1";
            this.ultraPanel1.Size = new System.Drawing.Size(1084, 60);
            this.ultraPanel1.TabIndex = 18;
            // 
            // btnSave
            // 
            this.btnSave.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.Location = new System.Drawing.Point(573, 3);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(131, 40);
            this.btnSave.TabIndex = 7;
            this.btnSave.Tag = "0";
            this.btnSave.Text = "&Update";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
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
            // btnDelete
            // 
            this.btnDelete.Enabled = false;
            this.btnDelete.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDelete.Location = new System.Drawing.Point(704, 3);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(131, 40);
            this.btnDelete.TabIndex = 8;
            this.btnDelete.Tag = "2";
            this.btnDelete.Text = "&Delete";
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.Location = new System.Drawing.Point(835, 3);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(131, 40);
            this.btnCancel.TabIndex = 9;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // gridTran
            // 
            this.gridTran.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.gridTran.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            this.gridTran.DisplayLayout.MaxColScrollRegions = 1;
            this.gridTran.DisplayLayout.MaxRowScrollRegions = 1;
            this.gridTran.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.gridTran.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.gridTran.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.gridTran.Dock = System.Windows.Forms.DockStyle.Left;
            this.gridTran.Location = new System.Drawing.Point(432, 10);
            this.gridTran.Name = "gridTran";
            this.gridTran.Size = new System.Drawing.Size(536, 396);
            this.gridTran.TabIndex = 20;
            this.gridTran.Text = "ultraGrid2";
            this.gridTran.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.gridTran_InitializeLayout);
            // 
            // ultraPanel5
            // 
            this.ultraPanel5.Dock = System.Windows.Forms.DockStyle.Left;
            this.ultraPanel5.Location = new System.Drawing.Point(418, 10);
            this.ultraPanel5.Name = "ultraPanel5";
            this.ultraPanel5.Size = new System.Drawing.Size(14, 396);
            this.ultraPanel5.TabIndex = 21;
            // 
            // gridList
            // 
            this.gridList.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.gridList.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            this.gridList.DisplayLayout.MaxColScrollRegions = 1;
            this.gridList.DisplayLayout.MaxRowScrollRegions = 1;
            this.gridList.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.gridList.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.gridList.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.gridList.Dock = System.Windows.Forms.DockStyle.Left;
            this.gridList.Location = new System.Drawing.Point(14, 10);
            this.gridList.Name = "gridList";
            this.gridList.Size = new System.Drawing.Size(404, 396);
            this.gridList.TabIndex = 22;
            this.gridList.Text = "ultraGrid2";
            this.gridList.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.gridList_InitializeLayout);
            this.gridList.DoubleClick += new System.EventHandler(this.gridList_DoubleClick);
            // 
            // txtUserId
            // 
            this.txtUserId.Location = new System.Drawing.Point(974, 16);
            this.txtUserId.Name = "txtUserId";
            this.txtUserId.Size = new System.Drawing.Size(61, 21);
            this.txtUserId.TabIndex = 23;
            this.txtUserId.Text = "0";
            this.txtUserId.Visible = false;
            // 
            // frmUserCompany
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1084, 476);
            this.ControlBox = false;
            this.Controls.Add(this.txtUserId);
            this.Controls.Add(this.gridTran);
            this.Controls.Add(this.ultraPanel5);
            this.Controls.Add(this.gridList);
            this.Controls.Add(this.ultraPanel2);
            this.Controls.Add(this.ultraPanel3);
            this.Controls.Add(this.ultraPanel4);
            this.Controls.Add(this.ultraPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmUserCompany";
            this.Text = "Sister Concern Permission Setup";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmUserCompany_FormClosing);
            this.Load += new System.EventHandler(this.frmUserCompany_Load);
            this.ultraPanel3.ResumeLayout(false);
            this.ultraPanel2.ResumeLayout(false);
            this.ultraPanel4.ResumeLayout(false);
            this.ultraPanel1.ClientArea.ResumeLayout(false);
            this.ultraPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridTran)).EndInit();
            this.ultraPanel5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtUserId)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Infragistics.Win.Misc.UltraPanel ultraPanel3;
        private Infragistics.Win.Misc.UltraPanel ultraPanel2;
        private Infragistics.Win.Misc.UltraPanel ultraPanel4;
        private Infragistics.Win.Misc.UltraPanel ultraPanel1;
        private Infragistics.Win.Misc.UltraButton btnSave;
        private Infragistics.Win.Misc.UltraButton btnClose;
        private Infragistics.Win.Misc.UltraButton btnDelete;
        private Infragistics.Win.Misc.UltraButton btnCancel;
        private Infragistics.Win.UltraWinGrid.UltraGrid gridTran;
        private Infragistics.Win.Misc.UltraPanel ultraPanel5;
        private Infragistics.Win.UltraWinGrid.UltraGrid gridList;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtUserId;
    }
}