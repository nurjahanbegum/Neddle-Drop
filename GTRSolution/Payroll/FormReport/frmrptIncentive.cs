using System;
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
using ColumnStyle = Infragistics.Win.UltraWinGrid.ColumnStyle;
using Infragistics.Win.UltraWinGrid.ExcelExport;
using System.Collections;


namespace GTRHRIS.Payroll.FormReport
{
    public partial class frmrptIncentive : Form
    {
        System.Data.DataSet dsList;
        System.Data.DataSet dsDetails;
        clsProcedure clsProc = new clsProcedure();

        private Infragistics.Win.UltraWinTabControl.UltraTabControl uTab;
        Common.FormEntry.frmMaster FM;
        public frmrptIncentive(ref Infragistics.Win.UltraWinTabControl.UltraTabControl utab, Common.FormEntry.frmMaster fm)
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
                string sqlquary = "Exec [rptIncentive] " + Common.Classes.clsMain.intComId + ", '','0','0','','',0,''";
                clscon.GTRFillDatasetWithSQLCommand(ref dsList, sqlquary);
                
                dsList.Tables[0].TableName = "ProssType";
                dsList.Tables[1].TableName = "Section"; 
                dsList.Tables[2].TableName = "Employee";
                dsList.Tables[3].TableName = "Band";
                dsList.Tables[4].TableName = "ReportCategory";
                dsList.Tables[5].TableName = "EmpType";

                gridProssType.DataSource = dsList.Tables["ProssType"];
                gridSec.DataSource = dsList.Tables["Section"];
                gridEmployee.DataSource = dsList.Tables["Employee"];
                gridBand.DataSource = dsList.Tables["Band"];
                gridReportCategory.DataSource = dsList.Tables["ReportCategory"];
                gridEmpType.DataSource = dsList.Tables["EmpType"];
                
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }


