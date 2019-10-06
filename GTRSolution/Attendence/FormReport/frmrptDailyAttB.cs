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
    public partial class frmrptDailyAttB : Form
    {
        System.Data.DataSet dsList;
        System.Data.DataSet dsDetails;
        clsProcedure clsProc = new clsProcedure();

        string ReportPath = "", rptQuery = "", DataSourceName = "DataSet1", FormCaption = "";

        private Infragistics.Win.UltraWinTabControl.UltraTabControl uTab;
        Common.FormEntry.frmMaster FM;
        public frmrptDailyAttB(ref Infragistics.Win.UltraWinTabControl.UltraTabControl utab, Common.FormEntry.frmMaster fm)
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
                string sqlquary = "Exec prcrptDaily " + Common.Classes.clsMain.intComId;
                clscon.GTRFillDatasetWithSQLCommand(ref dsList, sqlquary);
               
                dsList.Tables[0].TableName = "Section";
                dsList.Tables[1].TableName = "Type";
                dsList.Tables[2].TableName = "ShiftTime";
                dsList.Tables[3].TableName = "Employee";
                dsList.Tables[4].TableName = "Band";

                gridSection.DataSource = null;
                gridSection.DataSource = dsList.Tables["Section"];

                gridType.DataSource = null;
                gridType.DataSource = dsList.Tables["Type"];

                gridShiftTime.DataSource = null;
                gridShiftTime.DataSource = dsList.Tables["ShiftTime"];

                gridEmployeeID.DataSource = null;
                gridEmployeeID.DataSource = dsList.Tables["Employee"];

                gridBand.DataSource = null;
                gridBand.DataSource = dsList.Tables["Band"];

                dtDateFrom.Value = DateTime.Now;

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

        private void gridShiftTime_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            gridShiftTime.DisplayLayout.Bands[0].Columns["ShiftCode"].Hidden = true;
            gridShiftTime.DisplayLayout.Bands[0].Columns["shiftid"].Hidden = true;
            gridShiftTime.DisplayLayout.Bands[0].Columns["ShiftName"].Width = 175;
            gridShiftTime.DisplayLayout.Bands[0].Columns["ShiftName"].Header.Caption = "Shift Name";

            //Change alternate color
            gridShiftTime.DisplayLayout.Override.RowAlternateAppearance.BackColor = Color.Cyan;
            gridShiftTime.DisplayLayout.Override.RowAlternateAppearance.ForeColor = Color.DarkBlue;

            //Select Full Row when click on any cell
            e.Layout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;

            //Selection Style Will Be Row Selector
            gridShiftTime.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;

            //Stop Updating
            gridShiftTime.DisplayLayout.Override.AllowUpdate = DefaultableBoolean.False;

            //Hiding +/- Indicator
            gridShiftTime.DisplayLayout.Override.ExpansionIndicator = ShowExpansionIndicator.Never;

            //Hide Group Box Display
            e.Layout.GroupByBox.Hidden = true;

            //Use Filtering
            //gridShiftTime.DisplayLayout.Override.AllowRowFiltering = DefaultableBoolean.False;

            e.Layout.Override.FilterUIType = FilterUIType.FilterRow;
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


        private void gridEmployeeID_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            gridEmployeeID.DisplayLayout.Bands[0].Columns["empId"].Hidden = true;
            gridEmployeeID.DisplayLayout.Bands[0].Columns["empCode"].Width = 95;

            gridEmployeeID.DisplayLayout.Bands[0].Columns["isChecked"].Width = 55;
            this.gridEmployeeID.DisplayLayout.Bands[0].Columns["isChecked"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox;
            gridEmployeeID.DisplayLayout.Bands[0].Columns["empCode"].Header.Caption = "Employee Code";
            gridEmployeeID.DisplayLayout.Bands[0].Columns["EmpName"].Width = 215;
            gridEmployeeID.DisplayLayout.Bands[0].Columns["EmpName"].Header.Caption = "Employee Name";

            //Stop Cell Modify
            //gridEmployeeID.DisplayLayout.Bands[0].Columns["EmpCode"].CellActivation = Activation.NoEdit;
            //gridEmployeeID.DisplayLayout.Bands[0].Columns["EmpName"].CellActivation = Activation.NoEdit;

            //Change alternate color
            gridEmployeeID.DisplayLayout.Override.RowAlternateAppearance.BackColor = Color.Cyan;
            gridEmployeeID.DisplayLayout.Override.RowAlternateAppearance.ForeColor = Color.DarkBlue;

            //Select Full Row when click on any cell
           // e.Layout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;

            //Selection Style Will Be Row Selector
           // gridEmployeeID.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;

            //Stop Updating
            //gridEmployeeID.DisplayLayout.Override.AllowUpdate = DefaultableBoolean.False;

            //Hiding +/- Indicator
            gridEmployeeID.DisplayLayout.Override.ExpansionIndicator = ShowExpansionIndicator.Never;

            //Hide Group Box Display
            e.Layout.GroupByBox.Hidden = true;

            //Use Filtering
            //gridEmployeeID.DisplayLayout.Override.AllowRowFiltering = DefaultableBoolean.False;

            e.Layout.Override.FilterUIType = FilterUIType.FilterRow;
        }

        
        private void prcShowAbsent(string Param)
        {
                       
            try
            {
                DataSourceName = "DataSet1";
                FormCaption = "Report :: Absent Information...";
                

                string SectId = "0", DesigId = "0", ShiftId = "0", EmpId = "0";

                if (optCriteria.Value.ToString().ToUpper() == "All".ToUpper())
                {
                }
                else if (optCriteria.Value.ToString().ToUpper() == "Section".ToUpper())
                {
                    SectId = gridSection.ActiveRow.Cells["SectId"].Value.ToString();
                }
                if (optCriteria.Value.ToString().ToUpper() == "Employee".ToUpper())
                {
                    EmpId = gridEmployeeID.ActiveRow.Cells["EmpId"].Value.ToString();
                }

                rptQuery = "Exec rptAttend " + Common.Classes.clsMain.intComId + ", '" + clsProc.GTRDate(dtDateFrom.Value.ToString()) + "', '" + clsProc.GTRDate(dtDateTo.Value.ToString()) + "', " + ShiftId + ", " + DesigId + ", " + SectId + ", '', '', "+EmpId+",'"+Param+"'";

                clsReport.strReportPathMain = ReportPath;
                clsReport.strQueryMain = rptQuery;
                clsReport.strDSNMain = DataSourceName;

                FM.prcShowReport(FormCaption);
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
               // ReportPath = null;
            }            
            }


        private void prcShowReport(string Param, string Param2="")
        {
            dsDetails = new DataSet();

            ArrayList arQuery = new ArrayList();
            GTRLibrary.clsConnection clsCon = new GTRLibrary.clsConnection();
            string sqlQuery = "";

            try
            {
                DataSourceName = "DataSet1";
                FormCaption = "Report :: Attendance Information**...";

                string SectId = "0", DesigId = "0", ShiftId = "0", EmpId = "0", EmpType = " ",Band = " ";

                if (optCriteria.Value.ToString().ToUpper() == "All".ToUpper())
                {
                    EmpType = gridType.ActiveRow.Cells["varName"].Value.ToString();
                    Band = gridBand.ActiveRow.Cells["varName"].Value.ToString();
                }
                else if (optCriteria.Value.ToString().ToUpper() == "Section".ToUpper())
                {
                    SectId = gridSection.ActiveRow.Cells["SectId"].Value.ToString();
                    ShiftId = gridShiftTime.ActiveRow.Cells["shiftid"].Value.ToString();
                    Band = gridBand.ActiveRow.Cells["varName"].Value.ToString();
                    EmpType = gridType.ActiveRow.Cells["varName"].Value.ToString();
                }

                else if (optCriteria.Value.ToString().ToUpper() == "Shifting".ToUpper())
                {
                    ShiftId = gridShiftTime.ActiveRow.Cells["shiftid"].Value.ToString();
                    Band = gridBand.ActiveRow.Cells["varName"].Value.ToString();
                    EmpType = gridType.ActiveRow.Cells["varName"].Value.ToString();
                }

                else if (optCriteria.Value.ToString().ToUpper() == "Type".ToUpper())
                {
                    EmpType = gridType.ActiveRow.Cells["varName"].Value.ToString();
                    Band = gridBand.ActiveRow.Cells["varName"].Value.ToString();
                }

                else if (optCriteria.Value.ToString().ToUpper() == "Band".ToUpper())
                {
                    Band = gridBand.ActiveRow.Cells["varName"].Value.ToString();
                    EmpType = gridType.ActiveRow.Cells["varName"].Value.ToString();
                }

                if (optCriteria.Value.ToString().ToUpper() == "Employee".ToUpper())
                {
                    //EmpId = gridEmployeeID.ActiveRow.Cells["EmpId"].Value.ToString();
                    //string EmpId = "";
                    EmpType = gridType.ActiveRow.Cells["varName"].Value.ToString();

                    sqlQuery = "delete tbltempattIndividual ";

                    arQuery.Add(sqlQuery);

                    EmpId = gridEmployeeID.ActiveRow.Cells["empid"].Value.ToString();

                    foreach (UltraGridRow row in gridEmployeeID.Rows)
                    {
                        if (row.Cells["isChecked"].Text.ToString() == "1")
                        {
                            //EmpId += row.Cells["EmpId"].Value + ",";

                            //sqlQuery = " Delete  tblLeave_Balance where empid = '" + row.Cells["empid"].Text.ToString() + "' and dtOpBal =  '" + row.Cells["dtOpeningDate"].Text.ToString() + "'";
                            //arQuery.Add(sqlQuery);


                            sqlQuery = "insert into tbltempattIndividual (EmpId)"
                                       + "values ('" + row.Cells["empid"].Text.ToString() + "')";

                            arQuery.Add(sqlQuery);
                            // EmpId += row.Cells["EmpId"].Value + ",";

                        }

                        
                    }
                    clsCon.GTRSaveDataWithSQLCommand(arQuery);
                    //EmpId = EmpId.Substring(0, EmpId.Length - 1);
                }

                if(Param=="Summary")
                {
                    rptQuery = "Exec rptAttendSum " + Common.Classes.clsMain.intComId + ", '" + clsProc.GTRDate(dtDateFrom.Value.ToString()) + "', '"+Param2+"'";
                    clsCon.GTRFillDatasetWithSQLCommand(ref dsDetails, rptQuery);
                    if (dsDetails.Tables[0].Rows.Count == 0)
                    {
                        MessageBox.Show("Data Not Found");
                        return;
                    }        
                }


                else
                {
                    rptQuery = "Exec rptAttendB " + Common.Classes.clsMain.intComId + ", '" + clsProc.GTRDate(dtDateFrom.Value.ToString()) + "', '" + clsProc.GTRDate(dtDateFrom.Value.ToString()) + "', " + ShiftId + ", " + DesigId + ", " + SectId + ", '', '', " + EmpId + ",'" + EmpType + "','" + Param + "','" + optCriteria.Value.ToString() + "','" + Band + "' ";
                    clsCon.GTRFillDatasetWithSQLCommand(ref dsDetails, rptQuery);
                    
                    if (dsDetails.Tables[0].Rows.Count == 0)
                    {
                        MessageBox.Show("Data Not Found");
                        return;
                    }

                    dsDetails.Tables[0].TableName = "Atten";

                    gridExcel.DataSource = null;
                    gridExcel.DataSource = dsDetails.Tables["Atten"];


                
                }
                

                clsReport.strReportPathMain = ReportPath;
                clsReport.strQueryMain = rptQuery;
                clsReport.dsReport = dsDetails;
                clsReport.strDSNMain = DataSourceName;
                
                FM.prcShowReport(FormCaption);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            finally
            {
                rptQuery = null;
                DataSourceName = null;
                //clsProc = null;
                ReportPath = null;
            }
        }


        private void optCriteria_ValueChanged(object sender, EventArgs e)
        {                        
            gridSection.Enabled = true;
            gridEmployeeID.Enabled = true;
            gridShiftTime.Enabled = true;
            gridBand.Enabled = true;

            if(optCriteria.Value.ToString() =="All")
            {
                gridType.Enabled = true;
                gridSection.Enabled = false;
                gridEmployeeID.Enabled = false;
                gridShiftTime.Enabled = false;
            }
            else if (optCriteria.Value.ToString() == "Section")
            {
                gridType.Enabled = true;
                gridEmployeeID.Enabled = false;
                //gridShiftTime.Enabled = false;
                //gridType.Enabled = false;
            }
            else if (optCriteria.Value.ToString() == "Employee")
            {
                gridType.Enabled = true;
                gridSection.Enabled = false;
                gridShiftTime.Enabled = false;      
            }
            else if (optCriteria.Value.ToString() == "Shifting")
            {
                gridType.Enabled = true;
                gridShiftTime.Enabled = true; 
                gridSection.Enabled = false;
                gridEmployeeID.Enabled = false;

            }
            else if (optCriteria.Value.ToString() == "Type")
            {
                gridType.Enabled = true;
                gridSection.Enabled = false;
                gridEmployeeID.Enabled = false;
                gridShiftTime.Enabled = false; 
            }
        }

        private void btnAttendance_Click(object sender, EventArgs e)

        {
            ReportPath = Common.Classes.clsMain.AppPath + @"\Attendence\Reports\rptAttendPresentB.rdlc";                      
            prcShowReport("Attend");
        }

        private void btnPresent_Click(object sender, EventArgs e)
        {
            ReportPath = Common.Classes.clsMain.AppPath + @"\Attendence\Reports\rptAttendPresentB.rdlc";
            prcShowReport("Present");
        }

        private void btnAbsent_Click(object sender, EventArgs e)
        {
            ReportPath = Common.Classes.clsMain.AppPath + @"\Attendence\Reports\rptAttendAbsent.rdlc";
            prcShowReport("Absent");
        }

        private void btnLate_Click(object sender, EventArgs e)
        {
            ReportPath = Common.Classes.clsMain.AppPath + @"\Attendence\Reports\rptAttendLate.rdlc";
            prcShowReport("Late");
        }

        private void btnMissingOut_Click(object sender, EventArgs e)
        {
            ReportPath = Common.Classes.clsMain.AppPath + @"\Attendence\Reports\rptAttendPresentB.rdlc";
            prcShowReport("Missing Out");
        }

        private void btnLeave_Click(object sender, EventArgs e)
        {
            ReportPath = Common.Classes.clsMain.AppPath + @"\Attendence\Reports\rptAttendLeave.rdlc";
            prcShowReport("Leave");
        }

        private void btnInOut_Click(object sender, EventArgs e)
        {
            ReportPath = Common.Classes.clsMain.AppPath + @"\Attendence\Reports\rptAttendInOut.rdlc";
            prcShowReport("Attend");
        }

        private void btnMovement_Click(object sender, EventArgs e)
        {
            ReportPath = Common.Classes.clsMain.AppPath + @"\Attendence\Reports\rptDailyMovement.rdlc";
            prcShowReport("Movment");
        }

        private void btnSummary_Click(object sender, EventArgs e)
        {
            ReportPath = Common.Classes.clsMain.AppPath + @"\Attendence\Reports\rptAttendSum.rdlc";
            prcShowReport("Summary");
        }

        private void btnOT_Click(object sender, EventArgs e)
        {
            ReportPath = Common.Classes.clsMain.AppPath + @"\Attendence\Reports\rptAttendPresentB.rdlc";
            prcShowReport("Overtime");
        }

        private void btnContAbsent_Click(object sender, EventArgs e)
        {
            ReportPath = Common.Classes.clsMain.AppPath + @"\Attendence\Reports\rptAttendAbsentCont.rdlc";
            prcShowReport("Absent Continuous");
        }

        private void btnOffDay_Click(object sender, EventArgs e)
        {
            ReportPath = Common.Classes.clsMain.AppPath + @"\Attendence\Reports\rptAttendPresentB.rdlc";
            prcShowReport("Off Day");
        }
        private void btnAttendAll_Click(object sender, EventArgs e)
        {
           
            ReportPath = Common.Classes.clsMain.AppPath + @"\Attendence\Reports\rptAttendAll.rdlc";
            prcShowReport("AttendAll");

        }

        private void btnWHPresent_Click(object sender, EventArgs e)
        {

            ReportPath = Common.Classes.clsMain.AppPath + @"\Attendence\Reports\rptAttendAll.rdlc";
            prcShowReport("WHPresent");

        }

        private void btnManualAtt_Click(object sender, EventArgs e)
        {

            try
            {
                string ReportPath = "", SQLQuery = "", DataSourceName = "DataSet1", FormCaption = "";
                dsDetails = new DataSet();


                GTRLibrary.clsConnection clsCon = new GTRLibrary.clsConnection();
                

                DataSourceName = "DataSet1";
                FormCaption = "Report :: Daily Manual Attendance...";

                ReportPath = Common.Classes.clsMain.AppPath + @"\Attendence\Reports\rptAttendFixed.rdlc";
                SQLQuery = "Exec rptAttendFixedManual " + Common.Classes.clsMain.intComId + ", '" + clsProc.GTRDate(dtDateFrom.Value.ToString()) + "', '" + clsProc.GTRDate(dtDateFrom.Value.ToString()) + "' ";


                clsCon.GTRFillDatasetWithSQLCommand(ref dsDetails, SQLQuery);
                if (dsDetails.Tables[0].Rows.Count == 0)
                {
                    MessageBox.Show("Data Not Found");
                    return;
                }


                clsReport.strReportPathMain = ReportPath;
                clsReport.strQueryMain = SQLQuery;
                clsReport.strDSNMain = DataSourceName;

                FM.prcShowReport(FormCaption);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            { 
            
            }
        }

        private void btnSummaryDesig_Click(object sender, EventArgs e)
        {
            ReportPath = Common.Classes.clsMain.AppPath + @"\Attendence\Reports\rptAttendSumDesig.rdlc";
            prcShowReport("Summary","Desig");
        }

        private void frmrptDailyAttB_FormClosing(object sender, FormClosingEventArgs e)
        {
            int index = Common.Classes.clsMain.fncFindOpenFormIndex(this);
            uTab.Tabs.RemoveAt(index);
            Common.Classes.clsMain.fncExistOpenForm(this, "Remove");

            clsProc = null;
            dsList = null;
            dsDetails = null;
            FM = null;
        }

        private void frmrptDailyAttB_Load(object sender, EventArgs e)
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

        private void gridType_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            gridType.DisplayLayout.Bands[0].Columns["aID"].Hidden = true;
            gridType.DisplayLayout.Bands[0].Columns["varName"].Width = 218;
            gridType.DisplayLayout.Bands[0].Columns["varName"].Header.Caption = "Emp Type";

            //Change alternate color
            gridType.DisplayLayout.Override.RowAlternateAppearance.BackColor = Color.Cyan;
            gridType.DisplayLayout.Override.RowAlternateAppearance.ForeColor = Color.DarkBlue;

            //Select Full Row when click on any cell
            e.Layout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;

            //Selection Style Will Be Row Selector
            gridType.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;

            //Stop Updating
            gridType.DisplayLayout.Override.AllowUpdate = DefaultableBoolean.False;

            //Hiding +/- Indicator
            gridType.DisplayLayout.Override.ExpansionIndicator = ShowExpansionIndicator.Never;

            //Hide Group Box Display
            e.Layout.GroupByBox.Hidden = true;

            //Use Filtering
            // gridDesignation.DisplayLayout.Override.AllowRowFiltering = DefaultableBoolean.False;
            e.Layout.Override.FilterUIType = FilterUIType.FilterRow;
        }

        private void gridBand_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            gridBand.DisplayLayout.Bands[0].Columns["varID"].Hidden = true;
            gridBand.DisplayLayout.Bands[0].Columns["varName"].Width = 218;
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
            // gridDesignation.DisplayLayout.Override.AllowRowFiltering = DefaultableBoolean.False;
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
            DialogResult dlgRes =
            MessageBox.Show("Do You Want to Save the Data Sheet");
            if (dlgRes != DialogResult.OK)
            {
                return;
            }

            SaveFileDialog dlgSurveyExcel = new SaveFileDialog();
            dlgSurveyExcel.Filter = "Excel WorkBook (*.xls)|.xls";
            dlgSurveyExcel.FileName = "Attendance List" + "_" + DateTime.Now.ToShortDateString().Replace(@"/", "_");

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
            gridExcel.DisplayLayout.Bands[0].Columns["ComName"].Hidden = true;
            gridExcel.DisplayLayout.Bands[0].Columns["ComAdd1"].Hidden = true;
            gridExcel.DisplayLayout.Bands[0].Columns["ComAdd2"].Hidden = true;
            gridExcel.DisplayLayout.Bands[0].Columns["Caption"].Hidden = true;
            gridExcel.DisplayLayout.Bands[0].Columns["CardNo"].Hidden = true;
            gridExcel.DisplayLayout.Bands[0].Columns["Late"].Hidden = true;
            gridExcel.DisplayLayout.Bands[0].Columns["RegHour"].Hidden = true;
            gridExcel.DisplayLayout.Bands[0].Columns["PStatus"].Hidden = true;
            gridExcel.DisplayLayout.Bands[0].Columns["PTimeIn"].Hidden = true;
            gridExcel.DisplayLayout.Bands[0].Columns["PTimeOut"].Hidden = true;
            gridExcel.DisplayLayout.Bands[0].Columns["AbTn"].Hidden = true;

            gridExcel.DisplayLayout.Bands[0].Columns["dtFromDate"].Hidden = true;
            gridExcel.DisplayLayout.Bands[0].Columns["Remarks"].Hidden = true;
            gridExcel.DisplayLayout.Bands[0].Columns["sSlNo"].Hidden = true;
            gridExcel.DisplayLayout.Bands[0].Columns["DSlNo"].Hidden = true;

            gridExcel.DisplayLayout.Bands[0].Columns["EmpID"].Hidden = true;
            gridExcel.DisplayLayout.Bands[0].Columns["DeptName"].Hidden = true;
            gridExcel.DisplayLayout.Bands[0].Columns["OTHour"].Hidden = true;
            //gridType.DisplayLayout.Bands[0].Columns["DSlNo"].Hidden = true;
            //gridType.DisplayLayout.Bands[0].Columns["varName"].Width = 218;
            //gridType.DisplayLayout.Bands[0].Columns["varName"].Header.Caption = "Emp Type";

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

