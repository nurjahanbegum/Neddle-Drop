using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using GTRHRIS.Common.Classes;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;

namespace GTRHRIS.Master
{
    public partial class frmCreateUserGroup : Form
    {
        System.Data.DataSet dsList;
        System.Data.DataSet dsDetails;

        GTRLibrary.clsProcedure clsProc = new GTRLibrary.clsProcedure();
        clsMain clsM = new clsMain();

        private Infragistics.Win.UltraWinTabControl.UltraTabControl uTab;
        Common.FormEntry.frmMaster FM;
        public frmCreateUserGroup(ref Infragistics.Win.UltraWinTabControl.UltraTabControl utab, Common.FormEntry.frmMaster fm)
        {
            InitializeComponent();
            uTab = utab;
            FM = fm;
        }

        private void frmCreateUserGroup_FormClosing(object sender, FormClosingEventArgs e)
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

        private void frmCreateUserGroup_Load(object sender, EventArgs e)
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
                string sqlQuery = "Exec prcGetUserGroup " + Common.Classes.clsMain.intUserId + ", 0";
                clsCon.GTRFillDatasetWithSQLCommand(ref dsList, sqlQuery);
                dsList.Tables[0].TableName = "GroupList";

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
        }

        private void gridList_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            try
            {
                //Setup Grid
                gridList.DisplayLayout.Bands[0].Columns[0].Hidden = true;//ModuleId
                gridList.DisplayLayout.Bands[0].Columns[1].Width = 550;  //ModuleName

                gridList.DisplayLayout.Bands[0].Columns[0].Header.Caption = "Group Id";
                gridList.DisplayLayout.Bands[0].Columns[1].Header.Caption = "Group Name";

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
            clsM.GTRGotFocus(ref txtGroupName);
        }

        private void txtGroupName_MouseClick(object sender, MouseEventArgs e)
        {
            clsM.GTRGotFocus(ref txtGroupName);
        }

        private void txtGroupName_Leave(object sender, EventArgs e)
        {
            txtGroupName.Text = txtGroupName.Text.ToString();
        }

        private void prcDisplayDetails(string strParam)
        {
            string sqlQuery = "Exec prcGetUserGroup " + Common.Classes.clsMain.intUserId + "," + Int32.Parse(strParam);
            dsDetails = new System.Data.DataSet();

            GTRLibrary.clsConnection clsCon = new GTRLibrary.clsConnection();
            clsCon.GTRFillDatasetWithSQLCommand(ref dsDetails, sqlQuery);
            dsDetails.Tables[0].TableName = "Group";

            DataRow dr;
            if (dsDetails.Tables["Group"].Rows.Count > 0)
            {
                dr = dsDetails.Tables["Group"].Rows[0];

                this.txtGroupId.Text = dr["LSubGroupId"].ToString();
                this.txtGroupName.Text = dr["LSubGroupName"].ToString();
                this.btnSave.Text = "&Update";

                this.btnDelete.Enabled = true;
            }
        }

        private void prcClearData()
        {
            this.txtGroupId.Text = "";
            this.txtGroupName.Text = "";
            this.btnSave.Text = "&Save";

            this.btnDelete.Enabled = false;

            this.txtGroupName.Focus();
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
                if (txtGroupId.Text.Length != 0)
                {
                    //Update
                    sqlQuery = " Update tblLogin_Group_Sub Set LSubGroupName = '" + txtGroupName.Text.ToString() + "'";
                    sqlQuery += " Where LSubGroupId = " + Int32.Parse(txtGroupId.Text);

                    NewId = clsCon.GTRSaveDataWithSQLCommand(sqlQuery);
                    if (NewId > 0)
                    {
                        MessageBox.Show("Data Updated Successfully");
                    }
                }
                else
                {
                    //add new
                    sqlQuery = "Select Isnull(Max(LSubGroupId),0)+1 As NewId from tblLogin_Group_Sub";
                    NewId = clsCon.GTRCountingData(sqlQuery);

                    sqlQuery = "Insert Into tblLogin_Group_Sub (LSubGroupId, aId, LSubGroupName, LGroupId) ";
                    sqlQuery += " Values (" + NewId + ", " + NewId + ", '" + txtGroupName.Text.ToString() + "',2)";

                    NewId = clsCon.GTRSaveDataWithSQLCommand(sqlQuery);
                    if (NewId > 0)
                    {
                        MessageBox.Show("Data Saved Successfully");
                    }
                }
                prcClearData();
                txtGroupName.Focus();

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
            if (MessageBox.Show("Do you want to delete user group information of [" + txtGroupName.Text + "]", "", System.Windows.Forms.MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.No)
            {
                return;
            }

            GTRLibrary.clsConnection clsCon = new GTRLibrary.clsConnection();
            try
            {
                int Result = 0;
                string sqlQuery = "";
                sqlQuery = "Delete from tblLogin_Group_Sub Where LSubGroupId = " + Int32.Parse(txtGroupId.Text);
                Result = clsCon.GTRSaveDataWithSQLCommand(sqlQuery);
                if (Result > 0)
                {
                    prcClearData();
                    txtGroupName.Focus();

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
            if (this.txtGroupName.Text.Length == 0)
            {
                MessageBox.Show("Please provide group name.");
                txtGroupName.Focus();
                return true;
            }
            return false;
        }
    }
}
