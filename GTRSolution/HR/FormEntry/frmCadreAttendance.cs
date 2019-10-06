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

namespace GTRHRIS.HR.FormEntry
{
    public partial class frmCadreAttendance : Form
    {
        System.Data.DataSet dsList;
        System.Data.DataSet dsDetails;
        clsProcedure clsProc = new clsProcedure();

        string ReportPath = "", rptQuery = "", DataSourceName = "DataSet1", FormCaption = "";

        private Infragistics.Win.UltraWinTabControl.UltraTabControl uTab;
        Common.FormEntry.frmMaster FM;
        public frmCadreAttendance(ref Infragistics.Win.UltraWinTabControl.UltraTabControl utab, Common.FormEntry.frmMaster fm)
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
                string sqlquary = "Exec rptCadreAttendance  " + Common.Classes.clsMain.intComId + ",'','',0,0,'','',0";
                clscon.GTRFillDatasetWithSQLCommand(ref dsList, sqlquary);

                dsList.Tables[0].TableName = "ProssType";
                dsList.Tables[1].TableName = "ReportCategory";
                dsList.Tables[2].TableName = "Company";
                dsList.Tables[3].TableName = "Position";

                gridProssType.DataSource = null;
                gridProssType.DataSource = dsList.Tables["ProssType"];


                gridReportCategory.DataSource = null;
                gridReportCategory.DataSource = dsList.Tables["ReportCategory"];


                dtPross.Value = DateTime.Now;


                DateTime firstDay = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                dtFrom.Value = firstDay;

                DateTime lastDay = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                lastDay = lastDay.AddMonths(1);
                lastDay = lastDay.AddDays(-(lastDay.Day));
                dtTo.Value = lastDay;

                //clsProc.GTRDate(dtPross.Value.ToString())


            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        private void prcLoadCombo()
        {

            cboComp.DataSource = null;
            cboComp.DataSource = dsList.Tables["Company"];

            cboPosition.DataSource = null;
            cboPosition.DataSource = dsList.Tables["Position"];

        }

        private void cboComp_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            cboComp.DisplayLayout.Bands[0].Columns["ComName"].Width = cboComp.Width;
            cboComp.DisplayLayout.Bands[0].Columns["ComName"].Header.Caption = "Unit";
            cboComp.DisplayLayout.Bands[0].Columns["ComID"].Hidden = true;
            cboComp.DisplayMember = "ComName";
            cboComp.ValueMember = "ComID";
        }

        private void cboPosition_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            cboPosition.DisplayLayout.Bands[0].Columns["Position"].Width = cboPosition.Width;
            cboPosition.DisplayLayout.Bands[0].Columns["MPosition"].Width = 170;
            cboPosition.DisplayLayout.Bands[0].Columns["Position"].Header.Caption = "Position";
            cboPosition.DisplayLayout.Bands[0].Columns["CDID"].Hidden = true;
            cboPosition.DisplayMember = "Position";
            cboPosition.ValueMember = "CDID";
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        private void frmCadreAttendance_FormClosing(object sender, FormClosingEventArgs e)
        {
            int index = Common.Classes.clsMain.fncFindOpenFormIndex(this);
            uTab.Tabs.RemoveAt(index);
            Common.Classes.clsMain.fncExistOpenForm(this, "Remove");

            clsProc = null;
            FM = null;
        }

        private void frmCadreAttendance_Load(object sender, EventArgs e)
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
                FormCaption = "Report :: Cadre Summary..";

                string ProssType = "", Rpt = "";

                ProssType = gridProssType.ActiveRow.Cells["ProssType"].Value.ToString();

                Rpt = gridReportCategory.ActiveRow.Cells["rptName"].Value.ToString();


