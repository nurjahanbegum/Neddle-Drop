using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;

namespace GTRHRIS.Master
{
    public partial class frmUserCompany : Form
    {
        System.Data.DataSet dsList;
        System.Data.DataSet dsDetails;

        GTRLibrary.clsProcedure clsProc = new GTRLibrary.clsProcedure();
        private Infragistics.Win.UltraWinTabControl.UltraTabControl uTab;
        Common.FormEntry.frmMaster FM;
        public frmUserCompany(ref Infragistics.Win.UltraWinTabControl.UltraTabControl utab, Common.FormEntry.frmMaster fm)
        {
            InitializeComponent();
            uTab = utab;
            FM = fm;
        }

        private void frmUserCompany_Load(object sender, System.EventArgs e)
        {
            prcLoadList();
            prcLoadCombo("");
        }

        private void frmUserCompany_FormClosing(object sender, FormClosingEventArgs e)
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
                string sqlQuery = "Exec prcGetPermission_Company " + Common.Classes.clsMain.intUserId + ", 0";
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
                    sqlQuery = "Exec prcGetPermission_CompanyUser 0, 0";
                }
                clsCon.GTRFillDatasetWithSQLCommand(ref dsDetails, sqlQuery);
                dsDetails.Tables[0].TableName = "CompanyUser";

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
            gridTran.DisplayLayout.Bands[0].Columns[0].Width = 50;      //IsAllow
            gridTran.DisplayLayout.Bands[0].Columns[1].Hidden = true;   //Module Id
            gridTran.DisplayLayout.Bands[0].Columns[2].Width = 250;      //Module Name
            gridTran.DisplayLayout.Bands[0].Columns[3].Width = 50;      //Default
            gridTran.DisplayLayout.Bands[0].Columns[4].Width = 50;      //SortNo
            gridTran.DisplayLayout.Bands[0].Columns[5].Width = 50;      //IsGroup

            gridTran.DisplayLayout.Bands[0].Columns[0].Header.Caption = "Allow";
            gridTran.DisplayLayout.Bands[0].Columns[1].Header.Caption = "Company Id";
            gridTran.DisplayLayout.Bands[0].Columns[2].Header.Caption = "Company Name";
            gridTran.DisplayLayout.Bands[0].Columns[3].Header.Caption = "Default";
            gridTran.DisplayLayout.Bands[0].Columns[4].Header.Caption = "SortNo";
            gridTran.DisplayLayout.Bands[0].Columns[5].Header.Caption = "Is Group";

            //Show Check Box Columns
            this.gridTran.DisplayLayout.Bands[0].Columns[0].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox;
            this.gridTran.DisplayLayout.Bands[0].Columns[3].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox;
            this.gridTran.DisplayLayout.Bands[0].Columns[4].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.IntegerPositiveWithSpin;
            this.gridTran.DisplayLayout.Bands[0].Columns[5].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox;

            //Lock Specific Cell for no activation
            this.gridTran.DisplayLayout.Bands[0].Columns[2].CellActivation=Activation.NoEdit;
            this.gridTran.DisplayLayout.Bands[0].Columns[5].CellActivation = Activation.NoEdit;

            //Infragistics.Win.UltraWinGrid.ColumnStyle.DoublePositiveWithSpin
            //Change alternate color
            this.gridTran.DisplayLayout.Override.RowAlternateAppearance.BackColor = Color.Cyan;
            this.gridTran.DisplayLayout.Override.RowAlternateAppearance.ForeColor = Color.DarkBlue;

            //Select Full Row when click on any cell
            //e.Layout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;

            //Selection Style Will Be Row Selector
            this.gridTran.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;

            //Stop Updating
            //this.gridTran.DisplayLayout.Override.AllowUpdate = DefaultableBoolean.False;

            //Hiding +/- Indicator
            this.gridTran.DisplayLayout.Override.ExpansionIndicator = ShowExpansionIndicator.Never;

            //Hide Group Box Display
            e.Layout.GroupByBox.Hidden = true;

            //Use Filtering
            //this.gridTran.DisplayLayout.Override.AllowRowFiltering = DefaultableBoolean.True;
        }

        private void gridList_DoubleClick(object sender, EventArgs e)
        {
            txtUserId.Text = gridList.ActiveRow.Cells[0].Value.ToString();
            string sqlQuery = "Exec prcGetPermission_CompanyUser " + (Int32.Parse(txtUserId.Text)) + ", 0";

            prcLoadCombo(sqlQuery);
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
                //To Delete Existing Data
                sqlQuery = "Delete from tblUser_Company Where LUserId = " + (Int32.Parse(txtUserId.Text));

                //To Insert Data With New Value
                foreach (Infragistics.Win.UltraWinGrid.UltraGridRow row in this.gridTran.Rows)
                {
                    if (Int16.Parse(row.Cells["IsAllow"].Text.ToString()) != 0)
                    {
                        sqlQuery = sqlQuery + " Insert Into tblUser_Company (LUserId, comId, IsDefault, sortNo) ";
                        sqlQuery = sqlQuery + " Values (" + Int32.Parse(txtUserId.Text) + ", " + Int32.Parse(row.Cells["comId"].Text.ToString()) + ", " + Int32.Parse(row.Cells["isDefault"].Text.ToString()) + ", " + Int32.Parse(row.Cells["sortNo"].Text.ToString()) + ")";
                    }
                }
                NewId = clsCon.GTRSaveDataWithSQLCommand(sqlQuery);
                if (NewId > 0)
                {
                    MessageBox.Show("Data Saved Successfully");
                }

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
                clsCon = null;
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you want to delete company permission information for user : [" + txtUserId.Text +"]", "", System.Windows.Forms.MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.No)
            {
                return;
            }

            GTRLibrary.clsConnection clsCon = new GTRLibrary.clsConnection();
            try
            {
                int Result = 0;
                string sqlQuery = "";
                sqlQuery = "Delete from tblUser_company Where LUserId = " + Int32.Parse(txtUserId.Text);
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
            txtUserId.Text = "0";
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

            //To Insert Data With New Value
            foreach (Infragistics.Win.UltraWinGrid.UltraGridRow row in this.gridTran.Rows)
            {
                if (Int16.Parse(row.Cells["isGroup"].Text.ToString()) == 1)
                {
                    if(Int32.Parse(row.Cells["IsDefault"].Text.ToString())==1)
                    {
                        MessageBox.Show("You cannot select group as your default company.");
                        this.gridList.Focus();
                        return true;
                    }
                }
            }
            return false;
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