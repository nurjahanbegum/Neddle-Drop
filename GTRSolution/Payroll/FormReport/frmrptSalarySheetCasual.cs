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

namespace GTRHRIS.Payroll.FormReport
{
    public partial class frmrptSalarySheetCasual : Form
    {
        System.Data.DataSet dsList;
        System.Data.DataSet dsDetails;
        clsProcedure clsProc = new clsProcedure();

        private Infragistics.Win.UltraWinTabControl.UltraTabControl uTab;
        Common.FormEntry.frmMaster FM;
        public frmrptSalarySheetCasual(ref Infragistics.Win.UltraWinTabControl.UltraTabControl utab, Common.FormEntry.frmMaster fm)
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
                string sqlquary = "Exec prcrptSalarySheetCasual " + Common.Classes.clsMain.intComId + ", 'Admin','" + cboType.Text.ToString() + "'";
                clscon.GTRFillDatasetWithSQLCommand(ref dsList, sqlquary);
                
                dsList.Tables[0].TableName = "ProssType";
                dsList.Tables[1].TableName = "Company"; 
                dsList.Tables[2].TableName = "Employee";
                dsList.Tables[3].TableName = "EmpType";
                dsList.Tables[4].TableName = "ReportCategory";

                gridProssType.DataSource = dsList.Tables["ProssType"];
                gridSec.DataSource = dsList.Tables["Company"];
                gridEmployee.DataSource = dsList.Tables["Employee"];
                gridReportCategory.DataSource = dsList.Tables["ReportCategory"];
                
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

                cboType.DataSource = dsList.Tables["EmpType"];

                cboType.Text = "Casual Worker";


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
                string sqlquary = "Exec prcrptSalarySheetCasual " + Common.Classes.clsMain.intComId + ", 'Admin','" + cboType.Text.ToString() + "'";
                clscon.GTRFillDatasetWithSQLCommand(ref dsList, sqlquary);

                dsList.Tables[0].TableName = "ProssType";

                gridProssType.DataSource = dsList.Tables["ProssType"];

            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        private void frmrptSalarySheetCasual_Load(object sender, EventArgs e)
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

        private void frmrptSalarySheetCasual_FormClosing(object sender, FormClosingEventArgs e)
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
            gridSec.DisplayLayout.Bands[0].Columns["CSComId"].Hidden = true;
            gridSec.DisplayLayout.Bands[0].Columns["CSComName"].Width = 190;
            gridSec.DisplayLayout.Bands[0].Columns["CSComName"].Header.Caption = "Company";

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
                string ReportPath = "", SQLQuery = "", DataSourceName = "DataSet1", FormCaption = "";

                DataSourceName = "DataSet1";
                FormCaption = "Report :: Payment Information...";

