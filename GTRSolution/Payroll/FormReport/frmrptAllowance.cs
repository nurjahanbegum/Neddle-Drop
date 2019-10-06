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
using ColumnStyle = Infragistics.Win.UltraWinGrid.ColumnStyle;
using Infragistics.Win.UltraWinGrid.ExcelExport;

namespace GTRHRIS.Payroll.FormReport
{
    public partial class frmrptAllowance : Form
    {
        System.Data.DataSet dsList;
        System.Data.DataSet dsDetails;
        clsProcedure clsProc = new clsProcedure();

        private Infragistics.Win.UltraWinTabControl.UltraTabControl uTab;
        Common.FormEntry.frmMaster FM;
        public frmrptAllowance(ref Infragistics.Win.UltraWinTabControl.UltraTabControl utab, Common.FormEntry.frmMaster fm)
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
                string sqlquary = "Exec prcrptAllowance " + Common.Classes.clsMain.intComId + ", 'Admin','" + cboType.Text.ToString() + "'";
                clscon.GTRFillDatasetWithSQLCommand(ref dsList, sqlquary);
                
                dsList.Tables[0].TableName = "ProssType";
                dsList.Tables[1].TableName = "Section"; 
                dsList.Tables[2].TableName = "Employee";
                dsList.Tables[3].TableName = "AllowType";
                dsList.Tables[4].TableName = "ReportCategory";
                dsList.Tables[5].TableName = "Band";

