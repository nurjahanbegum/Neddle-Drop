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
using GTRLibrary;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;

namespace GTRHRIS.Admin.FormEntry
{
    public partial class frmProssType : Form
    {
        System.Data.DataSet dsList;
        System.Data.DataSet dsDetails;

        clsMain clsM = new clsMain();
        clsProcedure clsProc = new clsProcedure();

        private Infragistics.Win.UltraWinTabControl.UltraTabControl uTab;
        GTRHRIS.Common.FormEntry.frmMaster FM;
        public frmProssType(ref Infragistics.Win.UltraWinTabControl.UltraTabControl utab,Common.FormEntry.frmMaster fm)
        {
            InitializeComponent();
            uTab = utab;
            FM = fm;
        }

        private void prcLoadList()
        {
            GTRLibrary.clsConnection clsCon = new GTRLibrary.clsConnection();
            dsList = new System.Data.DataSet();

            try
            {
                string sqlQuery = "Exec [prcProssType]  " + Common.Classes.clsMain.intComId + ",0";
                clsCon.GTRFillDatasetWithSQLCommand(ref dsList, sqlQuery);
                dsList.Tables[0].TableName = "type";
                dsList.Tables[1].TableName = "adminType";
                dsList.Tables[2].TableName = "ComType";


                gridList.DataSource = null;
                gridList.DataSource = dsList.Tables["type"];
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
                cboType.DataSource = null;
                cboType.DataSource = dsList.Tables["adminType"];

                cboBuyer.DataSource = null;
                cboBuyer.DataSource = dsList.Tables["ComType"];
            }

        private void frmProssType_Load(object sender, EventArgs e)
        {

            try
            {
                prcLoadList();
                prcLoadCombo();
                dtProcess.Value = DateTime.Today;
            }
            catch (Exception ex)
            {
                throw (ex);
            }

        }

        private void frmProssType_FormClosing(object sender, FormClosingEventArgs e)
        {
            int index = Common.Classes.clsMain.fncFindOpenFormIndex(this);
            uTab.Tabs.RemoveAt(index);
            Common.Classes.clsMain.fncExistOpenForm(this, "Remove");

          
            dsList = null;
            clsProc = null;
        }

        private void dtProcess_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void prcDisplayDetails(string strParam)
        {
            GTRLibrary.clsConnection clsCon = new GTRLibrary.clsConnection();
            dsDetails = new System.Data.DataSet();

            try
            {
                string sqlQuery = "Exec prcProssType  " + Common.Classes.clsMain.intComId + "," + Int32.Parse(strParam) + " ";
                clsCon.GTRFillDatasetWithSQLCommand(ref dsDetails, sqlQuery);
                dsDetails.Tables[0].TableName = "Details";

                DataRow dr;
                if (dsDetails.Tables["Details"].Rows.Count > 0)
                {
                    dr = dsDetails.Tables["Details"].Rows[0];

                    this.dtProcess.Value = dr["ProssDt"].ToString();
                    this.cboType.Text = dr["DaySts"].ToString();
                    this.cboBuyer.Text = dr["DayStsB"].ToString();



                    this.btnSave.Text = "&Update";
                    this.btnDelete.Enabled = true;
                    //this.cboEmpID.Enabled = false;
                }
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

        private void gridList_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            try
            {
                gridList.DisplayLayout.Bands[0].Columns["aId"].Hidden = true;
                //Width
                gridList.DisplayLayout.Bands[0].Columns["ProssDt"].Width = 85;
                gridList.DisplayLayout.Bands[0].Columns["DaySts"].Width = 85;
                gridList.DisplayLayout.Bands[0].Columns["DayStsB"].Width = 90;
                //Caption
                gridList.DisplayLayout.Bands[0].Columns["ProssDt"].Header.Caption = "Process Date";
                gridList.DisplayLayout.Bands[0].Columns["DaySts"].Header.Caption = "Prcess Type";
                gridList.DisplayLayout.Bands[0].Columns["DayStsB"].Header.Caption = "Process Type Buyer";

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

                //Using Filter
                e.Layout.Override.FilterUIType = FilterUIType.FilterRow;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void prcClearData()
        {
            dtProcess.Value = "";
            cboType.Text = "";
            cboBuyer.Text = "";

            dtProcess.Value = DateTime.Today;

            btnSave.Text = "&Save";
            btnDelete.Enabled = false;
            dtProcess.Focus();
        }

       
        private void cboType_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            //Hidden
            //cboType.DisplayLayout.Bands[0].Columns["aId"].Hidden = true;
            //cboType.DisplayLayout.Bands[0].Columns["ProssDt"].Hidden = true;
            //cboType.DisplayLayout.Bands[0].Columns["DayStsB"].Hidden = true;

            cboType.DisplayLayout.Bands[0].Columns["DaySts"].Width = cboType.Width;
            cboType.DisplayLayout.Bands[0].Columns["DaySts"].Header.Caption = "Process Type";
            cboType.DisplayMember = "DaySts";
            //cboType.ValueMember = "aId";
        }

        private void cboBuyer_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            //cboBuyer.DisplayLayout.Bands[0].Columns["aId"].Hidden = true;
            //cboBuyer.DisplayLayout.Bands[0].Columns["ProssDt"].Hidden = true;
            //cboBuyer.DisplayLayout.Bands[0].Columns["DayStsB"].Hidden = true;

            cboBuyer.DisplayLayout.Bands[0].Columns["DayStsB"].Width = cboBuyer.Width;
            cboBuyer.DisplayLayout.Bands[0].Columns["DayStsB"].Header.Caption = "Process Type";
            cboBuyer.DisplayMember = "DayStsB";
            //cboBuyer.ValueMember = "aId";
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            prcClearData();
        }

        private Boolean fncBlank()
        {
            if (this.cboType.Text.Length == 0)
            {
                MessageBox.Show("Please provide Process Type.");
                cboType.Focus();
                return true;
            }
            return false;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (fncBlank())
            {
                return;
            }

            ArrayList arQuery = new ArrayList();
            GTRLibrary.clsConnection clsCon = new clsConnection();

            string sqlQuery = "";
            Int64 NewId = 0;
            try
            {

                //Member Master Table
                if (btnSave.Text != "&Save")
                {

                    //Update
                    sqlQuery = "Update tblProssType set ProssDt = '" + clsProc.GTRDate(this.dtProcess.Value.ToString()) +
                               "', DaySts = '" + this.cboType.Text.ToString() + "', DayStsB = ' " +
                               this.cboBuyer.Text.ToString() + "'  where ComId = " + Common.Classes.clsMain.intComId + " and ProssDt = '" + clsProc.GTRDate(this.dtProcess.Value.ToString()) + "' ";
                    arQuery.Add(sqlQuery);

                    // Insert Information To Log File
                    sqlQuery = "Insert Into tblUser_Trans_Log (LUserId, formName, tranStatement, tranType)"
                               + " Values (" + GTRHRIS.Common.Classes.clsMain.intUserId + ", '" + this.Name.ToString() +
                               "','" + sqlQuery.Replace("'", "|") + "','Update')";
                    arQuery.Add(sqlQuery);

                    //Transaction with database
                    clsCon.GTRSaveDataWithSQLCommand(arQuery);

                    MessageBox.Show("Data Updated Successfully");
                }
                else
                {
                    //NewId
                    //sqlQuery = "Select Isnull(Max(EmpId),0)+1 As NewId from tblEmp_Info";
                    //NewId = clsCon.GTRCountingDataLarge(sqlQuery);

                    //Insert Data
                    sqlQuery = "Insert Into tblProssType (ComId,ProssDt, DaySts,DayStsB)"
                               + " Values (" + Common.Classes.clsMain.intComId + ",'" + this.dtProcess.Value.ToString() + "', '" + this.cboType.Value.ToString() + "', '" +
                               this.cboBuyer.Value.ToString() + "') ";
                    arQuery.Add(sqlQuery);

                    // Insert Information To Log File
                    sqlQuery = "Insert Into tblUser_Trans_Log (LUserId, formName, tranStatement, tranType)"
                               + " Values (" + GTRHRIS.Common.Classes.clsMain.intUserId + ", '" + this.Name.ToString() +
                               "','" + sqlQuery.Replace("'", "|") + "','Insert')";
                    arQuery.Add(sqlQuery);

                    //Transaction with database
                    clsCon.GTRSaveDataWithSQLCommand(arQuery);

                    MessageBox.Show("Data Saved Successfully");
                }

                prcLoadList();
                prcLoadCombo();
                prcClearData();
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
            if (
               MessageBox.Show("Do you want to Delete Employee information of [" + dtProcess.Text.ToString() + "]", "",
                               System.Windows.Forms.MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.No)
            {
                return;
            }

            ArrayList arQuery = new ArrayList();
            GTRLibrary.clsConnection clsCon = new clsConnection();
            try
            {
                string sqlQuery = "";
                //Delete Data
                sqlQuery = "Delete from tblProssType Where ComId = " + Common.Classes.clsMain.intComId + " and ProssDt = '" + clsProc.GTRDate(this.dtProcess.Value.ToString()) + "' ";
                arQuery.Add(sqlQuery);

                // Insert Information To Log File
                sqlQuery = "Insert Into tblUser_Trans_Log (LUserId, formName, tranStatement, tranType)"
                           + " Values (" + GTRHRIS.Common.Classes.clsMain.intUserId + ", '" + this.Name.ToString() +
                           "','" + sqlQuery.Replace("'", "|") + "','Delete')";
                arQuery.Add(sqlQuery);

                //Transaction with database
                clsCon.GTRSaveDataWithSQLCommand(arQuery);

                prcClearData();
                prcLoadList();
                prcLoadList();
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

        private void cboType_KeyDown_1(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void cboBuyer_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void btnSave_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void btnDelete_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void btnCancel_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void btnClose_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void gridList_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                if (gridList.ActiveRow.IsFilterRow == false)
                {
                    prcClearData();
                    prcDisplayDetails(gridList.ActiveRow.Cells["aId"].Value.ToString());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

    }
}
