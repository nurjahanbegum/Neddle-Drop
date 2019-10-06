using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using GTRHRIS.Common.Classes;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;

namespace GTRHRIS.Master
{
    public partial class frmUserPermissionTransfer : Form
    {
        System.Data.DataSet dsList;
        System.Data.DataSet dsDetails;
        GTRLibrary.clsProcedure clsProc = new GTRLibrary.clsProcedure();
        clsMain clsM = new clsMain();

        private Infragistics.Win.UltraWinTabControl.UltraTabControl uTab;
        private Common.FormEntry.frmMaster FM;

        public frmUserPermissionTransfer(ref Infragistics.Win.UltraWinTabControl.UltraTabControl utab, Common.FormEntry.frmMaster fm)
        {
            InitializeComponent();
            uTab = utab;
            FM = fm;
        }

        private void frmUserPermissionTransfer_FormClosing(object sender, FormClosingEventArgs e)
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

        private void frmUserPermissionTransfer_Load(object sender, EventArgs e)
        {
            try
            {
                prcLoadList();
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
                string sqlQuery = "Exec prcGetUserMenuPermission " + Common.Classes.clsMain.intUserId + ", 0,0";
                clsCon.GTRFillDatasetWithSQLCommand(ref dsList, sqlQuery);
                dsList.Tables[0].TableName = "UserList";
                dsList.Tables[1].TableName = "UserListTran";


                prcModifyDataset();

                gridList.DataSource = null;
                gridList.DataSource = dsList.Tables["UserList"];

                prcModifyDataset1();

                gridListTran.DataSource = null;
                gridListTran.DataSource = dsList.Tables["UserListTran"];
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

        private void prcClearData()
        {

            this.btnSave.Text = "&Save";

        }

        private void btnSave_Click(object sender, EventArgs e)
        {

            GTRLibrary.clsConnection clsCon = new GTRLibrary.clsConnection();

            try
            {

                string LUserId = gridList.ActiveRow.Cells["LUserId"].Value.ToString();
                string LUserIdTran = gridListTran.ActiveRow.Cells["LUserId"].Value.ToString();

                string sqlQuery = "Exec prcGetUserMenuPermission " + Common.Classes.clsMain.intUserId + ", " + LUserId + "," + LUserIdTran + "";
                clsCon.GTRFillDatasetWithSQLCommand(ref dsList, sqlQuery);

                // Insert Information To Log File
                string sqlQuery1 = "Insert Into tblUser_Trans_Log (LUserId, formName, tranStatement, PCName, tranType)"
                           + " Values (" + GTRHRIS.Common.Classes.clsMain.intUserId + ", '" + this.Name.ToString() +
                           "','" + sqlQuery.Replace("'", "|") + "','" + Common.Classes.clsMain.strComputerName + "','Save')";
                               
                clsCon.GTRFillDatasetWithSQLCommand(ref dsList, sqlQuery1);

                //// Insert Information To Log File
                //string sqlQuery1 = "Insert Into tblUser_Trans_Log (LUserId, formName, tranStatement, PCName, tranType)"
                //           + " Values (" + GTRHRIS.Common.Classes.clsMain.intUserId + ", '" + this.Name.ToString() +
                //           "','" + sqlQuery.Replace("'", "|") + "','" + Common.Classes.clsMain.strComputerName + "','Save')";
                //arQuery.Add(sqlQuery);

                ////Transaction with database
                //clsCon.GTRSaveDataWithSQLCommand(arQuery);

                //MessageBox.Show("Data Updated Successfully");

                MessageBox.Show("Data Saved Successfully");

                prcClearData();
                prcLoadList();
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


        public void prcModifyDataset()
        {
            for (int i = 0; i <= dsList.Tables[0].Rows.Count - 1; i++)
            {
                dsList.Tables[0].Rows[i]["LUserName"] = clsProc.GTRDecryptWord(dsList.Tables[0].Rows[i]["LUserName"].ToString());
            }
        }

        public void prcModifyDataset1()
        {
            for (int i = 0; i <= dsList.Tables[0].Rows.Count - 1; i++)
            {
                dsList.Tables[1].Rows[i]["LUserName"] = clsProc.GTRDecryptWord(dsList.Tables[1].Rows[i]["LUserName"].ToString());
            }
        }

        private void gridList_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            try
            {
                //Setup Grid
                gridList.DisplayLayout.Bands[0].Columns[0].Hidden = true;//User Id
                gridList.DisplayLayout.Bands[0].Columns[1].Width = 200;  //User Name
                gridList.DisplayLayout.Bands[0].Columns[2].Hidden = true;  //User Password
                gridList.DisplayLayout.Bands[0].Columns[3].Hidden = true;//Group Id
                gridList.DisplayLayout.Bands[0].Columns[4].Width = 140;  //Group Name


                gridList.DisplayLayout.Bands[0].Columns[0].Header.Caption = "User Id";
                gridList.DisplayLayout.Bands[0].Columns[1].Header.Caption = "User Name";
                gridList.DisplayLayout.Bands[0].Columns[2].Header.Caption = "User Password";
                gridList.DisplayLayout.Bands[0].Columns[3].Header.Caption = "Group Id";
                gridList.DisplayLayout.Bands[0].Columns[4].Header.Caption = "Group Name";


                //Change alternate color
                gridList.DisplayLayout.Override.RowAlternateAppearance.BackColor = Color.Cyan;
                gridList.DisplayLayout.Override.RowAlternateAppearance.ForeColor = Color.DarkBlue;

                //Show Check Box Columns
                //this.gridList.DisplayLayout.Bands[0].Columns[5].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox;

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

        private void gridListTran_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            try
            {
                //Setup Grid
                gridListTran.DisplayLayout.Bands[0].Columns[0].Hidden = true;//User Id
                gridListTran.DisplayLayout.Bands[0].Columns[1].Width = 200;  //User Name
                gridListTran.DisplayLayout.Bands[0].Columns[2].Hidden = true;  //User Password
                gridListTran.DisplayLayout.Bands[0].Columns[3].Hidden = true;//Group Id
                gridListTran.DisplayLayout.Bands[0].Columns[4].Width = 130;  //Group Name


                gridListTran.DisplayLayout.Bands[0].Columns[0].Header.Caption = "User Id";
                gridListTran.DisplayLayout.Bands[0].Columns[1].Header.Caption = "User Name";
                gridListTran.DisplayLayout.Bands[0].Columns[2].Header.Caption = "User Password";
                gridListTran.DisplayLayout.Bands[0].Columns[3].Header.Caption = "Group Id";
                gridListTran.DisplayLayout.Bands[0].Columns[4].Header.Caption = "Group Name";


                //Change alternate color
                gridListTran.DisplayLayout.Override.RowAlternateAppearance.BackColor = Color.Cyan;
                gridListTran.DisplayLayout.Override.RowAlternateAppearance.ForeColor = Color.DarkBlue;

                //Show Check Box Columns
                //this.gridList.DisplayLayout.Bands[0].Columns[5].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox;

                //Select Full Row when click on any cell
                e.Layout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;

                //Selection Style Will Be Row Selector
                this.gridListTran.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;

                //Stop Updating
                this.gridListTran.DisplayLayout.Override.AllowUpdate = DefaultableBoolean.False;

                //Hiding +/- Indicator
                this.gridListTran.DisplayLayout.Override.ExpansionIndicator = ShowExpansionIndicator.Never;

                //Hide Group Box Display
                e.Layout.GroupByBox.Hidden = true;

                //Use Filtering
                this.gridListTran.DisplayLayout.Override.AllowRowFiltering = DefaultableBoolean.True;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }



    }
}
