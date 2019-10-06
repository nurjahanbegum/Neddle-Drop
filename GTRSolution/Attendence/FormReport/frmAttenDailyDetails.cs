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
    public partial class frmAttenDailyDetails : Form
    {
        System.Data.DataSet dsList;
        System.Data.DataSet dsDetails;
        clsProcedure clsProc = new clsProcedure();

        string ReportPath = "", rptQuery = "", DataSourceName = "DataSet1", FormCaption = "";

        private Infragistics.Win.UltraWinTabControl.UltraTabControl uTab;
        Common.FormEntry.frmMaster FM;
        public frmAttenDailyDetails(ref Infragistics.Win.UltraWinTabControl.UltraTabControl utab, Common.FormEntry.frmMaster fm)
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
                string sqlquary = "Exec prcrptDaily  " + Common.Classes.clsMain.intComId;
                clscon.GTRFillDatasetWithSQLCommand(ref dsList, sqlquary);
               
                dsList.Tables[0].TableName = "Section";
                dsList.Tables[3].TableName = "Employee";
                dsList.Tables[4].TableName = "Band";
                dsList.Tables[5].TableName = "EmpType";
                dsList.Tables[6].TableName = "SubSection";

                gridSection.DataSource = dsList.Tables["Section"];
                gridEmployeeID.DataSource = dsList.Tables["Employee"];
                gridBand.DataSource = dsList.Tables["Band"];
                gridEmpType.DataSource = dsList.Tables["EmpType"];
                gridSubSec.DataSource = dsList.Tables["SubSection"];

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
        
        private void prcShowReport()
        {
            dsDetails = new DataSet();
            ArrayList arQuery = new ArrayList();
            clsConnection clsCon = new clsConnection();

            try
            {
                DataSourceName = "DataSet1";
                FormCaption = "Report :: Attendance Details...";

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
                else if (optCriteria.Value.ToString().ToUpper() == "SubSec".ToUpper())
                {
                    SubSectId = gridSubSec.ActiveRow.Cells["SubSectId"].Value.ToString();
                }
                rptQuery = "Exec rptAttendDetails " + Common.Classes.clsMain.intComId + ",'" + clsProc.GTRDate(dtDateFrom.Value.ToString()) + "', '" + clsProc.GTRDate(dtDateTo.Value.ToString()) + "', " + SectId + ", '', '', " + EmpId + ",'" + Band + "','" + EmpType + "'," + SubSectId + "";
                clsCon.GTRFillDatasetWithSQLCommand(ref dsDetails,rptQuery);
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
        private void optCriteria_ValueChanged(object sender, EventArgs e)
        {                        
            gridSection.Enabled = false;
            gridEmployeeID.Enabled = false;
            gridBand.Enabled = false;
            gridEmpType.Enabled = true;
            gridSubSec.Enabled = false;

            if(optCriteria.Value.ToString() =="All")
            {
                gridSection.Enabled = false;
                gridEmployeeID.Enabled = false;
                gridBand.Enabled = false;
                gridEmpType.Enabled = true;
                gridSubSec.Enabled = false;
            }
            else if (optCriteria.Value.ToString() == "Section")
            {
                gridSection.Enabled = true;
                gridEmployeeID.Enabled = false;
                gridBand.Enabled = false;
                gridEmpType.Enabled = true;
                gridSubSec.Enabled = false;
            }
            else if (optCriteria.Value.ToString() == "Employee")
            {
                gridEmployeeID.Enabled = true;
                gridSection.Enabled = false;
                gridBand.Enabled = false;
                gridEmpType.Enabled = true;
                gridSubSec.Enabled = false;
            }
            else if (optCriteria.Value.ToString() == "Band")
            {
                gridBand.Enabled = true;
                gridSection.Enabled = false;
                gridEmployeeID.Enabled = false;
                gridEmpType.Enabled = true;
                gridSubSec.Enabled = false;
            }
            else if (optCriteria.Value.ToString() == "SubSec")
            {
                gridBand.Enabled = false;
                gridSection.Enabled = false;
                gridEmployeeID.Enabled = false;
                gridEmpType.Enabled = true;
                gridSubSec.Enabled = true;
            }
        }
        private void frmAttenDailyDetails_FormClosing(object sender, FormClosingEventArgs e)
        {
            int index = Common.Classes.clsMain.fncFindOpenFormIndex(this);
            uTab.Tabs.RemoveAt(index);
            Common.Classes.clsMain.fncExistOpenForm(this, "Remove");

            clsProc = null;
            FM = null;
        }

        private void frmAttenDailyDetails_Load(object sender, EventArgs e)
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
            ReportPath = Common.Classes.clsMain.AppPath + @"\Attendence\Reports\rptAttendAll.rdlc";
            prcShowReport();
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

        private void gridSubSec_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            gridSubSec.DisplayLayout.Bands[0].Columns["SubSectId"].Hidden = true;
            gridSubSec.DisplayLayout.Bands[0].Columns["SubSectName"].Width = 221;
            gridSubSec.DisplayLayout.Bands[0].Columns["SubSectName"].Header.Caption = "Sub Section";

            //Change alternate color
            gridSubSec.DisplayLayout.Override.RowAlternateAppearance.BackColor = Color.Cyan;
            gridSubSec.DisplayLayout.Override.RowAlternateAppearance.ForeColor = Color.DarkBlue;

            //Select Full Row when click on any cell
            e.Layout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;

            //Selection Style Will Be Row Selector
            gridSubSec.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;

            //Stop Updating
            gridSubSec.DisplayLayout.Override.AllowUpdate = DefaultableBoolean.False;

            //Hiding +/- Indicator
            gridSubSec.DisplayLayout.Override.ExpansionIndicator = ShowExpansionIndicator.Never;

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
            else if (optCriteria.Value.ToString().ToUpper() == "SubSec".ToUpper())
            {
                SubSectId = gridSubSec.ActiveRow.Cells["SubSectId"].Value.ToString();
            }
            String sqlquary = "Exec rptAttendDetails " + Common.Classes.clsMain.intComId + ",'" + clsProc.GTRDate(dtDateFrom.Value.ToString()) + "', '" + clsProc.GTRDate(dtDateTo.Value.ToString()) + "', " + SectId + ", '', '', " + EmpId + ",'" + Band + "','" + EmpType + "'," + SubSectId + "";
            clscon.GTRFillDatasetWithSQLCommand(ref dsList, sqlquary);
                 
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
            dlgSurveyExcel.FileName = "Daily Attendance Details" + "_" + DateTime.Now.ToShortDateString().Replace(@"/", "_");

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

