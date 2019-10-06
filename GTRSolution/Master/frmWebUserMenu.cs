using System;
using System.Data;
using System.Collections;
using System.Drawing;
using System.Windows.Forms;

using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using GTRHRIS.Common.Classes;

namespace GTRHRIS.Master
{
    public partial class frmWebUserMenu : Form
    {

        System.Data.DataSet dsList;
        System.Data.DataSet dsDetails;
        //private System.Data.DataView dvgrid;
        System.Data.DataSet dsCombo;
        GTRLibrary.clsProcedure clsProc = new GTRLibrary.clsProcedure();
        clsMain clsM = new clsMain();

        private Infragistics.Win.UltraWinTabControl.UltraTabControl uTab;
        private Common.FormEntry.frmMaster FM;

        public frmWebUserMenu(ref Infragistics.Win.UltraWinTabControl.UltraTabControl utab, Common.FormEntry.frmMaster fm)
        {
            InitializeComponent();
            uTab = utab;
            FM = fm;
        }

        private void frmWebUserMenu_Load(object sender, System.EventArgs e)
        {
            prcLoadList();
            prcLoadCombo("");
        }

        private void frmWebUserMenu_FormClosing(object sender, FormClosingEventArgs e)
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
                string sqlQuery = "Exec WebprcGetPermission_Web_Module " + Common.Classes.clsMain.intUserId + ", 0";
                clsCon.GTRFillDatasetWithSQLCommand(ref dsList, sqlQuery);
                dsList.Tables[0].TableName = "UserList";

                gridList.DataSource = null;
                gridList.DataSource = dsList.Tables[0];
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
                    sqlQuery = "Exec WebprcPermission_MenuUser_Web 0";
                }
                clsCon.GTRFillDatasetWithSQLCommand(ref dsDetails, sqlQuery);
                dsDetails.Tables[0].TableName = "MenuGroup";
                dsDetails.Tables[1].TableName = "MenuItem";

                DataRelation rel = new DataRelation("relMenu", dsDetails.Tables["MenuGroup"].Columns["MenuGroupId"], dsDetails.Tables["MenuItem"].Columns["MenuGroupId"]);
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
                gridList.DisplayLayout.Bands[0].Columns[1].Width = 120;      //User Name
                gridList.DisplayLayout.Bands[0].Columns[2].Width = 120;   //Group Id
                //gridList.DisplayLayout.Bands[0].Columns[3].Width = 185;     //Group Name

                gridList.DisplayLayout.Bands[0].Columns[0].Header.Caption = "User Id";
                gridList.DisplayLayout.Bands[0].Columns[1].Header.Caption = "User Name";
                gridList.DisplayLayout.Bands[0].Columns[2].Header.Caption = "Display Name";
                //gridList.DisplayLayout.Bands[0].Columns[3].Header.Caption = "Group Name";

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
            gridTran.DisplayLayout.Bands[0].Columns["aid"].Hidden = true;     //IsAllow
            //gridTran.DisplayLayout.Bands[0].Columns["isAllow"].Hidden = true;   //Group Id
            gridTran.DisplayLayout.Bands[0].Columns["menuid"].Hidden = true;   //Group Name
            //gridTran.DisplayLayout.Bands[0].Columns["menuname"].Hidden = true;     //Group Caption
            gridTran.DisplayLayout.Bands[0].Columns["menugroupid"].Hidden = true;     //Group Caption
            gridTran.DisplayLayout.Bands[0].Columns["sortNo"].Hidden = true;     //Group Caption


            gridTran.DisplayLayout.Bands[0].Columns["isAllow"].Header.Caption = "Allow";
            gridTran.DisplayLayout.Bands[0].Columns["menuname"].Header.Caption = "Group Name";


            //================= Menu Item Band
            //aid	isAllow	menuid	menuName	menugroupid	sortNo
            gridTran.DisplayLayout.Bands[1].Columns["isAllow"].Width = 70;     //IsAllow
            gridTran.DisplayLayout.Bands[1].Columns["aid"].Hidden = true;   //Menu Id
            gridTran.DisplayLayout.Bands[1].Columns["menuid"].Hidden = true;   //Menu Name
            gridTran.DisplayLayout.Bands[1].Columns["menuName"].Width = 300;     //Menu Caption
            gridTran.DisplayLayout.Bands[1].Columns["menugroupid"].Hidden = true;   //Group Id For Relationship With Group
            gridTran.DisplayLayout.Bands[1].Columns["sortNo"].Width = 120;     //Serial No
            
