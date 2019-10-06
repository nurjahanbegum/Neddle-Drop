using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GTRLibrary;
using GTRHRIS.Common.Classes;

namespace GTRHRIS.Common.FormEntry
{    
    public partial class frmLogin : Form
    {
        clsProcedure clsProc = new clsProcedure();
        FormClass.clsLogin clsForm = new FormClass.clsLogin();
        clsMain clsM = new clsMain();

        frmMaster FM;

        public frmLogin(frmMaster fm)
        {
            InitializeComponent();
            FM = fm;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            ParentForm.Close();
        }

        private void frmLogin_Load(object sender, EventArgs e)
        {
            Infragistics.Win.AppStyling.StyleManager.Load(clsProc.GTRXMLReader("Style"));
            btnAuto.Top = -100;
        }

        private void frmLogin_Resize(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;

            panelLogin.Left = this.Width - (panelLogin.Width + 120);
            panelLogin.Top = this.Height - (panelLogin.Height + 300);
        }

        private void frmLogin_FormClosing(object sender, FormClosingEventArgs e)
        {
            clsForm = null;
            clsProc = null;
            clsM = null;

            //ParentForm.Dispose();
            FM = null;
        }

        private Boolean fncSoftwareExDate()
        {
            try
            {
                string sqlQuery = "";
                System.Data.DataSet dsCheck=new System.Data.DataSet();
                clsConnection clscon = new clsConnection();
                sqlQuery = "select exDate,isInActive from  tbl_varMenu";
                clscon.GTRFillDatasetWithSQLCommand(ref dsCheck,sqlQuery);
                dsCheck.Tables[0].TableName = "CheckDate";
                DataRow dr;
                
                dr = dsCheck.Tables["CheckDate"].Rows[0];
                dtCheck.Value = dr["exDate"];
                string isInactive = dr["isInActive"].ToString();
                if (dtCheck.Value.ToString() ==DateTime.Now.Date.ToString() || isInactive=="1")
                {
                    sqlQuery = "update tbl_varMenu set isInActive=1";
                    clscon.GTRSaveDataWithSQLCommand(sqlQuery);
                    frmMaster frm=new frmMaster();
                    frm.Close();
                    return true;
                }
                
            }
            catch (Exception ex)
            {
                
                MessageBox.Show(ex.Message);
            }
            return false;
        }


        private Boolean fncSoftwareExDateDB()
        {

            if (clsMain.strValidationDB == "GT!@No")
            {
                frmMaster frm = new frmMaster();
                frm.Close();
                MessageBox.Show("System32 dll File Missing");
                return true;
            }

            return false;

        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            //Check all textbox filled or not
            if (fncBlank() == true)
            {
                return;
            }


            try
            {
                #region Check Valid User in Database

                //if (fncSoftwareExDate()==true)
                //{
                // return;   
                //}

                //Software Validation

                clsMain.strValidationDB = clsProc.GTRVaildationSQL();
                clsMain.strValidBTDB = clsProc.GTRVaildationSQLBT();


                if (fncSoftwareExDateDB() == true)
                {
                    return;
                }
                
                System.Data.DataSet dsLogin = new System.Data.DataSet();
                clsForm.prcGetLoginDetails(ref dsLogin,txtUser.Text.ToString().Trim(),txtPassword.Text.ToString().Trim());
                 
                if (dsLogin.Tables["Login"].Rows.Count == 0)
                {
                    //if invalid user
                    MessageBox.Show("Invalid user name or password", this.Text, MessageBoxButtons.OK);
                    txtUser.Focus();
                    return;
                }
                else
                {
                    //If valid user
                    DataTable dt = dsLogin.Tables["Login"];

                    //User Id & Name
                    clsMain.intUserId = (Int32)dt.Rows[0]["LUserId"];
                    clsMain.strUser = dt.Rows[0]["EmpNameCode"].ToString();
                    clsMain.strUserCode = dt.Rows[0]["EmpCode"].ToString();

                    //User SubGroupId & SubGroupName
                    clsMain.intSGroupId = Int32.Parse(dt.Rows[0]["LSubGroupId"].ToString());
                    clsMain.strSGroupName = dt.Rows[0]["LSubGroupName"].ToString();

                    //User GroupId & GroupName
                    clsMain.intGroupId = Int32.Parse(dt.Rows[0]["LGroupId"].ToString());
                    clsMain.strGroupName = dt.Rows[0]["LGroupName"].ToString();

                    //clsMain.intSalaryId = dt.Rows[0]["IsSalary"].ToString();
                    //clsMain.intAttProssId = dt.Rows[0]["IsAttPross"].ToString();

                    //Transaction Date
                    clsMain.strTranDate = dt.Rows[0]["LoginDate"].ToString();

                    //Computer Name
                    clsMain.strComputerName = clsProc.GTRGetComputerName();

                    //Computer IP Address
                    clsMain.strIPAddress = clsProc.GTRGetIPAddress();

                    //Computer Mac Address
                    clsMain.strMacAddress = "";//clsProc.GTRGetMacAddress();



                }
                #endregion Check Valid User in Database

                #region User log update
                    clsM.prcLogin();
                #endregion

                #region Configuration
                    clsMain.SetConfiguration();
                #endregion Configuration

                #region call & loading next window
                FM.prcConfigureForm();
                this.Close();
                #endregion call & loading next window
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void txtUser_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void txtPassword_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void txtPassword_MouseClick(object sender, MouseEventArgs e)
        {
            clsM.GTRGotFocus(ref txtPassword);
        }

        private void txtPassword_Enter(object sender, EventArgs e)
        {
            clsM.GTRGotFocus(ref txtPassword);
        }

        private void txtUser_Enter(object sender, EventArgs e)
        {
            clsM.GTRGotFocus(ref txtUser);
        }

        private void txtUser_MouseClick(object sender, MouseEventArgs e)
        {
            clsM.GTRGotFocus(ref txtPassword);
        }

        private void txtUser_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = clsProc.GTRSingleQuote((short)e.KeyChar);
        }

        private void txtPassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = clsProc.GTRSingleQuote((short)e.KeyChar);
        }

        private Boolean fncBlank()
        {
            if (txtUser.Text.Length == 0)
            {
                MessageBox.Show("Please provide user name.", this.Text, MessageBoxButtons.OK);
                txtUser.Focus();
                return true;
            }
            if (txtPassword.Text.Length == 0)
            {
                MessageBox.Show("Please provide password.", this.Text, MessageBoxButtons.OK);
                txtPassword.Focus();
                return true;
            }
            return false;
        }

        private void btnAuto_Click(object sender, EventArgs e)
        {
            txtUser.Text = "GTR";
            txtPassword.Text = "GTR@Pass.123";
            btnLogin.Focus();
        }
    }
}
