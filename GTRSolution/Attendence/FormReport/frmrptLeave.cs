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
    public partial class frmrptLeave : Form
    {
        System.Data.DataSet dsList;
        System.Data.DataSet dsDetails;
        clsProcedure clsProc = new clsProcedure();

        private Infragistics.Win.UltraWinTabControl.UltraTabControl uTab;
        Common.FormEntry.frmMaster FM;
        public frmrptLeave(ref Infragistics.Win.UltraWinTabControl.UltraTabControl utab, Common.FormEntry.frmMaster fm)
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
                string sqlquary = "Exec prcrptLeave " + Common.Classes.clsMain.intComId;
                clscon.GTRFillDatasetWithSQLCommand(ref dsList, sqlquary);

                dsList.Tables[0].TableName = "Criteria";
                dsList.Tables[1].TableName = "Section";
                dsList.Tables[2].TableName = "Employee";
                dsList.Tables[3].TableName = "OpenDate";
                dsList.Tables[4].TableName = "tblActive";
                dsList.Tables[5].TableName = "tblEmpType";


                gridCriteria.DataSource = dsList.Tables["Criteria"];
                gridSec.DataSource = dsList.Tables["Section"];
                gridEmp.DataSource = dsList.Tables["Employee"];
                gridActive.DataSource = dsList.Tables["tblActive"];
                gridEmpType.DataSource = dsList.Tables["tblEmpType"];
                //gridSec.DataSource = dsList.Tables["IncType"];

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
            cboFinyear.DataSource = null;
            cboFinyear.DataSource = dsList.Tables["OpenDate"];
            cboFinyear.DisplayMember = "dtOpBal";
            cboFinyear.ValueMember = "dtOpBal";
        }

        private void frmrptLeave_Load(object sender, EventArgs e)
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

        private void frmrptLeave_FormClosing(object sender, FormClosingEventArgs e)
        {
            int index = Common.Classes.clsMain.fncFindOpenFormIndex(this);
            uTab.Tabs.RemoveAt(index);
            Common.Classes.clsMain.fncExistOpenForm(this, "Remove");
        }
         
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
       

        private void gridCriteria_AfterRowActivate(object sender, EventArgs e)
        {
            if (gridCriteria.ActiveRow.Cells[0].Value.ToString().ToUpper() == "General".ToUpper())
            {
                group1.Visible = true;
                group2.Visible = false;
            }

            else if (gridCriteria.ActiveRow.Cells[0].Value.ToString().ToUpper() == "ML".ToUpper())
            {
                group1.Visible = true;
                group2.Visible = false;
            }

            else if (gridCriteria.ActiveRow.Cells[0].Value.ToString().ToUpper() == "FirstML".ToUpper())
            {
                group1.Visible = true;
                group2.Visible = false;
            }
            else if (gridCriteria.ActiveRow.Cells[0].Value.ToString().ToUpper() == "LastML".ToUpper())
            {
                group1.Visible = true;
                group2.Visible = false;
            }

            //else if (gridCriteria.ActiveRow.Cells[0].Text.ToString().ToUpper() == "MonthDetails".ToUpper())
            //{
            //    group1.Visible = true;
            //    group2.Visible = false;
            //}

            else if (gridCriteria.ActiveRow.Cells[0].Text.ToString().ToUpper() != "General".ToUpper())
            {
                group1.Visible = false;
                group2.Visible = true;
            }

            if (gridCriteria.Text != "General")
            {


                DataRow dr;
                if (dsList.Tables["OpenDate"].Rows.Count > 0)
                {
                    dr = dsList.Tables["OpenDate"].Rows[0];

                    this.cboFinyear.Text = dr["dtOpBal"].ToString();
                    //this.txtShipVatPr.Text = dr["vatper"].ToString();

                }
            }
            else
            {

            }
        }



        private void gridCriteria_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            gridCriteria.DisplayLayout.Bands[0].Columns["CValue"].Hidden = true;
            gridCriteria.DisplayLayout.Bands[0].Columns["SlNo"].Hidden = true;

            gridCriteria.DisplayLayout.Bands[0].Columns["Criteria"].Width = 200;
            gridCriteria.DisplayLayout.Bands[0].Columns["Criteria"].Header.Caption = "Criteria";

            //Change alternate color
            gridCriteria.DisplayLayout.Override.RowAlternateAppearance.BackColor = Color.Cyan;
            gridCriteria.DisplayLayout.Override.RowAlternateAppearance.ForeColor = Color.DarkBlue;

            //Select Full Row when click on any cell
            e.Layout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;

            //Selection Style Will Be Row Selector
            gridCriteria.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;

            //Stop Updating
            gridCriteria.DisplayLayout.Override.AllowUpdate = DefaultableBoolean.False;

            //Hiding +/- Indicator
            gridCriteria.DisplayLayout.Override.ExpansionIndicator = ShowExpansionIndicator.Never;

            //Hide Group Box Display
            e.Layout.GroupByBox.Hidden = true;

            //Use Filtering
            gridCriteria.DisplayLayout.Override.AllowRowFiltering = DefaultableBoolean.False;
        }

        private void gridSec_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            gridSec.DisplayLayout.Bands[0].Columns["SectId"].Hidden = true;
            gridSec.DisplayLayout.Bands[0].Columns["SectName"].Width = 215;
            gridSec.DisplayLayout.Bands[0].Columns["SectName"].Header.Caption = "Section";

            //Change alternate color
            gridSec.DisplayLayout.Override.RowAlternateAppearance.BackColor = Color.Cyan;
            gridSec.DisplayLayout.Override.RowAlternateAppearance.ForeColor = Color.DarkBlue;

            
            //Select Full Row when click on any cell
            e.Layout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;

            //Selection Style Will Be Row Selector
            this.gridSec.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;

            //Stop Updating
            this.gridSec.DisplayLayout.Override.AllowUpdate = DefaultableBoolean.False;

            //Hiding +/- Indicator
            this.gridSec.DisplayLayout.Override.ExpansionIndicator = ShowExpansionIndicator.Never;
            e.Layout.Override.FilterUIType = FilterUIType.FilterRow;
            //Hide Group Box Display
            e.Layout.GroupByBox.Hidden = true;
        }

        private void gridEmp_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            gridEmp.DisplayLayout.Bands[0].Columns["empId"].Hidden = true;
            gridEmp.DisplayLayout.Bands[0].Columns["empCode"].Width = 110;
            gridEmp.DisplayLayout.Bands[0].Columns["empCode"].Header.Caption = "Employee Code";
            gridEmp.DisplayLayout.Bands[0].Columns["EmpName"].Width = 265;
            gridEmp.DisplayLayout.Bands[0].Columns["EmpName"].Header.Caption = "Employee Name";

            //Change alternate color
            gridEmp.DisplayLayout.Override.RowAlternateAppearance.BackColor = Color.Cyan;
            gridEmp.DisplayLayout.Override.RowAlternateAppearance.ForeColor = Color.DarkBlue;


            //Select Full Row when click on any cell
            e.Layout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;

            //Selection Style Will Be Row Selector
            this.gridEmp.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;

            //Stop Updating
            this.gridEmp.DisplayLayout.Override.AllowUpdate = DefaultableBoolean.False;

            //Hiding +/- Indicator
            this.gridEmp.DisplayLayout.Override.ExpansionIndicator = ShowExpansionIndicator.Never;
            e.Layout.Override.FilterUIType = FilterUIType.FilterRow;
            //Hide Group Box Display
            e.Layout.GroupByBox.Hidden = true;

        }

        private void btnPreview_Click(object sender, EventArgs e)
        {
            dsDetails = new DataSet();

            ArrayList arQuery = new ArrayList();
            GTRLibrary.clsConnection clsCon = new GTRLibrary.clsConnection();

            try
            {
                string ReportPath = "", SQLQuery = "", DataSourceName = "DataSet1", FormCaption = "";

                DataSourceName = "DataSet1";
                FormCaption = "Report :: Leave Information...";

                if (gridCriteria.ActiveRow.Cells[0].Value.ToString().ToUpper() == "General".ToUpper())
                {
                    ReportPath = Common.Classes.clsMain.AppPath + @"\Attendence\Reports\rptLeaveList.rdlc";
                    SQLQuery = "Exec rptLeave " + Common.Classes.clsMain.intComId + ", '" + clsProc.GTRDate(dtDateFrom.Value.ToString()) + "', '" + clsProc.GTRDate(dtDateTo.Value.ToString()) + "', '" + (gridSec.ActiveRow.Cells["sectid"].Value.ToString()) + "',  '" + (gridEmp.ActiveRow.Cells["empid"].Value.ToString()) + "','" + (gridActive.ActiveRow.Cells["SL"].Value.ToString()) + "','=ALL=','General'";
                }

                else if (gridCriteria.ActiveRow.Cells[0].Value.ToString().ToUpper() == "ML".ToUpper())
                {
                    ReportPath = Common.Classes.clsMain.AppPath + @"\Attendence\Reports\rptLeaveList.rdlc";
                    SQLQuery = "Exec rptLeave " + Common.Classes.clsMain.intComId + ", '" + clsProc.GTRDate(dtDateFrom.Value.ToString()) + "', '" + clsProc.GTRDate(dtDateTo.Value.ToString()) + "', '" + (gridSec.ActiveRow.Cells["sectid"].Value.ToString()) + "',  '" + (gridEmp.ActiveRow.Cells["empid"].Value.ToString()) + "','" + (gridActive.ActiveRow.Cells["SL"].Value.ToString()) + "','" + (gridEmpType.ActiveRow.Cells["varName"].Value.ToString()) + "', 'ML'";
                }
                else if (gridCriteria.ActiveRow.Cells[0].Value.ToString().ToUpper() == "FirstML".ToUpper())
                {
                    ReportPath = Common.Classes.clsMain.AppPath + @"\Attendence\Reports\rptLeaveList.rdlc";
                    SQLQuery = "Exec rptLeave " + Common.Classes.clsMain.intComId + ", '" + clsProc.GTRDate(dtDateFrom.Value.ToString()) + "', '" + clsProc.GTRDate(dtDateTo.Value.ToString()) + "', '" + (gridSec.ActiveRow.Cells["sectid"].Value.ToString()) + "',  '" + (gridEmp.ActiveRow.Cells["empid"].Value.ToString()) + "','" + (gridActive.ActiveRow.Cells["SL"].Value.ToString()) + "','" + (gridEmpType.ActiveRow.Cells["varName"].Value.ToString()) + "', 'FirstML'";
                }
                else if (gridCriteria.ActiveRow.Cells[0].Value.ToString().ToUpper() == "LastML".ToUpper())
                {
                    ReportPath = Common.Classes.clsMain.AppPath + @"\Attendence\Reports\rptLeaveList.rdlc";
                    SQLQuery = "Exec rptLeave " + Common.Classes.clsMain.intComId + ", '" + clsProc.GTRDate(dtDateFrom.Value.ToString()) + "', '" + clsProc.GTRDate(dtDateTo.Value.ToString()) + "', '" + (gridSec.ActiveRow.Cells["sectid"].Value.ToString()) + "',  '" + (gridEmp.ActiveRow.Cells["empid"].Value.ToString()) + "','" + (gridActive.ActiveRow.Cells["SL"].Value.ToString()) + "','" + (gridEmpType.ActiveRow.Cells["varName"].Value.ToString()) + "', 'LastML'";
                }
                else if (gridCriteria.ActiveRow.Cells[0].Value.ToString().ToUpper() == "Details".ToUpper())
                {
                    ReportPath = Common.Classes.clsMain.AppPath + @"\Attendence\Reports\rptLeaveDetails.rdlc";
                    SQLQuery = "Exec rptLeaveBalance " + Common.Classes.clsMain.intComId + ", '" + cboFinyear.Value.ToString() + "', '" + (gridSec.ActiveRow.Cells["sectid"].Value.ToString()) + "',  '" + (gridEmp.ActiveRow.Cells["empid"].Value.ToString()) + "','" + (gridActive.ActiveRow.Cells["SL"].Value.ToString()) + "','" + (gridEmpType.ActiveRow.Cells["varName"].Value.ToString()) + "', 'Details'";
                }
                else if (gridCriteria.ActiveRow.Cells[0].Value.ToString().ToUpper() == "Summary".ToUpper())
                {
                    ReportPath = Common.Classes.clsMain.AppPath + @"\Attendence\Reports\rptLeaveSum.rdlc";
                    SQLQuery = "Exec rptLeaveBalance " + Common.Classes.clsMain.intComId + ", " + cboFinyear.Value.ToString() + ",'" + (gridSec.ActiveRow.Cells["sectid"].Value.ToString()) + "',  '" + (gridEmp.ActiveRow.Cells["empid"].Value.ToString()) + "','" + (gridActive.ActiveRow.Cells["SL"].Value.ToString()) + "','" + (gridEmpType.ActiveRow.Cells["varName"].Value.ToString()) + "','Summary'";
                }

                else if (gridCriteria.ActiveRow.Cells[0].Value.ToString().ToUpper() == "MonthDetails".ToUpper())
                {
                    ReportPath = Common.Classes.clsMain.AppPath + @"\Attendence\Reports\rptLeaveMonthDetails.rdlc";
                    SQLQuery = "Exec rptLeaveBalanceDetails " + Common.Classes.clsMain.intComId + ", " + cboFinyear.Value.ToString() + ",'" + (gridSec.ActiveRow.Cells["sectid"].Value.ToString()) + "',  '" + (gridEmp.ActiveRow.Cells["empid"].Value.ToString()) + "','" + (gridActive.ActiveRow.Cells["SL"].Value.ToString()) + "','" + (gridEmpType.ActiveRow.Cells["varName"].Value.ToString()) + "', 'MonthDetails'";
                    
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
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void group1_Click(object sender, EventArgs e)
        {

        }

        private void cboFinyear_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            cboFinyear.DisplayLayout.Bands[0].Columns["dtOpBal"].Width = cboFinyear.Width;
            cboFinyear.DisplayLayout.Bands[0].Columns["dtOpBal"].Header.Caption = "Finential Year";
        }

        private void gridCriteria_TextChanged(object sender, EventArgs e)
        {
          
        }

        private void gridActive_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            gridActive.DisplayLayout.Bands[0].Columns["SL"].Hidden = true;
            gridActive.DisplayLayout.Bands[0].Columns["ActiveYN"].Width = 215;
            gridActive.DisplayLayout.Bands[0].Columns["ActiveYN"].Header.Caption = "ActiveYN";

            //Change alternate color
            gridActive.DisplayLayout.Override.RowAlternateAppearance.BackColor = Color.Cyan;
            gridActive.DisplayLayout.Override.RowAlternateAppearance.ForeColor = Color.DarkBlue;


            //Select Full Row when click on any cell
            e.Layout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;

            //Selection Style Will Be Row Selector
            this.gridActive.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;

            //Stop Updating
            this.gridActive.DisplayLayout.Override.AllowUpdate = DefaultableBoolean.False;

            //Hiding +/- Indicator
            this.gridActive.DisplayLayout.Override.ExpansionIndicator = ShowExpansionIndicator.Never;
            e.Layout.Override.FilterUIType = FilterUIType.FilterRow;
            //Hide Group Box Display
            e.Layout.GroupByBox.Hidden = true;
        }

        private void gridEmpType_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            gridEmpType.DisplayLayout.Bands[0].Columns["varId"].Hidden = true;
            gridEmpType.DisplayLayout.Bands[0].Columns["varName"].Width = 215;
            gridEmpType.DisplayLayout.Bands[0].Columns["varName"].Header.Caption = "Emp Type";

            //Change alternate color
            gridEmpType.DisplayLayout.Override.RowAlternateAppearance.BackColor = Color.Cyan;
            gridEmpType.DisplayLayout.Override.RowAlternateAppearance.ForeColor = Color.DarkBlue;


            //Select Full Row when click on any cell
            e.Layout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;

            //Selection Style Will Be Row Selector
            this.gridEmpType.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;

            //Stop Updating
            this.gridEmpType.DisplayLayout.Override.AllowUpdate = DefaultableBoolean.False;

            //Hiding +/- Indicator
            this.gridEmpType.DisplayLayout.Override.ExpansionIndicator = ShowExpansionIndicator.Never;
            e.Layout.Override.FilterUIType = FilterUIType.FilterRow;
            //Hide Group Box Display
            e.Layout.GroupByBox.Hidden = true;
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



            if (gridCriteria.ActiveRow.Cells[0].Value.ToString().ToUpper() == "General".ToUpper())
            {

                string sqlquary = "Exec rptLeave " + Common.Classes.clsMain.intComId + ", '" + clsProc.GTRDate(dtDateFrom.Value.ToString()) + "', '" + clsProc.GTRDate(dtDateTo.Value.ToString()) + "', '" + (gridSec.ActiveRow.Cells["sectid"].Value.ToString()) + "',  '" + (gridEmp.ActiveRow.Cells["empid"].Value.ToString()) + "','" + (gridActive.ActiveRow.Cells["SL"].Value.ToString()) + "','" + (gridEmpType.ActiveRow.Cells["varName"].Value.ToString()) + "','General'";
                clscon.GTRFillDatasetWithSQLCommand(ref dsList, sqlquary);

                dsList.Tables[0].TableName = "tblLeaveDetails";

                gridExcel.DataSource = null;
                gridExcel.DataSource = dsList.Tables["tblLeaveDetails"];
            }

            else if (gridCriteria.ActiveRow.Cells[0].Value.ToString().ToUpper() == "ML".ToUpper())
            {
                string sqlquary = "Exec rptLeave " + Common.Classes.clsMain.intComId + ", '" + clsProc.GTRDate(dtDateFrom.Value.ToString()) + "', '" + clsProc.GTRDate(dtDateTo.Value.ToString()) + "', '" + (gridSec.ActiveRow.Cells["sectid"].Value.ToString()) + "',  '" + (gridEmp.ActiveRow.Cells["empid"].Value.ToString()) + "','" + (gridActive.ActiveRow.Cells["SL"].Value.ToString()) + "','" + (gridEmpType.ActiveRow.Cells["varName"].Value.ToString()) + "', 'ML'";
                clscon.GTRFillDatasetWithSQLCommand(ref dsList, sqlquary);

                dsList.Tables[0].TableName = "tblLeaveDetails";

                gridExcel.DataSource = null;
                gridExcel.DataSource = dsList.Tables["tblLeaveDetails"];
            }
            else if (gridCriteria.ActiveRow.Cells[0].Value.ToString().ToUpper() == "FirstML".ToUpper())
            {
                string sqlquary = "Exec rptLeave " + Common.Classes.clsMain.intComId + ", '" + clsProc.GTRDate(dtDateFrom.Value.ToString()) + "', '" + clsProc.GTRDate(dtDateTo.Value.ToString()) + "', '" + (gridSec.ActiveRow.Cells["sectid"].Value.ToString()) + "',  '" + (gridEmp.ActiveRow.Cells["empid"].Value.ToString()) + "','" + (gridActive.ActiveRow.Cells["SL"].Value.ToString()) + "','" + (gridEmpType.ActiveRow.Cells["varName"].Value.ToString()) + "', 'FirstML'";
                clscon.GTRFillDatasetWithSQLCommand(ref dsList, sqlquary);

                dsList.Tables[0].TableName = "tblLeaveDetails";

                gridExcel.DataSource = null;
                gridExcel.DataSource = dsList.Tables["tblLeaveDetails"];
            }

            else if (gridCriteria.ActiveRow.Cells[0].Value.ToString().ToUpper() == "LastML".ToUpper())
            { 
                string sqlquary = "Exec rptLeave " + Common.Classes.clsMain.intComId + ", '" + clsProc.GTRDate(dtDateFrom.Value.ToString()) + "', '" + clsProc.GTRDate(dtDateTo.Value.ToString()) + "', '" + (gridSec.ActiveRow.Cells["sectid"].Value.ToString()) + "',  '" + (gridEmp.ActiveRow.Cells["empid"].Value.ToString()) + "','" + (gridActive.ActiveRow.Cells["SL"].Value.ToString()) + "','" + (gridEmpType.ActiveRow.Cells["varName"].Value.ToString()) + "', 'LastML'";
                clscon.GTRFillDatasetWithSQLCommand(ref dsList, sqlquary);

                dsList.Tables[0].TableName = "tblLeaveDetails";

                gridExcel.DataSource = null;
                gridExcel.DataSource = dsList.Tables["tblLeaveDetails"];
            }

            else if (gridCriteria.ActiveRow.Cells[0].Value.ToString().ToUpper() == "Details".ToUpper())
            {
                string sqlquary = "Exec rptLeaveBalance " + Common.Classes.clsMain.intComId + ", '" + cboFinyear.Value.ToString() + "', '" + (gridSec.ActiveRow.Cells["sectid"].Value.ToString()) + "',  '" + (gridEmp.ActiveRow.Cells["empid"].Value.ToString()) + "','" + (gridActive.ActiveRow.Cells["SL"].Value.ToString()) + "','" + (gridEmpType.ActiveRow.Cells["varName"].Value.ToString()) + "', 'Details'";
                clscon.GTRFillDatasetWithSQLCommand(ref dsList, sqlquary);

                dsList.Tables[0].TableName = "tblLeaveDetails";

                gridExcel.DataSource = null;
                gridExcel.DataSource = dsList.Tables["tblLeaveDetails"];
            }

            else if (gridCriteria.ActiveRow.Cells[0].Value.ToString().ToUpper() == "Summary".ToUpper())
            {
                string sqlquary = "Exec rptLeaveBalance " + Common.Classes.clsMain.intComId + ", " + cboFinyear.Value.ToString() + ",'" + (gridSec.ActiveRow.Cells["sectid"].Value.ToString()) + "',  '" + (gridEmp.ActiveRow.Cells["empid"].Value.ToString()) + "','" + (gridActive.ActiveRow.Cells["SL"].Value.ToString()) + "','" + (gridEmpType.ActiveRow.Cells["varName"].Value.ToString()) + "','Summary'";
                clscon.GTRFillDatasetWithSQLCommand(ref dsList, sqlquary);

                dsList.Tables[0].TableName = "tblLeaveDetails";

                gridExcel.DataSource = null;
                gridExcel.DataSource = dsList.Tables["tblLeaveDetails"];
            }

            else if (gridCriteria.ActiveRow.Cells[0].Value.ToString().ToUpper() == "MonthDetails".ToUpper())
            {
                string sqlquary = "Exec rptLeaveBalanceDetails " + Common.Classes.clsMain.intComId + ", " + cboFinyear.Value.ToString() + ",'" + (gridSec.ActiveRow.Cells["sectid"].Value.ToString()) + "',  '" + (gridEmp.ActiveRow.Cells["empid"].Value.ToString()) + "','" + (gridActive.ActiveRow.Cells["SL"].Value.ToString()) + "','" + (gridEmpType.ActiveRow.Cells["varName"].Value.ToString()) + "', 'MonthDetails'";
                clscon.GTRFillDatasetWithSQLCommand(ref dsList, sqlquary);

                dsList.Tables[0].TableName = "tblLeaveDetails";

                gridExcel.DataSource = null;
                gridExcel.DataSource = dsList.Tables["tblLeaveDetails"];

            }


            DialogResult dlgRes =
            MessageBox.Show("Do You Want to Save the Data Sheet");
            if (dlgRes != DialogResult.OK)
            {
                return;
            }

            SaveFileDialog dlgSurveyExcel = new SaveFileDialog();
            dlgSurveyExcel.Filter = "Excel WorkBook (*.xls)|.xls";
            dlgSurveyExcel.FileName = gridCriteria.ActiveRow.Cells["Criteria"].Text.ToString() + "_" + DateTime.Now.ToShortDateString().Replace(@"/", "_");

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

        private void gridExcel_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            //Change alternate color
            gridExcel.DisplayLayout.Override.RowAlternateAppearance.BackColor = Color.Cyan;
            gridExcel.DisplayLayout.Override.RowAlternateAppearance.ForeColor = Color.DarkBlue;

            //Select Full Row when click on any cell
            e.Layout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;

            //Selection Style Will Be Row Selector
            gridExcel.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;

            //Stop Updating
            gridExcel.DisplayLayout.Override.AllowUpdate = DefaultableBoolean.False;

            //Hiding +/- Indicator
            gridExcel.DisplayLayout.Override.ExpansionIndicator = ShowExpansionIndicator.Never;

            //Hide Group Box Display
            e.Layout.GroupByBox.Hidden = true;
        }
   }
}