namespace GTRHRIS.Master
{
    partial class frmPassChange
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
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance31 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance17 = new Infragistics.Win.Appearance();
            this.btnClose = new Infragistics.Win.Misc.UltraButton();
            this.btnCancel = new Infragistics.Win.Misc.UltraButton();
            this.btnSave = new Infragistics.Win.Misc.UltraButton();
            this.ultraPanel1 = new Infragistics.Win.Misc.UltraPanel();
            this.ultraLabel2 = new Infragistics.Win.Misc.UltraLabel();
            this.txtPassword = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.ultraPanel2 = new Infragistics.Win.Misc.UltraPanel();
            this.ultraPanel3 = new Infragistics.Win.Misc.UltraPanel();
            this.ultraPanel4 = new Infragistics.Win.Misc.UltraPanel();
            this.txtConfirmPassword = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.ultraLabel4 = new Infragistics.Win.Misc.UltraLabel();
            this.txtOldPassword = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.ultraLabel3 = new Infragistics.Win.Misc.UltraLabel();
            this.txtUserName = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.ultraLabel1 = new Infragistics.Win.Misc.UltraLabel();
            this.txtUserID = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.ultraLabel5 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraPanel1.ClientArea.SuspendLayout();
            this.ultraPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtPassword)).BeginInit();
            this.ultraPanel2.SuspendLayout();
            this.ultraPanel3.SuspendLayout();
            this.ultraPanel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtConfirmPassword)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtOldPassword)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtUserName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtUserID)).BeginInit();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            this.btnClose.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.Location = new System.Drawing.Point(12, 3);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(131, 40);
            this.btnClose.TabIndex = 2;
            this.btnClose.Text = "C&lose";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.Location = new System.Drawing.Point(413, 3);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(131, 40);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "&Clear";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.Location = new System.Drawing.Point(282, 3);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(131, 40);
            this.btnSave.TabIndex = 0;
            this.btnSave.Tag = "0";
            this.btnSave.Text = "&Update";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
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
            this.ultraPanel1.ClientArea.Controls.Add(this.btnCancel);
            this.ultraPanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.ultraPanel1.Location = new System.Drawing.Point(0, 445);
            this.ultraPanel1.Name = "ultraPanel1";
            this.ultraPanel1.Size = new System.Drawing.Size(1076, 60);
            this.ultraPanel1.TabIndex = 6;
            // 
            // ultraLabel2
            // 
            appearance3.TextHAlignAsString = "Right";
            appearance3.TextVAlignAsString = "Middle";
            this.ultraLabel2.Appearance = appearance3;
            this.ultraLabel2.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabel2.Location = new System.Drawing.Point(131, 182);
            this.ultraLabel2.Name = "ultraLabel2";
            this.ultraLabel2.Size = new System.Drawing.Size(147, 21);
            this.ultraLabel2.TabIndex = 4;
            this.ultraLabel2.Text = "Old Password :";
            // 
            // txtPassword
            // 
            this.txtPassword.AutoSize = false;
            this.txtPassword.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.txtPassword.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPassword.Location = new System.Drawing.Point(284, 210);
            this.txtPassword.MaxLength = 15;
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(257, 23);
            this.txtPassword.TabIndex = 1;
            this.txtPassword.Enter += new System.EventHandler(this.txtPassword_Enter);
            this.txtPassword.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtPassword_KeyDown);
            this.txtPassword.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtPassword_KeyPress);
            this.txtPassword.Leave += new System.EventHandler(this.txtPassword_Leave);
            this.txtPassword.MouseClick += new System.Windows.Forms.MouseEventHandler(this.txtPassword_MouseClick);
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
            this.ultraPanel3.Size = new System.Drawing.Size(1076, 10);
            this.ultraPanel3.TabIndex = 14;
            // 
            // ultraPanel4
            // 
            this.ultraPanel4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.ultraPanel4.Location = new System.Drawing.Point(0, 435);
            this.ultraPanel4.Name = "ultraPanel4";
            this.ultraPanel4.Size = new System.Drawing.Size(1076, 10);
            this.ultraPanel4.TabIndex = 15;
            // 
            // txtConfirmPassword
            // 
            this.txtConfirmPassword.AutoSize = false;
            this.txtConfirmPassword.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.txtConfirmPassword.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtConfirmPassword.Location = new System.Drawing.Point(284, 240);
            this.txtConfirmPassword.MaxLength = 15;
            this.txtConfirmPassword.Name = "txtConfirmPassword";
            this.txtConfirmPassword.PasswordChar = '*';
            this.txtConfirmPassword.Size = new System.Drawing.Size(257, 23);
            this.txtConfirmPassword.TabIndex = 2;
            this.txtConfirmPassword.Enter += new System.EventHandler(this.txtConfirmPassword_Enter);
            this.txtConfirmPassword.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtConfirmPassword_KeyDown);
            this.txtConfirmPassword.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtConfirmPassword_KeyPress);
            this.txtConfirmPassword.Leave += new System.EventHandler(this.txtConfirmPassword_Leave);
            this.txtConfirmPassword.MouseClick += new System.Windows.Forms.MouseEventHandler(this.txtConfirmPassword_MouseClick);
            // 
            // ultraLabel4
            // 
            appearance2.TextHAlignAsString = "Right";
            appearance2.TextVAlignAsString = "Middle";
            this.ultraLabel4.Appearance = appearance2;
            this.ultraLabel4.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabel4.Location = new System.Drawing.Point(131, 212);
            this.ultraLabel4.Name = "ultraLabel4";
            this.ultraLabel4.Size = new System.Drawing.Size(147, 21);
            this.ultraLabel4.TabIndex = 21;
            this.ultraLabel4.Text = "New Password :";
            // 
            // txtOldPassword
            // 
            this.txtOldPassword.AutoSize = false;
            this.txtOldPassword.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.txtOldPassword.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtOldPassword.Location = new System.Drawing.Point(284, 182);
            this.txtOldPassword.MaxLength = 15;
            this.txtOldPassword.Name = "txtOldPassword";
            this.txtOldPassword.PasswordChar = '*';
            this.txtOldPassword.Size = new System.Drawing.Size(257, 23);
            this.txtOldPassword.TabIndex = 0;
            this.txtOldPassword.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtNewPassword_KeyDown);
            this.txtOldPassword.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtNewPassword_KeyPress);
            // 
            // ultraLabel3
            // 
            appearance31.TextHAlignAsString = "Right";
            appearance31.TextVAlignAsString = "Middle";
            this.ultraLabel3.Appearance = appearance31;
            this.ultraLabel3.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabel3.Location = new System.Drawing.Point(131, 241);
            this.ultraLabel3.Name = "ultraLabel3";
            this.ultraLabel3.Size = new System.Drawing.Size(147, 21);
            this.ultraLabel3.TabIndex = 23;
            this.ultraLabel3.Text = "Confirm Password :";
            // 
            // txtUserName
            // 
            this.txtUserName.AutoSize = false;
            this.txtUserName.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.txtUserName.Enabled = false;
            this.txtUserName.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtUserName.Location = new System.Drawing.Point(284, 153);
            this.txtUserName.MaxLength = 15;
            this.txtUserName.Name = "txtUserName";
            this.txtUserName.Size = new System.Drawing.Size(257, 23);
            this.txtUserName.TabIndex = 24;
            // 
            // ultraLabel1
            // 
            appearance4.TextHAlignAsString = "Right";
            appearance4.TextVAlignAsString = "Middle";
            this.ultraLabel1.Appearance = appearance4;
            this.ultraLabel1.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabel1.Location = new System.Drawing.Point(131, 153);
            this.ultraLabel1.Name = "ultraLabel1";
            this.ultraLabel1.Size = new System.Drawing.Size(147, 21);
            this.ultraLabel1.TabIndex = 25;
            this.ultraLabel1.Text = "User Name :";
            // 
            // txtUserID
            // 
            this.txtUserID.AutoSize = false;
            this.txtUserID.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.txtUserID.Enabled = false;
            this.txtUserID.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtUserID.Location = new System.Drawing.Point(284, 124);
            this.txtUserID.MaxLength = 15;
            this.txtUserID.Name = "txtUserID";
            this.txtUserID.Size = new System.Drawing.Size(257, 23);
            this.txtUserID.TabIndex = 26;
            // 
            // ultraLabel5
            // 
            appearance17.TextHAlignAsString = "Right";
            appearance17.TextVAlignAsString = "Middle";
            this.ultraLabel5.Appearance = appearance17;
            this.ultraLabel5.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabel5.Location = new System.Drawing.Point(131, 124);
            this.ultraLabel5.Name = "ultraLabel5";
            this.ultraLabel5.Size = new System.Drawing.Size(147, 21);
            this.ultraLabel5.TabIndex = 27;
            this.ultraLabel5.Text = "User ID :";
            // 
            // frmPassChange
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1076, 505);
            this.ControlBox = false;
            this.Controls.Add(this.txtUserID);
            this.Controls.Add(this.ultraLabel5);
            this.Controls.Add(this.txtUserName);
            this.Controls.Add(this.ultraLabel1);
            this.Controls.Add(this.txtOldPassword);
            this.Controls.Add(this.ultraLabel3);
            this.Controls.Add(this.txtConfirmPassword);
            this.Controls.Add(this.ultraLabel4);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.ultraLabel2);
            this.Controls.Add(this.ultraPanel2);
            this.Controls.Add(this.ultraPanel3);
            this.Controls.Add(this.ultraPanel4);
            this.Controls.Add(this.ultraPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmPassChange";
            this.Text = "Change Password ...";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmPassChange_FormClosing);
            this.Load += new System.EventHandler(this.frmPassChange_Load);
            this.ultraPanel1.ClientArea.ResumeLayout(false);
            this.ultraPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtPassword)).EndInit();
            this.ultraPanel2.ResumeLayout(false);
            this.ultraPanel3.ResumeLayout(false);
            this.ultraPanel4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtConfirmPassword)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtOldPassword)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtUserName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtUserID)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.Misc.UltraButton btnClose;
        private Infragistics.Win.Misc.UltraButton btnSave;
        private Infragistics.Win.Misc.UltraButton btnCancel;
        private Infragistics.Win.Misc.UltraPanel ultraPanel1;
        private Infragistics.Win.Misc.UltraLabel ultraLabel2;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtPassword;
        private Infragistics.Win.Misc.UltraPanel ultraPanel2;
        private Infragistics.Win.Misc.UltraPanel ultraPanel3;
        private Infragistics.Win.Misc.UltraPanel ultraPanel4;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtConfirmPassword;
        private Infragistics.Win.Misc.UltraLabel ultraLabel4;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtOldPassword;
        private Infragistics.Win.Misc.UltraLabel ultraLabel3;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtUserName;
        private Infragistics.Win.Misc.UltraLabel ultraLabel1;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtUserID;
        private Infragistics.Win.Misc.UltraLabel ultraLabel5;
    }
}