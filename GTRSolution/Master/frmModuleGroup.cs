using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GTRHRIS.Common.Classes;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;

namespace GTRHRIS.Master
{
    public partial class frmModuleGroup : Form
    {
        System.Data.DataSet dsList;
        System.Data.DataSet dsDetails;

        GTRLibrary.clsProcedure clsProc = new GTRLibrary.clsProcedure();
        clsMain clsM = new clsMain();

        private Infragistics.Win.UltraWinTabControl.UltraTabControl uTab;
        Common.FormEntry.frmMaster FM;
        public frmModuleGroup(ref Infragistics.Win.UltraWinTabControl.UltraTabControl utab, Common.FormEntry.frmMaster fm)
        {
            InitializeComponent();
            uTab = utab;
            FM = fm;
        }

        private void frmModuleGroup_FormClosing(object sender, FormClosingEventArgs e)
        {
            int index = Common.Classes.clsMain.fncFindOpenFormIndex(this);
            uTab.Tabs.RemoveAt(index);
            Common.Classes.clsMain.fncExistOpenForm(this, "Remove");

            dsList = null;
            dsDetails = null;
            FM = null;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmModuleGroup_Load(object sender, EventArgs e)
        {
            try
            {
                prcLoadList();
                prcLoadCombo();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void prcLoadList()
        {
            GTRLibrary.clsConnection clsCon = new GTRLibrary.clsConnection();
            dsList = new System.Data.DataSet();

            try
            {
                string sqlQuery = "Exec prcGetGroup 0";
                clsCon.GTRFillDatasetWithSQLCommand(ref dsList, sqlQuery);
                dsList.Tables[0].TableName = "GroupList";
                dsList.Tables[1].TableName = "ModuleList";

                gridList.DataSource = null;
                gridList.DataSource = dsList;
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
                cboModule.DataSource = null;
                cboModule.DataSource = dsList.Tables["ModuleList"];
                cboModule.DisplayMember = "ModuleCaption";
                cboModule.ValueMember = "ModuleId";

                cboModule.DisplayLayout.Bands[0].Columns[0].Hidden = true;

                cboModule.DisplayLayout.Bands[0].Columns[1].Width = 100;
                cboModule.DisplayLayout.Bands[0].Columns[2].Width = 250;
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
                gridList.DisplayLayout.Bands[0].Columns[0].Hidden = true;//Group Id
                gridList.DisplayLayout.Bands[0].Columns[1].Width = 100;  //Group Name
                gridList.DisplayLayout.Bands[0].Columns[2].Width = 250;  //Group Caption
                gridList.DisplayLayout.Bands[0].Columns[3].Hidden = true;//Module Id
                gridList.DisplayLayout.Bands[0].Columns[4].Hidden = true;//Module Name
                //gridList.DisplayLayout.Bands[0].Columns[4].Width = 80;  //Module Name
                gridList.DisplayLayout.Bands[0].Columns[5].Width = 200;  //Module Caption

                gridList.DisplayLayout.Bands[0].Columns[0].Header.Caption = "GroupId Id";
                gridList.DisplayLayout.Bands[0].Columns[1].Header.Caption = "Group Name";
                gridList.DisplayLayout.Bands[0].Columns[2].Header.Caption = "Group Caption";
                gridList.DisplayLayout.Bands[0].Columns[3].Header.Caption = "Module Id";
                gridList.DisplayLayout.Bands[0].Columns[4].Header.Caption = "Module Name";
                gridList.DisplayLayout.Bands[0].Columns[5].Header.Caption = "Module Caption";

                //Change alternate color
                gridList.DisplayLayout.Override.RowAlternateAppearance.BackColor = Color.Cyan;
                gridList.DisplayLayout.Override.RowAlternateAppearance.ForeColor = Color.DarkBlue;

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

        private void txtGroupName_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void txtGroupName_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = clsProc.GTRSingleQuote((short)e.KeyChar);
        }

        private void txtGroupName_Enter(object sender, EventArgs e)
        {
            clsM.GTRGotFocus(ref txtMenuName);
        }

        private void txtGroupName_MouseClick(object sender, MouseEventArgs e)
        {
            clsM.GTRGotFocus(ref txtMenuName);
        }

        private void txtGroupName_Leave(object sender, EventArgs e)
        {
            txtMenuName.Text = txtMenuName.Text.ToString();
        }

        private void txtGroupCaption_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void txtGroupCaption_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = clsProc.GTRSingleQuote((short)e.KeyChar);
        }

        private void txtGroupCaption_Leave(object sender, EventArgs e)
        {
            txtMenuCaption.Text = txtMenuCaption.Text.ToString();
        }

        private void txtGroupCaption_MouseClick(object sender, MouseEventArgs e)
        {
            clsM.GTRGotFocus(ref txtMenuCaption);
        }

        private void txtGroupCaption_Enter(object sender, EventArgs e)
        {
            clsM.GTRGotFocus(ref txtMenuCaption);
        }

        private void prcDisplayDetails(string strParam)
        {
            string sqlQuery = "Exec prcGetGroup " + Int32.Parse(strParam);
            dsDetails = new System.Data.DataSet();

            GTRLibrary.clsConnection clsCon = new GTRLibrary.clsConnection();

            clsCon.GTRFillDatasetWithSQLCommand(ref dsDetails, sqlQuery);
            dsDetails.Tables[0].TableName = "Group";

            DataRow dr;
            if (dsDetails.Tables["Group"].Rows.Count > 0)
            {
                dr = dsDetails.Tables["Group"].Rows[0];

                this.txtMenuId.Text = dr["mMenuGroupId"].ToString();
                this.txtMenuName.Text = dr["mMenuGroupName"].ToString();
                this.txtMenuCaption.Text = dr["mMenuGroupCaption"].ToString();
                this.cboModule.Text = dr["ModuleId"].ToString();

                this.btnDelete.Enabled = true;
            }
        }

        private void prcClearData()
        {
            this.txtMenuId.Text = "";
            this.txtMenuName.Text = "";
            this.txtMenuCaption.Text = "";
            this.cboModule.Text = "";

            this.btnDelete.Enabled = false;

            this.txtMenuName.Focus();
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
                //Member Master Table
                if (txtMenuId.Text.Length != 0)
                {
                    //Update
                    sqlQuery = " Update tblModule_Group Set mMenuGroupName = '" + txtMenuName.Text.ToString() + "', mMenuGroupCaption='" + txtMenuCaption.Text.ToString() + "', "
                        + " ModuleId = " + cboModule.Value + ", PCName='" + Common.Classes.clsMain.strComputerName + "', LUserId = " + Common.Classes.clsMain.intUserId + ""
                        + " Where mMenuGroupId = " + Int32.Parse(txtMenuId.Text);
                    arQuery.Add(sqlQuery);

                    // Insert Information To Log File
                    sqlQuery = "Insert Into tblUser_Trans_Log (LUserId, formName, tranStatement, tranType)"
                        + " Values (" + Common.Classes.clsMain.intUserId + ", '" + this.Name.ToString() + "','" + sqlQuery.Replace("'", "|") + "','Update')";
                    arQuery.Add(sqlQuery);

                    //Transaction with database
                    clsCon.GTRSaveDataWithSQLCommand(arQuery);
                    MessageBox.Show("Data Updated Successfully");
                }
                else
                {
                    //add new
                    sqlQuery = "Select Isnull(Max(mMenuGroupId),0)+1 As NewId from tblModule_Group";
                    NewId = clsCon.GTRCountingData(sqlQuery);

                    sqlQuery = "Insert Into tblModule_Group (mMenuGroupId, aId, mMenuGroupName, mMenuGroupCaption, ModuleId, PCName, LUserId) "
                        + " Values (" + NewId + ", " + NewId + ", '" + txtMenuName.Text.ToString() + "', '" + txtMenuCaption.Text.ToString() + "', " + cboModule.Value + ", '" + Common.Classes.clsMain.strComputerName + "'," + Common.Classes.clsMain.intUserId + ")";
                    arQuery.Add(sqlQuery);

                    // Insert Information To Log File
                    sqlQuery = "Insert Into tblUser_Trans_Log (LUserId, formName, tranStatement, tranType)"
                        + " Values (" + Common.Classes.clsMain.intUserId + ", '" + this.Name.ToString() + "','" + sqlQuery.Replace("'", "|") + "','Insert')";
                    arQuery.Add(sqlQuery);

                    //Transaction with database
                    clsCon.GTRSaveDataWithSQLCommand(arQuery);
                    MessageBox.Show("Data Saved Successfully");
                }
                prcClearData();
                txtMenuName.Focus();

                prcLoadList();
                //prcLoadCombo();
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

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you want to delete group information of [" + txtMenuName.Text + "]", "", System.Windows.Forms.MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.No)
            {
                return;
            }

            GTRLibrary.clsConnection clsCon = new GTRLibrary.clsConnection();
            try
            {
                int Result = 0;
                string sqlQuery = "";
                sqlQuery = "Delete from tblModule_Group Where mMenuGroupId = " + Int32.Parse(txtMenuId.Text);
                Result = clsCon.GTRSaveDataWithSQLCommand(sqlQuery);
                if (Result > 0)
                {
                    prcClearData();
                    txtMenuName.Focus();

                    prcLoadList();
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
            if (this.txtMenuName.Text.Length == 0)
            {
                MessageBox.Show("Please provide group name.");
                txtMenuName.Focus();
                return true;
            }
            if (this.txtMenuCaption.Text.Length == 0)
            {
                MessageBox.Show("Please provide group caption.");
                txtMenuCaption.Focus();
                return true;
            }
            if (this.cboModule.Text.Length == 0)
            {
                MessageBox.Show("Please provide under module.");
                cboModule.Focus();
                return true;
            }
            return false;
        }

        private void cboModule_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }
    }
}