                if ((cboType.Text.ToString() == "Casual Worker"))
                {
                    if ((gridReportCategory.ActiveRow.Cells["rptname"].Text.ToString() == "Salary Sheet"))
                    {

                        ReportPath = Common.Classes.clsMain.AppPath + @"\Payroll\Reports\rptSalaryCasual.rdlc";
                        SQLQuery = "Exec [rptSalarySheetCasual] " + Common.Classes.clsMain.intComId + ", '" + gridProssType.ActiveRow.Cells[0].Value.ToString() + "','" + gridSec.ActiveRow.Cells[0].Value.ToString() + "','" + gridEmployee.ActiveRow.Cells[0].Value.ToString() + "','" + cboType.Text.ToString() + "'";

                    }


                    else if ((gridReportCategory.ActiveRow.Cells["rptname"].Text.ToString() == "Summary Sheet"))
                    {


                        ReportPath = Common.Classes.clsMain.AppPath + @"\Payroll\Reports\rptSalaryCasualSum.rdlc";
                        SQLQuery = "Exec [rptSalarySheetCasualSum] " + Common.Classes.clsMain.intComId + ", '" + gridProssType.ActiveRow.Cells[0].Value.ToString() + "','" + gridSec.ActiveRow.Cells[0].Value.ToString() + "','" + gridEmployee.ActiveRow.Cells[0].Value.ToString() + "','" + cboType.Text.ToString() + "'";

                    }

                    else if ((gridReportCategory.ActiveRow.Cells["rptname"].Text.ToString() == "Denomination"))
                    {


                        ReportPath = Common.Classes.clsMain.AppPath + @"\Payroll\Reports\rptSalaryCasualDeno.rdlc";
                        SQLQuery = "Exec [rptCasualDenomination] " + Common.Classes.clsMain.intComId + ", '" + gridProssType.ActiveRow.Cells[0].Value.ToString() + "','" + gridSec.ActiveRow.Cells[0].Value.ToString() + "','" + gridEmployee.ActiveRow.Cells[0].Value.ToString() + "','" + cboType.Text.ToString() + "'";

                    }
                }
                else
                {
                    if ((gridReportCategory.ActiveRow.Cells["rptname"].Text.ToString() == "Salary Sheet"))
                    {

                        ReportPath = Common.Classes.clsMain.AppPath + @"\Payroll\Reports\rptSalarySubCont.rdlc";
                        SQLQuery = "Exec [rptSalarySheetCasual] " + Common.Classes.clsMain.intComId + ", '" + gridProssType.ActiveRow.Cells[0].Value.ToString() + "','" + gridSec.ActiveRow.Cells[0].Value.ToString() + "','" + gridEmployee.ActiveRow.Cells[0].Value.ToString() + "','" + cboType.Text.ToString() + "'";

                    }


                    else if ((gridReportCategory.ActiveRow.Cells["rptname"].Text.ToString() == "Summary Sheet"))
                    {
                       
                        ReportPath = Common.Classes.clsMain.AppPath + @"\Payroll\Reports\rptSalaryCasualSum.rdlc";
                        SQLQuery = "Exec [rptSalarySheetCasualSum] " + Common.Classes.clsMain.intComId + ", '" + gridProssType.ActiveRow.Cells[0].Value.ToString() + "','" + gridSec.ActiveRow.Cells[0].Value.ToString() + "','" + gridEmployee.ActiveRow.Cells[0].Value.ToString() + "','" + cboType.Text.ToString() + "'";

                    }

                    else if ((gridReportCategory.ActiveRow.Cells["rptname"].Text.ToString() == "Denomination"))
                    {

                        ReportPath = Common.Classes.clsMain.AppPath + @"\Payroll\Reports\rptSalaryCasualDeno.rdlc";
                        SQLQuery = "Exec [rptCasualDenomination] " + Common.Classes.clsMain.intComId + ", '" + gridProssType.ActiveRow.Cells[0].Value.ToString() + "','" + gridSec.ActiveRow.Cells[0].Value.ToString() + "','" + gridEmployee.ActiveRow.Cells[0].Value.ToString() + "','" + cboType.Text.ToString() + "'";

                    }
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

        private void gridEmployee_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            gridEmployee.DisplayLayout.Bands[0].Columns["empId"].Hidden = true;
            gridEmployee.DisplayLayout.Bands[0].Columns["empCode"].Width = 100;
            gridEmployee.DisplayLayout.Bands[0].Columns["empCode"].Header.Caption = "Employee Code";
            gridEmployee.DisplayLayout.Bands[0].Columns["EmpName"].Width = 228;
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
            gridEmployee.DisplayLayout.Bands[0].Columns["EmpCode"].AllowRowFiltering = DefaultableBoolean.False;
        }

        private void gridProssType_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            gridProssType.DisplayLayout.Bands[0].Columns["Month"].Hidden = true;
            gridProssType.DisplayLayout.Bands[0].Columns["year"].Hidden = true;
            gridProssType.DisplayLayout.Bands[0].Columns["date"].Hidden = true;

            gridProssType.DisplayLayout.Bands[0].Columns["ProssType"].Width = 275;
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

        private void gridReportCategory_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            gridReportCategory.DisplayLayout.Bands[0].Columns["rptid"].Hidden = true;
            gridReportCategory.DisplayLayout.Bands[0].Columns["rptname"].Width = 190;
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

        private void cboType_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            cboType.DisplayLayout.Bands[0].Columns["varName"].Width = cboType.Width;
            cboType.DisplayLayout.Bands[0].Columns["varName"].Header.Caption = "Employee Type";
            cboType.DisplayMember = "varName";
        }

   }
}