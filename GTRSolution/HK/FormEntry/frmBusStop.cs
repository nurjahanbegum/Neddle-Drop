using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using GTRLibrary;
using Infragistics.Win.UltraWinGrid;

namespace GTRHRIS.HK.FormEntry
{
    public partial class frmBusStop : Form
    {
        private System.Data.DataSet dsList;
        private System.Data.DataSet dsDetail;
        private clsProcedure clsProc = new clsProcedure();

        private Infragistics.Win.UltraWinTabControl.UltraTabControl uTab;
        private Common.FormEntry.frmMaster FM;


        public frmBusStop(ref Infragistics.Win.UltraWinTabControl.UltraTabControl utab, Common.FormEntry.frmMaster fm)
        {
            InitializeComponent();
            uTab = utab;
            FM = fm;
        }

        private void frmBusStop_FormClosing(object sender, FormClosingEventArgs e)
        {
            int index = Common.Classes.clsMain.fncFindOpenFormIndex(this);
            uTab.Tabs.RemoveAt(index);
            Common.Classes.clsMain.fncExistOpenForm(this, "Remove");

            dsList = null;
            dsDetail = null;
            uTab = null;
            FM = null;
            clsProc = null;
        }

        public void prcLoadList()
        {
            clsConnection clsCon = new clsConnection();
            dsList = new System.Data.DataSet();
            try
            {
                string SqlQuery = "Exec prcGetBusStop  " + Common.Classes.clsMain.intComId + ",0,'" + this.Tag.ToString() + "'";
                clsCon.GTRFillDatasetWithSQLCommand(ref dsList, SqlQuery);
                dsList.Tables[0].TableName = "BusStop";

                gridList.DataSource = null;
                gridList.DataSource = dsList.Tables["BusStop"];
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

        public void prcLoadCombo()
        {
        }

        public void prcDisplayDetails(string strParam)
        {
        }

        public void prcClearData()
        {
            btnSave.Enabled = false;
            btnDelete.Enabled = false;
        }

        private void frmBusStop_Load(object sender, EventArgs e)
        {
            try
            {
                lblCaption.Text = this.Tag + " Entry ...";
                prcLoadList();
                prcLoadCombo();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void gridList_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            //Hide Column
            gridList.DisplayLayout.Bands[0].Columns["BusId"].Hidden = true;
            //gridList.DisplayLayout.Bands[0].Columns["BusStop"].Hidden = true;
            //gridList.DisplayLayout.Bands[0].Columns["Flag"].Hidden = true;

            //Set Caption
            gridList.DisplayLayout.Bands[0].Columns["aId"].Header.Caption = "SL Number";
            gridList.DisplayLayout.Bands[0].Columns["BusStop"].Header.Caption = "Location";
            gridList.DisplayLayout.Bands[0].Columns["Rate"].Header.Caption = "Rate";
            gridList.DisplayLayout.Bands[0].Columns["isInactive"].Header.Caption = "Is Inactive";
            gridList.DisplayLayout.Bands[0].Columns["Remarks"].Header.Caption = "Remarks";

            //Set Width
            gridList.DisplayLayout.Bands[0].Columns["aId"].Width = 100;
            gridList.DisplayLayout.Bands[0].Columns["BusStop"].Width = 150;
            gridList.DisplayLayout.Bands[0].Columns["Rate"].Width = 100;
            gridList.DisplayLayout.Bands[0].Columns["isInactive"].Width = 80;
            gridList.DisplayLayout.Bands[0].Columns["Remarks"].Width = 300;

            //Change alternate color
            gridList.DisplayLayout.Override.RowAlternateAppearance.BackColor = Color.Cyan;
            gridList.DisplayLayout.Override.RowAlternateAppearance.ForeColor = Color.DarkBlue;

            gridList.DisplayLayout.Bands[0].Columns["isInactive"].Style =
               Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox;
            gridList.DisplayLayout.Bands[0].Columns["aId"].Style =
               Infragistics.Win.UltraWinGrid.ColumnStyle.IntegerWithSpin;
            
            //Select Full Row when click on any cell
            //e.Layout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;

            //Selection Style Will Be Row Selector
            //gridList.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;

            //Stop Updating
            //gridList.DisplayLayout.Override.AllowUpdate = DefaultableBoolean.False;

            //Hiding +/- Indicator
            gridList.DisplayLayout.Override.ExpansionIndicator = ShowExpansionIndicator.Never;

            //Hide Group Box Display
            e.Layout.GroupByBox.Hidden = true;

            //Use Filtering
            e.Layout.Override.FilterUIType = FilterUIType.FilterRow;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAddNew_Click(object sender, EventArgs e)
        {
            DataRow dr;
            dr = dsList.Tables["BusStop"].NewRow();

            dsList.Tables["BusStop"].Rows.Add(dr);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            //MessageBox.Show(dsList.Tables["Varible"].Rows.Count.ToString());
            for (int rowCount = dsList.Tables["Varible"].Rows.Count - 1; rowCount >= 0; rowCount--)
            {
                if (dsList.Tables["BusStop"].Rows[rowCount][0].ToString().Trim().Length == 0)
                {
                    dsList.Tables["BusStop"].Rows[rowCount].Delete();
                }
            }
        }

        public Boolean fncBlank(string tbl, int Rowno)
        {
            if (dsList.Tables[tbl].Rows[Rowno][1].ToString().Length == 0)
            {
                MessageBox.Show("Provide " + this.Tag + " Name.");
                return true;
            }

            if (dsList.Tables[tbl].Rows[Rowno][3].ToString().Length == 0)
            {
                MessageBox.Show("Provide SL Number.");
                return true;
            }

            if (dsList.Tables[tbl].Rows[Rowno][4].ToString().Length == 0)
            {
                dsList.Tables[tbl].Rows[Rowno][4] = 0;
            }

            return false;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            ArrayList arQuery = new ArrayList();
            clsConnection clsCon = new clsConnection();

            string sqlQuery = "",sqlQuery1 = "";
            Int32 NewId = 0;
            try
            {
                int rowCount;
                for (rowCount = 0; rowCount < dsList.Tables["BusStop"].Rows.Count; rowCount++)
                {
                    if (fncBlank("BusStop", rowCount))
                    {
                        return;
                    }
                }

                for (rowCount = 0; rowCount < dsList.Tables["BusStop"].Rows.Count; rowCount++)
                {

                    if (dsList.Tables["BusStop"].Rows[rowCount][0].ToString().Trim().Length > 0 &&
                        dsList.Tables["BusStop"].Rows[rowCount]["Flag"].ToString() == "1")
                    {

                        sqlQuery1 = "Update E Set E.BusStop = '" + dsList.Tables["BusStop"].Rows[rowCount][1] + "',E.Trn = '" + dsList.Tables["BusStop"].Rows[rowCount][2] + 
                                    "'  from tblEmp_Info E,tblCat_BusStop S where E.BusStop = S.BusStop and E.ComID = S.ComID and S.ComID = " + Common.Classes.clsMain.intComId + " and BusId = '" + dsList.Tables["BusStop"].Rows[rowCount][0] + "'";
                        arQuery.Add(sqlQuery1);

                        //Update Table
                        sqlQuery = "Update tblCat_BusStop Set BusStop = '" + dsList.Tables["BusStop"].Rows[rowCount][1] +
                                   "', Rate = '" + dsList.Tables["BusStop"].Rows[rowCount][2] + "', aId = '" +
                                   dsList.Tables["BusStop"].Rows[rowCount][3] + "', isInactive = '" +
                                   dsList.Tables["BusStop"].Rows[rowCount][4] + "', Remarks = '" +
                                   dsList.Tables["BusStop"].Rows[rowCount][5] + "' where BusId = '" +
                                   dsList.Tables["BusStop"].Rows[rowCount][0] + "' and ComID = " + Common.Classes.clsMain.intComId + "";
                        arQuery.Add(sqlQuery);

                        // Insert Information To Log File
                        sqlQuery = "Insert Into tblUser_Trans_Log (LUserId, formName, tranStatement,PCName, tranType)"
                                   + " Values (" + Common.Classes.clsMain.intUserId + ", '" + this.Name.ToString() +
                                   "','" + sqlQuery.Replace("'", "|") + "','" + Common.Classes.clsMain.strComputerName + "','Update')";
                        arQuery.Add(sqlQuery);

                    }
                    else if (dsList.Tables["BusStop"].Rows[rowCount][0].ToString().Trim().Length == 0)
                    {
                        // Insert To Table
                        sqlQuery = "Insert into tblCat_BusStop(BusStop, Rate, aId, isInactive, Remarks,ComID) values('" +
                                   dsList.Tables["BusStop"].Rows[rowCount][1] + "','" +
                                   dsList.Tables["BusStop"].Rows[rowCount][2] + "','" +
                                   dsList.Tables["BusStop"].Rows[rowCount][3] + "','" +
                                   dsList.Tables["BusStop"].Rows[rowCount][4] + "','" +
                                   dsList.Tables["BusStop"].Rows[rowCount][5] + "'," + 
                                   Common.Classes.clsMain.intComId + ")";
                        arQuery.Add(sqlQuery);

                        // Insert Information To Log File
                        sqlQuery = "Insert Into tblUser_Trans_Log (LUserId, formName, tranStatement, PCName,tranType)"
                                   + " Values (" + Common.Classes.clsMain.intUserId + ", '" + this.Name.ToString() +
                                   "','" + sqlQuery.Replace("'", "|") + "','" + Common.Classes.clsMain.strComputerName + "','Insert')";
                        arQuery.Add(sqlQuery);
                    }
                }

                clsCon.GTRSaveDataWithSQLCommand(arQuery);

                MessageBox.Show("Data Update Or Saved Successfully");

                prcClearData();
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

        private void gridList_AfterCellUpdate(object sender, CellEventArgs e)
        {
            gridList.ActiveRow.Cells[6].Value = 1;
            btnSave.Enabled = true;
        }

        private void gridList_AfterCellActivate(object sender, EventArgs e)
        {
            btnDelete.Enabled = true;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you want to delete " + this.Tag + " information of [" + gridList.ActiveRow.Cells[1].Text.ToString() + "]", "",
                   System.Windows.Forms.MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.No)
            {
                return;
            }
            ArrayList arQuery = new ArrayList();
            clsConnection clsCon = new clsConnection();
            try
            {
                string sqlQuery = "";

                //Delete Data
                sqlQuery = "Delete From tblCat_BusStop where BusId = " + Int32.Parse(gridList.ActiveRow.Cells[0].Value.ToString()) + " and ComID = " + Common.Classes.clsMain.intComId + "";
                arQuery.Add(sqlQuery);

                // Insert Information To Log File
                sqlQuery = "Insert Into tblUser_Trans_Log (LUserId, formName, tranStatement, PCName,tranType)"
                           + " Values (" + Common.Classes.clsMain.intUserId + ", '" + this.Name.ToString() + "','" +
                           sqlQuery.Replace("'", "|") + "','" + Common.Classes.clsMain.strComputerName + "','Delete')";
                arQuery.Add(sqlQuery);
                clsCon.GTRSaveDataWithSQLCommand(arQuery);

                MessageBox.Show("Data Deleted Successfully.");

                prcClearData();
                prcLoadList();
                // prcLoadCombo();

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

    }
}