                if ((gridReportCategory.ActiveRow.Cells["rptname"].Text.ToString() == "Cadre Attendance"))
                {
                    ReportPath = Common.Classes.clsMain.AppPath + @"\Attendence\Reports\rptCadreSumAttend.rdlc";
                    rptQuery = "Exec rptCadreAttendance " + Common.Classes.clsMain.intComId + ",'" + ProssType + "','" + clsProc.GTRDate(dtPross.Value.ToString()) + "',0,0,'','',2";
                }

                else if ((gridReportCategory.ActiveRow.Cells["rptname"].Text.ToString() == "Cadre Attendance Summary"))
                {
                    ReportPath = Common.Classes.clsMain.AppPath + @"\Attendence\Reports\rptCadreSumStatusAttend.rdlc";
                    rptQuery = "Exec rptCadreAttendance " + Common.Classes.clsMain.intComId + ",'" + ProssType + "','" + clsProc.GTRDate(dtPross.Value.ToString()) + "',0,0,'','',3";
                }

                else if ((gridReportCategory.ActiveRow.Cells["rptname"].Text.ToString() == "Struck Off Summary"))
                {
                    ReportPath = Common.Classes.clsMain.AppPath + @"\Attendence\Reports\rptCadreSumStatusStruck.rdlc";
                    rptQuery = "Exec rptCadreAttendance " + Common.Classes.clsMain.intComId + ",'" + ProssType + "','" + clsProc.GTRDate(dtPross.Value.ToString()) + "',0,0,'','',4";
                }

                else if ((gridReportCategory.ActiveRow.Cells["rptname"].Text.ToString() == "Cadre Absent Analysis"))
                {
                    ReportPath = Common.Classes.clsMain.AppPath + @"\Attendence\Reports\rptCadreABAnalysis.rdlc";
                    rptQuery = "Exec rptCadreAttendance " + Common.Classes.clsMain.intComId + ",'" + ProssType + "','" + clsProc.GTRDate(dtPross.Value.ToString()) + "',0,0,'" + clsProc.GTRDate(dtFrom.Value.ToString()) + "','" + clsProc.GTRDate(dtTo.Value.ToString()) + "',5";
                }

                else if ((gridReportCategory.ActiveRow.Cells["rptname"].Text.ToString() == "Struck Off Summary Analysis"))
                {
                    ReportPath = Common.Classes.clsMain.AppPath + @"\Attendence\Reports\rptCadreStruckAnalysis.rdlc";
                    rptQuery = "Exec rptCadreAttendance " + Common.Classes.clsMain.intComId + ",'" + ProssType + "','" + clsProc.GTRDate(dtPross.Value.ToString()) + "',0,0,'" + clsProc.GTRDate(dtFrom.Value.ToString()) + "','" + clsProc.GTRDate(dtTo.Value.ToString()) + "',6";
                }


                clsCon.GTRFillDatasetWithSQLCommand(ref dsDetails, rptQuery);


