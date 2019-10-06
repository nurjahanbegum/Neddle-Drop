namespace GTRHRIS.Common.FormEntry
{
    partial class frmLogin
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
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmLogin));
            Infragistics.Win.Appearance appearance12 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            this.panelLogin = new Infragistics.Win.Misc.UltraPanel();
            this.ultraPanel7 = new Infragistics.Win.Misc.UltraPanel();
            this.ultraPanel9 = new Infragistics.Win.Misc.UltraPanel();
            this.btnClose = new Infragistics.Win.Misc.UltraButton();
            this.btnLogin = new Infragistics.Win.Misc.UltraButton();
            this.txtPassword = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.txtUser = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.ultraPanel6 = new Infragistics.Win.Misc.UltraPanel();
            this.ultraPanel2 = new Infragistics.Win.Misc.UltraPanel();
            this.btnAuto = new Infragistics.Win.Misc.UltraButton();
            this.dtCheck = new Infragistics.Win.UltraWinEditors.UltraDateTimeEditor();
            this.panelLogin.ClientArea.SuspendLayout();
            this.panelLogin.SuspendLayout();
            this.ultraPanel7.ClientArea.SuspendLayout();
            this.ultraPanel7.SuspendLayout();
            this.ultraPanel9.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtPassword)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtUser)).BeginInit();
            this.ultraPanel6.ClientArea.SuspendLayout();
            this.ultraPanel6.SuspendLayout();
            this.ultraPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtCheck)).BeginInit();
            this.SuspendLayout();
            // 
            // panelLogin
            // 
            appearance3.BackColorAlpha = Infragistics.Win.Alpha.Transparent;
            appearance3.BackHatchStyle = Infragistics.Win.BackHatchStyle.Horizontal;
            appearance3.ImageBackground = ((System.Drawing.Image)(resources.GetObject("appearance3.ImageBackground")));
            this.panelLogin.Appearance = appearance3;
            this.panelLogin.BorderStyle = Infragistics.Win.UIElementBorderStyle.None;
            // 
            // panelLogin.ClientArea
            // 
            this.panelLogin.ClientArea.Controls.Add(this.ultraPanel7);
            this.panelLogin.ClientArea.Controls.Add(this.txtPassword);
            this.panelLogin.ClientArea.Controls.Add(this.txtUser);
            this.panelLogin.Location = new System.Drawing.Point(364, 121);
            this.panelLogin.Name = "panelLogin";
            this.panelLogin.Size = new System.Drawing.Size(396, 198);
            this.panelLogin.TabIndex = 1;
            // 
            // ultraPanel7
            // 
            appearance12.BackColorAlpha = Infragistics.Win.Alpha.Transparent;
            this.ultraPanel7.Appearance = appearance12;
            // 
            // ultraPanel7.ClientArea
            // 
            this.ultraPanel7.ClientArea.Controls.Add(this.dtCheck);
            this.ultraPanel7.ClientArea.Controls.Add(this.btnAuto);
            this.ultraPanel7.ClientArea.Controls.Add(this.ultraPanel9);
            this.ultraPanel7.ClientArea.Controls.Add(this.btnClose);
            this.ultraPanel7.ClientArea.Controls.Add(this.btnLogin);
            this.ultraPanel7.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.ultraPanel7.Location = new System.Drawing.Point(0, 139);
            this.ultraPanel7.Name = "ultraPanel7";
            this.ultraPanel7.Size = new System.Drawing.Size(396, 59);
            this.ultraPanel7.TabIndex = 4;
            // 
            // ultraPanel9
            // 
            this.ultraPanel9.Dock = System.Windows.Forms.DockStyle.Top;
            this.ultraPanel9.Location = new System.Drawing.Point(0, 0);
            this.ultraPanel9.Name = "ultraPanel9";
            this.ultraPanel9.Size = new System.Drawing.Size(396, 1);
            this.ultraPanel9.TabIndex = 6;
            // 
            // btnClose
            // 
            this.btnClose.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.Location = new System.Drawing.Point(129, 8);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(93, 27);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "C&lose";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnLogin
            // 
            this.btnLogin.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLogin.Location = new System.Drawing.Point(228, 8);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(98, 27);
            this.btnLogin.TabIndex = 0;
            this.btnLogin.Text = "&Login";
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // txtPassword
            // 
            appearance2.TextHAlignAsString = "Center";
            appearance2.TextVAlignAsString = "Middle";
            this.txtPassword.Appearance = appearance2;
            this.txtPassword.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPassword.Location = new System.Drawing.Point(130, 108);
            this.txtPassword.MaxLength = 15;
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(197, 25);
            this.txtPassword.TabIndex = 3;
            this.txtPassword.Enter += new System.EventHandler(this.txtPassword_Enter);
            this.txtPassword.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtPassword_KeyDown);
            this.txtPassword.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtPassword_KeyPress);
            this.txtPassword.MouseClick += new System.Windows.Forms.MouseEventHandler(this.txtPassword_MouseClick);
            // 
            // txtUser
            // 
            appearance1.TextHAlignAsString = "Center";
            appearance1.TextVAlignAsString = "Middle";
            this.txtUser.Appearance = appearance1;
            this.txtUser.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtUser.Location = new System.Drawing.Point(130, 77);
            this.txtUser.MaxLength = 30;
            this.txtUser.Name = "txtUser";
            this.txtUser.Size = new System.Drawing.Size(197, 25);
            this.txtUser.TabIndex = 2;
            this.txtUser.Enter += new System.EventHandler(this.txtUser_Enter);
            this.txtUser.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtUser_KeyDown);
            this.txtUser.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtUser_KeyPress);
            this.txtUser.MouseClick += new System.Windows.Forms.MouseEventHandler(this.txtUser_MouseClick);
            // 
            // ultraPanel6
            // 
            appearance4.BackColor2 = System.Drawing.Color.Transparent;
            appearance4.BackHatchStyle = Infragistics.Win.BackHatchStyle.Horizontal;
            appearance4.ImageBackground = ((System.Drawing.Image)(resources.GetObject("appearance4.ImageBackground")));
            this.ultraPanel6.Appearance = appearance4;
            // 
            // ultraPanel6.ClientArea
            // 
            this.ultraPanel6.ClientArea.Controls.Add(this.panelLogin);
            this.ultraPanel6.ClientArea.Controls.Add(this.ultraPanel2);
            this.ultraPanel6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraPanel6.Location = new System.Drawing.Point(0, 0);
            this.ultraPanel6.Name = "ultraPanel6";
            this.ultraPanel6.Size = new System.Drawing.Size(1079, 518);
            this.ultraPanel6.TabIndex = 5;
            // 
            // ultraPanel2
            // 
            appearance5.BackColor = System.Drawing.Color.Black;
            this.ultraPanel2.Appearance = appearance5;
            this.ultraPanel2.Dock = System.Windows.Forms.DockStyle.Left;
            this.ultraPanel2.Location = new System.Drawing.Point(0, 0);
            this.ultraPanel2.Name = "ultraPanel2";
            this.ultraPanel2.Size = new System.Drawing.Size(1, 518);
            this.ultraPanel2.TabIndex = 8;
            // 
            // btnAuto
            // 
            this.btnAuto.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAuto.Location = new System.Drawing.Point(15, 27);
            this.btnAuto.Name = "btnAuto";
            this.btnAuto.Size = new System.Drawing.Size(61, 23);
            this.btnAuto.TabIndex = 7;
            this.btnAuto.Text = "&Auto";
            this.btnAuto.Click += new System.EventHandler(this.btnAuto_Click);
            // 
            // dtCheck
            // 
            this.dtCheck.Location = new System.Drawing.Point(14, 11);
            this.dtCheck.Name = "dtCheck";
            this.dtCheck.Size = new System.Drawing.Size(82, 21);
            this.dtCheck.TabIndex = 0;
            this.dtCheck.Visible = false;
            // 
            // frmLogin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1079, 518);
            this.ControlBox = false;
            this.Controls.Add(this.ultraPanel6);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmLogin";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Login ...";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmLogin_FormClosing);
            this.Load += new System.EventHandler(this.frmLogin_Load);
            this.Resize += new System.EventHandler(this.frmLogin_Resize);
            this.panelLogin.ClientArea.ResumeLayout(false);
            this.panelLogin.ClientArea.PerformLayout();
            this.panelLogin.ResumeLayout(false);
            this.ultraPanel7.ClientArea.ResumeLayout(false);
            this.ultraPanel7.ClientArea.PerformLayout();
            this.ultraPanel7.ResumeLayout(false);
            this.ultraPanel9.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtPassword)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtUser)).EndInit();
            this.ultraPanel6.ClientArea.ResumeLayout(false);
            this.ultraPanel6.ResumeLayout(false);
            this.ultraPanel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dtCheck)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.Misc.UltraPanel panelLogin;
        private Infragistics.Win.Misc.UltraPanel ultraPanel6;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtPassword;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtUser;
        private Infragistics.Win.Misc.UltraPanel ultraPanel7;
        private Infragistics.Win.Misc.UltraButton btnClose;
        private Infragistics.Win.Misc.UltraButton btnLogin;
        private Infragistics.Win.Misc.UltraPanel ultraPanel9;
        private Infragistics.Win.Misc.UltraPanel ultraPanel2;
        private Infragistics.Win.Misc.UltraButton btnAuto;
        private Infragistics.Win.UltraWinEditors.UltraDateTimeEditor dtCheck;
    }
}