            gridTran.DisplayLayout.Bands[1].Columns["isAllow"].Header.Caption = "Allow";
            gridTran.DisplayLayout.Bands[1].Columns["menuid"].Header.Caption = "Menu Id";
            gridTran.DisplayLayout.Bands[1].Columns["menuName"].Header.Caption = "Menu Name";
            gridTran.DisplayLayout.Bands[1].Columns["menugroupid"].Header.Caption = "Group Id";
            gridTran.DisplayLayout.Bands[1].Columns["sortNo"].Header.Caption = "Sort No";

            //Show Check Box Columns
            this.gridTran.DisplayLayout.Bands[0].Columns["isAllow"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox;
            this.gridTran.DisplayLayout.Bands[1].Columns["isAllow"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox;
            this.gridTran.DisplayLayout.Bands[1].Columns["SortNo"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.IntegerPositiveWithSpin;

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

            //txtUserId.Text = gridList.ActiveRow.Cells[0].Value.ToString();
            string sqlQuery = "Exec WebprcPermission_MenuUser_Web " + gridList.ActiveRow.Cells[0].Text.ToString() + "";


            //DataRelation rel = new DataRelation("relMenu", dsDetails.Tables["MenuGroup"].Columns["MenuGroupId"], dsDetails.Tables["MenuItem"].Columns["MenuGroupId"]);
            //dsDetails.Relations.Add(rel);

            //gridTran.DataSource = null;
            //gridTran.DataSource = dsDetails;

            GTRLibrary.clsConnection clsCon = new GTRLibrary.clsConnection();
            dsCombo = new System.Data.DataSet();


            try
            {
                clsCon.GTRFillDatasetWithSQLCommand(ref dsCombo, sqlQuery);
                dsCombo.Tables[0].TableName = "MenuGroup";
                dsCombo.Tables[1].TableName = "MenuItem";

                DataRelation rel = new DataRelation("relMenu", dsCombo.Tables["MenuGroup"].Columns["MenuGroupId"], dsCombo.Tables["MenuItem"].Columns["MenuGroupId"]);
                dsCombo.Relations.Add(rel);

                gridTran.DataSource = null;
                gridTran.DataSource = dsCombo;

                //cboModule.DataSource = null;
                //cboModule.DataSource = dsCombo;
                //cboModule.DisplayMember = "moduleCaption";
                //cboModule.ValueMember = "moduleId";
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
            dsList = new System.Data.DataSet();

            string sqlQuery = "";
            Int32 NewId = 0;

            try
            {
                //To Delete Existing Data
                sqlQuery = " Delete from tblWeb_User_Menu Where UserId = " + gridList.ActiveRow.Cells["userid"].Text.ToString() + "";
                arQuery.Add(sqlQuery);

                //To Insert Data With New Value
                foreach (UltraGridRow row in this.gridTran.Rows)
                {
                    if (Int16.Parse(row.Cells["IsAllow"].Text.ToString()) != 0)
                    {
                        sqlQuery = " Insert Into tblWeb_User_Menu (UserId, menuId, SortNo) ";
                        sqlQuery += " Values ( " + gridList.ActiveRow.Cells["userid"].Text.ToString() + ", " + Int32.Parse(row.Cells["menuId"].Text.ToString()) + ", " + Int32.Parse(row.Cells["aId"].Text.ToString()) + ")";
                        arQuery.Add(sqlQuery);

                        // Get the child rows for each of the parent rows and set the checked state
                        foreach (UltraGridRow childRow in row.ChildBands[0].Rows)
                        {
                           
                            if (Int16.Parse(childRow.Cells["IsAllow"].Text.ToString()) != 0)
                            {
                                sqlQuery = " Insert Into tblWeb_User_Menu (UserId, menuId, SortNo) ";
                                sqlQuery += " Values ( " + gridList.ActiveRow.Cells["userid"].Text.ToString() + ", " + Int32.Parse(childRow.Cells["menuId"].Text.ToString()) + ", " + Int32.Parse(childRow.Cells["aId"].Text.ToString()) + ")";
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
            dsList = new System.Data.DataSet();

            try
            {
                int Result = 0;
                string sqlQuery = "";
                sqlQuery = "Delete from tblWeb_User_Menu Where LUserId = " + Int32.Parse(txtUserId.Text);
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
    }
}