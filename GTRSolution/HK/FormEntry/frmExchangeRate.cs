using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Collections;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using GTRLibrary;
using System.Windows.Forms;

namespace GTRHRIS.HK.FormEntry
{
    public partial class frmExchangeRate : Form
    {
        private System.Data.DataSet dsList;
        private System.Data.DataSet dsDetail;
        private clsProcedure clsProc = new clsProcedure();

        private Infragistics.Win.UltraWinTabControl.UltraTabControl uTab;
        private Common.FormEntry.frmMaster FM;

        public frmExchangeRate(ref Infragistics.Win.UltraWinTabControl.UltraTabControl utab,
                               Common.FormEntry.frmMaster fm)
        {
            InitializeComponent();
            uTab = utab;
            FM = fm;
        }

        private void frmExchangeRate_FormClosing(object sender, FormClosingEventArgs e)
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
                string SqlQuery = "Exec prcGetExchangeRate 0";
                clsCon.GTRFillDatasetWithSQLCommand(ref dsList, SqlQuery);
                dsList.Tables[0].TableName = "ExchangeRate";

                gridList.DataSource = null;
                gridList.DataSource = dsList.Tables["ExchangeRate"];
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

        public void prcDisplayDetails(string strParam)
        {
            clsConnection clsCon = new clsConnection();
            dsDetail = new System.Data.DataSet();
            try
            {
                string SqlQuery = "Exec prcGetExchangeRate " + Int32.Parse(strParam);
                clsCon.GTRFillDatasetWithSQLCommand(ref dsDetail, SqlQuery);
                dsDetail.Tables[0].TableName = "ExchangeRate";
                DataRow dr;

                if (dsDetail.Tables["ExchangeRate"].Rows.Count > 0)
                {
                    dr = dsDetail.Tables["ExchangeRate"].Rows[0];
                    txtId.Text = dr["ExchId"].ToString();
                    dtInputeDate.Text = dr["dtInput"].ToString();
                    txtExchangeRate.Text = dr["ExchRate"].ToString();

                    btnSave.Text = "&Update";
                    btnDelete.Enabled = true;
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

        public void prcLoadCombo()
        {

        }

        public void prcClearData()
        {
            txtId.Text = "";
            txtExchangeRate.Text = "";

            DateTime lastDay = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            lastDay = lastDay.AddMonths(1);
            lastDay = lastDay.AddDays(-(lastDay.Day));
            dtInputeDate.Value = lastDay;

            this.btnSave.Text = "&Save";
            this.btnDelete.Enabled = false;
        }

        public Boolean fncBlank()
        {

            if(txtExchangeRate.Text.ToString().Trim()=="")
            {
                MessageBox.Show("Provide Exchange Rate.");
                return true;
            }
            else if (clsProc.GTRValidateDouble(txtExchangeRate.Text.ToString().Trim())<= 0)
            {
                MessageBox.Show("Exchange Rate Must be grater than Zero.");
                return true;
            }
            return false;
        }

        private void frmExchangeRate_Load(object sender, EventArgs e)
        {
            try
            {
                prcLoadList();
                prcLoadCombo();

                DateTime lastDay = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                lastDay = lastDay.AddMonths(1);
                lastDay = lastDay.AddDays(-(lastDay.Day));
                dtInputeDate.Value = lastDay;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (fncBlank())
            {
                return;
            }
            ArrayList arQuery = new ArrayList();
            clsConnection clsCon = new clsConnection();

            string sqlQuery = "";
            Int32 NewId = 0;
            Int32 MonthId = 0;
            Int32 CountRow = 0;


            DateTime lastDay = new DateTime(dtInputeDate.DateTime.Year, dtInputeDate.DateTime.Month, 1);
            lastDay = lastDay.AddMonths(1);
            lastDay = lastDay.AddDays(-(lastDay.Day));
            dtInputeDate.Value = lastDay;


            try
            {
                if (btnSave.Text.ToString() != "&Save")
                {

                    //Update     
                    sqlQuery = " Update tblCat_ExchangeRate  Set dtInput = '" + clsProc.GTRDate(dtInputeDate.DateTime.ToString())  + "', ExchRate = '" + clsProc.GTRValidateDouble(txtExchangeRate.Text) + "' Where ExchId = '"+ int.Parse(txtId.Text.ToString()) + "' ";
                    arQuery.Add(sqlQuery);

                    // Insert Information To Log File
                    sqlQuery = "Insert Into tblUser_Trans_Log (LUserId, formName, tranStatement, PCName, tranType)"
                               + " Values (" + Common.Classes.clsMain.intUserId + ", '" + this.Name.ToString() + "','" +
                               sqlQuery.Replace("'", "|") + "','" + Common.Classes.clsMain.strComputerName + "','Update')";
                    arQuery.Add(sqlQuery);

                    clsCon.GTRSaveDataWithSQLCommand(arQuery);

                    MessageBox.Show("Data Updated Succefully");

                }
                else
                {
                    sqlQuery = "Select Isnull(Max(ExchId),0)+1 As NewId from tblCat_ExchangeRate";
                    NewId = clsCon.GTRCountingData(sqlQuery);

                        //Insert to Table
                        sqlQuery =
                            " Insert Into tblCat_ExchangeRate (ComId, ExchId, dtInput, ExchRate, aId, PCName, LUserId) " +
                            "Values('" + Common.Classes.clsMain.intComId + "','" + NewId + "','" + clsProc.GTRDate(dtInputeDate.Value.ToString()) + "','" +
                            clsProc.GTRValidateDouble(txtExchangeRate.Text) + "','" + NewId + "','" +
                            Common.Classes.clsMain.strComputerName + "','" + Common.Classes.clsMain.intUserId + "') ";
                        arQuery.Add(sqlQuery);

                        // Insert Information To Log File
                        sqlQuery = "Insert Into tblUser_Trans_Log (LUserId, formName, tranStatement, PCName, tranType)"
                                   + " Values (" + Common.Classes.clsMain.intUserId + ", '" + this.Name.ToString() +
                                   "','" + sqlQuery.Replace("'", "|") + "','" + Common.Classes.clsMain.strComputerName + "','Insert')";
                        arQuery.Add(sqlQuery);

                        clsCon.GTRSaveDataWithSQLCommand(arQuery);

                        MessageBox.Show("Data Saved Succefully");

                }
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

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (
                MessageBox.Show("Do you want to delete Exchange information of [" + dtInputeDate.Value.ToString() + "]", "",
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
                sqlQuery = "Delete From tblCat_ExchangeRate where ExchId =  " + Int32.Parse(txtId.Text);
                arQuery.Add(sqlQuery);

                // Insert Information To Log File
                sqlQuery = "Insert Into tblUser_Trans_Log (LUserId, formName, tranStatement, PCName, tranType)"
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

        private void btnCancel_Click(object sender, EventArgs e)
        {
            prcClearData();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void gridList_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {

            gridList.DisplayLayout.Bands[0].Columns["ExchId"].Hidden = true;
            gridList.DisplayLayout.Bands[0].Columns["dtDate"].Hidden = true;

            gridList.DisplayLayout.Bands[0].Columns["dtInput"].Header.Caption = "Input Date";
            gridList.DisplayLayout.Bands[0].Columns["ExchRate"].Header.Caption = "Exchange Rate";

            gridList.DisplayLayout.Bands[0].Columns["dtInput"].Width  = 170;
            gridList.DisplayLayout.Bands[0].Columns["ExchRate"].Width = 150;


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

        private void gridList_DoubleClick(object sender, EventArgs e)
        {
            prcClearData();
            prcDisplayDetails(gridList.ActiveRow.Cells[0].Value.ToString());
        }

        private void txtId_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }


        private void dtInputeDate_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void txtExchangeRate_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void txtId_KeyPress(object sender, KeyPressEventArgs e)
        {
            clsProc.GTRSingleQuote((Int16)e.KeyChar);
        }

        private void dtInputeDate_KeyPress(object sender, KeyPressEventArgs e)
        {
            clsProc.GTRSingleQuote((Int16)e.KeyChar);
        }

        private void txtExchangeRate_KeyPress(object sender, KeyPressEventArgs e)
        {
            clsProc.GTRSingleQuote((Int16)e.KeyChar);
        }


    }
}
