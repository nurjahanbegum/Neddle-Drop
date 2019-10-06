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

namespace GTRHRIS.Attendence.FormReport
{
    public partial class frmAttenMonthly : Form
    {
        System.Data.DataSet dsList;
        System.Data.DataSet dsDetails;
        clsProcedure clsProc = new clsProcedure();

        string ReportPath = "", rptQuery = "", SQLQuery = "", DataSourceName = "DataSet1", FormCaption = "";

        private Infragistics.Win.UltraWinTabControl.UltraTabControl uTab;
        Common.FormEntry.frmMaster FM;
        public frmAttenMonthly(ref Infragistics.Win.UltraWinTabControl.UltraTabControl utab, Common.FormEntry.frmMaster fm)
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
                string sqlquary = "Exec prcrptAttendMonthly  " + Common.Classes.clsMain.intComId;
                clscon.GTRFillDatasetWithSQLCommand(ref dsList, sqlquary);
               
                dsList.Tables[0].TableName = "Section";
                dsList.Tables[3].TableName = "Employee";
                dsList.Tables[4].TableName = "Band";
                dsList.Tables[5].TableName = "EmpType";
                dsList.Tables[6].TableName = "Report";

                gridSection.DataSource = dsList.Tables["Section"];
                gridEmployeeID.DataSource = dsList.Tables["Employee"];
                gridBand.DataSource = dsList.Tables["Band"];
                gridEmpType.DataSource = dsList.Tables["EmpType"];
                gridRpt.DataSource = dsList.Tables["Report"];

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

        private void gridSection_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            gridSection.DisplayLayout.Bands[0].Columns["SectId"].Hidden = true;
            gridSection.DisplayLayout.Bands[0].Columns["SectName"].Width = 221;
            gridSection.DisplayLayout.Bands[0].Columns["SectName"].Header.Caption = "Section";

            //Change alternate color
            gridSection.DisplayLayout.Override.RowAlternateAppearance.BackColor = Color.Cyan;
            gridSection.DisplayLayout.Override.RowAlternateAppearance.ForeColor = Color.DarkBlue;

            //Select Full Row when click on any cell
            e.Layout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;

            //Selection Style Will Be Row Selector
            gridSection.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;

            //Stop Updating
            gridSection.DisplayLayout.Override.AllowUpdate = DefaultableBoolean.False;

            //Hiding +/- Indicator
            gridSection.DisplayLayout.Override.ExpansionIndicator = ShowExpansionIndicator.Never;

            //Hide Group Box Display
            e.Layout.GroupByBox.Hidden = true;

            //Use Filtering
            //gridSection.DisplayLayout.Override.AllowRowFiltering = DefaultableBoolean.False;

            e.Layout.Override.FilterUIType = FilterUIType.FilterRow;
        }

        private void gridBand_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            gridBand.DisplayLayout.Bands[0].Columns["varID"].Hidden = true;
            gridBand.DisplayLayout.Bands[0].Columns["varName"].Width = 175;
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
            //gridSection.DisplayLayout.Override.AllowRowFiltering = DefaultableBoolean.False;