                gridProssType.DataSource = dsList.Tables["ProssType"];
                gridSec.DataSource = dsList.Tables["Section"];
                gridEmployee.DataSource = dsList.Tables["Employee"];
                gridReportCategory.DataSource = dsList.Tables["ReportCategory"];
                gridBand.DataSource = dsList.Tables["Band"];
                
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        private void prcLoadCombo()
        {
            try
            {

                cboType.DataSource = dsList.Tables["AllowType"];

                cboType.Text = "Transport";


            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        private void cboType_ValueChanged(object sender, EventArgs e)
        {
            clsConnection clscon = new clsConnection();
            dsList = new System.Data.DataSet();
            try
            {
                string sqlquary = "Exec prcrptAllowance " + Common.Classes.clsMain.intComId + ", 'Admin','" + cboType.Text.ToString() + "'";
                clscon.GTRFillDatasetWithSQLCommand(ref dsList, sqlquary);

                dsList.Tables[0].TableName = "ProssType";

                gridProssType.DataSource = dsList.Tables["ProssType"];

            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        private void frmrptAllowance_Load(object sender, EventArgs e)
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

        private void frmrptAllowance_FormClosing(object sender, FormClosingEventArgs e)
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

            dsDetails = new DataSet();

            ArrayList arQuery = new ArrayList();
            GTRLibrary.clsConnection clsCon = new GTRLibrary.clsConnection();
            
            try
            {
                string ReportPath = "", SQLQuery = "", DataSourceName = "DataSet1", FormCaption = "", Band = "";
                Band = gridBand.ActiveRow.Cells["varName"].Value.ToString();

                DataSourceName = "DataSet1";
                FormCaption = "Report :: Payment Information...";

                if ((cboType.Text.ToString() == "Transport"))
                {
                    if ((gridReportCategory.ActiveRow.Cells["rptname"].Text.ToString() == "Allowance Sheet"))
                    {
                        ReportPath = Common.Classes.clsMain.AppPath + @"\Payroll\Reports\rptTransport.rdlc";
                        SQLQuery = "Exec [rptAllowanceSheet] " + Common.Classes.clsMain.intComId + ", '" + gridProssType.ActiveRow.Cells[0].Value.ToString() + "','" + gridSec.ActiveRow.Cells[0].Value.ToString() + "','" + gridEmployee.ActiveRow.Cells[0].Value.ToString() + "','" + cboType.Text.ToString() + "','" + Band + "'";
                    }
                    else if ((gridReportCategory.ActiveRow.Cells["rptname"].Text.ToString() == "Summary Sheet"))
                    {
                        ReportPath = Common.Classes.clsMain.AppPath + @"\Payroll\Reports\rptTransportSum.rdlc";
                        SQLQuery = "Exec [rptAllowanceSheetSum] " + Common.Classes.clsMain.intComId + ", '" + gridProssType.ActiveRow.Cells[0].Value.ToString() + "','" + gridSec.ActiveRow.Cells[0].Value.ToString() + "','" + gridEmployee.ActiveRow.Cells[0].Value.ToString() + "','" + cboType.Text.ToString() + "','" + Band + "'";
                    }
                    else if ((gridReportCategory.ActiveRow.Cells["rptname"].Text.ToString() == "Denomination"))
                    {
                        ReportPath = Common.Classes.clsMain.AppPath + @"\Payroll\Reports\rptAllowDeno.rdlc";
                        SQLQuery = "Exec [rptAllowanceDeno] " + Common.Classes.clsMain.intComId + ", '" + gridProssType.ActiveRow.Cells[0].Value.ToString() + "','" + gridSec.ActiveRow.Cells[0].Value.ToString() + "','" + gridEmployee.ActiveRow.Cells[0].Value.ToString() + "','" + cboType.Text.ToString() + "','" + Band + "'";
                    }
                }
                else if ((cboType.Text.ToString() == "Friday"))
                {
                    if ((gridReportCategory.ActiveRow.Cells["rptname"].Text.ToString() == "Allowance Sheet"))
                    {
                        ReportPath = Common.Classes.clsMain.AppPath + @"\Payroll\Reports\rptFriday.rdlc";
                        SQLQuery = "Exec [rptAllowanceSheet] " + Common.Classes.clsMain.intComId + ", '" + gridProssType.ActiveRow.Cells[0].Value.ToString() + "','" + gridSec.ActiveRow.Cells[0].Value.ToString() + "','" + gridEmployee.ActiveRow.Cells[0].Value.ToString() + "','" + cboType.Text.ToString() + "','" + Band + "'";
                    }
                    else if ((gridReportCategory.ActiveRow.Cells["rptname"].Text.ToString() == "Summary Sheet"))
                    {
                        ReportPath = Common.Classes.clsMain.AppPath + @"\Payroll\Reports\rptFridaySum.rdlc";
                        SQLQuery = "Exec [rptAllowanceSheetSum] " + Common.Classes.clsMain.intComId + ", '" + gridProssType.ActiveRow.Cells[0].Value.ToString() + "','" + gridSec.ActiveRow.Cells[0].Value.ToString() + "','" + gridEmployee.ActiveRow.Cells[0].Value.ToString() + "','" + cboType.Text.ToString() + "','" + Band + "'";
                    }
                    else if ((gridReportCategory.ActiveRow.Cells["rptname"].Text.ToString() == "Denomination"))
                    {
                        ReportPath = Common.Classes.clsMain.AppPath + @"\Payroll\Reports\rptAllowDeno.rdlc";
                        SQLQuery = "Exec [rptAllowanceDeno] " + Common.Classes.clsMain.intComId + ", '" + gridProssType.ActiveRow.Cells[0].Value.ToString() + "','" + gridSec.ActiveRow.Cells[0].Value.ToString() + "','" + gridEmployee.ActiveRow.Cells[0].Value.ToString() + "','" + cboType.Text.ToString() + "','" + Band + "'";
                    }
                }

                else if ((cboType.Text.ToString() == "Holiday"))
                {
                    if ((gridReportCategory.ActiveRow.Cells["rptname"].Text.ToString() == "Allowance Sheet"))
                    {
                        ReportPath = Common.Classes.clsMain.AppPath + @"\Payroll\Reports\rptHoliday.rdlc";
                        SQLQuery = "Exec [rptAllowanceSheet] " + Common.Classes.clsMain.intComId + ", '" + gridProssType.ActiveRow.Cells[0].Value.ToString() + "','" + gridSec.ActiveRow.Cells[0].Value.ToString() + "','" + gridEmployee.ActiveRow.Cells[0].Value.ToString() + "','" + cboType.Text.ToString() + "','" + Band + "'";
                    }
                    else if ((gridReportCategory.ActiveRow.Cells["rptname"].Text.ToString() == "Summary Sheet"))
                    {
                        ReportPath = Common.Classes.clsMain.AppPath + @"\Payroll\Reports\rptHolidaySum.rdlc";
                        SQLQuery = "Exec [rptAllowanceSheetSum] " + Common.Classes.clsMain.intComId + ", '" + gridProssType.ActiveRow.Cells[0].Value.ToString() + "','" + gridSec.ActiveRow.Cells[0].Value.ToString() + "','" + gridEmployee.ActiveRow.Cells[0].Value.ToString() + "','" + cboType.Text.ToString() + "','" + Band + "'";
                    }

                    else if ((gridReportCategory.ActiveRow.Cells["rptname"].Text.ToString() == "Denomination"))
                    {
                        ReportPath = Common.Classes.clsMain.AppPath + @"\Payroll\Reports\rptAllowDeno.rdlc";
                        SQLQuery = "Exec [rptAllowanceDeno] " + Common.Classes.clsMain.intComId + ", '" + gridProssType.ActiveRow.Cells[0].Value.ToString() + "','" + gridSec.ActiveRow.Cells[0].Value.ToString() + "','" + gridEmployee.ActiveRow.Cells[0].Value.ToString() + "','" + cboType.Text.ToString() + "','" + Band + "'";
                    }
                }

                else if ((cboType.Text.ToString() == "Night"))
                {
                    if ((gridReportCategory.ActiveRow.Cells["rptname"].Text.ToString() == "Allowance Sheet"))
                    {
                        ReportPath = Common.Classes.clsMain.AppPath + @"\Payroll\Reports\rptNight.rdlc";
                        SQLQuery = "Exec [rptAllowanceSheet] " + Common.Classes.clsMain.intComId + ", '" + gridProssType.ActiveRow.Cells[0].Value.ToString() + "','" + gridSec.ActiveRow.Cells[0].Value.ToString() + "','" + gridEmployee.ActiveRow.Cells[0].Value.ToString() + "','" + cboType.Text.ToString() + "','" + Band + "'";
                    }
                    else if ((gridReportCategory.ActiveRow.Cells["rptname"].Text.ToString() == "Summary Sheet"))
                    {
                        ReportPath = Common.Classes.clsMain.AppPath + @"\Payroll\Reports\rptNightSum.rdlc";
                        SQLQuery = "Exec [rptAllowanceSheetSum] " + Common.Classes.clsMain.intComId + ", '" + gridProssType.ActiveRow.Cells[0].Value.ToString() + "','" + gridSec.ActiveRow.Cells[0].Value.ToString() + "','" + gridEmployee.ActiveRow.Cells[0].Value.ToString() + "','" + cboType.Text.ToString() + "','" + Band + "'";
                    }

                    else if ((gridReportCategory.ActiveRow.Cells["rptname"].Text.ToString() == "Denomination"))
                    {
                        ReportPath = Common.Classes.clsMain.AppPath + @"\Payroll\Reports\rptAllowDeno.rdlc";
                        SQLQuery = "Exec [rptAllowanceDeno] " + Common.Classes.clsMain.intComId + ", '" + gridProssType.ActiveRow.Cells[0].Value.ToString() + "','" + gridSec.ActiveRow.Cells[0].Value.ToString() + "','" + gridEmployee.ActiveRow.Cells[0].Value.ToString() + "','" + cboType.Text.ToString() + "','" + Band + "'";
                    }
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
            gridProssType.DisplayLayout.Bands[0].Columns["date"].Hidden = true;

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


        private void cboType_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            cboType.DisplayLayout.Bands[0].Columns["AllowName"].Width = cboType.Width;
            cboType.DisplayLayout.Bands[0].Columns["AllowName"].Header.Caption = "Allowance Type";
            cboType.DisplayLayout.Bands[0].Columns["AllowID"].Hidden = true;
            cboType.DisplayMember = "AllowName";
            cboType.ValueMember = "AllowName";

        }

        private void gridBand_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            gridBand.DisplayLayout.Bands[0].Columns["varId"].Hidden = true;
            gridBand.DisplayLayout.Bands[0].Columns["varName"].Width = 125;
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

            //create Check Box in List
            //gridEmployee.DisplayLayout.Bands[0].Columns["empCode"].Style = ColumnStyle.CheckBox;
            // gridEmployee.DisplayLayout.Override.HeaderCheckBoxVisibility = HeaderCheckBoxVisibility.Always;

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
            gridReportCategory.DisplayLayout.Bands[0].Columns["rptname"].Width = 210;
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
            dsDetails = new DataSet();

            ArrayList arQuery = new ArrayList();
            GTRLibrary.clsConnection clsCon = new GTRLibrary.clsConnection();
            
                string ReportPath = "", SQLQuery = "",Band = "";
                Band = gridBand.ActiveRow.Cells["varName"].Value.ToString();



                if ((cboType.Text.ToString() == "Transport"))
                {
                    if ((gridReportCategory.ActiveRow.Cells["rptname"].Text.ToString() == "Allowance Sheet"))
                    {
                        SQLQuery = "Exec [rptAllowanceSheet] " + Common.Classes.clsMain.intComId + ", '" + gridProssType.ActiveRow.Cells[0].Value.ToString() + "','" + gridSec.ActiveRow.Cells[0].Value.ToString() + "','" + gridEmployee.ActiveRow.Cells[0].Value.ToString() + "','" + cboType.Text.ToString() + "','" + Band + "'";
                        clsCon.GTRFillDatasetWithSQLCommand(ref dsDetails, SQLQuery);

                        dsDetails.Tables[0].TableName = "Rpt";

                        gridExcel.DataSource = null;
                        gridExcel.DataSource = dsDetails.Tables["Rpt"];
                    }
                    else if ((gridReportCategory.ActiveRow.Cells["rptname"].Text.ToString() == "Summary Sheet"))
                    {
                        SQLQuery = "Exec [rptAllowanceSheetSum] " + Common.Classes.clsMain.intComId + ", '" + gridProssType.ActiveRow.Cells[0].Value.ToString() + "','" + gridSec.ActiveRow.Cells[0].Value.ToString() + "','" + gridEmployee.ActiveRow.Cells[0].Value.ToString() + "','" + cboType.Text.ToString() + "','" + Band + "'";
                        clsCon.GTRFillDatasetWithSQLCommand(ref dsDetails, SQLQuery);

                        dsDetails.Tables[0].TableName = "Rpt";

                        gridExcel.DataSource = null;
                        gridExcel.DataSource = dsDetails.Tables["Rpt"];
                    }
                    else if ((gridReportCategory.ActiveRow.Cells["rptname"].Text.ToString() == "Denomination"))
                    {
                        SQLQuery = "Exec [rptAllowanceDeno] " + Common.Classes.clsMain.intComId + ", '" + gridProssType.ActiveRow.Cells[0].Value.ToString() + "','" + gridSec.ActiveRow.Cells[0].Value.ToString() + "','" + gridEmployee.ActiveRow.Cells[0].Value.ToString() + "','" + cboType.Text.ToString() + "','" + Band + "'";
                        clsCon.GTRFillDatasetWithSQLCommand(ref dsDetails, SQLQuery);

                        dsDetails.Tables[0].TableName = "Rpt";

                        gridExcel.DataSource = null;
                        gridExcel.DataSource = dsDetails.Tables["Rpt"];
                    }
                }
                else if ((cboType.Text.ToString() == "Friday"))
                {
                    if ((gridReportCategory.ActiveRow.Cells["rptname"].Text.ToString() == "Allowance Sheet"))
                    {
                        SQLQuery = "Exec [rptAllowanceSheet] " + Common.Classes.clsMain.intComId + ", '" + gridProssType.ActiveRow.Cells[0].Value.ToString() + "','" + gridSec.ActiveRow.Cells[0].Value.ToString() + "','" + gridEmployee.ActiveRow.Cells[0].Value.ToString() + "','" + cboType.Text.ToString() + "','" + Band + "'";

                        clsCon.GTRFillDatasetWithSQLCommand(ref dsDetails, SQLQuery);

                        dsDetails.Tables[0].TableName = "Rpt";

                        gridExcel.DataSource = null;
                        gridExcel.DataSource = dsDetails.Tables["Rpt"];
                    }
                    else if ((gridReportCategory.ActiveRow.Cells["rptname"].Text.ToString() == "Summary Sheet"))
                    {
                        SQLQuery = "Exec [rptAllowanceSheetSum] " + Common.Classes.clsMain.intComId + ", '" + gridProssType.ActiveRow.Cells[0].Value.ToString() + "','" + gridSec.ActiveRow.Cells[0].Value.ToString() + "','" + gridEmployee.ActiveRow.Cells[0].Value.ToString() + "','" + cboType.Text.ToString() + "','" + Band + "'";
                        clsCon.GTRFillDatasetWithSQLCommand(ref dsDetails, SQLQuery);

                        dsDetails.Tables[0].TableName = "Rpt";

                        gridExcel.DataSource = null;
                        gridExcel.DataSource = dsDetails.Tables["Rpt"];
                    }
                    else if ((gridReportCategory.ActiveRow.Cells["rptname"].Text.ToString() == "Denomination"))
                    {
                        SQLQuery = "Exec [rptAllowanceDeno] " + Common.Classes.clsMain.intComId + ", '" + gridProssType.ActiveRow.Cells[0].Value.ToString() + "','" + gridSec.ActiveRow.Cells[0].Value.ToString() + "','" + gridEmployee.ActiveRow.Cells[0].Value.ToString() + "','" + cboType.Text.ToString() + "','" + Band + "'";
                        clsCon.GTRFillDatasetWithSQLCommand(ref dsDetails, SQLQuery);

                        dsDetails.Tables[0].TableName = "Rpt";

                        gridExcel.DataSource = null;
                        gridExcel.DataSource = dsDetails.Tables["Rpt"];
                    }
                }

                else if ((cboType.Text.ToString() == "Holiday"))
                {
                    if ((gridReportCategory.ActiveRow.Cells["rptname"].Text.ToString() == "Allowance Sheet"))
                    {
                        SQLQuery = "Exec [rptAllowanceSheet] " + Common.Classes.clsMain.intComId + ", '" + gridProssType.ActiveRow.Cells[0].Value.ToString() + "','" + gridSec.ActiveRow.Cells[0].Value.ToString() + "','" + gridEmployee.ActiveRow.Cells[0].Value.ToString() + "','" + cboType.Text.ToString() + "','" + Band + "'";
                        clsCon.GTRFillDatasetWithSQLCommand(ref dsDetails, SQLQuery);

                        dsDetails.Tables[0].TableName = "Rpt";

                        gridExcel.DataSource = null;
                        gridExcel.DataSource = dsDetails.Tables["Rpt"];
                    }
                    else if ((gridReportCategory.ActiveRow.Cells["rptname"].Text.ToString() == "Summary Sheet"))
                    {
                        SQLQuery = "Exec [rptAllowanceSheetSum] " + Common.Classes.clsMain.intComId + ", '" + gridProssType.ActiveRow.Cells[0].Value.ToString() + "','" + gridSec.ActiveRow.Cells[0].Value.ToString() + "','" + gridEmployee.ActiveRow.Cells[0].Value.ToString() + "','" + cboType.Text.ToString() + "','" + Band + "'";
                        clsCon.GTRFillDatasetWithSQLCommand(ref dsDetails, SQLQuery);

                        dsDetails.Tables[0].TableName = "Rpt";

                        gridExcel.DataSource = null;
                        gridExcel.DataSource = dsDetails.Tables["Rpt"];
                    }

                    else if ((gridReportCategory.ActiveRow.Cells["rptname"].Text.ToString() == "Denomination"))
                    {
                        SQLQuery = "Exec [rptAllowanceDeno] " + Common.Classes.clsMain.intComId + ", '" + gridProssType.ActiveRow.Cells[0].Value.ToString() + "','" + gridSec.ActiveRow.Cells[0].Value.ToString() + "','" + gridEmployee.ActiveRow.Cells[0].Value.ToString() + "','" + cboType.Text.ToString() + "','" + Band + "'";
                        clsCon.GTRFillDatasetWithSQLCommand(ref dsDetails, SQLQuery);

                        dsDetails.Tables[0].TableName = "Rpt";

                        gridExcel.DataSource = null;
                        gridExcel.DataSource = dsDetails.Tables["Rpt"];
                    }
                }

                else if ((cboType.Text.ToString() == "Night"))
                {
                    if ((gridReportCategory.ActiveRow.Cells["rptname"].Text.ToString() == "Allowance Sheet"))
                    {
                        SQLQuery = "Exec [rptAllowanceSheet] " + Common.Classes.clsMain.intComId + ", '" + gridProssType.ActiveRow.Cells[0].Value.ToString() + "','" + gridSec.ActiveRow.Cells[0].Value.ToString() + "','" + gridEmployee.ActiveRow.Cells[0].Value.ToString() + "','" + cboType.Text.ToString() + "','" + Band + "'";
                        clsCon.GTRFillDatasetWithSQLCommand(ref dsDetails, SQLQuery);

                        dsDetails.Tables[0].TableName = "Rpt";

                        gridExcel.DataSource = null;
                        gridExcel.DataSource = dsDetails.Tables["Rpt"];
                    }
                    else if ((gridReportCategory.ActiveRow.Cells["rptname"].Text.ToString() == "Summary Sheet"))
                    {
                        SQLQuery = "Exec [rptAllowanceSheetSum] " + Common.Classes.clsMain.intComId + ", '" + gridProssType.ActiveRow.Cells[0].Value.ToString() + "','" + gridSec.ActiveRow.Cells[0].Value.ToString() + "','" + gridEmployee.ActiveRow.Cells[0].Value.ToString() + "','" + cboType.Text.ToString() + "','" + Band + "'";
                        clsCon.GTRFillDatasetWithSQLCommand(ref dsDetails, SQLQuery);

                        dsDetails.Tables[0].TableName = "Rpt";

                        gridExcel.DataSource = null;
                        gridExcel.DataSource = dsDetails.Tables["Rpt"];
                    }

                    else if ((gridReportCategory.ActiveRow.Cells["rptname"].Text.ToString() == "Denomination"))
                    {
                        SQLQuery = "Exec [rptAllowanceDeno] " + Common.Classes.clsMain.intComId + ", '" + gridProssType.ActiveRow.Cells[0].Value.ToString() + "','" + gridSec.ActiveRow.Cells[0].Value.ToString() + "','" + gridEmployee.ActiveRow.Cells[0].Value.ToString() + "','" + cboType.Text.ToString() + "','" + Band + "'";
                        clsCon.GTRFillDatasetWithSQLCommand(ref dsDetails, SQLQuery);

                        dsDetails.Tables[0].TableName = "Rpt";

                        gridExcel.DataSource = null;
                        gridExcel.DataSource = dsDetails.Tables["Rpt"];
                    }
                }



            DialogResult dlgRes =
            MessageBox.Show("Do You Want to Save the Data Sheet");
            if (dlgRes != DialogResult.OK)
            {
                return;
            }

            SaveFileDialog dlgSurveyExcel = new SaveFileDialog();
            dlgSurveyExcel.Filter = "Excel WorkBook (*.xls)|.xls";
            dlgSurveyExcel.FileName = cboType.Text.ToString() + "-" + gridReportCategory.ActiveRow.Cells["rptname"].Text.ToString() + "_" + DateTime.Now.ToShortDateString().Replace(@"/", "_");

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