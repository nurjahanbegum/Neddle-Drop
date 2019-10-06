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
using Infragistics.Win.UltraWinGrid.ExcelExport;

namespace GTRHRIS.Payroll.FormReport
{
    public partial class frmrptNightAllow : Form
    {
        System.Data.DataSet dsList;
        System.Data.DataSet dsDetails;
        clsProcedure clsProc = new clsProcedure();

        string ReportPath = "", rptQuery = "", DataSourceName = "DataSet1", FormCaption = "";

        private Infragistics.Win.UltraWinTabControl.UltraTabControl uTab;
        Common.FormEntry.frmMaster FM;
        public frmrptNightAllow(ref Infragistics.Win.UltraWinTabControl.UltraTabControl utab, Common.FormEntry.frmMaster fm)
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
                string sqlquary = "Exec rptNightAllowance  " + Common.Classes.clsMain.intComId + ",'',0,'','','',0";
                clscon.GTRFillDatasetWithSQLCommand(ref dsList, sqlquary);
               
                dsList.Tables[0].TableName = "ProssType";
                dsList.Tables[1].TableName = "tblSect";
                dsList.Tables[2].TableName = "tblBand";
                dsList.Tables[3].TableName = "tblGrid";

                gridProssType.DataSource = null;
                gridProssType.DataSource = dsList.Tables["ProssType"];

                gridSec.DataSource = null;
                gridSec.DataSource = dsList.Tables["tblSect"];

                gridBand.DataSource = null;
                gridBand.DataSource = dsList.Tables["tblBand"];

                gridDetails.DataSource = null;
                gridDetails.DataSource = dsList.Tables["tblGrid"];

                DateTime firstDay = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                dtDateFrom.Value = firstDay;

                DateTime lastDay = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                lastDay = lastDay.AddMonths(1);
                lastDay = lastDay.AddDays(-(lastDay.Day));
                dtDateTo.Value = lastDay;
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




        
        private void prcShowReport()
        {
            dsDetails = new DataSet();
            ArrayList arQuery = new ArrayList();
            clsConnection clsCon = new clsConnection();

            try
            {
                DataSourceName = "DataSet1";
                FormCaption = "Report :: Night Allowance...";

                string ProssType = "",SectId = "0", Band = "";

                ProssType = gridProssType.ActiveRow.Cells["ProssType"].Value.ToString();

                SectId = gridSec.ActiveRow.Cells["SectId"].Value.ToString();

                Band = gridBand.ActiveRow.Cells["varName"].Value.ToString();

                rptQuery = "Exec rptNightAllowance " + Common.Classes.clsMain.intComId + ",'" + ProssType + "'," + SectId + ",'" + Band + "','" + clsProc.GTRDate(dtDateFrom.Value.ToString()) + "', '" + clsProc.GTRDate(dtDateTo.Value.ToString()) + "',2";
                clsCon.GTRFillDatasetWithSQLCommand(ref dsDetails,rptQuery);

                //SQLQuery = "Exec rptSalaryCasual 3,'10-Nov-13','16-Nov-2013', 0, 0, 'Casual Worker'";

                if(dsDetails.Tables[0].Rows.Count==0)
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


                //clsReport.strReportPathMain = ReportPath;
                //clsReport.strQueryMain = rptQuery;
                //clsReport.strDSNMain = DataSourceName;
                //clsReport.dsReport = dsDetails;

                //FM.prcShowReport(FormCaption);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            finally
            {
                rptQuery = null;
                DataSourceName = null;
                DataSourceName = null;
                ReportPath = null;
                dsDetails = null;
            }
        }

        private void frmrptNightAllow_FormClosing(object sender, FormClosingEventArgs e)
        {
            int index = Common.Classes.clsMain.fncFindOpenFormIndex(this);
            uTab.Tabs.RemoveAt(index);
            Common.Classes.clsMain.fncExistOpenForm(this, "Remove");

            clsProc = null;
            FM = null;
        }

        private void frmrptNightAllow_Load(object sender, EventArgs e)
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

        private void btnPreview_Click(object sender, EventArgs e)
        {
            ReportPath = Common.Classes.clsMain.AppPath + @"\Payroll\Reports\rptNightAllow.rdlc";
            prcShowReport();
        }

        private void gridProssType_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            gridProssType.DisplayLayout.Bands[0].Columns["Month"].Hidden = true;
            gridProssType.DisplayLayout.Bands[0].Columns["year"].Hidden = true;
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
            gridSec.DisplayLayout.Bands[0].Columns["SectName"].Width = 180;
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
            gridBand.DisplayLayout.Bands[0].Columns["varId"].Hidden = true;
            gridBand.DisplayLayout.Bands[0].Columns["varName"].Width = 125;
            gridBand.DisplayLayout.Bands[0].Columns["varName"].Header.Caption = "Band";

            //Change alternate color
            gridBand.DisplayLayout.Override.RowAlternateAppearance.BackColor = Color.Cyan;
            gridBand.DisplayLayout.Override.RowAlternateAppearance.ForeColor = Color.DarkBlue;

            //Select Full Row when click on any cell
            e.Layout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;

            //Selection Style Will Be Row Selector
            gridBand.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;

            //Stop Updating
            gridBand.DisplayLayout.Override.AllowUpdate = DefaultableBoolean.False;

            //Hiding +/- Indicator
            gridBand.DisplayLayout.Override.ExpansionIndicator = ShowExpansionIndicator.Never;

            //Hide Group Box Display
            e.Layout.GroupByBox.Hidden = true;

            //Use Filtering
            //gridSec.DisplayLayout.Override.AllowRowFiltering = DefaultableBoolean.False;
            e.Layout.Override.FilterUIType = FilterUIType.FilterRow;
        }

