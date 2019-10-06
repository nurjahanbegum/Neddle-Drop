using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Collections;
using GTRHRIS.Common.Classes;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using System.IO;

namespace GTRHRIS.Master
{
    public partial class frmPassChange : Form
    {
        System.Data.DataSet dsList;

        GTRLibrary.clsProcedure clsProc = new GTRLibrary.clsProcedure();
        clsMain clsM = new clsMain();

        
        private Infragistics.Win.UltraWinTabControl.UltraTabControl uTab;
        private Common.FormEntry.frmMaster FM;

        public frmPassChange(ref Infragistics.Win.UltraWinTabControl.UltraTabControl utab, Common.FormEntry.frmMaster fm)
        {
            InitializeComponent();
            uTab = utab;
            FM = fm;
        }

        private void frmPassChange_FormClosing(object sender, FormClosingEventArgs e)
        {
            int index = Common.Classes.clsMain.fncFindOpenFormIndex(this);
            uTab.Tabs.RemoveAt(index);
            Common.Classes.clsMain.fncExistOpenForm(this, "Remove");

            clsM = null;
            FM = null;
            this.Dispose();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmPassChange_Load(object sender, EventArgs e)
        {
            prcLoadList();
        }

        private void txtPassword_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void txtPassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = clsProc.GTRSingleQuote((short)e.KeyChar);
        }

        private void txtPassword_Leave(object sender, EventArgs e)
        {
            txtPassword.Text = txtPassword.Text.ToString();
        }

        private void txtPassword_MouseClick(object sender, MouseEventArgs e)
        {
            clsM.GTRGotFocus(ref txtPassword);
        }

        private void txtPassword_Enter(object sender, EventArgs e)
        {
            clsM.GTRGotFocus(ref txtPassword);
        }

        private void txtConfirmPassword_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void txtConfirmPassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = clsProc.GTRSingleQuote((short)e.KeyChar);
        }

        private void txtConfirmPassword_Enter(object sender, EventArgs e)
        {
            clsM.GTRGotFocus(ref txtConfirmPassword);
        }

        private void txtConfirmPassword_MouseClick(object sender, MouseEventArgs e)
        {
            clsM.GTRGotFocus(ref txtConfirmPassword);
        }

        private void txtConfirmPassword_Leave(object sender, EventArgs e)
        {
            txtConfirmPassword.Text = txtConfirmPassword.Text.ToString();
        }

        private void prcClearData()
        {
            this.txtOldPassword.Text = "";
            this.txtPassword.Text = "";
            this.txtConfirmPassword.Text = "";
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (fncBlank())
            {
                return;
            }
            ArrayList arQuery = new ArrayList();
            GTRLibrary.clsConnection clsCon = new GTRLibrary.clsConnection();
             
            string sqlQuery = "";
            Int32 NewId = 0; 

            try
            {
                //Check old password 
                System.Data.DataSet ds = new System.Data.DataSet();
                sqlQuery = "Select * from tblLogin_user Where LUserId = " + Common.Classes.clsMain.intUserId +"";
                clsCon.GTRFillDatasetWithSQLCommand(ref ds, sqlQuery);

                if (ds.Tables[0].Rows.Count == 0)
                {
                    MessageBox.Show("Please provide valid old password");
                    txtOldPassword.Focus();
                    return;
                }
                ds = null;

                //Update database
                sqlQuery = " Update tblLogin_User Set  LUserPass='" + clsProc.GTREncryptWord(txtPassword.Text.ToString()) + "' Where LUserId = " +Common.Classes.clsMain.intUserId + "";
                arQuery.Add(sqlQuery);

                //Transaction with database
                clsCon.GTRSaveDataWithSQLCommand(arQuery);
                
                MessageBox.Show("Data Updated Successfully");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                arQuery = null;
                clsCon = null;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            prcClearData();
        }

        private Boolean fncBlank()
        {
            if (this.txtOldPassword.Text.Length == 0)
            {
                MessageBox.Show("Please provide old password.");
                txtOldPassword.Focus();
                return true;
            }
            if (this.txtPassword.Text.Length == 0)
            {
                MessageBox.Show("Please provide new password.");
                txtPassword.Focus();
                return true;
            }
            if (this.txtConfirmPassword.Text.Length == 0)
            {
                MessageBox.Show("Please provide confirm password.");
                txtConfirmPassword.Focus();
                return true;
            }

            if (this.txtPassword.Text!=txtConfirmPassword.Text)
            {
                MessageBox.Show("Password & confirm password should be same.");
                txtPassword.Focus();
                return true;
            }
            return false;
        }

        private void txtNewPassword_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void txtNewPassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = clsProc.GTRSingleQuote((short)e.KeyChar);
        }

        private void prcLoadList()
        {
            GTRLibrary.clsConnection clsCon = new GTRLibrary.clsConnection();
            dsList = new System.Data.DataSet();

            try
            {
                string sqlQuery = "Exec prcGetUserSingle 0," + Common.Classes.clsMain.intUserId + "";
                clsCon.GTRFillDatasetWithSQLCommand(ref dsList, sqlQuery);
                dsList.Tables[0].TableName = "tbluser";


                DataRow dr;

                if (dsList.Tables["tbluser"].Rows.Count > 0)
                {
                    dr = dsList.Tables["tbluser"].Rows[0];

                    this.txtUserID.Text = clsProc.GTRDecryptWord(dr["luserName"].ToString()).ToString();
                    this.txtUserName.Text = (dr["empname"].ToString());
                }

                //Load for Filter Filed oparator Name
                //cboFilterOperator.DataSource = null;
                //cboFilterOperator.DataSource = dsList.Tables["FilterOperetor"];

            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                clsCon = null;
            }
        }

    }
}