            e.Layout.Override.FilterUIType = FilterUIType.FilterRow;
        }

        private void gridEmployeeID_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            gridEmployeeID.DisplayLayout.Bands[0].Columns["empId"].Hidden = true;
            gridEmployeeID.DisplayLayout.Bands[0].Columns["isChecked"].Hidden = true;
            gridEmployeeID.DisplayLayout.Bands[0].Columns["empCode"].Width = 95;
            gridEmployeeID.DisplayLayout.Bands[0].Columns["empCode"].Header.Caption = "Employee Code";
            gridEmployeeID.DisplayLayout.Bands[0].Columns["EmpName"].Width = 215;
            gridEmployeeID.DisplayLayout.Bands[0].Columns["EmpName"].Header.Caption = "Employee Name";

            //Change alternate color
            gridEmployeeID.DisplayLayout.Override.RowAlternateAppearance.BackColor = Color.Cyan;
            gridEmployeeID.DisplayLayout.Override.RowAlternateAppearance.ForeColor = Color.DarkBlue;

            //Select Full Row when click on any cell
            e.Layout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;

            //Selection Style Will Be Row Selector
            gridEmployeeID.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;

            //Stop Updating
            gridEmployeeID.DisplayLayout.Override.AllowUpdate = DefaultableBoolean.False;

            //Hiding +/- Indicator
            gridEmployeeID.DisplayLayout.Override.ExpansionIndicator = ShowExpansionIndicator.Never;

            //Hide Group Box Display
            e.Layout.GroupByBox.Hidden = true;

            //Use Filtering
            //gridEmployeeID.DisplayLayout.Override.AllowRowFiltering = DefaultableBoolean.False;

            e.Layout.Override.FilterUIType = FilterUIType.FilterRow;
        }
        
        private void optCriteria_ValueChanged(object sender, EventArgs e)
        {                        
            gridSection.Enabled = false;
            gridEmployeeID.Enabled = false;
            gridBand.Enabled = false;
            gridEmpType.Enabled = true;
            gridRpt.Enabled = true;

            if(optCriteria.Value.ToString() =="All")
            {
                gridSection.Enabled = false;
                gridEmployeeID.Enabled = false;
                gridBand.Enabled = false;
                gridEmpType.Enabled = true;
            }
            else if (optCriteria.Value.ToString() == "Section")
            {
                gridSection.Enabled = true;
                gridEmployeeID.Enabled = false;
                gridBand.Enabled = false;
                gridEmpType.Enabled = true;
            }
            else if (optCriteria.Value.ToString() == "Employee")
            {
                gridEmployeeID.Enabled = true;
                gridSection.Enabled = false;
                gridBand.Enabled = false;
                gridEmpType.Enabled = true;
            }
            else if (optCriteria.Value.ToString() == "Band")
            {
                gridBand.Enabled = true;
                gridSection.Enabled = false;
                gridEmployeeID.Enabled = false;
                gridEmpType.Enabled = true;
            }
        }
        private void frmAttenMonthly_FormClosing(object sender, FormClosingEventArgs e)
        {
            int index = Common.Classes.clsMain.fncFindOpenFormIndex(this);
            uTab.Tabs.RemoveAt(index);
            Common.Classes.clsMain.fncExistOpenForm(this, "Remove");

            clsProc = null;
            FM = null;
        }

        private void frmAttenMonthly_Load(object sender, EventArgs e)
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

            dsDetails = new DataSet();
            ArrayList arQuery = new ArrayList();
            clsConnection clsCon = new clsConnection();

            try
            {
                DataSourceName = "DataSet1";
                FormCaption = "Report :: Monthly Attendance Info...";

                string SectId = "0", EmpId = "0", Band = "=All=", EmpType = "";

                EmpType = gridEmpType.ActiveRow.Cells["varName"].Value.ToString();
                Band = gridBand.ActiveRow.Cells["varName"].Value.ToString();

                if (optCriteria.Value.ToString().ToUpper() == "All".ToUpper())
                {
                }
                else if (optCriteria.Value.ToString().ToUpper() == "Section".ToUpper())
                {
                    SectId = gridSection.ActiveRow.Cells["SectId"].Value.ToString();
                }
                else if (optCriteria.Value.ToString().ToUpper() == "Employee".ToUpper())
                {
                    EmpId = gridEmployeeID.ActiveRow.Cells["EmpId"].Value.ToString();
                }

                else if (optCriteria.Value.ToString().ToUpper() == "Band".ToUpper())
                {
                    Band = gridBand.ActiveRow.Cells["varName"].Value.ToString();
                }


                if ((gridRpt.ActiveRow.Cells["rptname"].Text.ToString() == "Attendance Summary"))
                {
                    ReportPath = Common.Classes.clsMain.AppPath + @"\Attendence\Reports\rptAttendMonthly.rdlc";
                    SQLQuery = "Exec rptAttendMonthly " + Common.Classes.clsMain.intComId + ",'" + clsProc.GTRDate(dtDateFrom.Value.ToString()) + "', '" + clsProc.GTRDate(dtDateTo.Value.ToString()) + "', " + SectId + ", '', '', " + EmpId + ",'" + Band + "','" + EmpType + "',0";
                }
                else if ((gridRpt.ActiveRow.Cells["rptname"].Text.ToString() == "Monthly Absent"))
                {
                    ReportPath = Common.Classes.clsMain.AppPath + @"\Attendence\Reports\rptAbsentMonthly.rdlc";
                    SQLQuery = "Exec rptAttendMonthlyAbsent " + Common.Classes.clsMain.intComId + ",'" + clsProc.GTRDate(dtDateFrom.Value.ToString()) + "', '" + clsProc.GTRDate(dtDateTo.Value.ToString()) + "', " + SectId + ", '', '', " + EmpId + ",'" + Band + "','" + EmpType + "',0";
                }
                else if ((gridRpt.ActiveRow.Cells["rptname"].Text.ToString() == "Monthly Late"))
                {
                    ReportPath = Common.Classes.clsMain.AppPath + @"\Attendence\Reports\rptLateMonthly.rdlc";
                    SQLQuery = "Exec rptAttendMonthlyLate " + Common.Classes.clsMain.intComId + ",'" + clsProc.GTRDate(dtDateFrom.Value.ToString()) + "', '" + clsProc.GTRDate(dtDateTo.Value.ToString()) + "', " + SectId + ", '', '', " + EmpId + ",'" + Band + "','" + EmpType + "',0";
                }
                else if ((gridRpt.ActiveRow.Cells["rptname"].Text.ToString() == "Monthly OT"))
                {
                    ReportPath = Common.Classes.clsMain.AppPath + @"\Attendence\Reports\rptMonthlyOT.rdlc";
                    SQLQuery = "Exec rptMonthlyAttend " + Common.Classes.clsMain.intComId + ",'" + clsProc.GTRDate(dtDateFrom.Value.ToString()) + "', '" + clsProc.GTRDate(dtDateTo.Value.ToString()) + "', " + SectId + ", '', '', " + EmpId + ",'" + Band + "','" + EmpType + "',0";
                }
                else if ((gridRpt.ActiveRow.Cells["rptname"].Text.ToString() == "Monthly Job Card"))
                {
                    ReportPath = Common.Classes.clsMain.AppPath + @"\Attendence\Reports\rptJobCardDetails.rdlc";
                    SQLQuery = "Exec rptMonthlyAttend " + Common.Classes.clsMain.intComId + ",'" + clsProc.GTRDate(dtDateFrom.Value.ToString()) + "', '" + clsProc.GTRDate(dtDateTo.Value.ToString()) + "', " + SectId + ", '', '', " + EmpId + ",'" + Band + "','" + EmpType + "',1";
                }

                clsCon.GTRFillDatasetWithSQLCommand(ref dsDetails, SQLQuery);

                if (dsDetails.Tables[0].Rows.Count == 0)
                {
                    MessageBox.Show("Data Not Found");
                    return;
                }
                clsReport.strReportPathMain = ReportPath;
                clsReport.strQueryMain = SQLQuery;
                clsReport.strDSNMain = DataSourceName;
                clsReport.dsReport = dsDetails;

                Common.Classes.clsMain.strExtension = optFormat.Value.ToString();
                Common.Classes.clsMain.strFormat = optFormat.Text.ToString();

                FM.prcShowReport(FormCaption);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            finally
            {
                SQLQuery = null;
                DataSourceName = null;
                ReportPath = null;
                dsDetails = null;
                clsCon = null;
            }
        }

        private void gridEmpType_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            gridEmpType.DisplayLayout.Bands[0].Columns["varID"].Hidden = true;
            gridEmpType.DisplayLayout.Bands[0].Columns["varName"].Width = 175;
            gridEmpType.DisplayLayout.Bands[0].Columns["varName"].Header.Caption = "Emp Type";

            //Change alternate color
            gridEmpType.DisplayLayout.Override.RowAlternateAppearance.BackColor = Color.Cyan;
            gridEmpType.DisplayLayout.Override.RowAlternateAppearance.ForeColor = Color.DarkBlue;

            //Select Full Row when click on any cell
            e.Layout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;

            //Selection Style Will Be Row Selector
            gridEmpType.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;

            //Stop Updating
            gridEmpType.DisplayLayout.Override.AllowUpdate = DefaultableBoolean.False;

            //Hiding +/- Indicator
            gridEmpType.DisplayLayout.Override.ExpansionIndicator = ShowExpansionIndicator.Never;

            //Hide Group Box Display
            e.Layout.GroupByBox.Hidden = true;

            //Use Filtering
            //gridSection.DisplayLayout.Override.AllowRowFiltering = DefaultableBoolean.False;

            e.Layout.Override.FilterUIType = FilterUIType.FilterRow;
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
            
            string SectId = "0", EmpId = "0", Band = "=All=", EmpType = "", SubSectId = "0";

            EmpType = gridEmpType.ActiveRow.Cells["varName"].Value.ToString();

            if (optCriteria.Value.ToString().ToUpper() == "All".ToUpper())
            {
            }
            else if (optCriteria.Value.ToString().ToUpper() == "Section".ToUpper())
            {
                SectId = gridSection.ActiveRow.Cells["SectId"].Value.ToString();
            }
            else if (optCriteria.Value.ToString().ToUpper() == "Employee".ToUpper())
            {
                EmpId = gridEmployeeID.ActiveRow.Cells["EmpId"].Value.ToString();
            }

            else if (optCriteria.Value.ToString().ToUpper() == "Band".ToUpper())
            {
                Band = gridBand.ActiveRow.Cells["varName"].Value.ToString();
            }

            if ((gridRpt.ActiveRow.Cells["rptname"].Text.ToString() == "Attendance Summary"))
            {
                SQLQuery = "Exec rptAttendMonthly " + Common.Classes.clsMain.intComId + ",'" + clsProc.GTRDate(dtDateFrom.Value.ToString()) + "', '" + clsProc.GTRDate(dtDateTo.Value.ToString()) + "', " + SectId + ", '', '', " + EmpId + ",'" + Band + "','" + EmpType + "',0";
            }
            else if ((gridRpt.ActiveRow.Cells["rptname"].Text.ToString() == "Monthly Absent"))
            {
                SQLQuery = "Exec rptAttendMonthlyAbsent " + Common.Classes.clsMain.intComId + ",'" + clsProc.GTRDate(dtDateFrom.Value.ToString()) + "', '" + clsProc.GTRDate(dtDateTo.Value.ToString()) + "', " + SectId + ", '', '', " + EmpId + ",'" + Band + "','" + EmpType + "',0";
            }
            else if ((gridRpt.ActiveRow.Cells["rptname"].Text.ToString() == "Monthly Late"))
            {
                SQLQuery = "Exec rptAttendMonthlyLate " + Common.Classes.clsMain.intComId + ",'" + clsProc.GTRDate(dtDateFrom.Value.ToString()) + "', '" + clsProc.GTRDate(dtDateTo.Value.ToString()) + "', " + SectId + ", '', '', " + EmpId + ",'" + Band + "','" + EmpType + "',0";
            }
            else if ((gridRpt.ActiveRow.Cells["rptname"].Text.ToString() == "Monthly OT"))
            {
                SQLQuery = "Exec rptMonthlyAttend " + Common.Classes.clsMain.intComId + ",'" + clsProc.GTRDate(dtDateFrom.Value.ToString()) + "', '" + clsProc.GTRDate(dtDateTo.Value.ToString()) + "', " + SectId + ", '', '', " + EmpId + ",'" + Band + "','" + EmpType + "',0";
            }
            else if ((gridRpt.ActiveRow.Cells["rptname"].Text.ToString() == "Monthly Job Card"))
            {
                SQLQuery = "Exec rptMonthlyAttend " + Common.Classes.clsMain.intComId + ",'" + clsProc.GTRDate(dtDateFrom.Value.ToString()) + "', '" + clsProc.GTRDate(dtDateTo.Value.ToString()) + "', " + SectId + ", '', '', " + EmpId + ",'" + Band + "','" + EmpType + "',1";
            }

            clscon.GTRFillDatasetWithSQLCommand(ref dsList, SQLQuery);
                 
            dsList.Tables[0].TableName = "MonthlyAtten";

            gridExcel.DataSource = null;
            gridExcel.DataSource = dsList.Tables["MonthlyAtten"];

            DialogResult dlgRes =
            MessageBox.Show("Do You Want to Save the Data Sheet");
            if (dlgRes != DialogResult.OK)
            {
                return;
            }

            SaveFileDialog dlgSurveyExcel = new SaveFileDialog();
            dlgSurveyExcel.Filter = "Excel WorkBook (*.xls)|.xls";
            dlgSurveyExcel.FileName = gridRpt.ActiveRow.Cells["rptname"].Text.ToString() + "_" + DateTime.Now.ToShortDateString().Replace(@"/", "_");

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

        private void gridRpt_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            gridRpt.DisplayLayout.Bands[0].Columns["rptID"].Hidden = true;
            gridRpt.DisplayLayout.Bands[0].Columns["rptName"].Width = 221;
            gridRpt.DisplayLayout.Bands[0].Columns["rptName"].Header.Caption = "Report";

            //Change alternate color
            gridRpt.DisplayLayout.Override.RowAlternateAppearance.BackColor = Color.Cyan;
            gridRpt.DisplayLayout.Override.RowAlternateAppearance.ForeColor = Color.DarkBlue;

            //Select Full Row when click on any cell
            e.Layout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;

            //Selection Style Will Be Row Selector
            gridRpt.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;

            //Stop Updating
            gridRpt.DisplayLayout.Override.AllowUpdate = DefaultableBoolean.False;

            //Hiding +/- Indicator
            gridRpt.DisplayLayout.Override.ExpansionIndicator = ShowExpansionIndicator.Never;

            //Hide Group Box Display
            e.Layout.GroupByBox.Hidden = true;

            //Use Filtering
            //gridSection.DisplayLayout.Override.AllowRowFiltering = DefaultableBoolean.False;

            e.Layout.Override.FilterUIType = FilterUIType.FilterRow;
        }

        private void btnLast_Click(object sender, EventArgs e)
        {
            if (dtDateFrom.DateTime.Month == 1)
            {
                var firstDay = new DateTime(dtDateFrom.DateTime.Year - 1, dtDateFrom.DateTime.Month + 11, 1);
                dtDateFrom.Value = firstDay;
                var DaysInMonth = DateTime.DaysInMonth(dtDateFrom.DateTime.Year, dtDateFrom.DateTime.Month);
                var lastDay = new DateTime(dtDateFrom.DateTime.Year, dtDateFrom.DateTime.Month, DaysInMonth);


                dtDateTo.Value = lastDay;
            }
            else
            {
                var DaysInMonth = DateTime.DaysInMonth(dtDateTo.DateTime.Year, dtDateTo.DateTime.Month - 1);
                var lastDay = new DateTime(dtDateTo.DateTime.Year, dtDateTo.DateTime.Month - 1, DaysInMonth);
                var firstDay = new DateTime(dtDateFrom.DateTime.Year, dtDateFrom.DateTime.Month - 1, 1);
                dtDateFrom.Value = firstDay;
                dtDateTo.Value = lastDay;
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (dtDateFrom.DateTime.Month == 12)
            {
                var firstDay = new DateTime(dtDateFrom.DateTime.Year + 1, dtDateFrom.DateTime.Month - 11, 1);
                dtDateFrom.Value = firstDay;
                var DaysInMonth = DateTime.DaysInMonth(dtDateFrom.DateTime.Year, dtDateFrom.DateTime.Month);
                var lastDay = new DateTime(dtDateFrom.DateTime.Year, dtDateFrom.DateTime.Month, DaysInMonth);


                dtDateTo.Value = lastDay;
            }
            else
            {
                var DaysInMonth = DateTime.DaysInMonth(dtDateTo.DateTime.Year, dtDateTo.DateTime.Month + 1);
                var lastDay = new DateTime(dtDateTo.DateTime.Year, dtDateTo.DateTime.Month + 1, DaysInMonth);
                var firstDay = new DateTime(dtDateFrom.DateTime.Year, dtDateFrom.DateTime.Month + 1, 1);
                dtDateFrom.Value = firstDay;
                dtDateTo.Value = lastDay;
            }
        }



      }
  }