        private void btnProcess_Click(object sender, EventArgs e)
        {
            string SelDescription = "";

            System.Globalization.DateTimeFormatInfo mfi = new System.Globalization.DateTimeFormatInfo();
            string strMonthName = mfi.GetMonthName(dtDateTo.DateTime.Month).ToString();


            SelDescription = strMonthName + "-" + (dtDateTo.DateTime.Year);
            btnProcess.Text = "Please Wait";

            ArrayList arQuery = new ArrayList();
            GTRLibrary.clsConnection clsCon = new GTRLibrary.clsConnection();
            dsList = new System.Data.DataSet();

            string sqlQuery1 = "";
            Int64 ChkLock = 0;


            //sqlQuery1 = "Select dbo.fncProcessLock (" + Common.Classes.clsMain.intComId + ", 'Salary Lock','" + clsProc.GTRDate(dtFirst.Value.ToString()) + "')";
            //ChkLock = clsCon.GTRCountingDataLarge(sqlQuery1);


            //if (ChkLock == 1)
            //{
            //    MessageBox.Show("Process Lock. Please communicate to Administrator.");
            //    return;
            //}

            try
            {

                string sqlQuery = "Exec rptNightAllowance " + Common.Classes.clsMain.intComId + ",'" + SelDescription + "',0,'','" + clsProc.GTRDate(dtDateFrom.Value.ToString()) + "','" + clsProc.GTRDate(dtDateTo.Value.ToString()) + "',1";
                arQuery.Add(sqlQuery);
                clsCon.GTRSaveDataWithSQLCommand(arQuery);


                MessageBox.Show("Process Complete");
                btnProcess.Text = "&Process";
                prcLoadList();

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

        private void gridDetails_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            try
            {

                //Hide Column
                gridDetails.DisplayLayout.Bands[0].Columns["EmpID"].Hidden = true;
                gridDetails.DisplayLayout.Bands[0].Columns["SectName"].Hidden = true;
                gridDetails.DisplayLayout.Bands[0].Columns["ProssType"].Hidden = true;
                gridDetails.DisplayLayout.Bands[0].Columns["Absent"].Hidden = true;

                //Set Caption
                gridDetails.DisplayLayout.Bands[0].Columns["isChecked"].Width = 60; //Short Name
                gridDetails.DisplayLayout.Bands[0].Columns["EmpCode"].Header.Caption = "Emp ID";
                gridDetails.DisplayLayout.Bands[0].Columns["EmpName"].Header.Caption = "Employee Name";
                //gridDetails.DisplayLayout.Bands[0].Columns["SectName"].Header.Caption = "Section Name";
                //gridDetails.DisplayLayout.Bands[0].Columns["Absent"].Header.Caption = "Absent";
                gridDetails.DisplayLayout.Bands[0].Columns["NightAbsent"].Header.Caption = "Absent";
                gridDetails.DisplayLayout.Bands[0].Columns["NightPay"].Header.Caption = "NightAmt";
                gridDetails.DisplayLayout.Bands[0].Columns["FoodPay"].Header.Caption = "FoodAmt";
                gridDetails.DisplayLayout.Bands[0].Columns["NetPay"].Header.Caption = "NetAmt";


                //Set Width
                gridDetails.DisplayLayout.Bands[0].Columns["EmpCode"].Width = 60;
                gridDetails.DisplayLayout.Bands[0].Columns["EmpName"].Width = 80;
                gridDetails.DisplayLayout.Bands[0].Columns["NightAbsent"].Width = 50;
                gridDetails.DisplayLayout.Bands[0].Columns["NightPay"].Width = 50;
                gridDetails.DisplayLayout.Bands[0].Columns["FoodPay"].Width = 50;
                gridDetails.DisplayLayout.Bands[0].Columns["NetPay"].Width = 50;


                this.gridDetails.DisplayLayout.Bands[0].Columns["isChecked"].Style =
                   Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox;

                //Stop Cell Modify
                gridDetails.DisplayLayout.Bands[0].Columns["EmpCode"].CellActivation = Activation.NoEdit;
                gridDetails.DisplayLayout.Bands[0].Columns["EmpName"].CellActivation = Activation.NoEdit;
                gridDetails.DisplayLayout.Bands[0].Columns["SectName"].CellActivation = Activation.NoEdit;


                //Change alternate color
                gridDetails.DisplayLayout.Override.RowAlternateAppearance.BackColor = Color.Cyan;
                gridDetails.DisplayLayout.Override.RowAlternateAppearance.ForeColor = Color.DarkBlue;

                //Hiding +/- Indicator
                gridDetails.DisplayLayout.Override.ExpansionIndicator = ShowExpansionIndicator.Never;

                //Hide Group Box Display
                e.Layout.GroupByBox.Hidden = true;

                //Use Filtering
                e.Layout.Override.FilterUIType = FilterUIType.FilterRow;

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {

            clsConnection clsCon = new clsConnection();
            string sqlQuery = "";
            dsList = new DataSet();

            string Band = "";
            string SectId = "0", EmpId = "0",ProssType = "";


            ProssType = gridProssType.ActiveRow.Cells["ProssType"].Value.ToString();

            SectId = gridSec.ActiveRow.Cells["SectId"].Value.ToString();

            Band = gridBand.ActiveRow.Cells["varName"].Value.ToString();


            try
            {
                sqlQuery = "Exec rptNightAllowance " + Common.Classes.clsMain.intComId + ",'" + ProssType + "'," + SectId + ",'" + Band + "','" + clsProc.GTRDate(dtDateFrom.Value.ToString()) + "','" + clsProc.GTRDate(dtDateTo.Value.ToString()) + "',3";
                clsCon.GTRFillDatasetWithSQLCommand(ref dsList, sqlQuery);
                dsList.Tables[0].TableName = "tblGrid";

                gridDetails.DataSource = null;
                gridDetails.DataSource = dsList.Tables["tblGrid"];


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

        private Boolean fncBlank()
        {


            if (dtDateFrom.Text.Length == 0)
            {
                MessageBox.Show("Please provide requisition date.", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                dtDateFrom.Focus();
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
            GTRLibrary.clsConnection clsCon = new GTRLibrary.clsConnection();
            string sqlQuery = "";
            Int32 NewId = 0;
            //string sqlQuery = "";
            Int32 RowID;

            try
            {
                //Member Master Table
                if (btnSave.Text.ToString() == "&Save")
                {

                    foreach (Infragistics.Win.UltraWinGrid.UltraGridRow row in this.gridDetails.Rows)
                    {
                        if (row.Cells["empid"].Text.ToString().Length != 0 &&
                            row.Cells["isChecked"].Value.ToString() == "1")
                        {

                            sqlQuery = " Update tblNightAllow Set NightAbsent = '" + row.Cells["NightAbsent"].Text.ToString() 
                                                             + "',NightPay = '" + row.Cells["NightPay"].Text.ToString() 
                                                             + "',FoodPay = '" + row.Cells["FoodPay"].Text.ToString() 
                                                             + "',NetPay = '" + row.Cells["NetPay"].Text.ToString() 
                                                             + "' Where EmpID = '" + row.Cells["EmpID"].Text.ToString() 
                                                             + "' and ProssType = '" + row.Cells["ProssType"].Text.ToString() + "'";
                            arQuery.Add(sqlQuery);

                        }
                    }

                    // Insert Information To Log File
                    sqlQuery = "Insert Into tblUser_Trans_Log (LUserId, formName, tranStatement,PCName, tranType)"
                               + " Values (" + Common.Classes.clsMain.intUserId + ", '" + this.Name.ToString() + "','" +
                               sqlQuery.Replace("'", "|") + "','" + Common.Classes.clsMain.strComputerName + "','Insert')";
                    arQuery.Add(sqlQuery);

                    //Transaction with database
                    clsCon.GTRSaveDataWithSQLCommand(arQuery);

                    MessageBox.Show("Data Update Succefully.");
                }

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

        private void gridDetails_AfterCellUpdate(object sender, CellEventArgs e)
        {
            if (gridDetails.ActiveRow.IsFilterRow != true)
            {

                DataSet dsChange = new DataSet();
                clsConnection clscon = new clsConnection();
                try
                {


                    Int32 CF = 0;
                    Int32 Night = 0;
                    Int32 Food = 0;
                    foreach (Infragistics.Win.UltraWinGrid.UltraGridRow row in gridDetails.Rows)
                    {

                        if (row.Cells["empid"].Text.ToString().Length != 0 &&
                            row.Cells["NightAbsent"].Value.ToString() != "0")
                        {
                            double NightPay = Math.Round(500 - (33.33 * Convert.ToDouble(row.Cells[7].Value)), 0);
                            double FoodPay = Math.Round(450 - (30 * Convert.ToDouble(row.Cells[7].Value)),0);

                            Night = Convert.ToInt32(NightPay);
                            Food = Convert.ToInt32(FoodPay);


                            CF = (Night % 5);

                            if (CF > 0)
                            {
                                CF = 5 - CF;
                            }
                            else
                            {
                                CF = 0;
                            }


                            Night = Night + CF;

                            row.Cells[8].Value = Night;
                            row.Cells[9].Value = Food;
                            row.Cells[10].Value = Night + Food;

                        }
                    }
                    
                               
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }


        private void GridToToExcel_InitializeColumn(object sender, InitializeColumnEventArgs e)
        {
            try
            {
                if (e.Column.DataType == typeof(System.DateTime?) && e.Column.Format != null)
                {
                    e.ExcelFormatStr = e.Column.Format.Replace("tt", "AM/PM");
                }
                else
                {
                    e.ExcelFormatStr = e.Column.Format;
                }
            }
            catch (Exception ex)
            {
                //ExceptionFramework.ExceptionPolicy.HandleException(ex, "DefaultPolicy");
            }
        }



        private void btnExcel_Click(object sender, EventArgs e)
        {
            clsConnection clscon = new clsConnection();
            dsList = new System.Data.DataSet();

            string ProssType = "", SectId = "0", Band = "";

            ProssType = gridProssType.ActiveRow.Cells["ProssType"].Value.ToString();

            SectId = gridSec.ActiveRow.Cells["SectId"].Value.ToString();

            Band = gridBand.ActiveRow.Cells["varName"].Value.ToString();

            String sqlquary = "Exec rptNightAllowance " + Common.Classes.clsMain.intComId + ",'" + ProssType + "'," + SectId + ",'" + Band + "','" + clsProc.GTRDate(dtDateFrom.Value.ToString()) + "', '" + clsProc.GTRDate(dtDateTo.Value.ToString()) + "',2";
            clscon.GTRFillDatasetWithSQLCommand(ref dsList, sqlquary);

            dsList.Tables[0].TableName = "Night";

            gridExcel.DataSource = null;
            gridExcel.DataSource = dsList.Tables["Night"];

            DialogResult dlgRes =
            MessageBox.Show("Do You Want to Save the Data Sheet");
            if (dlgRes != DialogResult.OK)
            {
                return;
            }

            SaveFileDialog dlgSurveyExcel = new SaveFileDialog();
            dlgSurveyExcel.Filter = "Excel WorkBook (*.xls)|.xls";
            dlgSurveyExcel.FileName = "Night Allowance Report" + "_" + DateTime.Now.ToShortDateString().Replace(@"/", "_");

            dlgSurveyExcel.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            DialogResult dlgResSaveFile = dlgSurveyExcel.ShowDialog();
            if (dlgResSaveFile == DialogResult.Cancel)
            {
                return;
            }

            Cursor.Current = Cursors.WaitCursor;

            Application.DoEvents();
            UltraGridExcelExporter GridToToExcel = new UltraGridExcelExporter();
            GridToToExcel.FileLimitBehaviour = FileLimitBehaviour.TruncateData;
            GridToToExcel.InitializeColumn += new InitializeColumnEventHandler(GridToToExcel_InitializeColumn);
            GridToToExcel.Export(gridExcel, dlgSurveyExcel.FileName);

            MessageBox.Show("Download complete.");
        }


      }
  }

