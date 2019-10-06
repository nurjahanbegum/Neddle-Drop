namespace GTRHRIS.Admin.FormEntry
{
    partial class frmEmpDataInput
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
            if (disposing && (components  != null))
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
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab3 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            this.ultraTabPageControl1 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.btnProcess = new Infragistics.Win.Misc.UltraButton();
            this.ultraLabel9 = new Infragistics.Win.Misc.UltraLabel();
            this.btnSalProcessFull = new Infragistics.Win.Misc.UltraButton();
            this.dtLast = new Infragistics.Win.UltraWinEditors.UltraDateTimeEditor();
            this.dtFirst = new Infragistics.Win.UltraWinEditors.UltraDateTimeEditor();
            this.ultraLabel1 = new Infragistics.Win.Misc.UltraLabel();
            this.btnClose = new Infragistics.Win.Misc.UltraButton();
            this.btnCancel = new Infragistics.Win.Misc.UltraButton();
            this.btnDelete = new Infragistics.Win.Misc.UltraButton();
            this.btnSave = new Infragistics.Win.Misc.UltraButton();
            this.ultraPanel1 = new Infragistics.Win.Misc.UltraPanel();
            this.ultraPanel2 = new Infragistics.Win.Misc.UltraPanel();
            this.ultraPanel3 = new Infragistics.Win.Misc.UltraPanel();
            this.tabEmployee = new Infragistics.Win.UltraWinTabControl.UltraTabControl();
            this.ultraTabSharedControlsPage1 = new Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage();
            this.pnlContainer = new Infragistics.Win.Misc.UltraPanel();
            this.ultraPanel8 = new Infragistics.Win.Misc.UltraPanel();
            this.btnSalary = new Infragistics.Win.Misc.UltraButton();
            this.ultraTabPageControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtLast)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFirst)).BeginInit();
            this.ultraPanel1.ClientArea.SuspendLayout();
            this.ultraPanel1.SuspendLayout();
            this.ultraPanel2.SuspendLayout();
            this.ultraPanel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tabEmployee)).BeginInit();
            this.tabEmployee.SuspendLayout();
            this.pnlContainer.ClientArea.SuspendLayout();
            this.pnlContainer.SuspendLayout();
            this.ultraPanel8.SuspendLayout();
            this.SuspendLayout();
            // 
            // ultraTabPageControl1
            // 
            this.ultraTabPageControl1.Controls.Add(this.btnSalary);
            this.ultraTabPageControl1.Controls.Add(this.btnProcess);
            this.ultraTabPageControl1.Controls.Add(this.ultraLabel9);
            this.ultraTabPageControl1.Controls.Add(this.btnSalProcessFull);
            this.ultraTabPageControl1.Controls.Add(this.dtLast);
            this.ultraTabPageControl1.Controls.Add(this.dtFirst);
            this.ultraTabPageControl1.Controls.Add(this.ultraLabel1);
            this.ultraTabPageControl1.Location = new System.Drawing.Point(1, 23);
            this.ultraTabPageControl1.Name = "ultraTabPageControl1";
            this.ultraTabPageControl1.Size = new System.Drawing.Size(674, 495);
            // 
            // btnProcess
            // 
            this.btnProcess.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnProcess.Location = new System.Drawing.Point(226, 255);
            this.btnProcess.Name = "btnProcess";
            this.btnProcess.Size = new System.Drawing.Size(131, 40);
            this.btnProcess.TabIndex = 112;
            this.btnProcess.Tag = "0";
            this.btnProcess.Text = "Process Data Input";
            this.btnProcess.Click += new System.EventHandler(this.btnProcess_Click);
            // 
            // ultraLabel9
            // 
            appearance1.BackColor = System.Drawing.Color.Beige;
            appearance1.TextHAlignAsString = "Left";
            appearance1.TextVAlignAsString = "Middle";
            this.ultraLabel9.Appearance = appearance1;
            this.ultraLabel9.BorderStyleInner = Infragistics.Win.UIElementBorderStyle.Solid;
            this.ultraLabel9.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabel9.Location = new System.Drawing.Point(147, 116);
            this.ultraLabel9.Name = "ultraLabel9";
            this.ultraLabel9.Size = new System.Drawing.Size(105, 20);
            this.ultraLabel9.TabIndex = 66;
            this.ultraLabel9.Text = "Last Date :";
            // 
            // btnSalProcessFull
            // 
            this.btnSalProcessFull.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSalProcessFull.Location = new System.Drawing.Point(226, 192);
            this.btnSalProcessFull.Name = "btnSalProcessFull";
            this.btnSalProcessFull.Size = new System.Drawing.Size(131, 40);
            this.btnSalProcessFull.TabIndex = 111;
            this.btnSalProcessFull.Tag = "0";
            this.btnSalProcessFull.Text = "Transfer";
            this.btnSalProcessFull.Click += new System.EventHandler(this.btnSalProcessFull_Click);
            // 
            // dtLast
            // 
            this.dtLast.AutoSize = false;
            this.dtLast.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtLast.FormatString = "dd-MMM-yyyy";
            this.dtLast.Location = new System.Drawing.Point(258, 116);
            this.dtLast.MaskInput = "";
            this.dtLast.Name = "dtLast";
            this.dtLast.Size = new System.Drawing.Size(189, 20);
            this.dtLast.TabIndex = 65;
            // 
            // dtFirst
            // 
            this.dtFirst.AutoSize = false;
            this.dtFirst.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtFirst.FormatString = "dd-MMM-yyyy";
            this.dtFirst.Location = new System.Drawing.Point(258, 80);
            this.dtFirst.MaskInput = "";
            this.dtFirst.Name = "dtFirst";
            this.dtFirst.Size = new System.Drawing.Size(189, 20);
            this.dtFirst.TabIndex = 63;
            // 
            // ultraLabel1
            // 
            appearance2.BackColor = System.Drawing.Color.Beige;
            appearance2.TextHAlignAsString = "Left";
            appearance2.TextVAlignAsString = "Middle";
            this.ultraLabel1.Appearance = appearance2;
            this.ultraLabel1.BorderStyleInner = Infragistics.Win.UIElementBorderStyle.Solid;
            this.ultraLabel1.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabel1.Location = new System.Drawing.Point(147, 80);
            this.ultraLabel1.Name = "ultraLabel1";
            this.ultraLabel1.Size = new System.Drawing.Size(105, 20);
            this.ultraLabel1.TabIndex = 64;
            this.ultraLabel1.Text = "First Date :";
            // 
            // btnClose
            // 
            this.btnClose.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.Location = new System.Drawing.Point(14, 4);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(131, 40);
            this.btnClose.TabIndex = 11;
            this.btnClose.Text = "C&lose";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.Location = new System.Drawing.Point(1030, 4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(131, 40);
            this.btnCancel.TabIndex = 10;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.Visible = false;
            // 
            // btnDelete
            // 
            this.btnDelete.Enabled = false;
            this.btnDelete.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDelete.Location = new System.Drawing.Point(895, 4);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(131, 40);
            this.btnDelete.TabIndex = 12;
            this.btnDelete.Tag = "2";
            this.btnDelete.Text = "&Delete";
            this.btnDelete.Visible = false;
            // 
            // btnSave
            // 
            this.btnSave.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.Location = new System.Drawing.Point(759, 4);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(131, 40);
            this.btnSave.TabIndex = 9;
            this.btnSave.Tag = "0";
            this.btnSave.Text = "&Save";
            this.btnSave.Visible = false;
            // 
            // ultraPanel1
            // 
            appearance3.BackColor = System.Drawing.Color.SlateGray;
            this.ultraPanel1.Appearance = appearance3;
            this.ultraPanel1.BorderStyle = Infragistics.Win.UIElementBorderStyle.Etched;
            // 
            // ultraPanel1.ClientArea
            // 
            this.ultraPanel1.ClientArea.Controls.Add(this.btnSave);
            this.ultraPanel1.ClientArea.Controls.Add(this.btnClose);
            this.ultraPanel1.ClientArea.Controls.Add(this.btnDelete);
            this.ultraPanel1.ClientArea.Controls.Add(this.btnCancel);
            this.ultraPanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.ultraPanel1.Location = new System.Drawing.Point(0, 541);
            this.ultraPanel1.Name = "ultraPanel1";
            this.ultraPanel1.Size = new System.Drawing.Size(1171, 51);
            this.ultraPanel1.TabIndex = 10;
            // 
            // ultraPanel2
            // 
            this.ultraPanel2.Dock = System.Windows.Forms.DockStyle.Left;
            this.ultraPanel2.Location = new System.Drawing.Point(0, 10);
            this.ultraPanel2.Name = "ultraPanel2";
            this.ultraPanel2.Size = new System.Drawing.Size(14, 531);
            this.ultraPanel2.TabIndex = 13;
            // 
            // ultraPanel3
            // 
            this.ultraPanel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.ultraPanel3.Location = new System.Drawing.Point(0, 0);
            this.ultraPanel3.Name = "ultraPanel3";
            this.ultraPanel3.Size = new System.Drawing.Size(1171, 10);
            this.ultraPanel3.TabIndex = 14;
            // 
            // tabEmployee
            // 
            this.tabEmployee.Controls.Add(this.ultraTabSharedControlsPage1);
            this.tabEmployee.Controls.Add(this.ultraTabPageControl1);
            this.tabEmployee.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabEmployee.Location = new System.Drawing.Point(0, 10);
            this.tabEmployee.Name = "tabEmployee";
            this.tabEmployee.SharedControlsPage = this.ultraTabSharedControlsPage1;
            this.tabEmployee.Size = new System.Drawing.Size(678, 521);
            this.tabEmployee.TabIndex = 63;
            ultraTab3.Key = "Entry";
            ultraTab3.TabPage = this.ultraTabPageControl1;
            ultraTab3.Text = "New Employee Data Insert";
            this.tabEmployee.Tabs.AddRange(new Infragistics.Win.UltraWinTabControl.UltraTab[] {
            ultraTab3});
            // 
            // ultraTabSharedControlsPage1
            // 
            this.ultraTabSharedControlsPage1.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabSharedControlsPage1.Name = "ultraTabSharedControlsPage1";
            this.ultraTabSharedControlsPage1.Size = new System.Drawing.Size(674, 495);
            // 
            // pnlContainer
            // 
            // 
            // pnlContainer.ClientArea
            // 
            this.pnlContainer.ClientArea.Controls.Add(this.tabEmployee);
            this.pnlContainer.ClientArea.Controls.Add(this.ultraPanel8);
            this.pnlContainer.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlContainer.Location = new System.Drawing.Point(14, 10);
            this.pnlContainer.Name = "pnlContainer";
            this.pnlContainer.Size = new System.Drawing.Size(678, 531);
            this.pnlContainer.TabIndex = 64;
            // 
            // ultraPanel8
            // 
            this.ultraPanel8.Dock = System.Windows.Forms.DockStyle.Top;
            this.ultraPanel8.Location = new System.Drawing.Point(0, 0);
            this.ultraPanel8.Name = "ultraPanel8";
            this.ultraPanel8.Size = new System.Drawing.Size(678, 10);
            this.ultraPanel8.TabIndex = 65;
            // 
            // btnSalary
            // 
            this.btnSalary.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSalary.Location = new System.Drawing.Point(226, 315);
            this.btnSalary.Name = "btnSalary";
            this.btnSalary.Size = new System.Drawing.Size(131, 40);
            this.btnSalary.TabIndex = 113;
            this.btnSalary.Tag = "0";
            this.btnSalary.Text = "Salary Data Input";
            this.btnSalary.Click += new System.EventHandler(this.btnSalary_Click);
            // 
            // frmEmpDataInput
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1171, 592);
            this.ControlBox = false;
            this.Controls.Add(this.pnlContainer);
            this.Controls.Add(this.ultraPanel2);
            this.Controls.Add(this.ultraPanel3);
            this.Controls.Add(this.ultraPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "frmEmpDataInput";
            this.Text = "New Employee Data Insert";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmEmpDataInput_FormClosing);
            this.ultraTabPageControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dtLast)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFirst)).EndInit();
            this.ultraPanel1.ClientArea.ResumeLayout(false);
            this.ultraPanel1.ResumeLayout(false);
            this.ultraPanel2.ResumeLayout(false);
            this.ultraPanel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tabEmployee)).EndInit();
            this.tabEmployee.ResumeLayout(false);
            this.pnlContainer.ClientArea.ResumeLayout(false);
            this.pnlContainer.ResumeLayout(false);
            this.ultraPanel8.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.Misc.UltraButton btnClose;
        private Infragistics.Win.Misc.UltraButton btnSave;
        private Infragistics.Win.Misc.UltraButton btnDelete;
        private Infragistics.Win.Misc.UltraButton btnCancel;
        private Infragistics.Win.Misc.UltraPanel ultraPanel1;
        private Infragistics.Win.Misc.UltraPanel ultraPanel2;
        private Infragistics.Win.Misc.UltraPanel ultraPanel3;
        private Infragistics.Win.UltraWinTabControl.UltraTabControl tabEmployee;
        private Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage ultraTabSharedControlsPage1;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl1;
        private Infragistics.Win.Misc.UltraPanel pnlContainer;
        private Infragistics.Win.Misc.UltraPanel ultraPanel8;
        private Infragistics.Win.Misc.UltraLabel ultraLabel9;
        private Infragistics.Win.UltraWinEditors.UltraDateTimeEditor dtLast;
        private Infragistics.Win.Misc.UltraLabel ultraLabel1;
        private Infragistics.Win.UltraWinEditors.UltraDateTimeEditor dtFirst;
        private Infragistics.Win.Misc.UltraButton btnSalProcessFull;
        private Infragistics.Win.Misc.UltraButton btnProcess;
        private Infragistics.Win.Misc.UltraButton btnSalary;
    }
}