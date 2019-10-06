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
    public partial class frmrptIncrement : Form
    {
        System.Data.DataSet dsList;
        System.Data.DataSet dsDetails;
        clsProcedure clsProc = new clsProcedure();

        private Infragistics.Win.UltraWinTabControl.UltraTabControl uTab;
        Common.FormEntry.frmMaster FM;
        public frmrptIncrement(ref Infragistics.Win.UltraWinTabControl.UltraTabControl utab, Common.FormEntry.frmMaster fm)
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
                string sqlquary = "Exec prcrptIncrement "+Common.Classes.clsMain.intComId;
                clscon.GTRFillDatasetWithSQLCommand(ref dsList, sqlquary);
                dsList.Tables[0].TableName = "Criteria";
                dsList.Tables[1].TableName = "IncType";
                dsList.Tables[2].TableName = "EmpStatus";
                dsList.Tables[3].TableName = "EmpType";
                dsList.Tables[4].TableName = "Section";
                dsList.Tables[5].TableName = "Employee";


                gridCriteria.DataSource = dsList.Tables["Criteria"];
                gridEmpStatus.DataSource = dsList.Tables["EmpStatus"];
                gridEmpType.DataSource = dsList.Tables["EmpType"];
                gridArea.DataSource = dsList.Tables["Section"];
                gridEmp.DataSource = dsList.Tables["Employee"];
                gridIncrType.DataSource = dsList.Tables["IncType"];

                DateTime firstDay = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                dtDateFrom.Value = firstDay;

                DateTime lastDay = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                lastDay = lastDay.AddMonths(1);
                lastDay = lastDay.AddDays(-(lastDay.Day));
                dtTo.Value = lastDay;

            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        private  void prcLoadCombo()
        {
            
        }

        private void frmrptSales_Load(object sender, EventArgs e)
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

        private void frmrptSales_FormClosing(object sender, FormClosingEventArgs e)
        {
            int index = Common.Classes.clsMain.fncFindOpenFormIndex(this);
            uTab.Tabs.RemoveAt(index);
            Common.Classes.clsMain.fncExistOpenForm(this, "Remove");
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void gridArea_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            gridArea.DisplayLayout.Bands[0].Columns["SectId"].Hidden = true;
            //gridArea.DisplayLayout.Bands[0].Columns["SLNO"].Hidden = true;
            gridArea.DisplayLayout.Bands[0].Columns["SectName"].Width = 160;
            gridArea.DisplayLayout.Bands[0].Columns["SectName"].Header.Caption = "Section";

            //Change alternate color
            gridArea.DisplayLayout.Override.RowAlternateAppearance.BackColor = Color.Cyan;
            gridArea.DisplayLayout.Override.RowAlternateAppearance.ForeColor = Color.DarkBlue;

            //Select Full Row when click on any cell
            e.Layout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;

            //Selection Style Will Be Row Selector
            gridArea.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;

            //Stop Updating
            gridArea.DisplayLayout.Override.AllowUpdate = DefaultableBoolean.False;

            //Hiding +/- Indicator
            gridArea.DisplayLayout.Override.ExpansionIndicator = ShowExpansionIndicator.Never;

            //Hide Group Box Display
            e.Layout.GroupByBox.Hidden = true;

            //Use Filtering
            gridArea.DisplayLayout.Override.AllowRowFiltering = DefaultableBoolean.False;
        }

       

        private void btnPreview_Click(object sender, EventArgs e)
        {
            dsDetails = new DataSet();
            ArrayList arQuery = new ArrayList();
            clsConnection clsCon = new clsConnection();
            try
            {
                string ReportPath = "", SQLQuery = "", DataSourceName = "DataSet1", FormCaption = "";

                DataSourceName = "DataSet1";
                FormCaption = "Report :: Increment Report ...";
                string SectId = "0", EmpId = "0", Type = "", EmpType = "";


                EmpId = gridEmp.ActiveRow.Cells["EmpId"].Value.ToString();
                SectId = gridArea.ActiveRow.Cells["SectId"].Value.ToString();
                Type = gridIncrType.ActiveRow.Cells["IncrType"].Value.ToString();
                EmpType = gridEmpType.ActiveRow.Cells["EmpType"].Value.ToString();

                if (gridIncrType.ActiveRow.Cells["IncrType"].Text.ToString() == "Increment")
                {
                    ReportPath = Common.Classes.clsMain.AppPath + @"\Attendence\Reports\rptIncrList.rdlc";
                    SQLQuery = "Exec rptIncrement " + Common.Classes.clsMain.intComId + ", '" + clsProc.GTRDate(dtDateFrom.Value.ToString()) + "', '" + clsProc.GTRDate(dtTo.Value.ToString()) + "', '" + EmpType + "', '', " + SectId + ", 0, '" + Type + "'";    
                }

                else if (gridIncrType.ActiveRow.Cells["IncrType"].Text.ToString() == "Promotion")
                {
                    ReportPath = Common.Classes.clsMain.AppPath + @"\Attendence\Reports\rptIncrList.rdlc";
                    SQLQuery = "Exec rptIncrement " + Common.Classes.clsMain.intComId + ", '" + clsProc.GTRDate(dtDateFrom.Value.ToString()) + "', '" + clsProc.GTRDate(dtTo.Value.ToString()) + "', '" + EmpType + "', '', " + SectId + ", 0, '" + Type + "'";

                }

                else if (gridIncrType.ActiveRow.Cells["IncrType"].Text.ToString() == "Increment with Promotion")
                {
                    ReportPath = Common.Classes.clsMain.AppPath + @"\Attendence\Reports\rptIncrList.rdlc";
                    SQLQuery = "Exec rptIncrement " + Common.Classes.clsMain.intComId + ", '" + clsProc.GTRDate(dtDateFrom.Value.ToString()) + "', '" + clsProc.GTRDate(dtTo.Value.ToString()) + "', '" + EmpType + "', '', " + SectId + ", 0, '" + Type + "'";

                }

                else if (gridIncrType.ActiveRow.Cells["IncrType"].Text.ToString() == "Adjustment")
                {
                    ReportPath = Common.Classes.clsMain.AppPath + @"\Attendence\Reports\rptIncrList.rdlc";
                    SQLQuery = "Exec rptIncrement " + Common.Classes.clsMain.intComId + ", '" + clsProc.GTRDate(dtDateFrom.Value.ToString()) + "', '" + clsProc.GTRDate(dtTo.Value.ToString()) + "', '" + EmpType + "', '', " + SectId + ", 0, '" + Type + "'";

                }

                else if (gridIncrType.ActiveRow.Cells["IncrType"].Text.ToString() == "Revised")
                {
                    ReportPath = Common.Classes.clsMain.AppPath + @"\Attendence\Reports\rptIncrList.rdlc";
                    SQLQuery = "Exec rptIncrement " + Common.Classes.clsMain.intComId + ", '" + clsProc.GTRDate(dtDateFrom.Value.ToString()) + "', '" + clsProc.GTRDate(dtTo.Value.ToString()) + "', '" + EmpType + "', '', " + SectId + ", 0, '" + Type + "'";

                }

                else if (gridIncrType.ActiveRow.Cells["IncrType"].Text.ToString() == "Increment Entitle")
                {
                    ReportPath = Common.Classes.clsMain.AppPath + @"\Attendence\Reports\rptIncrEntitle.rdlc";
                    SQLQuery = "Exec rptIncrementEntitle " + Common.Classes.clsMain.intComId + ", '" + clsProc.GTRDate(dtDateFrom.Value.ToString()) + "', '" + clsProc.GTRDate(dtTo.Value.ToString()) + "', '" + EmpType + "', '', " + SectId + ", 0, '" + Type + "'";  

                }

                else if (gridIncrType.ActiveRow.Cells["IncrType"].Text.ToString() == "Confirmation Entitle")
                {
                    ReportPath = Common.Classes.clsMain.AppPath + @"\Attendence\Reports\rptIncrEntitle.rdlc";
                    SQLQuery = "Exec rptIncrementEntitle " + Common.Classes.clsMain.intComId + ", '" + clsProc.GTRDate(dtDateFrom.Value.ToString()) + "', '" + clsProc.GTRDate(dtTo.Value.ToString()) + "', '" + EmpType + "', '', " + SectId + ", 0, '" + Type + "'";

                }

                else if (gridIncrType.ActiveRow.Cells["IncrType"].Text.ToString() == "New Confirmation Entitle")
                {
                    ReportPath = Common.Classes.clsMain.AppPath + @"\Attendence\Reports\rptIncrEntitle.rdlc";
                    SQLQuery = "Exec rptIncrementEntitle " + Common.Classes.clsMain.intComId + ", '" + clsProc.GTRDate(dtDateFrom.Value.ToString()) + "', '" + clsProc.GTRDate(dtTo.Value.ToString()) + "', '" + EmpType + "', '', " + SectId + ", 0, '" + Type + "'";

                }

                //else if (gridIncrType.ActiveRow.Cells["IncrType"].Text.ToString() == "Propose Increment")
                //{
                //    ReportPath = Common.Classes.clsMain.AppPath + @"\Attendence\Reports\rptIncrEntitle.rdlc";
                //    SQLQuery = "Exec rptIncrementEntitle " + Common.Classes.clsMain.intComId + ", '" + clsProc.GTRDate(dtDateFrom.Value.ToString()) + "', '" + clsProc.GTRDate(dtTo.Value.ToString()) + "', '" + EmpType + "', '', " + SectId + ", 0, '" + Type + "'";

                //}

 
             
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
                //clsReport.dsReport = dsDetails;

                //FM.prcShowReport(FormCaption);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                clsCon = null;
                dsDetails = null;
            }
        }
        
        private void dtDateFrom_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyValue);
        }

        private void dtDateTo_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyValue);
        }

        private void gridArea_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyValue);
        }

        private void gridCriteria_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            gridCriteria.DisplayLayout.Bands[0].Columns["CValue"].Hidden = true;
            gridCriteria.DisplayLayout.Bands[0].Columns["SLNo"].Hidden = true;
            gridCriteria.DisplayLayout.Bands[0].Columns["Criteria"].Width = 150;
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

        private void gridEmpStatus_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            //gridCriteria.DisplayLayout.Bands[0].Columns["CValue"].Hidden = true;
            //gridCriteria.DisplayLayout.Bands[0].Columns["SLNo"].Hidden = true;
            gridEmpStatus.DisplayLayout.Bands[0].Columns["EmpStatus"].Width = 150;
            gridEmpStatus.DisplayLayout.Bands[0].Columns["EmpStatus"].Header.Caption = "Employee Status";

            //Change alternate color
            gridEmpStatus.DisplayLayout.Override.RowAlternateAppearance.BackColor = Color.Cyan;
            gridEmpStatus.DisplayLayout.Override.RowAlternateAppearance.ForeColor = Color.DarkBlue;

            //Select Full Row when click on any cell
            e.Layout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;

            //Selection Style Will Be Row Selector
            gridEmpStatus.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;

            //Stop Updating
            gridEmpStatus.DisplayLayout.Override.AllowUpdate = DefaultableBoolean.False;

            //Hiding +/- Indicator
            gridEmpStatus.DisplayLayout.Override.ExpansionIndicator = ShowExpansionIndicator.Never;

            //Hide Group Box Display
            e.Layout.GroupByBox.Hidden = true;

            //Use Filtering
            gridEmpStatus.DisplayLayout.Override.AllowRowFiltering = DefaultableBoolean.False;
        }

        private void gridEmpType_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            //gridCriteria.DisplayLayout.Bands[0].Columns["CValue"].Hidden = true;
            //gridCriteria.DisplayLayout.Bands[0].Columns["SLNo"].Hidden = true;
            gridEmpType.DisplayLayout.Bands[0].Columns["EmpType"].Width = 150;
            gridEmpType.DisplayLayout.Bands[0].Columns["EmpType"].Header.Caption = "Employee Type";

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
            gridEmpType.DisplayLayout.Override.AllowRowFiltering = DefaultableBoolean.False;
        }

        private void gridEmp_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            gridEmp.DisplayLayout.Bands[0].Columns["empCode"].Hidden = true;
            gridEmp.DisplayLayout.Bands[0].Columns["EmpId"].Width = 130;
            gridEmp.DisplayLayout.Bands[0].Columns["EmpId"].Header.Caption = "Employee ID";
            gridEmp.DisplayLayout.Bands[0].Columns["EmpName"].Width = 245;
            gridEmp.DisplayLayout.Bands[0].Columns["EmpName"].Header.Caption = "Employee Name";
            
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
            e.Layout.Override.FilterUIType = FilterUIType.FilterRow;
        }

        private void gridIncrType_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            // gridIncrType.DisplayLayout.Bands[0].Columns["empCode"].Hidden = true;
            // gridIncrType.DisplayLayout.Bands[0].Columns["EmpId"].Width = 130;
            //gridIncrType.DisplayLayout.Bands[0].Columns["EmpId"].Header.Caption = "Employee ID";
            gridIncrType.DisplayLayout.Bands[0].Columns["IncrType"].Width = 245;
            gridIncrType.DisplayLayout.Bands[0].Columns["IncrType"].Header.Caption = "Increment Type";

            //Change alternate color
            gridIncrType.DisplayLayout.Override.RowAlternateAppearance.BackColor = Color.Cyan;
            gridIncrType.DisplayLayout.Override.RowAlternateAppearance.ForeColor = Color.DarkBlue;

            //Select Full Row when click on any cell
            e.Layout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;

            //Selection Style Will Be Row Selector
            gridIncrType.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;

            //Stop Updating
            gridIncrType.DisplayLayout.Override.AllowUpdate = DefaultableBoolean.False;

            //Hiding +/- Indicator
            gridIncrType.DisplayLayout.Override.ExpansionIndicator = ShowExpansionIndicator.Never;

            //Hide Group Box Display
            e.Layout.GroupByBox.Hidden = true;

            //Use Filtering
            gridIncrType.DisplayLayout.Override.AllowRowFiltering = DefaultableBoolean.False;
        }

        private void gridCriteria_AfterRowActivate(object sender, EventArgs e)
        {
            if(gridCriteria.ActiveRow.Cells["Criteria"].Text.ToString().ToUpper()=="Employee Wise".ToUpper())
            {
                gridEmp.Enabled = true;
                gridArea.Enabled = false;
            }
            else if (gridCriteria.ActiveRow.Cells["Criteria"].Text.ToString().ToUpper() == "Section Wise".ToUpper())
            {
                gridEmp.Enabled = false;
                gridArea.Enabled = true;
            }
            else
            {
                gridEmp.Enabled = false;
                gridArea.Enabled = false;
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


                string ReportPath = "", SQLQuery = "", DataSourceName = "DataSet1", FormCaption = "";

                DataSourceName = "DataSet1";
                FormCaption = "Report :: Increment Report ...";
                string SectId = "0", EmpId = "0", Type = "", EmpType = "";


                EmpId = gridEmp.ActiveRow.Cells["EmpId"].Value.ToString();
                SectId = gridArea.ActiveRow.Cells["SectId"].Value.ToString();
                Type = gridIncrType.ActiveRow.Cells["IncrType"].Value.ToString();
                EmpType = gridEmpType.ActiveRow.Cells["EmpType"].Value.ToString();

                if (gridIncrType.ActiveRow.Cells["IncrType"].Text.ToString() == "Increment")
                {
                    SQLQuery = "Exec rptIncrement " + Common.Classes.clsMain.intComId + ", '" + clsProc.GTRDate(dtDateFrom.Value.ToString()) + "', '" + clsProc.GTRDate(dtTo.Value.ToString()) + "', '" + EmpType + "', '', " + SectId + ", 0, '" + Type + "'";
                    clscon.GTRFillDatasetWithSQLCommand(ref dsList, SQLQuery);

                    dsList.Tables[0].TableName = "List";

                    gridExcel.DataSource = null;
                    gridExcel.DataSource = dsList.Tables["List"];
                
                }

                else if (gridIncrType.ActiveRow.Cells["IncrType"].Text.ToString() == "Promotion")
                {
                    SQLQuery = "Exec rptIncrement " + Common.Classes.clsMain.intComId + ", '" + clsProc.GTRDate(dtDateFrom.Value.ToString()) + "', '" + clsProc.GTRDate(dtTo.Value.ToString()) + "', '" + EmpType + "', '', " + SectId + ", 0, '" + Type + "'";
                    clscon.GTRFillDatasetWithSQLCommand(ref dsList, SQLQuery);

                    dsList.Tables[0].TableName = "List";

                    gridExcel.DataSource = null;
                    gridExcel.DataSource = dsList.Tables["List"];
                }

                else if (gridIncrType.ActiveRow.Cells["IncrType"].Text.ToString() == "Increment with Promotion")
                {
                    SQLQuery = "Exec rptIncrement " + Common.Classes.clsMain.intComId + ", '" + clsProc.GTRDate(dtDateFrom.Value.ToString()) + "', '" + clsProc.GTRDate(dtTo.Value.ToString()) + "', '" + EmpType + "', '', " + SectId + ", 0, '" + Type + "'";
                    clscon.GTRFillDatasetWithSQLCommand(ref dsList, SQLQuery);

                    dsList.Tables[0].TableName = "List";

                    gridExcel.DataSource = null;
                    gridExcel.DataSource = dsList.Tables["List"];
                }

                else if (gridIncrType.ActiveRow.Cells["IncrType"].Text.ToString() == "Adjustment")
                {
                    SQLQuery = "Exec rptIncrement " + Common.Classes.clsMain.intComId + ", '" + clsProc.GTRDate(dtDateFrom.Value.ToString()) + "', '" + clsProc.GTRDate(dtTo.Value.ToString()) + "', '" + EmpType + "', '', " + SectId + ", 0, '" + Type + "'";
                    clscon.GTRFillDatasetWithSQLCommand(ref dsList, SQLQuery);

                    dsList.Tables[0].TableName = "List";

                    gridExcel.DataSource = null;
                    gridExcel.DataSource = dsList.Tables["List"];
                }

                else if (gridIncrType.ActiveRow.Cells["IncrType"].Text.ToString() == "Revised")
                {
                    SQLQuery = "Exec rptIncrement " + Common.Classes.clsMain.intComId + ", '" + clsProc.GTRDate(dtDateFrom.Value.ToString()) + "', '" + clsProc.GTRDate(dtTo.Value.ToString()) + "', '" + EmpType + "', '', " + SectId + ", 0, '" + Type + "'";
                    clscon.GTRFillDatasetWithSQLCommand(ref dsList, SQLQuery);

                    dsList.Tables[0].TableName = "List";

                    gridExcel.DataSource = null;
                    gridExcel.DataSource = dsList.Tables["List"];
                }

                else if (gridIncrType.ActiveRow.Cells["IncrType"].Text.ToString() == "Increment Entitle")
                {
                    SQLQuery = "Exec rptIncrementEntitle " + Common.Classes.clsMain.intComId + ", '" + clsProc.GTRDate(dtDateFrom.Value.ToString()) + "', '" + clsProc.GTRDate(dtTo.Value.ToString()) + "', '" + EmpType + "', '', " + SectId + ", 0, '" + Type + "'";
                    clscon.GTRFillDatasetWithSQLCommand(ref dsList, SQLQuery);

                    dsList.Tables[0].TableName = "List";

                    gridExcel.DataSource = null;
                    gridExcel.DataSource = dsList.Tables["List"];
                }

                else if (gridIncrType.ActiveRow.Cells["IncrType"].Text.ToString() == "Confirmation Entitle")
                {
                    SQLQuery = "Exec rptIncrementEntitle " + Common.Classes.clsMain.intComId + ", '" + clsProc.GTRDate(dtDateFrom.Value.ToString()) + "', '" + clsProc.GTRDate(dtTo.Value.ToString()) + "', '" + EmpType + "', '', " + SectId + ", 0, '" + Type + "'";
                    clscon.GTRFillDatasetWithSQLCommand(ref dsList, SQLQuery);

                    dsList.Tables[0].TableName = "List";

                    gridExcel.DataSource = null;
                    gridExcel.DataSource = dsList.Tables["List"];
                }

                else if (gridIncrType.ActiveRow.Cells["IncrType"].Text.ToString() == "New Confirmation Entitle")
                {
                    SQLQuery = "Exec rptIncrementEntitle " + Common.Classes.clsMain.intComId + ", '" + clsProc.GTRDate(dtDateFrom.Value.ToString()) + "', '" + clsProc.GTRDate(dtTo.Value.ToString()) + "', '" + EmpType + "', '', " + SectId + ", 0, '" + Type + "'";
                    clscon.GTRFillDatasetWithSQLCommand(ref dsList, SQLQuery);

                    dsList.Tables[0].TableName = "List";

                    gridExcel.DataSource = null;
                    gridExcel.DataSource = dsList.Tables["List"];
                }
            
            DialogResult dlgRes =
            MessageBox.Show("Do You Want to Save the Data Sheet");
            if (dlgRes != DialogResult.OK)
            {
                return;
            }

            SaveFileDialog dlgSurveyExcel = new SaveFileDialog();
            dlgSurveyExcel.Filter = "Excel WorkBook (*.xls)|.xls";
            dlgSurveyExcel.FileName = gridIncrType.ActiveRow.Cells["IncrType"].Text.ToString() + "_" + DateTime.Now.ToShortDateString().Replace(@"/", "_");

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

            //Use Filtering
            // gridDesignation.DisplayLayout.Override.AllowRowFiltering = DefaultableBoolean.False;
            //e.Layout.Override.FilterUIType = FilterUIType.FilterRow;
        }
    }
}