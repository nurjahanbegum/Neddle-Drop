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
    public partial class frmrptGradeIns : Form
    {
        System.Data.DataSet dsList;
        System.Data.DataSet dsDetails;
        clsProcedure clsProc = new clsProcedure();

        private Infragistics.Win.UltraWinTabControl.UltraTabControl uTab;
        Common.FormEntry.frmMaster FM;
        public frmrptGradeIns(ref Infragistics.Win.UltraWinTabControl.UltraTabControl utab, Common.FormEntry.frmMaster fm)
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
                string sqlquary = "Exec prcrptGradeIns " + Common.Classes.clsMain.intComId;
                clscon.GTRFillDatasetWithSQLCommand(ref dsList, sqlquary);
                dsList.Tables[0].TableName = "Criteria";
                dsList.Tables[1].TableName = "IncType";
                dsList.Tables[2].TableName = "EmpStatus";
                dsList.Tables[3].TableName = "EmpType";
                dsList.Tables[4].TableName = "Section";
                dsList.Tables[5].TableName = "Employee";
                dsList.Tables[6].TableName = "tblGrade";


                gridCriteria.DataSource = dsList.Tables["Criteria"];
                gridEmpStatus.DataSource = dsList.Tables["EmpStatus"];
                gridEmpType.DataSource = dsList.Tables["EmpType"];
                gridArea.DataSource = dsList.Tables["Section"];
                gridEmp.DataSource = dsList.Tables["Employee"];
                gridBand.DataSource = dsList.Tables["tblGrade"];

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
            gridArea.DisplayLayout.Bands[0].Columns["SectName"].Width = 210;
            gridArea.DisplayLayout.Bands[0].Columns["SectName"].Header.Caption = "Section";

            //Change alternate color
            gridArea.DisplayLayout.Override.RowAlternateAppearance.BackColor = Color.Cyan;
            gridArea.DisplayLayout.Override.RowAlternateAppearance.ForeColor = Color.DarkBlue;


            //Select Full Row when click on any cell
            e.Layout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;

            //Selection Style Will Be Row Selector
            this.gridArea.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;

            //Stop Updating
            this.gridArea.DisplayLayout.Override.AllowUpdate = DefaultableBoolean.False;

            //Hiding +/- Indicator
            this.gridArea.DisplayLayout.Override.ExpansionIndicator = ShowExpansionIndicator.Never;
            e.Layout.Override.FilterUIType = FilterUIType.FilterRow;
            //Hide Group Box Display
            e.Layout.GroupByBox.Hidden = true;
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
                FormCaption = "Report :: Insurance List ...";

                string SectId = "0", type = "", EmpId = "0", Status = "0", Grade = "";
                SectId = gridArea.ActiveRow.Cells["SectId"].Value.ToString();
                EmpId = gridEmp.ActiveRow.Cells["EmpId"].Value.ToString();
                Status = gridEmpStatus.ActiveRow.Cells["EmpStatus"].Value.ToString();
                type = gridEmpType.ActiveRow.Cells["EmpType"].Value.ToString();
                Grade = gridBand.ActiveRow.Cells["Grade"].Value.ToString();


                    if ((gridEmpStatus.ActiveRow.Cells["EmpStatus"].Text.ToString() == "=ALL="))
                    {
                        ReportPath = Common.Classes.clsMain.AppPath + @"\Attendence\Reports\rptInsurance.rdlc";
                        SQLQuery = "Exec rptInsuranceList " + Common.Classes.clsMain.intComId + ", '" + clsProc.GTRDate(dtFrom.Value.ToString()) + "', '" + clsProc.GTRDate(dtTo.Value.ToString()) + "', " + SectId + " , '" + Status + "', '" + EmpId + "' ,'" + type + "','" + Grade + "' ";
                    }
                    else if ((gridEmpStatus.ActiveRow.Cells["EmpStatus"].Text.ToString() == "Active"))
                    {
                        ReportPath = Common.Classes.clsMain.AppPath + @"\Attendence\Reports\rptInsurance.rdlc";
                        SQLQuery = "Exec rptInsuranceList " + Common.Classes.clsMain.intComId + ", '" + clsProc.GTRDate(dtFrom.Value.ToString()) + "', '" + clsProc.GTRDate(dtTo.Value.ToString()) + "', " + SectId + " , '" + Status + "', '" + EmpId + "' ,'" + type + "','" + Grade + "' ";
                    }
                    else if ((gridEmpStatus.ActiveRow.Cells["EmpStatus"].Text.ToString() == "Inactive"))
                    {
                        ReportPath = Common.Classes.clsMain.AppPath + @"\Attendence\Reports\rptInsurance.rdlc";
                        SQLQuery = "Exec rptInsuranceList " + Common.Classes.clsMain.intComId + ", '" + clsProc.GTRDate(dtFrom.Value.ToString()) + "', '" + clsProc.GTRDate(dtTo.Value.ToString()) + "', " + SectId + " , '" + Status + "', '" + EmpId + "' ,'" + type + "','" + Grade + "' ";
                    }
                    else if ((gridEmpStatus.ActiveRow.Cells["EmpStatus"].Text.ToString() == "New Joining"))
                    {
                        ReportPath = Common.Classes.clsMain.AppPath + @"\Attendence\Reports\rptInsurance.rdlc";
                        SQLQuery = "Exec rptInsuranceList " + Common.Classes.clsMain.intComId + ", '" + clsProc.GTRDate(dtFrom.Value.ToString()) + "', '" + clsProc.GTRDate(dtTo.Value.ToString()) + "', " + SectId + " , '" + Status + "', '" + EmpId + "' ,'" + type + "','" + Grade + "' ";
                    }
                    else if ((gridEmpStatus.ActiveRow.Cells["EmpStatus"].Text.ToString() == "Released"))
                    {
                        ReportPath = Common.Classes.clsMain.AppPath + @"\Attendence\Reports\rptInsurance.rdlc";
                        SQLQuery = "Exec rptInsuranceList " + Common.Classes.clsMain.intComId + ", '" + clsProc.GTRDate(dtFrom.Value.ToString()) + "', '" + clsProc.GTRDate(dtTo.Value.ToString()) + "', " + SectId + " , '" + Status + "', '" + EmpId + "','" + type + "','" + Grade + "' ";
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
            }
        }
        
        private void gridArea_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyValue);
        }

        private void gridCriteria_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            gridCriteria.DisplayLayout.Bands[0].Columns["CValue"].Hidden = true;
            gridCriteria.DisplayLayout.Bands[0].Columns["SLNo"].Hidden = true;
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

        private void gridEmpStatus_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            //gridCriteria.DisplayLayout.Bands[0].Columns["CValue"].Hidden = true;
            //gridCriteria.DisplayLayout.Bands[0].Columns["SLNo"].Hidden = true;
            gridEmpStatus.DisplayLayout.Bands[0].Columns["EmpStatus"].Width = 200;
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
            gridEmpType.DisplayLayout.Bands[0].Columns["EmpType"].Width = 200;
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
            gridEmp.DisplayLayout.Bands[0].Columns["EmpId"].Hidden = true;
            gridEmp.DisplayLayout.Bands[0].Columns["EmpCode"].Width = 115;
            gridEmp.DisplayLayout.Bands[0].Columns["EmpCode"].Header.Caption = "Employee Code";
            gridEmp.DisplayLayout.Bands[0].Columns["EmpName"].Width = 220;
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


        private void gridCriteria_AfterRowActivate(object sender, EventArgs e)
        {
            
            if(gridCriteria.ActiveRow.Cells["Criteria"].Text.ToString().ToUpper()=="Employee Wise".ToUpper())
            {
                gridEmp.Enabled = true;
                gridArea.Enabled = false;
                gridBand.Enabled = false;
            }
            else if (gridCriteria.ActiveRow.Cells["Criteria"].Text.ToString().ToUpper() == "Section Wise".ToUpper())
            {
                gridEmp.Enabled = false;
                gridArea.Enabled = true;
                gridBand.Enabled = false;
            }
            else if (gridCriteria.ActiveRow.Cells["Criteria"].Text.ToString().ToUpper() == "Grade Insurance".ToUpper())
            {
                gridBand.Enabled = true;
                gridEmp.Enabled = false;
                gridArea.Enabled = false;
            }
            else
            {
                gridEmp.Enabled = false;
                gridArea.Enabled = false;
                gridBand.Enabled = false;
            }
        }

        private void gridEmpStatus_AfterRowActivate(object sender, EventArgs e)
        {
            if (gridEmpStatus.ActiveRow.Cells[0].Value.ToString().ToUpper() == "=ALL=" || gridEmpStatus.ActiveRow.Cells[0].Value.ToString().ToUpper() == "Current".ToUpper())
            {
                group1.Enabled = false;
                dtFrom.Value = "1-1-1980";
                dtTo.Value = DateTime.Now;
            }
            else if (gridEmpStatus.ActiveRow.Cells[0].Value.ToString().ToUpper() == "Active".ToUpper())
            {
                group1.Enabled = false;
                dtFrom.Value = "1-1-1980";
                dtTo.Value = DateTime.Now;
            }
            else if (gridEmpStatus.ActiveRow.Cells[0].Value.ToString().ToUpper() == "Inactive".ToUpper())
            {
                group1.Enabled = false;
                dtFrom.Value = "1-1-1980";
                dtTo.Value = DateTime.Now;
            }
             else
            {
                group1.Enabled = true;
                dtFrom.Value = DateTime.Now;
            }

        }

        private void gridBand_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            gridBand.DisplayLayout.Bands[0].Columns["aId"].Hidden = true;
            //gridArea.DisplayLayout.Bands[0].Columns["SLNO"].Hidden = true;
            gridBand.DisplayLayout.Bands[0].Columns["Grade"].Width = 210;
            gridBand.DisplayLayout.Bands[0].Columns["Grade"].Header.Caption = "Grade";

            //Change alternate color
            gridBand.DisplayLayout.Override.RowAlternateAppearance.BackColor = Color.Cyan;
            gridBand.DisplayLayout.Override.RowAlternateAppearance.ForeColor = Color.DarkBlue;


            //Select Full Row when click on any cell
            e.Layout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;

            //Selection Style Will Be Row Selector
            this.gridBand.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;

            //Stop Updating
            this.gridBand.DisplayLayout.Override.AllowUpdate = DefaultableBoolean.False;

            //Hiding +/- Indicator
            this.gridBand.DisplayLayout.Override.ExpansionIndicator = ShowExpansionIndicator.Never;
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

            string SQLQuery = "";


            string SectId = "0", type = "", EmpId = "0", Status = "0", Grade = "";
            SectId = gridArea.ActiveRow.Cells["SectId"].Value.ToString();
            EmpId = gridEmp.ActiveRow.Cells["EmpId"].Value.ToString();
            Status = gridEmpStatus.ActiveRow.Cells["EmpStatus"].Value.ToString();
            type = gridEmpType.ActiveRow.Cells["EmpType"].Value.ToString();
            Grade = gridBand.ActiveRow.Cells["Grade"].Value.ToString();

            if ((gridEmpStatus.ActiveRow.Cells["EmpStatus"].Text.ToString() == "=ALL="))
            {
                SQLQuery = "Exec rptInsuranceList " + Common.Classes.clsMain.intComId + ", '" + clsProc.GTRDate(dtFrom.Value.ToString()) + "', '" + clsProc.GTRDate(dtTo.Value.ToString()) + "', " + SectId + " , '" + Status + "', '" + EmpId + "' ,'" + type + "','" + Grade + "' ";
                clscon.GTRFillDatasetWithSQLCommand(ref dsList, SQLQuery);

                dsList.Tables[0].TableName = "List";

                gridExcel.DataSource = null;
                gridExcel.DataSource = dsList.Tables["List"];

            }
            else if ((gridEmpStatus.ActiveRow.Cells["EmpStatus"].Text.ToString() == "Active"))
            {
                SQLQuery = "Exec rptInsuranceList " + Common.Classes.clsMain.intComId + ", '" + clsProc.GTRDate(dtFrom.Value.ToString()) + "', '" + clsProc.GTRDate(dtTo.Value.ToString()) + "', " + SectId + " , '" + Status + "', '" + EmpId + "' ,'" + type + "','" + Grade + "' ";
                clscon.GTRFillDatasetWithSQLCommand(ref dsList, SQLQuery);

                dsList.Tables[0].TableName = "List";

                gridExcel.DataSource = null;
                gridExcel.DataSource = dsList.Tables["List"];
            }
            else if ((gridEmpStatus.ActiveRow.Cells["EmpStatus"].Text.ToString() == "Inactive"))
            {
                SQLQuery = "Exec rptInsuranceList " + Common.Classes.clsMain.intComId + ", '" + clsProc.GTRDate(dtFrom.Value.ToString()) + "', '" + clsProc.GTRDate(dtTo.Value.ToString()) + "', " + SectId + " , '" + Status + "', '" + EmpId + "' ,'" + type + "','" + Grade + "' ";
                clscon.GTRFillDatasetWithSQLCommand(ref dsList, SQLQuery);

                dsList.Tables[0].TableName = "List";

                gridExcel.DataSource = null;
                gridExcel.DataSource = dsList.Tables["List"];
            }
            else if ((gridEmpStatus.ActiveRow.Cells["EmpStatus"].Text.ToString() == "New Joining"))
            {
                SQLQuery = "Exec rptInsuranceList " + Common.Classes.clsMain.intComId + ", '" + clsProc.GTRDate(dtFrom.Value.ToString()) + "', '" + clsProc.GTRDate(dtTo.Value.ToString()) + "', " + SectId + " , '" + Status + "', '" + EmpId + "' ,'" + type + "','" + Grade + "' ";
                clscon.GTRFillDatasetWithSQLCommand(ref dsList, SQLQuery);

                dsList.Tables[0].TableName = "List";

                gridExcel.DataSource = null;
                gridExcel.DataSource = dsList.Tables["List"];
            }
            else if ((gridEmpStatus.ActiveRow.Cells["EmpStatus"].Text.ToString() == "Released"))
            {
                SQLQuery = "Exec rptInsuranceList " + Common.Classes.clsMain.intComId + ", '" + clsProc.GTRDate(dtFrom.Value.ToString()) + "', '" + clsProc.GTRDate(dtTo.Value.ToString()) + "', " + SectId + " , '" + Status + "', '" + EmpId + "','" + type + "','" + Grade + "' ";
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
            dlgSurveyExcel.FileName = "Employee Insurance List.xls" + "_" + DateTime.Now.ToShortDateString().Replace(@"/", "_");

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