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
    public partial class frmrptSalarySheetB : Form
    {
        System.Data.DataSet dsList;
        System.Data.DataSet dsDetails;
        clsProcedure clsProc = new clsProcedure();

        private Infragistics.Win.UltraWinTabControl.UltraTabControl uTab;
        Common.FormEntry.frmMaster FM;
        public frmrptSalarySheetB(ref Infragistics.Win.UltraWinTabControl.UltraTabControl utab, Common.FormEntry.frmMaster fm)
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
                string sqlquary = "Exec prcrptSalarySheetB " + Common.Classes.clsMain.intComId + "";
                clscon.GTRFillDatasetWithSQLCommand(ref dsList, sqlquary);
                
                dsList.Tables[0].TableName = "ProssType";
                dsList.Tables[2].TableName = "Section"; 
                dsList.Tables[3].TableName = "Employee";
                dsList.Tables[4].TableName = "PayMode";
                dsList.Tables[6].TableName = "EmpType";
                dsList.Tables[7].TableName = "EmpStatus";
                dsList.Tables[8].TableName = "ReportCategory"; 
                dsList.Tables[9].TableName = "Band";

                gridProssType.DataSource = dsList.Tables["ProssType"];
                gridSec.DataSource = dsList.Tables["Section"];
                gridEmployee.DataSource = dsList.Tables["Employee"];
                gridReportCategory.DataSource = dsList.Tables["ReportCategory"];
                gridBand.DataSource = dsList.Tables["Band"];

                dtFirst.Value = DateTime.Now;
                dtLast.Value = DateTime.Now;

            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        private  void prcLoadCombo()
        {
            try
            {
                cboPayMode.DataSource = dsList.Tables["PayMode"];
                cboEmpType.DataSource = dsList.Tables["EmpType"];
                cboStatus.DataSource = dsList.Tables["EmpStatus"];

                cboPayMode.Text = "=ALL=";
                cboEmpType.Text = "=ALL=";
                cboStatus.Text = "=ALL=";

            }
            catch (Exception ex)
            {
                throw(ex);
            }
        }

        private void frmrptSalarySheet_Load(object sender, EventArgs e)
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

        private void frmrptSalarySheet_FormClosing(object sender, FormClosingEventArgs e)
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
            gridSec.DisplayLayout.Bands[0].Columns["SectName"].Width = 190;
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
                string ReportPath = "", SQLQuery = "", DataSourceName = "DataSet1", FormCaption = "", Band = "=All="; //EmpStatus = "=All="

                DataSourceName = "DataSet1";
                FormCaption = "Report :: Salary Information...";
                Band = gridBand.ActiveRow.Cells["varName"].Value.ToString();
                //   EmpStatus = cboStatus.ActiveRow.Cells["EmpStatus"].Value.ToString();

                if ((gridReportCategory.ActiveRow.Cells["rptname"].Text.ToString() == "Salary Sheet"))
                {
                    if (cboStatus.Text == "=ALL=")
                    {
                        ReportPath = Common.Classes.clsMain.AppPath + @"\Payroll\Reports\rptSalarySheetB.rdlc";
                        SQLQuery = "Exec [rptSalarySheetB] " + Common.Classes.clsMain.intComId + ", '" + gridProssType.ActiveRow.Cells[0].Value.ToString() + "','" + cboPayMode.Value.ToString() + "', '" + cboEmpType.Value.ToString() + "','" + gridSec.ActiveRow.Cells[0].Value.ToString() + "','" + gridEmployee.ActiveRow.Cells[0].Value.ToString() + "','" + Band + "','" + cboStatus.Value.ToString() + "'";
                    }
                    else if (cboStatus.Text == "Released")
                    {
                        ReportPath = Common.Classes.clsMain.AppPath + @"\Payroll\Reports\rptSalarySheetB.rdlc";
                        SQLQuery = "Exec [rptSalarySheetReleasedB] " + Common.Classes.clsMain.intComId + ", '" + gridProssType.ActiveRow.Cells[0].Value.ToString() + "','" + cboPayMode.Value.ToString() + "', '" + cboEmpType.Value.ToString() + "','" + gridSec.ActiveRow.Cells[0].Value.ToString() + "','" + gridEmployee.ActiveRow.Cells[0].Value.ToString() + "','" + Band + "','" + cboStatus.Value.ToString() + "'";
                    }
                    else
                    {
                        ReportPath = Common.Classes.clsMain.AppPath + @"\Payroll\Reports\rptSalarySheetB.rdlc";
                        SQLQuery = "Exec [rptSalarySheetB] " + Common.Classes.clsMain.intComId + ", '" + gridProssType.ActiveRow.Cells[0].Value.ToString() + "','" + cboPayMode.Value.ToString() + "', '" + cboEmpType.Value.ToString() + "','" + gridSec.ActiveRow.Cells[0].Value.ToString() + "','" + gridEmployee.ActiveRow.Cells[0].Value.ToString() + "','" + Band + "','" + cboStatus.Value.ToString() + "'";
                    }
                }
                else if ((gridReportCategory.ActiveRow.Cells["rptname"].Text.ToString() == "Salary Sheet Excel"))
                {
                    ReportPath = Common.Classes.clsMain.AppPath + @"\Payroll\Reports\rptSalarySheetExcelB.rdlc";
                    SQLQuery = "Exec [rptSalarySheetB] " + Common.Classes.clsMain.intComId + ", '" + gridProssType.ActiveRow.Cells[0].Value.ToString() + "','" + cboPayMode.Value.ToString() + "', '" + cboEmpType.Value.ToString() + "','" + gridSec.ActiveRow.Cells[0].Value.ToString() + "','" + gridEmployee.ActiveRow.Cells[0].Value.ToString() + "','" + Band + "','" + cboStatus.Value.ToString() + "'";
                }          
                else if ((gridReportCategory.ActiveRow.Cells["rptname"].Text.ToString() == "Payslip"))
                {
                    ReportPath = Common.Classes.clsMain.AppPath + @"\Payroll\Reports\rptPayslipB.rdlc";
                    SQLQuery = "Exec [rptSalarySheetPaySlipB] " + Common.Classes.clsMain.intComId + ", '" + gridProssType.ActiveRow.Cells[0].Value.ToString() + "', '" + cboPayMode.Value.ToString() + "', '" + cboEmpType.Value.ToString() + "','" + gridSec.ActiveRow.Cells[0].Value.ToString() + "','" + gridEmployee.ActiveRow.Cells[0].Value.ToString() + "','" + Band + "','" + cboStatus.Value.ToString() + "'";
                }
                else if ((gridReportCategory.ActiveRow.Cells["rptname"].Text.ToString() == "Data Check List"))
                {
                    ReportPath = Common.Classes.clsMain.AppPath + @"\Payroll\Reports\rptDataCheckList.rdlc";
                    SQLQuery = "Exec [rptSalaryDataCheckList] " + Common.Classes.clsMain.intComId + ", '" + gridProssType.ActiveRow.Cells[0].Value.ToString() + "','" + cboPayMode.Value.ToString() + "', '" + cboEmpType.Value.ToString() + "','" + gridSec.ActiveRow.Cells[0].Value.ToString() + "','" + gridEmployee.ActiveRow.Cells[0].Value.ToString() + "','" + Band + "','" + cboStatus.Value.ToString() + "'";
                }
                else if ((gridReportCategory.ActiveRow.Cells["rptname"].Text.ToString() == "Summary Sheet"))
                {
                    if (cboStatus.Text == "=ALL=")
                    {
                        ReportPath = Common.Classes.clsMain.AppPath + @"\Payroll\Reports\rptSalarySumB.rdlc";
                        SQLQuery = "Exec [rptSalarySumB] " + Common.Classes.clsMain.intComId + ", '" + gridProssType.ActiveRow.Cells[0].Value.ToString() + "', '" + cboPayMode.Value.ToString() + "', '" + cboEmpType.Value.ToString() + "','" + gridSec.ActiveRow.Cells[0].Value.ToString() + "','" + gridEmployee.ActiveRow.Cells[0].Value.ToString() + "','" + Band + "','" + cboStatus.Value.ToString() + "'";
                    }

                    else if (cboStatus.Text == "Released")
                    {
                        ReportPath = Common.Classes.clsMain.AppPath + @"\Payroll\Reports\rptSalarySumB.rdlc";
                        SQLQuery = "Exec [rptSalaryReleasedSumB] " + Common.Classes.clsMain.intComId + ", '" + gridProssType.ActiveRow.Cells[0].Value.ToString() + "', '" + cboPayMode.Value.ToString() + "', '" + cboEmpType.Value.ToString() + "','" + gridSec.ActiveRow.Cells[0].Value.ToString() + "','" + gridEmployee.ActiveRow.Cells[0].Value.ToString() + "','" + Band + "','" + cboStatus.Value.ToString() + "'";
                    }

                    else
                    {
                        ReportPath = Common.Classes.clsMain.AppPath + @"\Payroll\Reports\rptSalarySumB.rdlc";
                        SQLQuery = "Exec [rptSalarySumB] " + Common.Classes.clsMain.intComId + ", '" + gridProssType.ActiveRow.Cells[0].Value.ToString() + "', '" + cboPayMode.Value.ToString() + "', '" + cboEmpType.Value.ToString() + "','" + gridSec.ActiveRow.Cells[0].Value.ToString() + "','" + gridEmployee.ActiveRow.Cells[0].Value.ToString() + "','" + Band + "','" + cboStatus.Value.ToString() + "'";
                    }
                }
                else if ((gridReportCategory.ActiveRow.Cells["rptname"].Text.ToString() == "Individual Salary Sheet"))
                {
                    ReportPath = Common.Classes.clsMain.AppPath + @"\Payroll\Reports\rptSalarySheetSingle.rdlc";
                    SQLQuery = "Exec [rptSalarySheetInd] " + Common.Classes.clsMain.intComId + ", '" + gridProssType.ActiveRow.Cells[0].Value.ToString() + "','" + cboPayMode.Value.ToString() + "', '" + cboEmpType.Value.ToString() + "','" + gridSec.ActiveRow.Cells[0].Value.ToString() + "','" + gridEmployee.ActiveRow.Cells[0].Value.ToString() + "','" + Band + "','" + cboStatus.Value.ToString() + "','" + clsProc.GTRDate(dtFirst.Value.ToString()) + "','" + clsProc.GTRDate(dtLast.Value.ToString()) + "'";
                }
                else if ((gridReportCategory.ActiveRow.Cells["rptname"].Text.ToString() == "Denomination"))
                {
                    ReportPath = Common.Classes.clsMain.AppPath + @"\Payroll\Reports\rptSalarySumDenomination.rdlc";
                    SQLQuery = "Exec [rptSalarySumDenomination] " + Common.Classes.clsMain.intComId + ", '" + gridProssType.ActiveRow.Cells[0].Value.ToString() + "', '" + cboPayMode.Value.ToString() + "', '" + cboEmpType.Value.ToString() + "','" + gridSec.ActiveRow.Cells[0].Value.ToString() + "','" + gridEmployee.ActiveRow.Cells[0].Value.ToString() + "','" + Band + "','" + cboStatus.Value.ToString() + "'";
                }
                else if ((gridReportCategory.ActiveRow.Cells["rptname"].Text.ToString() == "Other Allowance Sheet"))
                {
                    ReportPath = Common.Classes.clsMain.AppPath + @"\Payroll\Reports\rptSalaryAddition.rdlc";
                    SQLQuery = "Exec [rptSalaryAll] " + Common.Classes.clsMain.intComId + ", '" + gridProssType.ActiveRow.Cells[0].Value.ToString() + "','" + cboPayMode.Value.ToString() + "', '" + cboEmpType.Value.ToString() + "','" + gridSec.ActiveRow.Cells[0].Value.ToString() + "','" + gridEmployee.ActiveRow.Cells[0].Value.ToString() + "','" + Band + "','" + cboStatus.Value.ToString() + "','Other Allowance Sheet'";
                }
                else if ((gridReportCategory.ActiveRow.Cells["rptname"].Text.ToString() == "Addition Sheet"))
                {
                    ReportPath = Common.Classes.clsMain.AppPath + @"\Payroll\Reports\rptSalaryAddition.rdlc";
                    SQLQuery = "Exec [rptSalaryAll] " + Common.Classes.clsMain.intComId + ", '" + gridProssType.ActiveRow.Cells[0].Value.ToString() + "','" + cboPayMode.Value.ToString() + "', '" + cboEmpType.Value.ToString() + "','" + gridSec.ActiveRow.Cells[0].Value.ToString() + "','" + gridEmployee.ActiveRow.Cells[0].Value.ToString() + "','" + Band + "','" + cboStatus.Value.ToString() + "','Addition Sheet'";
                }
                else if ((gridReportCategory.ActiveRow.Cells["rptname"].Text.ToString() == "Production Bonus"))
                {
                    ReportPath = Common.Classes.clsMain.AppPath + @"\Payroll\Reports\rptSalaryAddition.rdlc";
                    SQLQuery = "Exec [rptSalaryAll] " + Common.Classes.clsMain.intComId + ", '" + gridProssType.ActiveRow.Cells[0].Value.ToString() + "','" + cboPayMode.Value.ToString() + "', '" + cboEmpType.Value.ToString() + "','" + gridSec.ActiveRow.Cells[0].Value.ToString() + "','" + gridEmployee.ActiveRow.Cells[0].Value.ToString() + "','" + Band + "','" + cboStatus.Value.ToString() + "','Production Bonus'";
                }
                else if ((gridReportCategory.ActiveRow.Cells["rptname"].Text.ToString() == "Loan Sheet"))
                {
                    ReportPath = Common.Classes.clsMain.AppPath + @"\Payroll\Reports\rptSalaryLoan.rdlc";
                    SQLQuery = "Exec [rptSalaryAll] " + Common.Classes.clsMain.intComId + ", '" + gridProssType.ActiveRow.Cells[0].Value.ToString() + "','" + cboPayMode.Value.ToString() + "', '" + cboEmpType.Value.ToString() + "','" + gridSec.ActiveRow.Cells[0].Value.ToString() + "','" + gridEmployee.ActiveRow.Cells[0].Value.ToString() + "','" + Band + "','" + cboStatus.Value.ToString() + "','Loan Sheet'";
                }
                else if ((gridReportCategory.ActiveRow.Cells["rptname"].Text.ToString() == "Advance Sheet"))
                {
                    ReportPath = Common.Classes.clsMain.AppPath + @"\Payroll\Reports\rptSalaryAdvance.rdlc";
                    SQLQuery = "Exec [rptSalaryAll] " + Common.Classes.clsMain.intComId + ", '" + gridProssType.ActiveRow.Cells[0].Value.ToString() + "','" + cboPayMode.Value.ToString() + "', '" + cboEmpType.Value.ToString() + "','" + gridSec.ActiveRow.Cells[0].Value.ToString() + "','" + gridEmployee.ActiveRow.Cells[0].Value.ToString() + "','" + Band + "','" + cboStatus.Value.ToString() + "','Advance Sheet'";
                }
                else if ((gridReportCategory.ActiveRow.Cells["rptname"].Text.ToString() == "Income Tax"))
                {
                    ReportPath = Common.Classes.clsMain.AppPath + @"\Payroll\Reports\rptSalaryAdvance.rdlc";
                    SQLQuery = "Exec [rptSalaryAll] " + Common.Classes.clsMain.intComId + ", '" + gridProssType.ActiveRow.Cells[0].Value.ToString() + "','" + cboPayMode.Value.ToString() + "', '" + cboEmpType.Value.ToString() + "','" + gridSec.ActiveRow.Cells[0].Value.ToString() + "','" + gridEmployee.ActiveRow.Cells[0].Value.ToString() + "','" + Band + "','" + cboStatus.Value.ToString() + "','Income Tax'";
                }
                else if ((gridReportCategory.ActiveRow.Cells["rptname"].Text.ToString() == "PF Sheet"))
                {
                    ReportPath = Common.Classes.clsMain.AppPath + @"\Payroll\Reports\rptSalaryPF.rdlc";
                    SQLQuery = "Exec [rptSalaryAll] " + Common.Classes.clsMain.intComId + ", '" + gridProssType.ActiveRow.Cells[0].Value.ToString() + "','" + cboPayMode.Value.ToString() + "', '" + cboEmpType.Value.ToString() + "','" + gridSec.ActiveRow.Cells[0].Value.ToString() + "','" + gridEmployee.ActiveRow.Cells[0].Value.ToString() + "','" + Band + "','" + cboStatus.Value.ToString() + "','PF Sheet'";
                }
                else if ((gridReportCategory.ActiveRow.Cells["rptname"].Text.ToString() == "Deduction Sheet"))
                {
                    ReportPath = Common.Classes.clsMain.AppPath + @"\Payroll\Reports\rptSalaryAdvance.rdlc";
                    SQLQuery = "Exec [rptSalaryAll] " + Common.Classes.clsMain.intComId + ", '" + gridProssType.ActiveRow.Cells[0].Value.ToString() + "','" + cboPayMode.Value.ToString() + "', '" + cboEmpType.Value.ToString() + "','" + gridSec.ActiveRow.Cells[0].Value.ToString() + "','" + gridEmployee.ActiveRow.Cells[0].Value.ToString() + "','" + Band + "','" + cboStatus.Value.ToString() + "','Deduction Sheet'";
                }
                else if ((gridReportCategory.ActiveRow.Cells["rptname"].Text.ToString() == "Bank Sheet"))
                {
                    ReportPath = Common.Classes.clsMain.AppPath + @"\Payroll\Reports\rptSalaryBankACC.rdlc";
                    SQLQuery = "Exec [rptSalarySheetCadre] " + Common.Classes.clsMain.intComId + ", '" + gridProssType.ActiveRow.Cells[0].Value.ToString() + "','" + cboPayMode.Value.ToString() + "', '" + cboEmpType.Value.ToString() + "','" + gridSec.ActiveRow.Cells[0].Value.ToString() + "','" + gridEmployee.ActiveRow.Cells[0].Value.ToString() + "','" + Band + "','" + cboStatus.Value.ToString() + "','Bank Sheet'";
                }
                else if ((gridReportCategory.ActiveRow.Cells["rptname"].Text.ToString() == "Cadre Sheet"))
                {
                    ReportPath = Common.Classes.clsMain.AppPath + @"\Payroll\Reports\rptSalaryCadre.rdlc";
                    SQLQuery = "Exec [rptSalarySheetCadre] " + Common.Classes.clsMain.intComId + ", '" + gridProssType.ActiveRow.Cells[0].Value.ToString() + "','" + cboPayMode.Value.ToString() + "', '" + cboEmpType.Value.ToString() + "','" + gridSec.ActiveRow.Cells[0].Value.ToString() + "','" + gridEmployee.ActiveRow.Cells[0].Value.ToString() + "','" + Band + "','" + cboStatus.Value.ToString() + "','Cadre Sheet'";
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
            //gridEmployee.DisplayLayout.Bands[0].Columns["EmpCode"].AllowRowFiltering = DefaultableBoolean.False;
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


        private void cboPayMode_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            cboPayMode.DisplayLayout.Bands[0].Columns["PayMode"].Width = cboPayMode.Width;
            cboPayMode.DisplayLayout.Bands[0].Columns["PayMode"].Header.Caption = "Pay Mode";
            cboPayMode.DisplayMember = "PayMode";
            cboPayMode.ValueMember = "PayMode";
        }

        private void cboEmpType_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            cboEmpType.DisplayLayout.Bands[0].Columns["EmpType"].Width = cboEmpType.Width;
            cboEmpType.DisplayLayout.Bands[0].Columns["EmpType"].Header.Caption = "Employee Type";
            cboEmpType.DisplayMember = "EmpType";
            cboEmpType.ValueMember = "EmpType";
        }

        private void cboStatus_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            cboStatus.DisplayLayout.Bands[0].Columns["EmpStatus"].Width = cboStatus.Width;
            cboStatus.DisplayLayout.Bands[0].Columns["EmpStatus"].Header.Caption = "Employee Status";
            cboStatus.DisplayMember = "EmpStatus";
            cboStatus.ValueMember = "EmpStatus";
        }


        private void optCriteria_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void cboUnit_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void cboPaySource_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void cboPayMode_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void cboEmpType_KeyDown(object sender, KeyEventArgs e)
        {
            clsProc.GTRTabMove((Int16)e.KeyCode);
        }

        private void cboEmpStatus_KeyDown(object sender, KeyEventArgs e)
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

            string Band = "";

            Band = gridBand.ActiveRow.Cells["varName"].Value.ToString();

            if ((gridReportCategory.ActiveRow.Cells["rptname"].Text.ToString() == "Salary Sheet"))
            {
                string sqlquary = "Exec [rptSalarySheetExcelB] " + Common.Classes.clsMain.intComId + ", '" + gridProssType.ActiveRow.Cells[0].Value.ToString() + "','" + cboPayMode.Value.ToString() + "', '" + cboEmpType.Value.ToString() + "','" + gridSec.ActiveRow.Cells[0].Value.ToString() + "','" + gridEmployee.ActiveRow.Cells[0].Value.ToString() + "','" + Band + "','" + cboStatus.Value.ToString() + "','Salary Sheet'";
                clscon.GTRFillDatasetWithSQLCommand(ref dsList, sqlquary);

                dsList.Tables[0].TableName = "Salary";
                gridExcel.DataSource = null;
                gridExcel.DataSource = dsList.Tables["Salary"];

            }

            else if ((gridReportCategory.ActiveRow.Cells["rptname"].Text.ToString() == "Summary Sheet"))
            {
                string sqlquary = "Exec [rptSalarySheetExcelB] " + Common.Classes.clsMain.intComId + ", '" + gridProssType.ActiveRow.Cells[0].Value.ToString() + "','" + cboPayMode.Value.ToString() + "', '" + cboEmpType.Value.ToString() + "','" + gridSec.ActiveRow.Cells[0].Value.ToString() + "','" + gridEmployee.ActiveRow.Cells[0].Value.ToString() + "','" + Band + "','" + cboStatus.Value.ToString() + "','Summary Sheet'";
                clscon.GTRFillDatasetWithSQLCommand(ref dsList, sqlquary);

                dsList.Tables[0].TableName = "Salary";

                gridExcel.DataSource = null;
                gridExcel.DataSource = dsList.Tables["Salary"];
            }

            else if ((gridReportCategory.ActiveRow.Cells["rptname"].Text.ToString() == "Addition Sheet"))
            {
                string sqlquary = "Exec [rptSalarySheetExcelB] " + Common.Classes.clsMain.intComId + ", '" + gridProssType.ActiveRow.Cells[0].Value.ToString() + "','" + cboPayMode.Value.ToString() + "', '" + cboEmpType.Value.ToString() + "','" + gridSec.ActiveRow.Cells[0].Value.ToString() + "','" + gridEmployee.ActiveRow.Cells[0].Value.ToString() + "','" + Band + "','" + cboStatus.Value.ToString() + "','Arrear Sheet'";
                clscon.GTRFillDatasetWithSQLCommand(ref dsList, sqlquary);

                dsList.Tables[0].TableName = "Salary";

                gridExcel.DataSource = null;
                gridExcel.DataSource = dsList.Tables["Salary"];
            }
            else if ((gridReportCategory.ActiveRow.Cells["rptname"].Text.ToString() == "Advance Sheet"))
            {
                string sqlquary = "Exec [rptSalarySheetExcelB] " + Common.Classes.clsMain.intComId + ", '" + gridProssType.ActiveRow.Cells[0].Value.ToString() + "','" + cboPayMode.Value.ToString() + "', '" + cboEmpType.Value.ToString() + "','" + gridSec.ActiveRow.Cells[0].Value.ToString() + "','" + gridEmployee.ActiveRow.Cells[0].Value.ToString() + "','" + Band + "','" + cboStatus.Value.ToString() + "','Advance Sheet'";
                clscon.GTRFillDatasetWithSQLCommand(ref dsList, sqlquary);

                dsList.Tables[0].TableName = "Salary";

                gridExcel.DataSource = null;
                gridExcel.DataSource = dsList.Tables["Salary"];
            }
            else if ((gridReportCategory.ActiveRow.Cells["rptname"].Text.ToString() == "Deduction Sheet"))
            {
                string sqlquary = "Exec [rptSalarySheetExcelB] " + Common.Classes.clsMain.intComId + ", '" + gridProssType.ActiveRow.Cells[0].Value.ToString() + "','" + cboPayMode.Value.ToString() + "', '" + cboEmpType.Value.ToString() + "','" + gridSec.ActiveRow.Cells[0].Value.ToString() + "','" + gridEmployee.ActiveRow.Cells[0].Value.ToString() + "','" + Band + "','" + cboStatus.Value.ToString() + "','Deduction Sheet'";
                clscon.GTRFillDatasetWithSQLCommand(ref dsList, sqlquary);

                dsList.Tables[0].TableName = "Salary";

                gridExcel.DataSource = null;
                gridExcel.DataSource = dsList.Tables["Salary"];
            }
            else if ((gridReportCategory.ActiveRow.Cells["rptname"].Text.ToString() == "Loan Sheet"))
            {
                string sqlquary = "Exec [rptSalarySheetExcelB] " + Common.Classes.clsMain.intComId + ", '" + gridProssType.ActiveRow.Cells[0].Value.ToString() + "','" + cboPayMode.Value.ToString() + "', '" + cboEmpType.Value.ToString() + "','" + gridSec.ActiveRow.Cells[0].Value.ToString() + "','" + gridEmployee.ActiveRow.Cells[0].Value.ToString() + "','" + Band + "','" + cboStatus.Value.ToString() + "','Loan Sheet'";
                clscon.GTRFillDatasetWithSQLCommand(ref dsList, sqlquary);

                dsList.Tables[0].TableName = "Salary";

                gridExcel.DataSource = null;
                gridExcel.DataSource = dsList.Tables["Salary"];
            }
            else if ((gridReportCategory.ActiveRow.Cells["rptname"].Text.ToString() == "PF Sheet"))
            {
                string sqlquary = "Exec [rptSalarySheetExcelB] " + Common.Classes.clsMain.intComId + ", '" + gridProssType.ActiveRow.Cells[0].Value.ToString() + "','" + cboPayMode.Value.ToString() + "', '" + cboEmpType.Value.ToString() + "','" + gridSec.ActiveRow.Cells[0].Value.ToString() + "','" + gridEmployee.ActiveRow.Cells[0].Value.ToString() + "','" + Band + "','" + cboStatus.Value.ToString() + "','PF Sheet'";
                clscon.GTRFillDatasetWithSQLCommand(ref dsList, sqlquary);

                dsList.Tables[0].TableName = "Salary";

                gridExcel.DataSource = null;
                gridExcel.DataSource = dsList.Tables["Salary"];
            }
            else if ((gridReportCategory.ActiveRow.Cells["rptname"].Text.ToString() == "Denomination"))
            {
                string sqlquary = "Exec [rptSalarySheetExcelB] " + Common.Classes.clsMain.intComId + ", '" + gridProssType.ActiveRow.Cells[0].Value.ToString() + "','" + cboPayMode.Value.ToString() + "', '" + cboEmpType.Value.ToString() + "','" + gridSec.ActiveRow.Cells[0].Value.ToString() + "','" + gridEmployee.ActiveRow.Cells[0].Value.ToString() + "','" + Band + "','" + cboStatus.Value.ToString() + "','Denomination'";
                clscon.GTRFillDatasetWithSQLCommand(ref dsList, sqlquary);

                dsList.Tables[0].TableName = "Salary";

                gridExcel.DataSource = null;
                gridExcel.DataSource = dsList.Tables["Salary"];
            }
            else if ((gridReportCategory.ActiveRow.Cells["rptname"].Text.ToString() == "Bank Sheet"))
            {
                string sqlquary = "Exec [rptSalarySheetExcelB] " + Common.Classes.clsMain.intComId + ", '" + gridProssType.ActiveRow.Cells[0].Value.ToString() + "','" + cboPayMode.Value.ToString() + "', '" + cboEmpType.Value.ToString() + "','" + gridSec.ActiveRow.Cells[0].Value.ToString() + "','" + gridEmployee.ActiveRow.Cells[0].Value.ToString() + "','" + Band + "','" + cboStatus.Value.ToString() + "','Bank Sheet'";
                clscon.GTRFillDatasetWithSQLCommand(ref dsList, sqlquary);

                dsList.Tables[0].TableName = "Salary";

                gridExcel.DataSource = null;
                gridExcel.DataSource = dsList.Tables["Salary"];
            }

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

        private void gridExcel_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            //gridExcel.DisplayLayout.Bands[0].Columns["ComName"].Hidden = true;
            //gridExcel.DisplayLayout.Bands[0].Columns["ComAdd1"].Hidden = true;
            //gridExcel.DisplayLayout.Bands[0].Columns["ComAdd2"].Hidden = true;
            //gridExcel.DisplayLayout.Bands[0].Columns["Caption"].Hidden = true;
            //gridExcel.DisplayLayout.Bands[0].Columns["CardNo"].Hidden = true;
            //gridExcel.DisplayLayout.Bands[0].Columns["PStatus"].Hidden = true;
            //gridExcel.DisplayLayout.Bands[0].Columns["PTimeIn"].Hidden = true;
            //gridExcel.DisplayLayout.Bands[0].Columns["PTimeOut"].Hidden = true;
            //gridExcel.DisplayLayout.Bands[0].Columns["AbTn"].Hidden = true;

            //gridExcel.DisplayLayout.Bands[0].Columns["dtFromDate"].Hidden = true;
            //gridExcel.DisplayLayout.Bands[0].Columns["Remarks"].Hidden = true;
            //gridExcel.DisplayLayout.Bands[0].Columns["sSlNo"].Hidden = true;
            //gridExcel.DisplayLayout.Bands[0].Columns["DSlNo"].Hidden = true;

            //gridExcel.DisplayLayout.Bands[0].Columns["EmpID"].Hidden = true;
            //gridExcel.DisplayLayout.Bands[0].Columns["DeptName"].Hidden = true;
            //gridExcel.DisplayLayout.Bands[0].Columns["OTHour"].Hidden = true;
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

        private void gridReportCategory_AfterRowActivate(object sender, EventArgs e)
        {
            if ((gridReportCategory.ActiveRow.Cells["rptname"].Text.ToString() == "Individual Salary Sheet"))
            {
                lvlFirst.Visible = true;
                lvlLast.Visible = true;
                dtFirst.Visible = true;
                dtLast.Visible = true;
            }
            else
            {
                lvlFirst.Visible = false;
                lvlLast.Visible = false;
                dtFirst.Visible = false;
                dtLast.Visible = false;
            }
        }

        private void gridBand_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            gridBand.DisplayLayout.Bands[0].Columns["varID"].Hidden = true;
            gridBand.DisplayLayout.Bands[0].Columns["varName"].Width = 175;
            gridBand.DisplayLayout.Bands[0].Columns["varName"].Header.Caption = "Line";

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
            //gridSection.DisplayLayout.Override.AllowRowFiltering = DefaultableBoolean.False;

            e.Layout.Override.FilterUIType = FilterUIType.FilterRow;
        }

   }
}