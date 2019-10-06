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
    public partial class frmrptTrainingEmp : Form
    {
        System.Data.DataSet dsList;
        System.Data.DataSet dsDetails;
        clsProcedure clsProc = new clsProcedure();

        string ReportPath = "", rptQuery = "", DataSourceName = "DataSet1", FormCaption = "";

        private Infragistics.Win.UltraWinTabControl.UltraTabControl uTab;
        Common.FormEntry.frmMaster FM;
        public frmrptTrainingEmp(ref Infragistics.Win.UltraWinTabControl.UltraTabControl utab, Common.FormEntry.frmMaster fm)
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

                string sqlquary = "Exec rptTraining " + Common.Classes.clsMain.intComId + ",'" + clsProc.GTRDate(dtDateFrom.Value.ToString()) + "', '" + clsProc.GTRDate(dtDateTo.Value.ToString()) + "', 0,'0','0',0";
                clscon.GTRFillDatasetWithSQLCommand(ref dsList, sqlquary);
                
                //string sqlquary = "Exec prcrptDaily  " + Common.Classes.clsMain.intComId;
                //clscon.GTRFillDatasetWithSQLCommand(ref dsList, sqlquary);

                dsList.Tables[0].TableName = "Employee";
                dsList.Tables[1].TableName = "tblTraining";

                gridEmployeeID.DataSource = dsList.Tables["Employee"];
                gridName.DataSource = dsList.Tables["tblTraining"];


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



        private void gridBand_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            //gridName.DisplayLayout.Bands[0].Columns["varID"].Hidden = true;
            gridName.DisplayLayout.Bands[0].Columns["TName"].Width = 250;
            gridName.DisplayLayout.Bands[0].Columns["TName"].Header.Caption = "Training Name";

            //Change alternate color
            gridName.DisplayLayout.Override.RowAlternateAppearance.BackColor = Color.Cyan;
            gridName.DisplayLayout.Override.RowAlternateAppearance.ForeColor = Color.DarkBlue;

            //Select Full Row when click on any cell
            e.Layout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;

            //Selection Style Will Be Row Selector
            gridName.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;

            //Stop Updating
            gridName.DisplayLayout.Override.AllowUpdate = DefaultableBoolean.False;

            //Hiding +/- Indicator
            gridName.DisplayLayout.Override.ExpansionIndicator = ShowExpansionIndicator.Never;

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
        
        //private void prcShowReport()
        //{
        //    dsDetails = new DataSet();
        //    ArrayList arQuery = new ArrayList();
        //    clsConnection clsCon = new clsConnection();

        //    try
        //    {
        //        DataSourceName = "DataSet1";
        //        FormCaption = "Report :: Monthly Attendance Info...";

        //        string EmpId = "0", TrainingName = "0", TraningName = "",RptType = "";

        //        EmpId = gridEmployeeID.ActiveRow.Cells["EmpId"].Value.ToString();
        //        TrainingName = gridName.ActiveRow.Cells["TName"].Value.ToString();

        //        if (optCriteria.Value.ToString().ToUpper() == "Employee".ToUpper())
        //        {

        //            rptQuery = "Exec rptTraining " + Common.Classes.clsMain.intComId + ",'" + clsProc.GTRDate(dtDateFrom.Value.ToString()) + "', '" + clsProc.GTRDate(dtDateTo.Value.ToString()) + "', " + EmpId + ",'0','0','1'";
        //            clsCon.GTRFillDatasetWithSQLCommand(ref dsDetails, rptQuery);                    

        //        }

        //        else if (optCriteria.Value.ToString().ToUpper() == "Training".ToUpper())
        //        {
        //            rptQuery = "Exec rptTraining " + Common.Classes.clsMain.intComId + ",'" + clsProc.GTRDate(dtDateFrom.Value.ToString()) + "', '" + clsProc.GTRDate(dtDateTo.Value.ToString()) + "', 0,'" + TrainingName + "','0','2'";
        //            clsCon.GTRFillDatasetWithSQLCommand(ref dsDetails, rptQuery);
                    
        //        }
        //        else if (optCriteria.Value.ToString().ToUpper() == "dtDate".ToUpper())
        //        {
        //            rptQuery = "Exec rptTraining " + Common.Classes.clsMain.intComId + ",'" + clsProc.GTRDate(dtDateFrom.Value.ToString()) + "', '" + clsProc.GTRDate(dtDateTo.Value.ToString()) + "', 0,'0','0','3'";
        //            clsCon.GTRFillDatasetWithSQLCommand(ref dsDetails, rptQuery);
        //        }

                
        //        if(dsDetails.Tables[0].Rows.Count==0)
        //        {
        //            MessageBox.Show("Data Not Found");
        //            return;
        //        }
        //        clsReport.strReportPathMain = ReportPath;
        //        clsReport.strQueryMain = rptQuery;
        //        clsReport.strDSNMain = DataSourceName;
        //        clsReport.dsReport = dsDetails;

        //        FM.prcShowReport(FormCaption);
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //    }

        //    finally
        //    {
        //        rptQuery = null;
        //        DataSourceName = null;
        //        DataSourceName = null;
        //        ReportPath = null;
        //        dsDetails = null;
        //    }
        //}
        private void optCriteria_ValueChanged(object sender, EventArgs e)
        {                        

            gridEmployeeID.Enabled = true;
            gridName.Enabled = false;


            if (optCriteria.Value.ToString() == "Employee")
            {
                gridEmployeeID.Enabled = true;
                gridName.Enabled = false;
            }
            else if (optCriteria.Value.ToString() == "Training")
            {
                gridName.Enabled = true;
                gridEmployeeID.Enabled = false;
            }
            else if (optCriteria.Value.ToString() == "dtDate")
            {
                gridName.Enabled = false;
                gridEmployeeID.Enabled = false;

            }
        }
        private void frmrptTrainingEmp_FormClosing(object sender, FormClosingEventArgs e)
        {
            int index = Common.Classes.clsMain.fncFindOpenFormIndex(this);
            uTab.Tabs.RemoveAt(index);
            Common.Classes.clsMain.fncExistOpenForm(this, "Remove");

            clsProc = null;
            FM = null;
        }

        private void frmrptTrainingEmp_Load(object sender, EventArgs e)
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
                FormCaption = "Report :: Training Report...";

                string EmpId = "0", TrainingName = "0", RptType = "";

                EmpId = gridEmployeeID.ActiveRow.Cells["EmpId"].Value.ToString();
                TrainingName = gridName.ActiveRow.Cells["TName"].Value.ToString();

                if (optCriteria.Value.ToString().ToUpper() == "Employee".ToUpper())
                {

                    ReportPath = Common.Classes.clsMain.AppPath + @"\Payroll\Reports\rptTrainingEmp.rdlc";
                    rptQuery = "Exec rptTraining " + Common.Classes.clsMain.intComId + ",'" + clsProc.GTRDate(dtDateFrom.Value.ToString()) + "', '" + clsProc.GTRDate(dtDateTo.Value.ToString()) + "', " + EmpId + ",'0','0','1'";

                }

                else if (optCriteria.Value.ToString().ToUpper() == "Training".ToUpper())
                {
                    ReportPath = Common.Classes.clsMain.AppPath + @"\Payroll\Reports\rptTraining.rdlc";
                    rptQuery = "Exec rptTraining " + Common.Classes.clsMain.intComId + ",'" + clsProc.GTRDate(dtDateFrom.Value.ToString()) + "', '" + clsProc.GTRDate(dtDateTo.Value.ToString()) + "', 0,'" + TrainingName + "','0','2'";

                }
                else if (optCriteria.Value.ToString().ToUpper() == "dtDate".ToUpper())
                {
                    ReportPath = Common.Classes.clsMain.AppPath + @"\Payroll\Reports\rptTraining.rdlc";
                    rptQuery = "Exec rptTraining " + Common.Classes.clsMain.intComId + ",'" + clsProc.GTRDate(dtDateFrom.Value.ToString()) + "', '" + clsProc.GTRDate(dtDateTo.Value.ToString()) + "', 0,'0','0','3'";

                }

                clsCon.GTRFillDatasetWithSQLCommand(ref dsDetails, rptQuery);


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

            string sqlquary = "",EmpId = "0", TrainingName = "0", RptType = "";

            EmpId = gridEmployeeID.ActiveRow.Cells["EmpId"].Value.ToString();
            TrainingName = gridName.ActiveRow.Cells["TName"].Value.ToString();

            if (optCriteria.Value.ToString().ToUpper() == "Employee".ToUpper())
            {
                sqlquary = "Exec rptTraining " + Common.Classes.clsMain.intComId + ",'" + clsProc.GTRDate(dtDateFrom.Value.ToString()) + "', '" + clsProc.GTRDate(dtDateTo.Value.ToString()) + "', " + EmpId + ",'0','0','1'";
            }

            else if (optCriteria.Value.ToString().ToUpper() == "Training".ToUpper())
            {
                sqlquary = "Exec rptTraining " + Common.Classes.clsMain.intComId + ",'" + clsProc.GTRDate(dtDateFrom.Value.ToString()) + "', '" + clsProc.GTRDate(dtDateTo.Value.ToString()) + "', 0,'" + TrainingName + "','0','2'";
            }
            else if (optCriteria.Value.ToString().ToUpper() == "dtDate".ToUpper())
            {
                sqlquary = "Exec rptTraining " + Common.Classes.clsMain.intComId + ",'" + clsProc.GTRDate(dtDateFrom.Value.ToString()) + "', '" + clsProc.GTRDate(dtDateTo.Value.ToString()) + "', 0,'0','0','3'";
            }
            
            clscon.GTRFillDatasetWithSQLCommand(ref dsList, sqlquary);
                 
            dsList.Tables[0].TableName = "Training";

            gridExcel.DataSource = null;
            gridExcel.DataSource = dsList.Tables["Training"];

            DialogResult dlgRes =
            MessageBox.Show("Do You Want to Save the Data Sheet");
            if (dlgRes != DialogResult.OK)
            {
                return;
            }

            SaveFileDialog dlgSurveyExcel = new SaveFileDialog();
            dlgSurveyExcel.Filter = "Excel WorkBook (*.xls)|.xls";
            dlgSurveyExcel.FileName = "Training Report" + "_" + DateTime.Now.ToShortDateString().Replace(@"/", "_");

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

