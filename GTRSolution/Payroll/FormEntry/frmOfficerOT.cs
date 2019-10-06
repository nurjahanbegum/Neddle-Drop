using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Infragistics.Win;
using Infragistics.Win.UltraWinEditors;
using Infragistics.Win.UltraWinGrid;
using GTRLibrary;

namespace GTRHRIS.Payroll.FormEntry
{
    public partial class frmOfficerOT : Form
    {
        System.Data.DataSet dsList;
        System.Data.DataSet dsDetails;
        clsProcedure clsProc = new clsProcedure();

        string ReportPath = "", rptQuery = "", DataSourceName = "DataSet1", FormCaption = "";

        private Infragistics.Win.UltraWinTabControl.UltraTabControl uTab;
        Common.FormEntry.frmMaster FM;
        public frmOfficerOT(ref Infragistics.Win.UltraWinTabControl.UltraTabControl utab, Common.FormEntry.frmMaster fm)
        {
            InitializeComponent();
            uTab = utab;
            FM = fm;
        }

        private void prcLoadList()
        {
            clsConnection clscon = new clsConnection();
            dsList = new System.Data.DataSet();

            try
            {
                String sqlquary = "Exec [rptOTOfficer] " + Common.Classes.clsMain.intComId + ", '0',0, 0,0";
                clscon.GTRFillDatasetWithSQLCommand(ref dsList, sqlquary);
               
                dsList.Tables[0].TableName = "ProssType";
                dsList.Tables[1].TableName = "tblSect";
                dsList.Tables[2].TableName = "tblEmp";

                gridProssType.DataSource = null;
                gridProssType.DataSource = dsList.Tables["ProssType"];

                gridSec.DataSource = null;
                gridSec.DataSource = dsList.Tables["tblSect"];

                gridEmp.DataSource = null;
                gridEmp.DataSource = dsList.Tables["tblEmp"];

            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        private  void prcLoadCombo()
        {
            
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }       



        private void frmOfficerOT_FormClosing(object sender, FormClosingEventArgs e)
        {
            int index = Common.Classes.clsMain.fncFindOpenFormIndex(this);
            uTab.Tabs.RemoveAt(index);
            Common.Classes.clsMain.fncExistOpenForm(this, "Remove");

            clsProc = null;
            FM = null;
        }

        private void frmOfficerOT_Load(object sender, EventArgs e)
        {
            try
            {
                prcLoadList();
                prcLoadCombo();
                prcClearData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void prcClearData()
        {

            dtFirst.Value = DateTime.Now;
            dtLast.Value = DateTime.Now;
            dtPayment.Value = DateTime.Now;



            if (dtFirst.DateTime.Month == 1)
            {
                if (dtFirst.DateTime.Day <= 6)
                {
                    var firstDay = new DateTime(dtFirst.DateTime.Year - 1, dtFirst.DateTime.Month + 11, 1);
                    dtFirst.Value = firstDay;
                    var DaysInMonth = DateTime.DaysInMonth(dtFirst.DateTime.Year, dtFirst.DateTime.Month);
                    var lastDay = new DateTime(dtFirst.DateTime.Year, dtFirst.DateTime.Month, DaysInMonth);
                    dtLast.Value = lastDay;
                }
                else
                {

                    DateTime firstDay = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                    dtFirst.Value = firstDay;

                    DateTime lastDay = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                    lastDay = lastDay.AddMonths(1);
                    lastDay = lastDay.AddDays(-(lastDay.Day));
                    dtLast.Value = lastDay;
                }
            }

            else
            {

                if (dtFirst.DateTime.Day <= 6)
                {
                    var DaysInMonth = DateTime.DaysInMonth(dtLast.DateTime.Year, dtLast.DateTime.Month - 1);
                    var lastDay = new DateTime(dtLast.DateTime.Year, dtLast.DateTime.Month - 1, DaysInMonth);
                    var firstDay = new DateTime(dtFirst.DateTime.Year, dtFirst.DateTime.Month - 1, 1);
                    dtFirst.Value = firstDay;
                    dtLast.Value = lastDay;
                }

                else
                {
                    DateTime firstDay = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                    dtFirst.Value = firstDay;

                    DateTime lastDay = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                    lastDay = lastDay.AddMonths(1);
                    lastDay = lastDay.AddDays(-(lastDay.Day));
                    dtLast.Value = lastDay;
                }

            }


        }

        private void gridProssType_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            gridProssType.DisplayLayout.Bands[0].Columns["Month"].Hidden = true;
            gridProssType.DisplayLayout.Bands[0].Columns["year"].Hidden = true;
            gridProssType.DisplayLayout.Bands[0].Columns["date"].Hidden = true;
            gridProssType.DisplayLayout.Bands[0].Columns["date"].Hidden = true;

            gridProssType.DisplayLayout.Bands[0].Columns["ProssType"].Width = 285;
            gridProssType.DisplayLayout.Bands[0].Columns["ProssType"].Header.Caption = "Process Type";

            //Change alternate color
            gridProssType.DisplayLayout.Override.RowAlternateAppearance.BackColor = Color.Cyan;
            gridProssType.DisplayLayout.Override.RowAlternateAppearance.ForeColor = Color.DarkBlue;

            //Select Full Row when click on any cell
            e.Layout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;

            //Selection Style Will Be Row Selector
            gridProssType.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;

            //Stop Updating
            gridProssType.DisplayLayout.Override.AllowUpdate = DefaultableBoolean.False;

            //Hiding +/- Indicator
            gridProssType.DisplayLayout.Override.ExpansionIndicator = ShowExpansionIndicator.Never;

            //Hide Group Box Display
            e.Layout.GroupByBox.Hidden = true;

            //Use Filtering
            //gridProssType.DisplayLayout.Override.AllowRowFiltering = DefaultableBoolean.False;
            e.Layout.Override.FilterUIType = FilterUIType.FilterRow;
        }

        private void gridSec_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            gridSec.DisplayLayout.Bands[0].Columns["SectId"].Hidden = true;
            gridSec.DisplayLayout.Bands[0].Columns["SectName"].Width = 170;
            gridSec.DisplayLayout.Bands[0].Columns["SectName"].Header.Caption = "Section";

            //Change alternate color
            gridSec.DisplayLayout.Override.RowAlternateAppearance.BackColor = Color.Cyan;
            gridSec.DisplayLayout.Override.RowAlternateAppearance.ForeColor = Color.DarkBlue;

            //Select Full Row when click on any cell
            e.Layout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;

            //Selection Style Will Be Row Selector
            gridSec.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;

            //Stop Updating
            gridSec.DisplayLayout.Override.AllowUpdate = DefaultableBoolean.False;

            //Hiding +/- Indicator
            gridSec.DisplayLayout.Override.ExpansionIndicator = ShowExpansionIndicator.Never;

            //Hide Group Box Display
            e.Layout.GroupByBox.Hidden = true;

            //Use Filtering
            //gridSec.DisplayLayout.Override.AllowRowFiltering = DefaultableBoolean.False;
            e.Layout.Override.FilterUIType = FilterUIType.FilterRow;
        }

        private void gridBand_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            gridEmp.DisplayLayout.Bands[0].Columns["EmpId"].Hidden = true;
            gridEmp.DisplayLayout.Bands[0].Columns["EmpCode"].Width = 80;
            gridEmp.DisplayLayout.Bands[0].Columns["EmpName"].Width = 165;
            gridEmp.DisplayLayout.Bands[0].Columns["EmpCode"].Header.Caption = "EmpId";
            gridEmp.DisplayLayout.Bands[0].Columns["EmpName"].Header.Caption = "Name";

            //Change alternate color
            gridEmp.DisplayLayout.Override.RowAlternateAppearance.BackColor = Color.Cyan;
            gridEmp.DisplayLayout.Override.RowAlternateAppearance.ForeColor = Color.DarkBlue;

            //Select Full Row when click on any cell
            e.Layout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;

            //Selection Style Will Be Row Selector
            gridEmp.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;

            //Stop Updating
            gridEmp.DisplayLayout.Override.AllowUpdate = DefaultableBoolean.False;

            //Hiding +/- Indicator
            gridEmp.DisplayLayout.Override.ExpansionIndicator = ShowExpansionIndicator.Never;

            //Hide Group Box Display
            e.Layout.GroupByBox.Hidden = true;

            //Use Filtering
            //gridSec.DisplayLayout.Override.AllowRowFiltering = DefaultableBoolean.False;
            e.Layout.Override.FilterUIType = FilterUIType.FilterRow;
        }
        
        

        private void btnCancel_Click(object sender, EventArgs e)
        {
            
            prcLoadList();

        }

        private void btnProcess_Click(object sender, EventArgs e)
        {
            string Description = "";

            System.Globalization.DateTimeFormatInfo mfi = new System.Globalization.DateTimeFormatInfo();
            string strMonthName = mfi.GetMonthName(dtLast.DateTime.Month).ToString();


            Description = strMonthName + "-" + (dtLast.DateTime.Year);
            btnProcess.Text = "Please Wait";

            ArrayList arQuery = new ArrayList();
            GTRLibrary.clsConnection clsCon = new GTRLibrary.clsConnection();
            dsList = new System.Data.DataSet();

            string sqlQuery1 = "";
            Int64 ChkLock = 0;


            //sqlQuery1 = "Select dbo.fncProcessLock (" + Common.Classes.clsMain.intComId + ", 'PF Process','" + clsProc.GTRDate(dtFirst.Value.ToString()) + "')";
            //ChkLock = clsCon.GTRCountingDataLarge(sqlQuery1);


            //if (ChkLock == 1)
            //{
            //    MessageBox.Show("Process Lock. Please communicate with Administrator.");
            //    return;
            //}

            try
            {

                string sqlQuery = "Exec prcProcessOTOfficer " + Common.Classes.clsMain.intComId + ",'" + clsProc.GTRDate(dtFirst.Value.ToString()) + "','" + clsProc.GTRDate(dtLast.Value.ToString()) + "','" + clsProc.GTRDate(dtPayment.Value.ToString()) + "','" + Description + "'";
                int i = clsCon.GTRSaveDataWithSQLCommand(sqlQuery);

                sqlQuery = "Insert Into tblUser_Trans_Log (LUserId, formName, tranStatement, PCName, tranType)"
                           + " Values (" + GTRHRIS.Common.Classes.clsMain.intUserId + ", '" + this.Name.ToString() +
                           "','" + sqlQuery.Replace("'", "|") + "','" + Common.Classes.clsMain.strComputerName + "','Process')";
                arQuery.Add(sqlQuery);
                clsCon.GTRSaveDataWithSQLCommand(arQuery);


                MessageBox.Show("Process Complete");
                btnProcess.Text = "Process";

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

        private void btnPreview_Click(object sender, EventArgs e)
        {
            dsDetails = new DataSet();

            ArrayList arQuery = new ArrayList();
            GTRLibrary.clsConnection clsCon = new GTRLibrary.clsConnection();

            try
            {
                string ReportPath = "", SQLQuery = "", DataSourceName = "DataSet1", FormCaption = "", Band = "";

                DataSourceName = "DataSet1";
                FormCaption = "Report :: Extra Payment...";

                ReportPath = Common.Classes.clsMain.AppPath + @"\Payroll\Reports\rptOTOfficer.rdlc";
                SQLQuery = "Exec [rptOTOfficer] " + Common.Classes.clsMain.intComId + ", '" + gridProssType.ActiveRow.Cells[0].Value.ToString() + "','" + gridSec.ActiveRow.Cells[0].Value.ToString() + "','" + gridEmp.ActiveRow.Cells[0].Value.ToString() + "',1";

                clsCon.GTRFillDatasetWithSQLCommand(ref dsDetails, SQLQuery);

                if (dsDetails.Tables[0].Rows.Count == 0)
                {
                    MessageBox.Show("Data Not Found");
                    return;
                }

                clsReport.strReportPathMain = ReportPath;
                clsReport.dsReport = dsDetails;
                clsReport.strDSNMain = DataSourceName;
                Common.Classes.clsMain.strExtension = optFormat.Value.ToString();
                Common.Classes.clsMain.strFormat = optFormat.Text.ToString();
                FM.prcShowReport(FormCaption);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }





      }
  }

