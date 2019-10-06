using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using GTRHRIS.Common.Classes;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;

namespace GTRHRIS.Master
{
    public partial class frmCreateUser : Form
    {
        System.Data.DataSet dsList;
        System.Data.DataSet dsDetails;
        GTRLibrary.clsProcedure clsProc = new GTRLibrary.clsProcedure();
        clsMain clsM = new clsMain();

        private Infragistics.Win.UltraWinTabControl.UltraTabControl uTab;
        private Common.FormEntry.frmMaster FM;

        public frmCreateUser(ref Infragistics.Win.UltraWinTabControl.UltraTabControl utab, Common.FormEntry.frmMaster fm)
        {
            InitializeComponent();
            uTab = utab;
            FM = fm;
        }

        private void frmCreateUser_FormClosing(object sender, FormClosingEventArgs e)
        {
            int index = Common.Classes.clsMain.fncFindOpenFormIndex(this);
            uTab.Tabs.RemoveAt(index);
            Common.Classes.clsMain.fncExistOpenForm(this, "Remove");

            clsProc = null;
            clsM = null;
            dsList = null;
            dsDetails = null;
            FM = null;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmCreateUser_Load(object sender, EventArgs e)
        {
            try
            {
                prcLoadList();
                prcLoadCombo();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void prcLoadList()
        {
            GTRLibrary.clsConnection clsCon = new GTRLibrary.clsConnection();
            dsList = new System.Data.DataSet();

            try
            {
                string sqlQuery = "Exec prcGetUser " + Common.Classes.clsMain.intUserId + ", 0";
                clsCon.GTRFillDatasetWithSQLCommand(ref dsList, sqlQuery);
                dsList.Tables[0].TableName = "UserList";
                dsList.Tables[1].TableName = "GroupList";
                dsList.Tables[2].TableName = "Employee";

                prcModifyDataset();

                gridList.DataSource = null;
                gridList.DataSource = dsList.Tables["UserList"];
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

        private void prcLoadCombo()
        {
            try
            {
                cboGroup.DataSource = null;
                cboGroup.DataSource = dsList.Tables["GroupList"];

                cboEmployee.DataSource = null;
                cboEmployee.DataSource = dsList.Tables["Employee"];
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        private void gridList_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            try
            {
                //Setup Grid
                gridList.DisplayLayout.Bands[0].Columns[0].Hidden = true;//User Id
                gridList.DisplayLayout.Bands[0].Columns[1].Width = 200;  //User Name
                gridList.DisplayLayout.Bands[0].Columns[2].Hidden = true;  //User Password
                gridList.DisplayLayout.Bands[0].Columns[3].Hidden = true;//Group Id
                gridList.DisplayLayout.Bands[0].Columns[4].Width = 250;  //Group Name
                gridList.DisplayLayout.Bands[0].Columns[5].Width = 100;  //Is Inactive

                gridList.DisplayLayout.Bands[0].Columns[0].Header.Caption = "User Id";
                gridList.DisplayLayout.Bands[0].Columns[1].Header.Caption = "User Name";
                gridList.DisplayLayout.Bands[0].Columns[2].Header.Caption = "User Password";
                gridList.DisplayLayout.Bands[0].Columns[3].Header.Caption = "Group Id";
                gridList.DisplayLayout.Bands[0].Columns[4].Header.Caption = "Group Name";
                gridList.DisplayLayout.Bands[0].Columns[5].Header.Caption = "Is Inactive";

                //Change alternate color
                gridList.DisplayLayout.Override.RowAlternateAppearance.BackColor = Color.Cyan;
                gridList.DisplayLayout.Override.RowAlternateAppearance.ForeColor = Color.DarkBlue;

                //Show Check Box Columns
                this.gridList.DisplayLayout.Bands[0].Columns[5].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox;

                //Select Full Row when click on any cell
                e.Layout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;

                //Selection Style Will Be Row Selector
                this.gridList.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;

                //Stop Updating
                this.gridList.DisplayLayout.Override.AllowUpdate = DefaultableBoolean.False;

                //Hiding +/- Indicator
                this.gridList.DisplayLayout.Override.ExpansionIndicator = ShowExpansionIndicator.Never;

                //Hide Group Box Display
                e.Layout.GroupByBox.Hidden = true;

                //Use Filtering
                this.gridList.DisplayLayout.Override.AllowRowFiltering = DefaultableBoolean.True;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void txtUserName_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void txtUserName_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = clsProc.GTRSingleQuote((short)e.KeyChar);
        }

        private void txtUserName_Enter(object sender, EventArgs e)
        {
            clsM.GTRGotFocus(ref txtUserName);
        }

        private void txtUserName_MouseClick(object sender, MouseEventArgs e)
        {
            clsM.GTRGotFocus(ref txtUserName);
        }

        private void txtUserName_Leave(object sender, EventArgs e)
        {
            txtUserName.Text = txtUserName.Text.ToString();
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

        private void prcDisplayDetails(string strParam)
        {
            string sqlQuery = "Exec prcGetUser " + Common.Classes.clsMain.intUserId + "," + Int32.Parse(strParam);
            dsDetails = new System.Data.DataSet();

            GTRLibrary.clsConnection clsCon = new GTRLibrary.clsConnection();

            clsCon.GTRFillDatasetWithSQLCommand(ref dsDetails, sqlQuery);
            dsDetails.Tables[0].TableName = "User";

            DataRow dr;
            if (dsDetails.Tables["User"].Rows.Count > 0)
            {
                dr = dsDetails.Tables["User"].Rows[0];

                this.txtUserId.Text = dr["LUserId"].ToString();
                this.txtUserName.Text = clsProc.GTRDecryptWord(dr["LUserName"].ToString());
                this.txtPassword.Text = clsProc.GTRDecryptWord(dr["LUserPass"].ToString());
                this.txtConfirmPassword.Text = clsProc.GTRDecryptWord(dr["LUserPass"].ToString());
                this.cboEmployee.Value= dr["EmpId"].ToString();
                this.cboGroup.Text = dr["LSubGroupName"].ToString();
                this.chkInactive.Checked = Boolean.Parse(dr["IsInactive"].ToString());

                this.txtPassword.Enabled = false;
                this.txtConfirmPassword.Enabled = false;

                this.btnSave.Text = "&Update";
                this.btnDelete.Enabled = true;
            }
        }

        private void prcClearData()
        {
            this.txtUserId.Text = "";
            this.txtUserName.Text = "";
            this.txtPassword.Text = "";
            this.txtConfirmPassword.Text = "";
            this.cboGroup.Value = null;
            this.cboEmployee.Value = null;
            this.chkInactive.Checked = false;

            this.txtPassword.Enabled = true;
            this.txtConfirmPassword.Enabled = true;

            this.btnSave.Text = "&Save";
            this.btnDelete.Enabled = false;

            this.txtUserName.Focus();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (fncBlank())
            {
                return;
            }

            GTRLibrary.clsConnection clsCon = new GTRLibrary.clsConnection();

            string sqlQuery = "";
            Int32 NewId = 0;

            try
            {
                //Member Master Table
                if (txtUserId.Text.Length != 0)
                {
                    //Update
                    sqlQuery = " Update tblLogin_User Set LUserName = '" + clsProc.GTREncryptWord(txtUserName.Text.ToString()) + "', LUserPass='" + clsProc.GTREncryptWord(txtPassword.Text.ToString()) + "', ";
                    sqlQuery += " LSubGroupId = " + cboGroup.Value + ", IsInactive=" + chkInactive.Tag + ", EmpId = "+ cboEmployee.Value.ToString() +"";
                    sqlQuery += " Where LUserId = " + Int32.Parse(txtUserId.Text);

                    NewId = clsCon.GTRSaveDataWithSQLCommand(sqlQuery);
                    if (NewId > 0)
                    {
                        MessageBox.Show("Data Updated Successfully");
                    }
                   //MessageBox.Show(clsProc.GTRDecryptWord(txtPassword.Text.ToString()));
                }
                else
                {
                    //add new
                    sqlQuery = "Select Isnull(Max(LUserId),0)+1 As NewId from tblLogin_User";
                    NewId = clsCon.GTRCountingData(sqlQuery);

                    sqlQuery = "Insert Into tblLogin_UserSalary (LUserId,ComID,isActiveSalary,isActiveSalaryOver,isActiveSalaryLess,EmpID,Amount)";
                    sqlQuery += " Values (" + NewId + ", " + Common.Classes.clsMain.intComId + ",0,0,0,0,0)";
                    clsCon.GTRSaveDataWithSQLCommand(sqlQuery);
                    
                    
                    sqlQuery = "Insert Into tblLogin_User (LUserId, aId, LUserName, LUserPass, LSubGroupId, IsInactive, EmpId) ";
                    sqlQuery += " Values (" + NewId + ", " + NewId + ", '" + clsProc.GTREncryptWord(txtUserName.Text.ToString()) + "', '" + clsProc.GTREncryptWord(txtPassword.Text.ToString()) + "', " + cboGroup.Value.ToString() + ", " + chkInactive.Tag.ToString() + ",  "+cboEmployee.Value.ToString()+")";

                    
                    NewId = clsCon.GTRSaveDataWithSQLCommand(sqlQuery);
                    if (NewId > 0)
                    {
                        MessageBox.Show("Data Saved Successfully");
                    }

                }
                prcClearData();
                txtUserName.Focus();

                prcLoadList();
                prcLoadCombo();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                clsCon = null;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            prcClearData();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you want to delete user information of [" + txtUserName.Text + "]", "", System.Windows.Forms.MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.No)
            {
                return;
            }

            GTRLibrary.clsConnection clsCon = new GTRLibrary.clsConnection();
            try
            {
                int Result = 0;
                string sqlQuery = "";
                sqlQuery = "Delete from tblLogin_User Where LUserId = " + Int32.Parse(txtUserId.Text) + "; Delete from tblLogin_UserSalary Where LUserId = " + Int32.Parse(txtUserId.Text) + "";
                Result = clsCon.GTRSaveDataWithSQLCommand(sqlQuery);
                if (Result > 0)
                {
                    prcClearData();
                    txtUserName.Focus();

                    prcLoadList();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                clsCon = null;
            }
        }

        private void gridList_DoubleClick(object sender, EventArgs e)
        {
            prcClearData();
            prcDisplayDetails(gridList.ActiveRow.Cells[0].Value.ToString());
        }

        private Boolean fncBlank()
        {
            if (this.txtUserName.Text.Length == 0)
            {
                MessageBox.Show("Please provide user name.");
                txtUserName.Focus();
                return true;
            }
            if (this.txtPassword.Text.Length == 0)
            {
                MessageBox.Show("Please provide user password.");
                txtPassword.Focus();
                return true;
            }
            if (this.txtConfirmPassword.Text.Length == 0)
            {
                MessageBox.Show("Please provide user confirm password.");
                txtConfirmPassword.Focus();
                return true;
            }
            if (this.txtPassword.Text.Trim() != this.txtConfirmPassword.Text.Trim())
            {
                MessageBox.Show("User password & comfirm password should be same.");
                txtPassword.Focus();
                return true;
            }

            if (this.cboEmployee.Text.ToString().Length==0)
            {
                MessageBox.Show("Please provide employee code.");
                cboEmployee.Focus();
                return true;
            }

            if (this.cboGroup.Text.Length == 0)
            {
                MessageBox.Show("Please provide under group.");
                cboGroup.Focus();
                return true;
            }
            return false;
        }

        private void cboModule_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void chkInactive_CheckedChanged(object sender, EventArgs e)
        {
            if (chkInactive.Checked)
                chkInactive.Tag = 1;
            else
                chkInactive.Tag = 0;
        }

        private void chkInactive_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void cboGroup_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            cboGroup.DisplayMember = "LSubGroupName";
            cboGroup.ValueMember = "LSubGroupId";

            cboGroup.DisplayLayout.Bands[0].Columns[0].Hidden = true;
            cboGroup.DisplayLayout.Bands[0].Columns[1].Width = 250;
            cboGroup.DisplayLayout.Bands[0].Columns[1].Header.Caption = "Group Name";
        }

        private void cboEmployee_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            cboEmployee.DisplayMember = "EmpCode";
            cboEmployee.ValueMember = "EmpId";

            cboEmployee.DisplayLayout.Bands[0].Columns[0].Hidden = true;
            cboEmployee.DisplayLayout.Bands[0].Columns[1].Width = 100;
            cboEmployee.DisplayLayout.Bands[0].Columns[2].Width = 200;
            cboEmployee.DisplayLayout.Bands[0].Columns[1].Header.Caption = "Code";
            cboEmployee.DisplayLayout.Bands[0].Columns[2].Header.Caption = "Name";
        }

        private void cboEmployee_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        public void prcModifyDataset()
        {
            for (int i = 0; i <= dsList.Tables[0].Rows.Count - 1; i++)
            {
                dsList.Tables[0].Rows[i]["LUserName"] = clsProc.GTRDecryptWord(dsList.Tables[0].Rows[i]["LUserName"].ToString());
            }
        }

    }
}
