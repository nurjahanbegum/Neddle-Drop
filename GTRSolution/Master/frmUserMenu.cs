using System;
using System.Data;
using System.Collections;
using System.Drawing;
using System.Windows.Forms;

using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;

namespace GTRHRIS.Master
{
    public partial class frmUserMenu : Form
    {
        System.Data.DataSet dsList;
        System.Data.DataSet dsDetails;
        System.Data.DataSet dsCombo;

        GTRLibrary.clsProcedure clsProc = new GTRLibrary.clsProcedure();
        private Infragistics.Win.UltraWinTabControl.UltraTabControl uTab;
        private Common.FormEntry.frmMaster FM;
        public frmUserMenu(ref Infragistics.Win.UltraWinTabControl.UltraTabControl utab, Common.FormEntry.frmMaster fm)
        {
            InitializeComponent();
            uTab = utab;
            FM = fm;
        }

        private void frmUserMenu_Load(object sender, System.EventArgs e)
        {
            prcLoadList();
            prcLoadCombo("");
        }

        private void frmUserMenu_FormClosing(object sender, FormClosingEventArgs e)
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

        private void prcLoadList()
        {
            GTRLibrary.clsConnection clsCon = new GTRLibrary.clsConnection();
            dsList = new System.Data.DataSet();

            try
            {
                string sqlQuery = "Exec prcGetPermission_Module " + Common.Classes.clsMain.intUserId + ", 0";
                clsCon.GTRFillDatasetWithSQLCommand(ref dsList, sqlQuery);
                dsList.Tables[0].TableName = "UserList";

                prcModifyDataset();
                
                gridList.DataSource = null;
                gridList.DataSource = dsList;
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

        private void prcLoadCombo(string sqlQuery)
        {
            GTRLibrary.clsConnection clsCon = new GTRLibrary.clsConnection();
            dsDetails = new System.Data.DataSet();
            try
            {
                if (sqlQuery.Length == 0)
                {
                    sqlQuery = "Exec prcPermission_MenuUser 1, 0";
                }
                clsCon.GTRFillDatasetWithSQLCommand(ref dsDetails, sqlQuery);
                dsDetails.Tables[0].TableName = "MenuGroup";
                dsDetails.Tables[1].TableName = "MenuItem";

                DataRelation rel = new DataRelation("relMenu", dsDetails.Tables["MenuGroup"].Columns["mMenuGroupId"], dsDetails.Tables["MenuItem"].Columns["mMenuGroupId"]);
                dsDetails.Relations.Add(rel);

                gridTran.DataSource = null;
                gridTran.DataSource = dsDetails;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
            finally
            {
                clsCon = null;
            }
        }

        private void gridList_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            try
            {
                //Setup Grid
                gridList.DisplayLayout.Bands[0].Columns[0].Hidden = true;   //User Id
                gridList.DisplayLayout.Bands[0].Columns[1].Width = 185;      //User Name
                gridList.DisplayLayout.Bands[0].Columns[2].Hidden = true;   //Group Id
                gridList.DisplayLayout.Bands[0].Columns[3].Width = 185;     //Group Name

                gridList.DisplayLayout.Bands[0].Columns[0].Header.Caption = "User Id";
                gridList.DisplayLayout.Bands[0].Columns[1].Header.Caption = "User Name";
                gridList.DisplayLayout.Bands[0].Columns[2].Header.Caption = "Group Id";
                gridList.DisplayLayout.Bands[0].Columns[3].Header.Caption = "Group Name";

                //Change alternate color
                this.gridList.DisplayLayout.Override.RowAlternateAppearance.BackColor = Color.Cyan;
                this.gridList.DisplayLayout.Override.RowAlternateAppearance.ForeColor = Color.DarkBlue;

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
                throw (ex);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            prcClearData();
        }

        private void gridTran_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            //Setup Grid 
            //================= Menu Group Band
            gridTran.DisplayLayout.Bands[0].Columns[0].Width = 70;     //IsAllow
            gridTran.DisplayLayout.Bands[0].Columns[1].Hidden = true;   //Group Id
            gridTran.DisplayLayout.Bands[0].Columns[2].Hidden = true;   //Group Name
            gridTran.DisplayLayout.Bands[0].Columns[3].Width = 300;     //Group Caption

            gridTran.DisplayLayout.Bands[0].Columns[0].Header.Caption = "Allow";
            gridTran.DisplayLayout.Bands[0].Columns[1].Header.Caption = "Group Id";
            gridTran.DisplayLayout.Bands[0].Columns[2].Header.Caption = "Group Name";
            gridTran.DisplayLayout.Bands[0].Columns[3].Header.Caption = "Group ";

            //================= Menu Item Band
            gridTran.DisplayLayout.Bands[1].Columns[0].Width = 70;     //IsAllow
            gridTran.DisplayLayout.Bands[1].Columns[1].Hidden = true;   //Menu Id
            gridTran.DisplayLayout.Bands[1].Columns[2].Hidden = true;   //Menu Name
            gridTran.DisplayLayout.Bands[1].Columns[3].Width = 300;     //Menu Caption
            gridTran.DisplayLayout.Bands[1].Columns[4].Hidden = true;   //Group Id For Relationship With Group
            gridTran.DisplayLayout.Bands[1].Columns[5].Width = 120;     //Serial No
            //gridTran.DisplayLayout.Bands[1].Columns[5].Hidden = true;

            gridTran.DisplayLayout.Bands[1].Columns[0].Header.Caption = "Allow";
            gridTran.DisplayLayout.Bands[1].Columns[1].Header.Caption = "Menu Id";
            gridTran.DisplayLayout.Bands[1].Columns[2].Header.Caption = "Menu Name";
            gridTran.DisplayLayout.Bands[1].Columns[3].Header.Caption = "Menu ";
            gridTran.DisplayLayout.Bands[1].Columns[4].Header.Caption = "Group Id";
            gridTran.DisplayLayout.Bands[1].Columns[5].Header.Caption = "Sort No";

            //Show Check Box Columns
            this.gridTran.DisplayLayout.Bands[0].Columns[0].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox;
            this.gridTran.DisplayLayout.Bands[1].Columns[0].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox;
            this.gridTran.DisplayLayout.Bands[1].Columns[5].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.IntegerPositiveWithSpin;

            //Lock Specific Cell for no activation
            this.gridTran.DisplayLayout.Bands[0].Columns[3].CellActivation = Activation.NoEdit;
            this.gridTran.DisplayLayout.Bands[1].Columns[3].CellActivation = Activation.NoEdit;

            //Infragistics.Win.UltraWinGrid.ColumnStyle.DoublePositiveWithSpin
            //Change alternate color
            this.gridTran.DisplayLayout.Override.RowAlternateAppearance.BackColor = Color.Cyan;
            this.gridTran.DisplayLayout.Override.RowAlternateAppearance.ForeColor = Color.DarkBlue;

            //Selection Style Will Be Row Selector
            this.gridTran.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;

            //Hide Group Box Display
            e.Layout.GroupByBox.Hidden = true;

            //Use Filtering
            //this.gridTran.DisplayLayout.Override.AllowRowFiltering = DefaultableBoolean.True;
        }

        private void gridList_DoubleClick(object sender, EventArgs e)
        {
            prcClearData();

            txtUserId.Text = gridList.ActiveRow.Cells[0].Value.ToString();
            string sqlQuery = "Exec prcGetPermission_Module 0 , "+ Int32.Parse(this.txtUserId.Text.ToString()) +"";

            GTRLibrary.clsConnection clsCon = new GTRLibrary.clsConnection();
            dsCombo = new System.Data.DataSet();
            try
            {
                clsCon.GTRFillDatasetWithSQLCommand(ref dsCombo, sqlQuery);
                dsDetails.Tables[0].TableName = "ModuleUser";

                cboModule.DataSource = null;
                cboModule.DataSource = dsCombo;
                cboModule.DisplayMember = "moduleCaption";
                cboModule.ValueMember = "moduleId";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
            finally
            {
                clsCon = null;
            }
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
                //To Delete Existing Data
                sqlQuery = " Delete from tblUser_Menu Where LUserId = " + Int32.Parse(txtUserId.Text) + " and menuId In (Select menuId from viewUser_Menu Where ModuleId = " + Int16.Parse(cboModule.Value.ToString()) + " and LUserId = " + Int32.Parse(txtUserId.Text) + ")";
                arQuery.Add(sqlQuery);

                //To Insert Data With New Value
                foreach (UltraGridRow row in this.gridTran.Rows)
                {
                    if (Int16.Parse(row.Cells["IsAllow"].Text.ToString()) != 0)
                    {
                        // Get the child rows for each of the parent rows and set the checked state
                        foreach (UltraGridRow childRow in row.ChildBands[0].Rows)
                        {
                            if (Int16.Parse(childRow.Cells["IsAllow"].Text.ToString()) != 0)
                            {
                                sqlQuery = " Insert Into tblUser_Menu (LUserId, menuId, SortNo) ";
                                sqlQuery += " Values (" + Int32.Parse(txtUserId.Text) + ", " + Int32.Parse(childRow.Cells["menuId"].Text.ToString()) + ", " + Int32.Parse(childRow.Cells["aId"].Text.ToString()) + ")";
                                arQuery.Add(sqlQuery);
                            }
                        }
                    }
                }

                clsCon.GTRSaveDataWithSQLCommand(arQuery);
                MessageBox.Show("Data Updated Successfully");

                prcClearData();

                prcLoadList();
                prcLoadCombo("");
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

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you want to delete menu permission information for user : [" + txtUserId.Text +"]", "", System.Windows.Forms.MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.No)
            {
                return;
            }

            GTRLibrary.clsConnection clsCon = new GTRLibrary.clsConnection();
            try
            {
                int Result = 0;
                string sqlQuery = "";
                sqlQuery = "Delete from tblUser_Menu Where LUserId = " + Int32.Parse(txtUserId.Text);
                Result = clsCon.GTRSaveDataWithSQLCommand(sqlQuery);
                if (Result > 0)
                {
                    prcClearData();
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

        private void prcClearData()
        {
            //txtUserId.Text = "0";
            cboModule.Text = "";
            prcLoadCombo("");
        }

        private Boolean fncBlank()
        {
            if (txtUserId.ToString().Length == 0)
            {
                MessageBox.Show("Please select user name.");
                gridList.Focus();
                return true;
            }
            if (this.gridTran.Rows.Count == 0)
            {
                MessageBox.Show("Data not found.");
                this.gridTran.Focus();
                return true;
            }
            return false;
        }

        private void cboModule_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            //Module Name Combo
            cboModule.DisplayLayout.Bands[0].Columns[0].Hidden = true;
            cboModule.DisplayLayout.Bands[0].Columns[1].Hidden = true;
            cboModule.DisplayLayout.Bands[0].Columns[2].Header.Caption = "Module";
            cboModule.DisplayLayout.Bands[0].Columns[2].Width = cboModule.Width;
        }

        private void btnProceed_Click(object sender, EventArgs e)
        {
            if (cboModule.Text.Length == 0)
            {
                MessageBox.Show("Please provide module name.");
                cboModule.Focus();
                return;
            }
            string sqlQuery = "Exec prcPermission_MenuUser "+ Int32.Parse(cboModule.Value.ToString()) +", "+ Int32.Parse(this.txtUserId.Text.ToString()) +"";
            prcLoadCombo(sqlQuery);
        }

        private void gridTran_KeyPress(object sender, KeyPressEventArgs e)
        {

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