                if (dsDetails.Tables[0].Rows.Count == 0)
                {
                    MessageBox.Show("Data Not Found");
                    return;
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
                DataSourceName = null;
                ReportPath = null;
                dsDetails = null;
            }
        }

        private void gridProssType_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            gridProssType.DisplayLayout.Bands[0].Columns["Month"].Hidden = true;
            gridProssType.DisplayLayout.Bands[0].Columns["year"].Hidden = true;
            gridProssType.DisplayLayout.Bands[0].Columns["date"].Hidden = true;

            gridProssType.DisplayLayout.Bands[0].Columns["ProssType"].Width = 220;
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

        private void btnProcess_Click(object sender, EventArgs e)
        {
            string Description = "";

            //System.Globalization.DateTimeFormatInfo mfi = new System.Globalization.DateTimeFormatInfo();
            //string strMonthName = mfi.GetMonthName(dtPross.DateTime.Month).ToString();
            //Description = strMonthName + "-" + (dtPross.DateTime.Year);

            Description = clsProc.GTRDate(dtPross.Value.ToString());
            btnProcess.Text = "Please Wait";

            ArrayList arQuery = new ArrayList();
            GTRLibrary.clsConnection clsCon = new GTRLibrary.clsConnection();
            dsList = new System.Data.DataSet();


            try
            {

                string sqlQuery = "Exec rptCadreAttendance " + Common.Classes.clsMain.intComId + ",'" + Description + "','" + clsProc.GTRDate(dtPross.Value.ToString()) + "',0,0,'','',1";
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


        private Boolean fncBlank()
        {


            if (dtPross.Text.Length == 0)
            {
                MessageBox.Show("Please provide requisition date.", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                dtPross.Focus();
                return true;
            }



            return false;


        }

        private void gridReportCategory_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            gridReportCategory.DisplayLayout.Bands[0].Columns["rptid"].Hidden = true;
            gridReportCategory.DisplayLayout.Bands[0].Columns["rptname"].Width = 280;
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

            string sqlquary = "", ProssType = "", Rpt = "";

            ProssType = gridProssType.ActiveRow.Cells["ProssType"].Value.ToString();

            Rpt = gridReportCategory.ActiveRow.Cells["rptName"].Value.ToString();


            if ((gridReportCategory.ActiveRow.Cells["rptname"].Text.ToString() == "Cadre Attendance"))
            {
                sqlquary = "Exec rptCadreAttendance " + Common.Classes.clsMain.intComId + ",'" + ProssType + "','" + clsProc.GTRDate(dtPross.Value.ToString()) + "',0,0,'','',2";
            }

            else if ((gridReportCategory.ActiveRow.Cells["rptname"].Text.ToString() == "Cadre Attendance Summary"))
            {
                sqlquary = "Exec rptCadreAttendance " + Common.Classes.clsMain.intComId + ",'" + ProssType + "','" + clsProc.GTRDate(dtPross.Value.ToString()) + "',0,0,'','',3";
            }

            else if ((gridReportCategory.ActiveRow.Cells["rptname"].Text.ToString() == "Struck Off Summary"))
            {
                sqlquary = "Exec rptCadreAttendance " + Common.Classes.clsMain.intComId + ",'" + ProssType + "','" + clsProc.GTRDate(dtPross.Value.ToString()) + "',0,0,'','',4";
            }

            else if ((gridReportCategory.ActiveRow.Cells["rptname"].Text.ToString() == "Cadre Absent Analysis"))
            {
                sqlquary = "Exec rptCadreAttendance " + Common.Classes.clsMain.intComId + ",'" + ProssType + "','" + clsProc.GTRDate(dtPross.Value.ToString()) + "',0,0,'" + clsProc.GTRDate(dtFrom.Value.ToString()) + "','" + clsProc.GTRDate(dtTo.Value.ToString()) + "',5";
            }

            else if ((gridReportCategory.ActiveRow.Cells["rptname"].Text.ToString() == "Struck Off Summary Analysis"))
            {
                sqlquary = "Exec rptCadreAttendance " + Common.Classes.clsMain.intComId + ",'" + ProssType + "','" + clsProc.GTRDate(dtPross.Value.ToString()) + "',0,0,'" + clsProc.GTRDate(dtFrom.Value.ToString()) + "','" + clsProc.GTRDate(dtTo.Value.ToString()) + "',6";
            }


            clscon.GTRFillDatasetWithSQLCommand(ref dsList, sqlquary);

            dsList.Tables[0].TableName = "Cadre";

            gridExcel.DataSource = null;
            gridExcel.DataSource = dsList.Tables["Cadre"];

            DialogResult dlgRes =
            MessageBox.Show("Do You Want to Save the Data Sheet");
            if (dlgRes != DialogResult.OK)
            {
                return;
            }

            SaveFileDialog dlgSurveyExcel = new SaveFileDialog();
            dlgSurveyExcel.Filter = "Excel WorkBook (*.xls)|.xls";
            dlgSurveyExcel.FileName = gridReportCategory.ActiveRow.Cells["rptName"].Value.ToString() + "_" + DateTime.Now.ToShortDateString().Replace(@"/", "_");

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

