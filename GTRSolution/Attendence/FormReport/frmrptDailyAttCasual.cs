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
    public partial class frmrptDailyAttCasual : Form
    {
        System.Data.DataSet dsList;
        System.Data.DataSet dsDetails;
        clsProcedure clsProc = new clsProcedure();

        string ReportPath = "", rptQuery = "", DataSourceName = "DataSet1", FormCaption = "";

        private Infragistics.Win.UltraWinTabControl.UltraTabControl uTab;
        Common.FormEntry.frmMaster FM;
        public frmrptDailyAttCasual(ref Infragistics.Win.UltraWinTabControl.UltraTabControl utab, Common.FormEntry.frmMaster fm)
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
                string sqlquary = "Exec prcrptDailyCasual " + Common.Classes.clsMain.intComId;
                clscon.GTRFillDatasetWithSQLCommand(ref dsList, sqlquary);
               
                dsList.Tables[0].TableName = "Section";
                dsList.Tables[1].TableName = "Type";
                dsList.Tables[2].TableName = "Employee";

                gridSection.DataSource = null;
                gridSection.DataSource = dsList.Tables["Section"];

                gridType.DataSource = null;
                gridType.DataSource = dsList.Tables["Type"];


                gridEmployeeID.DataSource = null;
                gridEmployeeID.DataSource = dsList.Tables["Employee"];

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
            gridSection.DisplayLayout.Bands[0].Columns["SectName"].Width = 300;
            gridSection.DisplayLayout.Bands[0].Columns["SectName"].Header.Caption = "Company";

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
            gridEmployeeID.DisplayLayout.Bands[0].Columns["empCode"].Width = 115;

            gridEmployeeID.DisplayLayout.Bands[0].Columns["isChecked"].Width = 55;
            this.gridEmployeeID.DisplayLayout.Bands[0].Columns["isChecked"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox;
            gridEmployeeID.DisplayLayout.Bands[0].Columns["empCode"].Header.Caption = "Employee Code";
            gridEmployeeID.DisplayLayout.Bands[0].Columns["EmpName"].Width = 280;
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
                FormCaption = "Report :: Attendance Information (Casual Worker)...";

                string SectId = "0", DesigId = "0", ShiftId = "0", EmpId = "0", EmpType = "=All=";

                EmpType = gridType.ActiveRow.Cells["varName"].Value.ToString();

                if (optCriteria.Value.ToString().ToUpper() == "All".ToUpper())
                {
                }
                else if (optCriteria.Value.ToString().ToUpper() == "Section".ToUpper())
                {
                    SectId = gridSection.ActiveRow.Cells["SectId"].Value.ToString();
                }


                //else if (optCriteria.Value.ToString().ToUpper() == "Type".ToUpper())
                //{
                //    EmpType = gridType.ActiveRow.Cells["varName"].Value.ToString();
                //}
                if (optCriteria.Value.ToString().ToUpper() == "Employee".ToUpper())
                {
                    //EmpId = gridEmployeeID.ActiveRow.Cells["EmpId"].Value.ToString();
                    //string EmpId = "";

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
                    rptQuery = "Exec rptAttendSumCasual " + Common.Classes.clsMain.intComId + ", '" + clsProc.GTRDate(dtDateFrom.Value.ToString()) + "', '" + Param2 + "','" + EmpType + "'";
                    clsCon.GTRFillDatasetWithSQLCommand(ref dsDetails, rptQuery);
                    if (dsDetails.Tables[0].Rows.Count == 0)
                    {
                        MessageBox.Show("Data Not Found");
                        return;
                    }
                
                
                }
                else if (Param == "Movment")
                {
                    rptQuery = "Exec rptDailyMovementCasual " + Common.Classes.clsMain.intComId + ", '" + clsProc.GTRDate(dtDateFrom.Value.ToString()) + "',  '" + clsProc.GTRDate(dtDateTo.Value.ToString()) + "', " + SectId + ", " + EmpId + " ";


                    clsCon.GTRFillDatasetWithSQLCommand(ref dsDetails, rptQuery);
                    if (dsDetails.Tables[0].Rows.Count == 0)
                    {
                        MessageBox.Show("Data Not Found");
                        return;
                    }
                
                }
                else if (Param == "Job Card")
                {
                    rptQuery = "Exec rptJobCardCasual " + Common.Classes.clsMain.intComId + ", '" + clsProc.GTRDate(dtDateFrom.Value.ToString()) + "',  '" + clsProc.GTRDate(dtDateTo.Value.ToString()) + "', " + SectId + ", " + EmpId + ",'" + EmpType + "' ";


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

                else
                {
                    rptQuery = "Exec rptAttendCasual " + Common.Classes.clsMain.intComId + ", '" + clsProc.GTRDate(dtDateFrom.Value.ToString()) + "', '" + clsProc.GTRDate(dtDateTo.Value.ToString()) + "', " + ShiftId + ", " + DesigId + ", " + SectId + ", '', '', " + EmpId + ",'" + EmpType + "','" + Param + "','" + optCriteria.Value.ToString() + "' ";
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
                clsReport.strDSNMain = DataSourceName;
                clsReport.dsReport = dsDetails;

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
            gridType.Enabled = true;

            if(optCriteria.Value.ToString() =="All")
            {
                gridSection.Enabled = false;
                gridEmployeeID.Enabled = false;

            }
            else if (optCriteria.Value.ToString() == "Section")
            {
                gridEmployeeID.Enabled = false;

            }
            else if (optCriteria.Value.ToString() == "Employee")
            {
                gridSection.Enabled = false;
                gridType.Enabled = false;    
            }

            else if (optCriteria.Value.ToString() == "Type")
            {
                gridType.Enabled = true;
                gridSection.Enabled = false;
                gridEmployeeID.Enabled = false;
            }
        }

        private void btnAttendance_Click(object sender, EventArgs e)

        {
            ReportPath = Common.Classes.clsMain.AppPath + @"\Attendence\Reports\rptAttendPresentCasual.rdlc";                      
            prcShowReport("Attend");
        }

        private void btnPresent_Click(object sender, EventArgs e)
        {
            ReportPath = Common.Classes.clsMain.AppPath + @"\Attendence\Reports\rptJobCardCasual.rdlc";
            prcShowReport("Job Card");
        }

        private void btnAbsent_Click(object sender, EventArgs e)
        {
            ReportPath = Common.Classes.clsMain.AppPath + @"\Attendence\Reports\rptAttendAbsentCasual.rdlc";
            prcShowReport("Absent");
        }

        private void btnLate_Click(object sender, EventArgs e)
        {
            ReportPath = Common.Classes.clsMain.AppPath + @"\Attendence\Reports\rptAttendLateCasual.rdlc";
            prcShowReport("Late");
        }

        private void btnMissingOut_Click(object sender, EventArgs e)
        {
            ReportPath = Common.Classes.clsMain.AppPath + @"\Attendence\Reports\rptAttendMissingCasual.rdlc";
            prcShowReport("Missing Out");
        }

        private void btnLeave_Click(object sender, EventArgs e)
        {
            ReportPath = Common.Classes.clsMain.AppPath + @"\Attendence\Reports\rptAttendLeave.rdlc";
            prcShowReport("Leave");
        }

        private void btnInOut_Click(object sender, EventArgs e)
        {
            ReportPath = Common.Classes.clsMain.AppPath + @"\Attendence\Reports\rptAttendInOutCasual.rdlc";
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
            ReportPath = Common.Classes.clsMain.AppPath + @"\Attendence\Reports\rptAttendOvertimeCasual.rdlc";
            prcShowReport("Overtime");
        }

        private void btnContAbsent_Click(object sender, EventArgs e)
        {
            ReportPath = Common.Classes.clsMain.AppPath + @"\Attendence\Reports\rptAttendAbsentCont.rdlc";
            prcShowReport("Absent Continuous");
        }

        private void btnOffDay_Click(object sender, EventArgs e)
        {
            ReportPath = Common.Classes.clsMain.AppPath + @"\Attendence\Reports\rptAttendPresent.rdlc";
            prcShowReport("Off Day");
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
                SQLQuery = "Exec rptAttendFixedManualCasual " + Common.Classes.clsMain.intComId + ", '" + clsProc.GTRDate(dtDateFrom.Value.ToString()) + "', '" + clsProc.GTRDate(dtDateTo.Value.ToString()) + "' ";

                clsCon.GTRFillDatasetWithSQLCommand(ref dsDetails, SQLQuery);
                if (dsDetails.Tables[0].Rows.Count == 0)
                {
                    MessageBox.Show("Data Not Found");
                    return;
                }

                dsDetails.Tables[0].TableName = "Atten";

                gridExcel.DataSource = null;
                gridExcel.DataSource = dsDetails.Tables["Atten"];

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

        private void frmrptDailyAttCasual_FormClosing(object sender, FormClosingEventArgs e)
        {
            int index = Common.Classes.clsMain.fncFindOpenFormIndex(this);
            uTab.Tabs.RemoveAt(index);
            Common.Classes.clsMain.fncExistOpenForm(this, "Remove");

            clsProc = null;
            dsList = null;
            dsDetails = null;
            FM = null;
        }

        private void frmrptDailyAttCasual_Load(object sender, EventArgs e)
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
            dlgSurveyExcel.FileName = "Casual Attendance List" + "_" + DateTime.Now.ToShortDateString().Replace(@"/", "_");

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