        private void frmrptIncentive_Load(object sender, EventArgs e)
        {
            try
            {
                prcLoadList();
                //prcLoadCombo();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void frmrptIncentive_FormClosing(object sender, FormClosingEventArgs e)
        {
            int index = Common.Classes.clsMain.fncFindOpenFormIndex(this);
            uTab.Tabs.RemoveAt(index);
            Common.Classes.clsMain.fncExistOpenForm(this, "Remove");
        }
         
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
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

        private void btnPreview_Click(object sender, EventArgs e)
        {
            try
            {

                dsDetails = new DataSet();

                ArrayList arQuery = new ArrayList();
                GTRLibrary.clsConnection clsCon = new GTRLibrary.clsConnection();

                
                string ReportPath = "", SQLQuery = "", DataSourceName = "DataSet1", FormCaption = "", Band = "",EmpType = "";
                Band = gridBand.ActiveRow.Cells["varName"].Value.ToString();
                EmpType = gridEmpType.ActiveRow.Cells["varName"].Value.ToString();

                DataSourceName = "DataSet1";
                FormCaption = "Report :: Payment Information...";

                if ((gridReportCategory.ActiveRow.Cells["rptname"].Text.ToString() == "Active Incentive Report"))
                {

                    ReportPath = Common.Classes.clsMain.AppPath + @"\Payroll\Reports\rptIncentive.rdlc";
                    SQLQuery = "Exec [rptIncentive] " + Common.Classes.clsMain.intComId + ", '" + gridProssType.ActiveRow.Cells[0].Value.ToString() + "','" + gridSec.ActiveRow.Cells[0].Value.ToString() + "','" + gridEmployee.ActiveRow.Cells[0].Value.ToString() + "','" + Band + "','" + EmpType + "',1,'Active Incentive Report'";

                }
                if ((gridReportCategory.ActiveRow.Cells["rptname"].Text.ToString() == "Released Incentive Report"))
                {

                    ReportPath = Common.Classes.clsMain.AppPath + @"\Payroll\Reports\rptIncentive.rdlc";
                    SQLQuery = "Exec [rptIncentive] " + Common.Classes.clsMain.intComId + ", '" + gridProssType.ActiveRow.Cells[0].Value.ToString() + "','" + gridSec.ActiveRow.Cells[0].Value.ToString() + "','" + gridEmployee.ActiveRow.Cells[0].Value.ToString() + "','" + Band + "','" + EmpType + "',1,'Released Incentive Report'";

                }

                else if ((gridReportCategory.ActiveRow.Cells["rptname"].Text.ToString() == "Active Incentive Payment List"))
                {

                    ReportPath = Common.Classes.clsMain.AppPath + @"\Payroll\Reports\rptIncentivePayList.rdlc";
                    SQLQuery = "Exec [rptIncentive] " + Common.Classes.clsMain.intComId + ", '" + gridProssType.ActiveRow.Cells[0].Value.ToString() + "','" + gridSec.ActiveRow.Cells[0].Value.ToString() + "','" + gridEmployee.ActiveRow.Cells[0].Value.ToString() + "','" + Band + "','" + EmpType + "',1,'Active Incentive Payment List'";

                }
                else if ((gridReportCategory.ActiveRow.Cells["rptname"].Text.ToString() == "Released Incentive Payment List"))
                {

                    ReportPath = Common.Classes.clsMain.AppPath + @"\Payroll\Reports\rptIncentivePayList.rdlc";
                    SQLQuery = "Exec [rptIncentive] " + Common.Classes.clsMain.intComId + ", '" + gridProssType.ActiveRow.Cells[0].Value.ToString() + "','" + gridSec.ActiveRow.Cells[0].Value.ToString() + "','" + gridEmployee.ActiveRow.Cells[0].Value.ToString() + "','" + Band + "','" + EmpType + "',1,'Released Incentive Payment List'";

                }

                else  if ((gridReportCategory.ActiveRow.Cells["rptname"].Text.ToString() == "Incentive Amount"))
                {

                    ReportPath = Common.Classes.clsMain.AppPath + @"\Payroll\Reports\rptIncentiveAmt.rdlc";
                    SQLQuery = "Exec [rptIncentive] " + Common.Classes.clsMain.intComId + ", '" + gridProssType.ActiveRow.Cells[0].Value.ToString() + "','" + gridSec.ActiveRow.Cells[0].Value.ToString() + "','" + gridEmployee.ActiveRow.Cells[0].Value.ToString() + "','" + Band + "','" + EmpType + "',1,'Incentive Amount'";

                }

                else if ((gridReportCategory.ActiveRow.Cells["rptname"].Text.ToString() == "Best Performance Report"))
                {

                    ReportPath = Common.Classes.clsMain.AppPath + @"\Payroll\Reports\rptBestPerform.rdlc";
                    SQLQuery = "Exec [rptIncentive] " + Common.Classes.clsMain.intComId + ", '" + gridProssType.ActiveRow.Cells[0].Value.ToString() + "','" + gridSec.ActiveRow.Cells[0].Value.ToString() + "','" + gridEmployee.ActiveRow.Cells[0].Value.ToString() + "','" + Band + "','" + EmpType + "',1,'Best Performance Report'";

                }

                else if ((gridReportCategory.ActiveRow.Cells["rptname"].Text.ToString() == "Best Performance Payslip"))
                {

                    ReportPath = Common.Classes.clsMain.AppPath + @"\Payroll\Reports\rptBestPerformPaySlip.rdlc";
                    SQLQuery = "Exec [rptIncentive] " + Common.Classes.clsMain.intComId + ", '" + gridProssType.ActiveRow.Cells[0].Value.ToString() + "','" + gridSec.ActiveRow.Cells[0].Value.ToString() + "','" + gridEmployee.ActiveRow.Cells[0].Value.ToString() + "','" + Band + "','" + EmpType + "',1,'Best Performance Payslip'";

                }


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

                //clsReport.strReportPathMain = ReportPath;
                //clsReport.strQueryMain = SQLQuery;
                //clsReport.strDSNMain = DataSourceName;

                //FM.prcShowReport(FormCaption);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }



        private void gridProssType_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            gridProssType.DisplayLayout.Bands[0].Columns["Month"].Hidden = true;
            gridProssType.DisplayLayout.Bands[0].Columns["year"].Hidden = true;
            gridProssType.DisplayLayout.Bands[0].Columns["dtDate"].Hidden = true;

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


        private void optCriteria_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }


        private void btnPreview_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void btnClose_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }


        private void gridBand_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            gridBand.DisplayLayout.Bands[0].Columns["varId"].Hidden = true;
            gridBand.DisplayLayout.Bands[0].Columns["varName"].Width = 125;
            gridBand.DisplayLayout.Bands[0].Columns["varName"].Header.Caption = "Incen Band";

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

        private void gridEmployee_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            gridEmployee.DisplayLayout.Bands[0].Columns["empId"].Hidden = true;
            gridEmployee.DisplayLayout.Bands[0].Columns["empCode"].Width = 70;
            gridEmployee.DisplayLayout.Bands[0].Columns["empCode"].Header.Caption = "Employee Code";
            gridEmployee.DisplayLayout.Bands[0].Columns["EmpName"].Width = 160;
            gridEmployee.DisplayLayout.Bands[0].Columns["EmpName"].Header.Caption = "Employee Name";

            //Change alternate color
            gridEmployee.DisplayLayout.Override.RowAlternateAppearance.BackColor = Color.Cyan;
            gridEmployee.DisplayLayout.Override.RowAlternateAppearance.ForeColor = Color.DarkBlue;

            //Select Full Row when click on any cell
            e.Layout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;

            //Selection Style Will Be Row Selector
            gridEmployee.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;

            //Stop Updating
            gridEmployee.DisplayLayout.Override.AllowUpdate = DefaultableBoolean.False;

            //Hiding +/- Indicator  
            gridEmployee.DisplayLayout.Override.ExpansionIndicator = ShowExpansionIndicator.Never;

            //Hide Group Box Display
            e.Layout.GroupByBox.Hidden = true;

            //Use Filtering
            //gridEmployee.DisplayLayout.Override.AllowRowFiltering = DefaultableBoolean.False;

            e.Layout.Override.FilterUIType = FilterUIType.FilterRow;
            //gridEmployee.DisplayLayout.Bands[0].Columns["EmpCode"].AllowRowFiltering = DefaultableBoolean.False;
        }

        private void gridReportCategory_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            gridReportCategory.DisplayLayout.Bands[0].Columns["rptid"].Hidden = true;
            gridReportCategory.DisplayLayout.Bands[0].Columns["rptname"].Width = 225;
            gridReportCategory.DisplayLayout.Bands[0].Columns["rptname"].Header.Caption = "Report Type";

            //Change alternate color
            gridReportCategory.DisplayLayout.Override.RowAlternateAppearance.BackColor = Color.Cyan;
            gridReportCategory.DisplayLayout.Override.RowAlternateAppearance.ForeColor = Color.DarkBlue;

            //Select Full Row when click on any cell
            e.Layout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;

            //Selection Style Will Be Row Selector
            gridReportCategory.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;

            //Stop Updating
            gridReportCategory.DisplayLayout.Override.AllowUpdate = DefaultableBoolean.False;

            //Hiding +/- Indicator
            gridReportCategory.DisplayLayout.Override.ExpansionIndicator = ShowExpansionIndicator.Never;

            //Hide Group Box Display
            e.Layout.GroupByBox.Hidden = true;

            //Use Filtering
            //gridReportCategory.DisplayLayout.Override.AllowRowFiltering = DefaultableBoolean.False;
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

            string SQLQuery = "", Band = "", EmpType = "";
            Band = gridBand.ActiveRow.Cells["varName"].Value.ToString();
            EmpType = gridEmpType.ActiveRow.Cells["varName"].Value.ToString();

            if ((gridReportCategory.ActiveRow.Cells["rptname"].Text.ToString() == "Active Incentive Report"))
            {
                SQLQuery = "Exec [rptIncentiveExcel] " + Common.Classes.clsMain.intComId + ", '" + gridProssType.ActiveRow.Cells[0].Value.ToString() + "','" + gridSec.ActiveRow.Cells[0].Value.ToString() + "','" + gridEmployee.ActiveRow.Cells[0].Value.ToString() + "','" + Band + "','" + EmpType + "',1,'Active Incentive Report'";

            }
            if ((gridReportCategory.ActiveRow.Cells["rptname"].Text.ToString() == "Released Incentive Report"))
            {

                SQLQuery = "Exec [rptIncentiveExcel] " + Common.Classes.clsMain.intComId + ", '" + gridProssType.ActiveRow.Cells[0].Value.ToString() + "','" + gridSec.ActiveRow.Cells[0].Value.ToString() + "','" + gridEmployee.ActiveRow.Cells[0].Value.ToString() + "','" + Band + "','" + EmpType + "',1,'Released Incentive Report'";

            }

            else if ((gridReportCategory.ActiveRow.Cells["rptname"].Text.ToString() == "Active Incentive Payment List"))
            {

                SQLQuery = "Exec [rptIncentiveExcel] " + Common.Classes.clsMain.intComId + ", '" + gridProssType.ActiveRow.Cells[0].Value.ToString() + "','" + gridSec.ActiveRow.Cells[0].Value.ToString() + "','" + gridEmployee.ActiveRow.Cells[0].Value.ToString() + "','" + Band + "','" + EmpType + "',1,'Active Incentive Payment List'";

            }
            else if ((gridReportCategory.ActiveRow.Cells["rptname"].Text.ToString() == "Released Incentive Payment List"))
            {

                SQLQuery = "Exec [rptIncentiveExcel] " + Common.Classes.clsMain.intComId + ", '" + gridProssType.ActiveRow.Cells[0].Value.ToString() + "','" + gridSec.ActiveRow.Cells[0].Value.ToString() + "','" + gridEmployee.ActiveRow.Cells[0].Value.ToString() + "','" + Band + "','" + EmpType + "',1,'Released Incentive Payment List'";

            }

            else if ((gridReportCategory.ActiveRow.Cells["rptname"].Text.ToString() == "Incentive Amount"))
            {

                SQLQuery = "Exec [rptIncentiveExcel] " + Common.Classes.clsMain.intComId + ", '" + gridProssType.ActiveRow.Cells[0].Value.ToString() + "','" + gridSec.ActiveRow.Cells[0].Value.ToString() + "','" + gridEmployee.ActiveRow.Cells[0].Value.ToString() + "','" + Band + "','" + EmpType + "',1,'Incentive Amount'";

            }

            else if ((gridReportCategory.ActiveRow.Cells["rptname"].Text.ToString() == "Best Performance Report"))
            {

                SQLQuery = "Exec [rptIncentiveExcel] " + Common.Classes.clsMain.intComId + ", '" + gridProssType.ActiveRow.Cells[0].Value.ToString() + "','" + gridSec.ActiveRow.Cells[0].Value.ToString() + "','" + gridEmployee.ActiveRow.Cells[0].Value.ToString() + "','" + Band + "','" + EmpType + "',1,'Best Performance Report'";

            }


            clscon.GTRFillDatasetWithSQLCommand(ref dsList, SQLQuery);

            dsList.Tables[0].TableName = "Grade";

            gridExcel.DataSource = null;
            gridExcel.DataSource = dsList.Tables["Grade"];

            DialogResult dlgRes =
            MessageBox.Show("Do You Want to Save the Data Sheet");
            if (dlgRes != DialogResult.OK)
            {
                return;
            }

            SaveFileDialog dlgSurveyExcel = new SaveFileDialog();
            dlgSurveyExcel.Filter = "Excel WorkBook (*.xls)|.xls";
            dlgSurveyExcel.FileName = gridReportCategory.ActiveRow.Cells["rptname"].Text.ToString() + "_" + DateTime.Now.ToShortDateString().Replace(@"/", "_");

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

        private void gridEmpType_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            gridEmpType.DisplayLayout.Bands[0].Columns["varId"].Hidden = true;
            gridEmpType.DisplayLayout.Bands[0].Columns["varName"].Width = 150;
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
            //gridSec.DisplayLayout.Override.AllowRowFiltering = DefaultableBoolean.False;
            e.Layout.Override.FilterUIType = FilterUIType.FilterRow;
        }



   }
}